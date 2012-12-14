using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CSMDevHelper
{
    public delegate UInt32 DllRegUnRegAPI();

    class LibraryHandler
    {
        static string defaultCSMPath = String.Format(@"{0}\Mitel Customer Service Manager\Server\Bin\", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));

        public LibraryHandler()
        {
            if (!Directory.Exists(defaultCSMPath))
            {
                defaultCSMPath = String.Format(@"{0}\Mitel Customer Service Manager\Server\Bin\", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            }

        }

        public bool RegisterDll(string filepath)
        {
            return RegisterAsm(filepath);
        }

        public bool UnregisterDll(string filepath)
        {
            return UnregisterAsm(filepath);
        }

        public bool RegisterAsm(string filepath)
        {
            Assembly asm = Assembly.LoadFile(defaultCSMPath + filepath);
            RegistrationServices regAsm = new RegistrationServices();
            return regAsm.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);
        }

        public bool UnregisterAsm(string filepath)
        {
            Assembly asm = Assembly.LoadFile(defaultCSMPath + filepath);
            RegistrationServices regAsm = new RegistrationServices();
            return regAsm.UnregisterAssembly(asm);
        }
    }
}
