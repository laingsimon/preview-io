using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using PreviewIo.ComInterop;

namespace PreviewIo
{
	[ProgId("PreviewIo.PreviewHandlerController")]
	[Guid("7A69DD75-FED4-4597-B35D-0765FCB8AD89")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class PreviewHandlerController : IPreviewHandler, IOleWindow, IObjectWithSite, IInitializeWithStream
	{
		private readonly PreviewContext _context;
		private readonly PreviewHandlerForm _previewForm;

		private IntPtr _previewWindowHandle;
		private Stream _previewFileStream = Stream.Null;
		private IPreviewHandlerFrame _frame;

		public PreviewHandlerController()
		{
			try
			{
				Logging.InstallListeners();

				Trace.TraceInformation("Creating PreviewHandlerController");

				_context = new PreviewContext();
				_previewForm = new PreviewHandlerForm(_context);
				_previewForm.Handle.GetHashCode(); //initialse the form
			}
			catch (Exception exc)
			{
				Trace.TraceError("PreviewHandlerController.ctor: {0}", exc);
			}
		}

		void IInitializeWithStream.Initialize(IStream pstream, uint grfMode)
		{
			try
			{
				Trace.TraceInformation("Initialising with a stream");

				_previewForm.Reset();
				_previewFileStream = pstream.ToStream().ToMemoryStream();
			}
			catch (Exception exc)
			{
				Trace.TraceError("PreviewHandlerController.Initialize: {0}", exc);
			}
		}

		void IPreviewHandler.SetWindow(IntPtr hwnd, ref RECT rect)
		{
			try
			{
				Trace.TraceInformation("Setting window");

				_previewForm.Invoke(new MethodInvoker(() => _previewForm.Show()));

				_previewWindowHandle = hwnd;
				_context.OnViewPortChanged(rect.ToRectangle());
				WinApi.SetParent(_previewForm.Handle, _previewWindowHandle);
			}
			catch (Exception exc)
			{
				Trace.TraceError("PreviewHandlerController.SetWindow: {0}", exc);
			}
		}

		void IPreviewHandler.SetRect(ref RECT rect)
		{
			try
			{
				Trace.TraceInformation("Setting rectangle");

				_previewForm.Invoke(new MethodInvoker(() => _previewForm.Show()));

				_context.OnViewPortChanged(rect.ToRectangle());

				WinApi.SetParent(_previewForm.Handle, _previewWindowHandle); //is this required? - if not then remove _previewWindowHandle?
			}
			catch (Exception exc)
			{
				Trace.TraceError("PreviewHandlerController.SetRect: {0}", exc);
			}
		}

		public void DoPreview()
		{
			try
			{
				Trace.TraceInformation("Starting preview");
				_previewForm.Invoke(new MethodInvoker(() => _previewForm.Show()));

				if (_previewFileStream != Stream.Null)
				{
					_context.OnPreviewRequired(_previewFileStream);
					WinApi.SetParent(_previewForm.Handle, _previewWindowHandle); //is this required? - if not then remove _previewWindowHandle
				}
				else
				{
					Trace.TraceError("No File stream set");
				}
			}
			catch (Exception exc)
			{
				Trace.TraceError("PreviewHandlerController.DoPreview: {0}", exc);
			}
		}

		public void Unload()
		{
			Trace.TraceInformation("Unloading");

			_previewForm.Invoke(new MethodInvoker(() => _previewForm.Reset()));
		}

		public void SetFocus()
		{
			Trace.TraceInformation("Setting focus");

			_previewForm.Invoke(new MethodInvoker(() => _previewForm.Focus()));
		}

		public void QueryFocus(out IntPtr phwnd)
		{
			Trace.TraceInformation("Querying focus");

			var focusResult = IntPtr.Zero;
			_previewForm.Invoke(new MethodInvoker(() => WinApi.GetFocus()));
			if (focusResult == IntPtr.Zero)
				throw new Win32Exception();

			phwnd = focusResult;
		}

		uint IPreviewHandler.TranslateAccelerator(ref MSG pmsg)
		{
			Trace.TraceInformation("TranslateAccelerator()");

			if (_frame != null)
				return _frame.TranslateAccelerator(ref pmsg);

			return WinApi.S_FALSE;
		}

		public void GetWindow(out IntPtr phwnd)
		{
			Trace.TraceInformation("Getting window");

			phwnd = _previewForm.Handle;
		}

		public void ContextSensitiveHelp(bool fEnterMode)
		{
			//not implemented
		}

		public void SetSite(object pUnkSite)
		{
			Trace.TraceInformation("Setting site");

			_frame = pUnkSite as IPreviewHandlerFrame;
		}

		public void GetSite(ref Guid riid, out object ppvSite)
		{
			Trace.TraceInformation("Getting site");

			ppvSite = _frame;
		}

		[ComRegisterFunction]
		public static void Register(Type type)
		{
			if (type != typeof(PreviewHandlerController))
				return;

			Installer.RegisterPreviewHandler("draw.io drawing previewer", type);
		}

		[ComUnregisterFunction]
		public static void Unregister(Type type)
		{
			if (type != typeof(PreviewHandlerController))
				return;

			Installer.UnregisterPreviewHandler(type);
		}
	}
}
