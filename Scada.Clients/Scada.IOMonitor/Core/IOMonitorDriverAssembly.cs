using Scada.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Core
{
  public abstract   class IOMonitorDriverAssembly : IDisposable
    {
        #region 加载通讯设备驱动Dll
        private static object CreateObject(string fullname, string dllname)
        {
            try
            {
                Assembly assm = Assembly.LoadFrom(Application.StartupPath + "\\" + dllname + ".dll");//第一步：通过程序集名称加载程序集
                object objType = assm.CreateInstance(fullname, true);// 第二步：通过命名空间+类名创建类的实例。
                return objType;


            }
            catch  
            {
             
                return null;
            }

        }


        /// <summary>
        /// 创建设备驱动
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ScadaDeviceKernel CreateDeviceDrive(Scada.Model.SCADA_DEVICE_DRIVER driveModel)
        {
            try
            {

                ScadaDeviceKernel river = (ScadaDeviceKernel)CreateObject(driveModel.DeviceFullName, driveModel.Dll_Name);
 
                return river;
            }
            catch 
            {
                return null;
            }
       
        }
        /// <summary>
        /// 创建通讯驱动
        /// </summary>
        /// <param name="commModel"></param>
        /// <returns></returns>
        public static ScadaCommunicateKernel CreateCommunicateDriver(Scada.Model.SCADA_DRIVER commModel)
        {
            try
            {

                ScadaCommunicateKernel river = (ScadaCommunicateKernel)CreateObject(commModel.CommunicationFullName, commModel.FillName);
 
                
 
                return river;
            }
            catch  
            {
                return null;
            }
       
        }

        public void Dispose()
        {
          
        }
        #endregion
    }
}
