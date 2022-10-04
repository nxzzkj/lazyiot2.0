
using Scada.Model;

using Scada.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
 
using System.Reflection;
using System.Windows.Forms;
using System.Net;

namespace Scada.Kernel
{  
    /// <summary>
    /// 自定义驱动开发接口
    /// </summary>
  
    public   class ScadaCommunicateKernel: IDisposable
    {
        #region 属性定义
        public virtual string GUID
        {
            get;
        }
        public virtual string Title { set; get; }
        public bool IsCreateControl = true;
        protected IO_SERVER IOServer = null;
        protected IO_COMMUNICATION IOCommunication = null;
        
       
        
        /// <summary>
        /// 当前通道下所有的设备
        /// </summary>
        protected List<IO_DEVICE> IODevices = new List<IO_DEVICE>();
        Scada.Business.SCADA_DRIVER driverBll = new Scada.Business.SCADA_DRIVER();
        Scada.Business.SCADA_DEVICE_DRIVER deviceDriverBll = new Scada.Business.SCADA_DEVICE_DRIVER();
        List<SCADA_DEVICE_DRIVER> DeviceDriverModels = new List<SCADA_DEVICE_DRIVER>();
        protected List<ScadaDeviceKernel> DeviceDrives = null;
        
        /// <summary>
        /// 当前通讯驱动下可以指定一个或者多个数据解析协议，如果不指定，则默认加载当前dll库下的所有数据解析协议
        /// </summary>
        public  List<string> AssignDeviceProtocols = new List<string>();

        public void Dispose()
        {
            IOServer = null;
            IOCommunication = null;
            IODevices = null;
            driverBll = null;
            deviceDriverBll = null;
            DeviceDriverModels = null;
            DeviceDrives = null;
            AssignDeviceProtocols = null;
            
        }
        /// <summary>
        /// 判断服务是否在运行
        /// </summary>
        public bool ServerIsRun
        {
            get {
                return IsSuspend;

            }
        }
        private bool IsSuspend = false;
        /// <summary>
        /// 判断服务是否处于暂停状态
        /// </summary>
        public bool ServerIsSuspend
        {
            get { return IsSuspend; }
        }
        public SCADA_DRIVER Driver = null;
        public string CommunicationDriverID
        {
            get
            {
                if (Driver == null)
                    return "";
                return Driver.Id.ToString();
            }
        }
        public string ServerID
        {
            get
            {
                if (IOServer == null)
                    return "";
                return IOServer.SERVER_ID;
            }
        }
        public string CommunicationID
        {
            get
            {
                if (IOCommunication == null)
                    return "";
                return IOCommunication.IO_COMM_ID;
            }
        }
        //驱动参数配置
        public string ParaString = "";
        #endregion
  
        #region 自定义驱动接口和方法


        /// <summary>
        /// 初始化驱动
        /// </summary>
        public   bool InitKernel(IO_SERVER server, IO_COMMUNICATION communication, List<IO_DEVICE> ioDevices, SCADA_DRIVER driver)
        {
            try
            {
                this.IsSuspend =false;
                this.ParaString = "";
                if (communication != null && server != null)
                {
                    this.IOServer = server;
                    this.IOCommunication = communication;
                    this.IODevices = ioDevices;
                    if (driver == null)
                    {
                      driver = driverBll.GetModel(communication.IO_COMM_DRIVER_ID);
                    }
                    if (driver != null)
                    {
                        this.Driver = driver;
                        this.DeviceDriverModels = deviceDriverBll.GetModelList(" Dll_GUID='" + driver.GUID + "'");
                        if(DeviceDriverModels!=null&& DeviceDriverModels.Count>0)
                        DeviceDrives = CreateDeviceKernel(DeviceDriverModels);
                    }

                    if (communication != null)
                        this.ParaString = communication.IO_COMM_PARASTRING;
                    
                }
                return InitCommunicateKernel(server, communication, ioDevices, driver);

            }
            catch (Exception ex)
            {
                this.CommunicationException(ex.Message);
                return false;
            }


        }
        /// <summary>
        /// 定时获取设备的状态信息
        /// </summary>
        /// <param name="sender"></param>
        public Task CheckDeviceStatus()
        {
           return KernelTaskHelper.Factory.StartNew(() =>
            {
                DateTime curr = DateTime.Now;

                for(int i=0;i < this.IODevices.Count;i++)
                {
                    IO_DEVICE device = this.IODevices[i];
                    if (!this.IsSuspend)
                    {
                        if (device.IO_DEVICE_STATUS == 1 && device.Last_Online_Time.AddSeconds(240) < curr)//判断设备在上次上线时间超过4分钟，则认为系统设备处理离线
                        {
                            device.Last_Online_Time = curr;
                            device.IO_DEVICE_STATUS = 0;
                        }


                    }
                }

                SetDeviceStatus(this.IOServer, this.IOCommunication, this.IODevices);
            });
        }
        protected virtual bool InitCommunicateKernel(IO_SERVER server, IO_COMMUNICATION communication, List<IO_DEVICE> ioDevices, SCADA_DRIVER driver)
        {
            try
            {


           
                return true;
            }
            catch
            {
                return false;
            }


        }
        public Task CloseServer()
        {
            
                try
                {
                    this.IsSuspend = true;
                return Close();
                    
                }
                catch (Exception ex)
                {
                    this.CommunicationException(ex.Message);
                }
            return null; 
        }
        protected virtual Task Close()
        {
            return null;
        }
        public event CommunicationEvent CommunicationStatusChanged;
        /// <summary>
        /// 通道上线或者链接通知
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        protected void CommunicationOnline()
        {
            if (IsSuspend)
                return;
            if (IOCommunication != null)
            {
                this.IOCommunication.IO_COMM_STATUS = 1;
                if (CommunicationStatusChanged != null)
                {
                    CommunicationStatusChanged(IOServer, IOCommunication, "1");
                }

            }

        }
        /// <summary>
        /// 通道断开链接或者下线
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        protected void CommunicationOffline()
        {
            if (IsSuspend)
                return;
            if (IOCommunication!=null)
            {
                this.IOCommunication.IO_COMM_STATUS = 0;
                if (CommunicationStatusChanged != null)
                {
                    CommunicationStatusChanged(IOServer, IOCommunication, "0");
                }

            }
        }
        #region 通讯驱动事件
        public event CommunicationEvent CommunctionStart;
        protected void CommunctionStartChanged(IO_SERVER server,object tag)
        {
            if(CommunctionStart!=null)
            {
                CommunctionStart(server,IOCommunication,tag);
            }
        }
        public event CommunicationEvent CommunctionPause;
        protected void CommunctionPauseChanged(IO_SERVER server, object tag)
        {
            if (CommunctionPause != null)
            {
                CommunctionPause(server, IOCommunication,tag);
            }
        }
        public event CommunicationEvent CommunctionStop;
        protected void CommunctionStopChanged(IO_SERVER server, object tag)
        {
            if (CommunctionStop != null)
            {
                CommunctionStop(server, IOCommunication, tag);
            }
        }
        public event CommunicationEvent CommunctionClose;
        protected void CommunctionCloseChanged(IO_SERVER server, object tag)
        {
            if (CommunctionClose != null)
            {
                CommunctionClose(server, IOCommunication, tag);
            }
        }
        public event CommunicationEvent CommunctionContinue;
       
        protected void CommunctionContinueChanged(IO_SERVER server, object tag)
        {
            if (CommunctionContinue != null)
            {
                CommunctionContinue(server, IOCommunication,tag);
            }
        }
        #endregion
 
     
        /// <summary>
        /// 定义用户的驱动配置界面
        /// </summary>
        public virtual CommunicationKernelControl CommunicationControl
        {
            set;
            get;
        }
       
        public virtual SimulatorKernelControl SimulatorControl
        {
            set;get;
        }

        //
        public event KernelEvent DeviceStatusChanged;
       
        /// <summary>
        /// 设备通讯状态变化的时候修改对应的界面显示效果，树结构中的参数
        /// </summary>
        /// <param name="server"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="tag"></param>
        private void SetDeviceStatus(IO_SERVER server, IO_COMMUNICATION comm, List<IO_DEVICE> devices)
        {
            if (IsSuspend)
                return;
            if (devices != null&& devices .Count>0&& DeviceStatusChanged != null)
            {
 
                    DeviceStatusChanged(server, comm, devices);
 
            }
          
        }
        /// <summary>
        /// 设置某个单独设备上线或者下线的通知
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        protected void SetOneDeviceStatus(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device)
        {
            if (IsSuspend)
                return;
            if (device != null && DeviceStatusChanged != null)
            {

                DeviceStatusChanged(server, comm, new List<IO_DEVICE>() { device });

            }
        }
        /// <summary>
        /// 设备上线通知
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        protected void DeviceOnline(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para)
        {
            if (IsSuspend)
                return;
            if (server != null)
                return;
            if (comm != null)
                return;
            if (device != null)
                return;
            if (device != null)
            {
                comm.IO_COMM_STATUS = 1;
              
                IO_DEVICE exDevice = this.IODevices.Find(x => x.IO_SERVER_ID == device.IO_SERVER_ID && x.IO_DEVICE_ID == device.IO_DEVICE_ID && x.IO_COMM_ID == device.IO_COMM_ID);
                if(exDevice!=null)
                {
                    device.IO_DEVICE_STATUS = 1;
                    device.Last_Online_Time = DateTime.Now;
                    exDevice.IO_DEVICE_STATUS = 1;
                    exDevice.Last_Online_Time = device.Last_Online_Time;
                }
               
            }

        }
        /// <summary>
        /// 设备离线通知
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        protected void DeviceOffline(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para)
        {
            if (IsSuspend)
                return;
            if (device != null)
            {
                device.IO_DEVICE_STATUS = 0;
                IO_DEVICE exDevice = this.IODevices.Find(x => x.IO_SERVER_ID == device.IO_SERVER_ID && x.IO_DEVICE_ID == device.IO_DEVICE_ID && x.IO_COMM_ID == device.IO_COMM_ID);
                if (exDevice != null)
                {
                    device.IO_DEVICE_STATUS = 0;
                    exDevice.IO_DEVICE_STATUS = 0;
                }

            }

        }
        public   string GetUIParameter()
        {
            if (CommunicationControl != null)
                return CommunicationControl.GetUIParameter();
            else
                return "";
        }
        public   void SetUIParameter(string patastring)
        {
            this.ParaString = patastring;
            if (CommunicationControl != null)
                CommunicationControl.SetUIParameter(patastring);
        }
        public void PauseServer()
        {
            try
            {
                IsSuspend = true;
                Pause();
            }
            catch (Exception ex)
            {
                this.CommunicationException(ex.Message);
            }
        }
        protected virtual void Pause()
        {
            
        }
        public void ContinueServer()
        {
            try
            {
                IsSuspend = false;
                Continue();
            }
            catch (Exception ex)
            {
                this.CommunicationException(ex.Message);
            }
        }
        protected virtual void Continue()
        {
          
        }
        /// <summary>
        /// 设备中的下置命令后的事件
        /// </summary>
        public event DeviceSendedEvent DeviceSended;

       
        //发送下置命令,没有返回结构的值
        public  ScadaResult SendCommand(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value)
        {
            try
            {
                ScadaResult result= IOSendCommand(server, comm, device, para, value);
                if (DeviceSended != null)
                {
                    DeviceSended(server, comm, device, para, value, result.Result);
                }
                return result;
            }
            catch
            {
                return new ScadaResult(false, "发送数据失败");
            }
        }
        protected virtual ScadaResult IOSendCommand(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value)
        {
            return new ScadaResult(false, "发送数据失败");
        }

        /// <summary>
        /// 发送数据完成的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="value"></param>
        protected void DataSended(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value,bool result)
        {
            if (DeviceSended != null)
            {
                DeviceSended(server, comm, device, para, value, result);
            }
        }
        public Task StartServer()
        {
            
                try
                {
                    this.IsSuspend = false;
                   return Start();
                    
                }
                catch (Exception ex)
                {
                    this.CommunicationException(ex.Message);
                }
            return null;
        }
        protected virtual Task Start()
        {
            return null;
        }
        public Task StopServer()
        {
           
                try
                {


                    IsSuspend = false;
                  return   Stop();
                }
                catch (Exception ex)
                {
                    this.CommunicationException(ex.Message);
                }
            return null;

        }
        protected virtual Task Stop()
        {
            return null;
        }
        /// <summary>
        /// 程序异常的事件
        /// </summary>
        public event CommunicationKernelError OnKernelException;
        public event KernelOutLog OnKernelLog;
        /// <summary>
        /// 程序异常处理
        /// </summary>
        /// <param name="msg"></param>
        protected  void CommunicationException(string msg)
        {

            Logger.Logger.GetInstance().Debug("通道 "+msg);
            if (this.OnKernelException != null)
                this.OnKernelException(IOServer, IOCommunication,null,msg);
        }
        protected void KernelLog(string msg)
        {
            if (this.OnKernelLog != null)
                this.OnKernelLog(msg);
        }
        public Func<IO_SERVER , IO_COMMUNICATION , IO_DEVICE , byte[] , string , object,Task>  KernelDataReceived;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="receivedatas">接收的数据的字节</param>
        /// <param name="date">接收数据的日期</param>
        /// <param name="sender">接收数据的其它限定参数</param>
        protected Task ReceiveData(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, byte[] receivedatas, string date, object sender = null)
        {
            if (IsSuspend)
                return null;
           
            if (KernelDataReceived != null)
            {
                //通知设备上线
             
                if(device!=null)
                    this.DeviceOnline(server, comm, device, null);
                  return KernelDataReceived(server, comm, device.Copy(), receivedatas, date, sender);
             
              
            }

            return null;
        }

        #endregion
        #region 下位机模拟器模拟实现接口
        protected IO_COMMUNICATION SimulatorCommunication = null;
        public bool SimulatorStatus = false;
        //模拟器返回的日志信息
        public Func<string,Task> SimulatorLog;
        public virtual void  Simulator(IO_COMMUNICATION communication)
        {
            SimulatorCommunication = communication;
        }

        protected virtual Task SimulatorStart()
        {
            return null;
        }
        protected virtual Task SimulatorClose()
        {
            return null;
        }
        public Task SimulatorClientStart()
        {
             
                try
                {
                    SimulatorStatus = true;
                  return  SimulatorStart();
                   
                }
                catch (Exception emx)
                {
                    SimulatorAppendLog(emx.Message);

                }
            return null;
           
        }
        public Task SimulatorClientClose()
        {
           
                try
                {
                    SimulatorStatus = false;
                   return  SimulatorClose();
               
                }
                catch (Exception emx)
                {
                    SimulatorAppendLog(emx.Message);

                }
            return null;
         
        }

        public Task SimulatorAppendLog(string msg)
        {
            
            if (SimulatorLog != null)
             return   SimulatorLog(msg);
            return null; 
        }
        #endregion
        #region 自定义驱动窗体配置界面接口
        /// <summary>
        /// 返回一个模拟器的编辑界面
        /// </summary>
        /// <returns></returns>
        public virtual SimulatorKernelControl CreateSimulatorKernelControl()
        {
            return new SimulatorKernelControl();
        }
        /// <summary>
        /// 返回一个模拟器中鼠标右键设备编辑的编辑界面
        /// </summary>
        /// <returns></returns>
        public virtual SimulatorDeviceEditControl CreateSimulatorDeviceEdit()
        {
            return new SimulatorDeviceEditControl();
        }
        /// <summary>
        /// 返回一个驱动参数配置界面
        /// </summary>
        /// <returns></returns>
        public virtual CommunicationKernelControl CreateCommunicationKernelControl()
        {
            return new CommunicationKernelControl();
        }


        #endregion
        #region 接口反射解析

        //不使用缓存
        protected object CreateObject(string fullname, string dllname)
        {
            try
            {
                object objType = Assembly.LoadFrom(Application.StartupPath + "/" + dllname + ".dll").CreateInstance(fullname, true);
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
        protected List<ScadaDeviceKernel> CreateDeviceKernel(List<SCADA_DEVICE_DRIVER> driveModels)
        {
            List<ScadaDeviceKernel> drives = new List<ScadaDeviceKernel>();
            for (int i = 0; i < driveModels.Count; i++)
            {
                ScadaDeviceKernel river = (ScadaDeviceKernel)CreateObject(driveModels[i].DeviceFullName, driveModels[i].Dll_Name);
                if (river != null)
                {

                    river.Driver = driveModels[i];


                    drives.Add(river);
                }

            }
            return drives;


        }
        protected ScadaCommunicateKernel CreateCommunicateDrive(SCADA_DRIVER communicateModel)
        {
            ScadaCommunicateKernel river = (ScadaCommunicateKernel)CreateObject(communicateModel.CommunicationFullName, communicateModel.FillName);
            if (river != null)
            {
                river.Driver = communicateModel;
            }

            return river;


        }
        #endregion
    }
}
