using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCenterServer.Core
{
    public  class IISExpressManager
    {
        public static void  IISExpress()
        {
            try
            {
                string str = System.Windows.Forms.Application.StartupPath + @"\IISExpress\iisexpress.exe";

                string strDirPath = System.IO.Path.GetDirectoryName(str);
                string strFilePath = System.IO.Path.GetFileName(str);

                string targetDir = string.Format(strDirPath);//this is where mybatch.bat lies
                Process iisApplication = new Process();
                iisApplication.StartInfo.WorkingDirectory = targetDir;
                iisApplication.StartInfo.FileName = strFilePath;
                iisApplication.StartInfo.Arguments = "/config:"+ System.Windows.Forms.Application.StartupPath + @"\IISExpress\config\applicationhost.config";
                iisApplication.StartInfo.CreateNoWindow = true;
                iisApplication.StartInfo.UseShellExecute = true; ;
                iisApplication.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
       
                iisApplication.Start();
            }
            catch
            {

            }
        }
        public static void Close()
        {
            ScadaProcessManager.KillProcess("iisexpress");
        }
    }
}
