using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace CSMDevHelper
{

    internal enum enumVersionOS : int
    {
        OS_32_bit = 0,
        OS_64_bit = 0,
    };

    class VersionOS
    {
        enumVersionOS versionOS;

        public VersionOS()
        {
            HashSet<String> keySet = new HashSet<String>();
            System.Collections.IEnumerator myEnumerator = Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames().GetEnumerator();

            while(myEnumerator.MoveNext())
            {
                keySet.Add(myEnumerator.Current.ToString());
            }
            if (keySet.Contains("Wow6432Node"))
            {
                this.versionOS = enumVersionOS.OS_64_bit;
            }
            else
            {
                this.versionOS = enumVersionOS.OS_32_bit;
            }
        }

        public enumVersionOS GetVersion()
        {
            return this.versionOS;
        }
    }
}
