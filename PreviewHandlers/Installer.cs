using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration.Install;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers
{
	[RunInstaller(true)]
	public class Installer : System.Configuration.Install.Installer
	{
        public override void Install(IDictionary stateSaver)
        {
            try
            {
                base.Install(stateSaver);
            
                var registrationServices = new RegistrationServices();
                if (!registrationServices.RegisterAssembly(GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
                    throw new InstallException("Failed to register for COM interop.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

		public override void Uninstall(IDictionary savedState)
		{
            base.Uninstall(savedState);

			var registrationServices = new RegistrationServices();
            if (!registrationServices.UnregisterAssembly(GetType().Assembly))
			    throw new InstallException("Failed to unregister for COM interop.");
        }
	}
}