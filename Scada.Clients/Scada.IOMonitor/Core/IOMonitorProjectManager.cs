
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Business;
using Scada.DBUtility;


 
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
    public class IOMonitorProjectManager
    {


        //当前服务器加载的采集站
        public static Scada.Model.IO_SERVER IOServer = null;
        //当前服务器加载的通道
        public static List<Scada.Model.IO_COMMUNICATION> IOCommunications = null;
        //当前服务器加载的设备
        public static List<Scada.Model.IO_DEVICE> IODevices = null;
        public static List<Scada.Model.SCADA_DRIVER> CommDrivers = null;
        public static List<Scada.Model.SCADA_DEVICE_DRIVER> DeviceDrivers = null;
        public static int ProgressMaxNum = 0;
        private static string mServerID = "";
        public static event MonitorException OnDataBaseExceptionHanped;
        public static event MonitorLog OnDataBaseLoged;

        public static string DataBaseFileName = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "IOProject\\Station.station";
        #region 异常处理，统一都输出到主任何界面
        private static   void AddLogToMainLog(string msg)
        {
            if (OnDataBaseLoged != null)
            {
                OnDataBaseLoged(msg);
            }

        }
        /// <summary>
        /// 异常信息在日志端显示
        /// </summary>
        /// <param name="ex"></param>
        private static void ThrowExceptionToMain(Exception ex)
        {
            if (OnDataBaseExceptionHanped != null)
            {
                OnDataBaseExceptionHanped(ex);
            }

        }


        #endregion
        public static string ServerID
        {
            get
            {
                if (mServerID == "")
                {
                    DbHelperSQLite.connectionString = DataBaseFileName;
                    IO_SERVER serverBll = new IO_SERVER();
                    AddLogToMainLog("读取采集站信息......");
                    IOServer = serverBll.GetModel();
                    mServerID = IOServer.SERVER_ID;
                    return mServerID;
                }
                return mServerID;
            }
        }
    
        public static void InitBaseModel()
        {
            DataBaseFileName= "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "IOProject\\Station.station";
            if (ServerID == "")
                return;
            try
            {
                DbHelperSQLite.connectionString = DataBaseFileName;
                IO_SERVER serverBll = new IO_SERVER();
                AddLogToMainLog("读取采集站信息......");
                IOServer = serverBll.GetModel(mServerID);
                if (IOServer == null)
                    return;
                mServerID = IOServer.SERVER_ID;
                IOServer.SERVER_IP = LocalIp.GetLocalIp();
                serverBll.Update(IOServer);
                //加载通道
                AddLogToMainLog("读取采集站通道信息......");
                IO_COMMUNICATION commBll = new IO_COMMUNICATION();
                SCADA_DRIVER DriverBll = new SCADA_DRIVER();
                SCADA_DEVICE_DRIVER DeviceDriverBll = new SCADA_DEVICE_DRIVER();
                CommDrivers = DriverBll.GetModelList("");
                DeviceDrivers = DeviceDriverBll.GetModelList("");
                IOCommunications = commBll.GetModelList(" IO_SERVER_ID='" + IOServer.SERVER_ID + "'");
               
                AddLogToMainLog("读取采集站通道下的所有设备信息......");
                IO_DEVICE deviceBll = new IO_DEVICE();
                IODevices = deviceBll.GetModelList(" IO_SERVER_ID='" + IOServer.SERVER_ID + "'");
                AddLogToMainLog("数据处理中.....");

                List<Task> tasks = new List<Task>();
                for (int index = 0; index < IOCommunications.Count; index++)
                {
                    int i = index;
                    var task = TaskHelper.Factory.StartNew(() => {
                
                    IOCommunications[i].DriverInfo = CommDrivers.Find(x => x.Id == IOCommunications[i].IO_COMM_DRIVER_ID);
                    if (IOCommunications[i].DriverInfo != null)
                    {
                        IOCommunications[i].CommunicateDriver = IOMonitorDriverAssembly.CreateCommunicateDriver(IOCommunications[i].DriverInfo);

                    }
                    AddLogToMainLog("处理 " + IOCommunications[i].IO_COMM_NAME + "[" + IOCommunications[i].IO_COMM_LABEL + "]");
                    IOCommunications[i].Devices = IODevices.FindAll(x => x.IO_COMM_ID == IOCommunications[i].IO_COMM_ID && x.IO_SERVER_ID == IOCommunications[i].IO_SERVER_ID);
                    for (int j = 0; j < IOCommunications[i].Devices.Count; j++)
                    {
                        IOCommunications[i].Devices[j].DriverInfo = DeviceDrivers.Find(x => x.Id == IOCommunications[i].Devices[j].DEVICE_DRIVER_ID);

                        if (IOCommunications[i].Devices[j].DriverInfo!=null)
                        {
                            IOCommunications[i].Devices[j].DeviceDrive = IOMonitorDriverAssembly.CreateDeviceDrive(IOCommunications[i].Devices[j].DriverInfo);
                        }
                    }

                    });
                    tasks.Add(task);
                }
                Task.WaitAll(tasks.ToArray());
                AddLogToMainLog("正在创建驱动.....");

                AddLogToMainLog("读取工程完成!");
                ProgressMaxNum = IOCommunications.Count + IODevices.Count;
            }
            catch(Exception ex)
            {
                ThrowExceptionToMain(ex);
            }
        

        }
    }
}
