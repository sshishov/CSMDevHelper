using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace CSMDevHelper
{

    internal enum enumDriverVersion : int
    {
        CP5000 = 10,
        MCD5x = 100,
        MCD4x = 100,
    };

    class RegistryHandler
    {
        RegistryKey regKey;
        RegistryKey drvKey;
        RegistryKey syncKey;

        public RegistryHandler()
        {
            enumVersionOS version = new VersionOS().GetVersion();
            string versionString = String.Empty;
            if (version == enumVersionOS.OS_64_bit)
            {
                versionString = @"Wow6432Node\";
            }
            else
            {
                // TODO Do not know what is the key for 32 bit
            }
            this.regKey = Registry.LocalMachine.OpenSubKey(String.Format(@"Software\{0}Mitel\CSM", versionString), true);
            this.drvKey = Registry.LocalMachine.OpenSubKey(String.Format(@"Software\{0}Mitel\CSM\Server\TelDriver", versionString), true);
            this.syncKey = Registry.LocalMachine.OpenSubKey(String.Format(@"Software\{0}Mitel\CSM\Server\TelDriver\Mitel 3300 ICP", versionString), true);
        }

        public enumDriverVersion DriverVersion
        {
            get
            {
                return (enumDriverVersion)drvKey.GetValue(@"CurrentTelDriver");
            }
            set{}
        }

        public bool InitialSync
        {
            get
            {
                return Convert.ToBoolean(syncKey.GetValue(@"SkipMIXMLInitialSync"));
            }
            set
            {
                syncKey.SetValue(@"SKipMIXMLInitialSync", value, RegistryValueKind.DWord);
            }
        }

        public bool AllSync
        {
            get
            {
                return Convert.ToBoolean(syncKey.GetValue(@"SkipMIXMLAllSync"));
            }
            set
            {
                syncKey.SetValue(@"SKipMIXMLAllSync", value, RegistryValueKind.DWord);
            }
        }

        public string ClientVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"Client").GetValue(@"Version"));
            }
        }

        public string DataManagerVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"Data Manager").GetValue(@"Version"));
            }
        }

        public string RealViewerVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"RealViewer").GetValue(@"Version"));
            }
        }

        public string ReporterVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"Reporter").GetValue(@"Version"));
            }
        }

        public string RouterVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"Router").GetValue(@"Version"));
            }
        }

        public string ReporterRealTimeVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"RT").GetValue(@"Version"));
            }
        }

        public string ServerVersion
        {
            get
            {
                return Convert.ToString(this.regKey.OpenSubKey(@"Server").GetValue(@"Version"));
            }
        }
    }
}
