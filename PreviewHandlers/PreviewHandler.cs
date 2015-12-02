using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FuelAdvance.PreviewHandlerPack.PreviewHandlers.ComInterop;
using Microsoft.Win32;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers
{
    public abstract class PreviewHandler : IPreviewHandler, IPreviewHandlerVisuals, IOleWindow, IObjectWithSite
    {
        bool showPreview;
        readonly PreviewHandlerControl previewControl;
        IntPtr parentHwnd;
        Rectangle windowBounds;
        object unkSite;
        IPreviewHandlerFrame frame;

        protected PreviewHandler()
        {
            previewControl = CreatePreviewHandlerControl(); // NOTE: shouldn't call virtual function from constructor; see article for more information
            previewControl.Handle.GetHashCode();
            previewControl.BackColor = SystemColors.Window;
        }

        protected abstract PreviewHandlerControl CreatePreviewHandlerControl();

        private void InvokeOnPreviewThread(MethodInvoker d)
        {
            previewControl.Invoke(d);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void UpdateWindowBounds()
        {
            if (!showPreview) return;
            
            InvokeOnPreviewThread(delegate
            {
                SetParent(previewControl.Handle, parentHwnd);
                previewControl.Bounds = windowBounds;
                previewControl.Visible = true;
            });
        }

        void IPreviewHandler.SetWindow(IntPtr hwnd, ref RECT rect)
        {
            parentHwnd = hwnd;
            windowBounds = rect.ToRectangle();
            UpdateWindowBounds();
        }

        void IPreviewHandler.SetRect(ref RECT rect)
        {
            windowBounds = rect.ToRectangle();
            UpdateWindowBounds();
        }

        protected abstract void Load(PreviewHandlerControl c);

        void IPreviewHandler.DoPreview()
        {
            showPreview = true;
            InvokeOnPreviewThread(delegate
            {
                try
                {
                    Load(previewControl);
                }
                catch (Exception exc)
                {
                    previewControl.Controls.Clear();
                    var text = new TextBox
                    {
                        ReadOnly = true,
                        Multiline = true,
                        Dock = DockStyle.Fill,
                        Text = exc.ToString()
                    };
                    previewControl.Controls.Add(text);
                }
                UpdateWindowBounds();
            });
        }

        void IPreviewHandler.Unload()
        {
            showPreview = false;
            InvokeOnPreviewThread(delegate
            {
                previewControl.Visible = false;
                previewControl.Unload();
            });
        }

        void IPreviewHandler.SetFocus()
        {
            InvokeOnPreviewThread(() => previewControl.Focus());
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetFocus();

        void IPreviewHandler.QueryFocus(out IntPtr phwnd)
        {
            var result = IntPtr.Zero;
            InvokeOnPreviewThread(delegate { result = GetFocus(); });
            phwnd = result;
            if (phwnd == IntPtr.Zero) throw new Win32Exception();
        }

        uint IPreviewHandler.TranslateAccelerator(ref MSG pmsg)
        {
            if (frame != null) return frame.TranslateAccelerator(ref pmsg);
            const uint S_FALSE = 1;
            return S_FALSE;
        }

        void IPreviewHandlerVisuals.SetBackgroundColor(COLORREF color)
        {
            var c = color.Color;
            InvokeOnPreviewThread(delegate { previewControl.BackColor = c; });
        }

        void IPreviewHandlerVisuals.SetTextColor(COLORREF color)
        {
            var c = color.Color;
            InvokeOnPreviewThread(delegate { previewControl.ForeColor = c; });
        }

        void IPreviewHandlerVisuals.SetFont(ref LOGFONT plf)
        {
            var f = Font.FromLogFont(plf);
            InvokeOnPreviewThread(delegate { previewControl.Font = f; });
        }

        void IOleWindow.GetWindow(out IntPtr phwnd)
        {
            phwnd = previewControl.Handle;
        }

        void IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            throw new NotImplementedException();
        }

        void IObjectWithSite.SetSite(object pUnkSite)
        {
            unkSite = pUnkSite;
            frame = unkSite as IPreviewHandlerFrame;
        }

        void IObjectWithSite.GetSite(ref Guid riid, out object ppvSite)
        {
            ppvSite = unkSite;
        }

        [ComRegisterFunction]
        public static void Register(Type t)
        {
            if (t == null) return;
            if (!t.IsSubclassOf(typeof(PreviewHandler))) return;
            
            var attrs = t.GetCustomAttributes(typeof(PreviewHandlerAttribute), true);
            if (attrs.Length != 1) return;

            var attr = (PreviewHandlerAttribute)attrs[0];
            RegisterPreviewHandler(attr.Name, attr.Extension, t.GUID.ToString("B"), attr.AppId);
        }

        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            if (t == null) return;
            if (!t.IsSubclassOf(typeof(PreviewHandler))) return;
            
            var attrs = t.GetCustomAttributes(typeof(PreviewHandlerAttribute), true);
            if (attrs.Length != 1) return;

            var attr = (PreviewHandlerAttribute)attrs[0];
            UnregisterPreviewHandler(attr.Extension, t.GUID.ToString("B"), attr.AppId);
        }

        protected static void RegisterPreviewHandler(string name, string extensions, string previewerGuid, string appId)
        {
            // Create a new prevhost AppID so that this always runs in its own isolated process
            using (var appIdsKey = Registry.ClassesRoot.OpenSubKey("AppID", true))
            using (var appIdKey = appIdsKey.CreateSubKey(appId))
            {
                appIdKey.SetValue("DllSurrogate", @"%SystemRoot%\system32\prevhost.exe", RegistryValueKind.ExpandString);
            }

            // Add preview handler to preview handler list
            using (var handlersKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", true))
            {
                handlersKey.SetValue(previewerGuid, name, RegistryValueKind.String);
            }

            // Modify preview handler registration
            using (var clsidKey = Registry.ClassesRoot.OpenSubKey("CLSID"))
            using (var idKey = clsidKey.OpenSubKey(previewerGuid, true))
            {
                idKey.SetValue("DisplayName", name, RegistryValueKind.String);
                idKey.SetValue("AppID", appId, RegistryValueKind.String);
                //idKey.SetValue("DisableLowILProcessIsolation", 1, RegistryValueKind.DWord);
            }

            foreach (var extension in extensions.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Trace.WriteLine("Registering extension '" + extension + "' with previewer '" + previewerGuid + "'");

                // Set preview handler for specific extension
                using (var extensionKey = Registry.ClassesRoot.CreateSubKey(extension))
                using (var shellexKey = extensionKey.CreateSubKey("shellex"))
                using (var previewKey = shellexKey.CreateSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}"))
                {
                    previewKey.SetValue(null, previewerGuid, RegistryValueKind.String);
                }
            }
        }

        protected static void UnregisterPreviewHandler(string extensions, string previewerGuid, string appId)
        {
            foreach (var extension in extensions.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Trace.WriteLine("Unregistering extension '" + extension + "' with previewer '" + previewerGuid + "'");
                using (var shellexKey = Registry.ClassesRoot.OpenSubKey(extension + "\\shellex", true))
                {
                    try { shellexKey.DeleteSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}"); }
                    catch { }
                }
            }

            using (var appIdsKey = Registry.ClassesRoot.OpenSubKey("AppID", true))
            {
                try { appIdsKey.DeleteSubKey(appId); }
                catch { }
            }

            using (var classesKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", true))
            {
                try { classesKey.DeleteValue(previewerGuid); }
                catch { }
            }
        }
    }
}