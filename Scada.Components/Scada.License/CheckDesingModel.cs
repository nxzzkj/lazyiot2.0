using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.License
{
    public class CheckDesingModel
    {
        public static bool IsDesingMode()
        {
            bool ReturnFlag = false;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                ReturnFlag = true;
            else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                ReturnFlag = true; 
            return ReturnFlag;
        }
    }
}
