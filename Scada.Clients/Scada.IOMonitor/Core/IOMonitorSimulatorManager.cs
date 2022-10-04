
using Scada.Kernel;
using Scada.TriggerAlarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Business;
using Scada.DBUtility;
using Scada.Model;


 
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
    public class IOMonitorSimulatorManager: ScadaTask
    {
        public  bool IsSimulator = false;
        List<Scada.Model.IO_SERVER> Servers = null;
        List<Scada.Model.IO_COMMUNICATION> Communications = null;
        List<Scada.Model.IO_DEVICE> Devices = null;
        List<Scada.Model.SCADA_DRIVER> CommDrivers = null;
        List<Scada.Model.SCADA_DEVICE_DRIVER> DeviceDrivers = null;
        public  string DataBaseFileName = Application.StartupPath + "\\IOProject\\Station.station";
        //常规日志处理事件
        public   event MonitorLog OnSimulatorLog;
        /// <summary>
        /// 模拟器初始化
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="comms"></param>
        /// <param name="devices"></param>
        /// <param name="commDrivers"></param>
        /// <param name="deviceDrivers"></param>
        public void InitSimulator(List<Scada.Model.IO_SERVER> servers, List<Scada.Model.IO_COMMUNICATION> comms, List<Scada.Model.IO_DEVICE> devices, List<Scada.Model.SCADA_DRIVER> commDrivers, List<Scada.Model.SCADA_DEVICE_DRIVER> deviceDrivers)
        {
            if (comms == null || devices == null)
                return;

            Servers = servers;
            Communications = comms.FindAll(x => x.SimulatorCheckedStatus == true);
            Devices = devices.FindAll(x=>x.SimulatorCheckedStatus==true);
            CommDrivers = commDrivers;
            DeviceDrivers = deviceDrivers;



        }


        public void StartSimulator()
        {

            if (this.Communications == null || this.Devices == null)
                return;


            IsSimulator = true;
            TaskHelper.Factory.StartNew(() =>
            {
                for (int i = 0; i < this.Communications.Count; i++)
                {
                    if (this.Communications[i].CommunicateDriver == null || this.Communications[i].SimulatorCheckedStatus == false)
                    {
                        continue;

                    }
                    try
                    {
                        ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)this.Communications[i].CommunicateDriver;
                        driverDll.IsCreateControl = false;
                        driverDll.SimulatorLog = (string msg) =>
                        {
                            return TaskHelper.Factory.StartNew(() =>
                                 {
                                     if (OnSimulatorLog != null)
                                         OnSimulatorLog(msg);
                                 });
                        };
                         driverDll.InitKernel(this.Servers.Find(x => x.SERVER_ID == this.Communications[i].IO_SERVER_ID), this.Communications[i],
                            this.Communications[i].Devices,
                             Communications[i].DriverInfo);
                        driverDll.Simulator(this.Communications[i]);
                        driverDll.SimulatorClientStart();

                    }
                    catch (Exception emx)
                    {
                        if (OnSimulatorLog != null)
                            OnSimulatorLog(emx.Message);

                    }

                }
            });

        }





        /// <summary>
        /// 结束模拟器
        /// </summary>
        public void ColseSimulator()
        {
            if (this.Communications == null)
                return;
            IsSimulator = false;
            for (int i = 0; i < this.Communications.Count; i++)
            {
                if (this.Communications[i].CommunicateDriver == null || this.Communications[i].SimulatorCheckedStatus == false)
                {
                    continue;

                }

                ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)this.Communications[i].CommunicateDriver;
                if (driverDll != null)
                    driverDll.SimulatorClientClose();
            }
        }
      
        public override void Dispose()
        {
            try
            {
                ColseSimulator();

                IsSimulator = false;

                base.Dispose();
                GC.Collect();
            }
            catch
            {

            }
        }
    }
}
