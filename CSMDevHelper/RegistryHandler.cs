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
                RegistryKey _key = this.regKey.OpenSubKey(@"Client");
                if (_key != null)
                {
                    return Convert.ToString(_key.GetValue(@"Version"));
                }
                return "";
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
                RegistryKey _key = this.regKey.OpenSubKey(@"RealViewer");
                if (_key != null)
                {
                    return Convert.ToString(_key.GetValue(@"Version"));
                }
                return "";
            }
        }

        public string ReporterVersion
        {
            get
            {
                RegistryKey _key = this.regKey.OpenSubKey(@"Reporter");
                if (_key != null)
                {
                    return Convert.ToString(_key.GetValue(@"Version"));
                }
                return "";
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
                RegistryKey _key = this.regKey.OpenSubKey(@"RT");
                if (_key != null)
                {
                    return Convert.ToString(_key.GetValue(@"Version"));
                }
                return "";
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
