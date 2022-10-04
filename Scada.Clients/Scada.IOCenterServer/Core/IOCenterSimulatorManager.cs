
using Scada.DBUtility;
using Scada.TriggerAlarm;
using Scada.Model;
using ScadaCenterServer.Pages;
using System;
using System.Collections.Generic;
using System.Threading;
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
namespace ScadaCenterServer.Core
{
    public class IOCenterSimulatorManager : ScadaTask
    {
        public bool IsSimulator = false;
        public List<Scada.Model.IO_SERVER> Servers = new List<Scada.Model.IO_SERVER>();
        public List<Scada.Model.IO_COMMUNICATION> Communications = new List<Scada.Model.IO_COMMUNICATION>();
        public List<Scada.Model.IO_DEVICE> Devices = new List<Scada.Model.IO_DEVICE>();
        public string DataBaseFileName = Application.StartupPath + "\\IOProject\\IOCenterServer.station";

        public int Interval = 3;//默认是3秒

        /// <summary>
        /// 启动模拟器
        /// </summary>
        /// <param name="form"></param>
        public void IniSimulator()
        {
            try
            {
                if (SimulatorForm != null)
                {
                    ColseSimulator();
                    SimulatorForm.Close();
                    SimulatorForm = null;
                }

                Scada.Business.IO_DEVICE deviceBll = new Scada.Business.IO_DEVICE();
                Scada.Business.IO_COMMUNICATION commBll = new Scada.Business.IO_COMMUNICATION();
                Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
                Interval = 120;//获取时间间隔
                Servers = serverBll.GetModelList("");
                Communications = commBll.GetModelList("");
                Devices = deviceBll.GetModelList("");
                for (int i = 0; i < Communications.Count; i++)
                {
                    Communications[i].Devices = Devices.FindAll(x => x.IO_COMM_ID == Communications[i].IO_COMM_ID && x.IO_SERVER_ID == Communications[i].IO_SERVER_ID);
                }
            }
            catch
            {

            }


        }
        SimulatorForm SimulatorForm = null;

        public void ReloadSimulator(string IO_SERVER_ID)
        {
            TaskHelper.Factory.StartNew(() =>
            {
                if (IOCenterManager.SimulatorManager.SimulatorForm != null)
                {
                    IOCenterManager.SimulatorManager.ColseSimulator();
                }
                Scada.Business.IO_DEVICE deviceBll = new Scada.Business.IO_DEVICE();
                Scada.Business.IO_COMMUNICATION commBll = new Scada.Business.IO_COMMUNICATION();
                Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
                lock (Servers)
                {
                    Scada.Model.IO_SERVER deleteServer = Servers.Find(x => x.SERVER_ID == IO_SERVER_ID);
                    if (deleteServer != null)
                    {
                        Servers.Remove(deleteServer);//删除这个采集站信息
                    }

                    Scada.Model.IO_SERVER newsServer = serverBll.GetModel(IO_SERVER_ID);
                    if (newsServer != null)
                    {
                        Servers.Add(newsServer);
                    }
                    else
                    {
                        return;
                    }
                }
                lock (Communications)
                {

                    for (int i = Communications.Count - 1; i >= 0; i--)
                    {
                        if (Communications[i].IO_SERVER_ID == IO_SERVER_ID)
                        {
                            //首先删除设备
                            for (int d = Communications[i].Devices.Count - 1; d >= 0; d--)
                            {
                                Devices.Remove(Communications[i].Devices[d]);
                            }
                            Communications.RemoveAt(i);
                        }
                    }

                    List<Scada.Model.IO_COMMUNICATION> newsCommunications = commBll.GetModelList(" IO_SERVER_ID='" + IO_SERVER_ID + "' ");
                    Communications.AddRange(newsCommunications);
                    List<Scada.Model.IO_DEVICE> newsDevices = deviceBll.GetModelList("  IO_SERVER_ID='" + IO_SERVER_ID + "'");
                    Devices.AddRange(newsDevices);

                    for (int i = 0; i < newsCommunications.Count; i++)
                    {
                        newsCommunications[i].Devices = newsDevices.FindAll(x => x.IO_COMM_ID == newsCommunications[i].IO_COMM_ID && x.IO_SERVER_ID == newsCommunications[i].IO_SERVER_ID);
                    }


                }
            });
        }
        public void ShowSimulator()
        {

            if (SimulatorForm == null || SimulatorForm.IsDisposed)
            {

                SimulatorForm = new SimulatorForm();
                SimulatorForm.EnableTool = true;
            }

            SimulatorForm.Show();
        }
        public void StartSimulator()
        {
            if (SimulatorForm == null)
            {
                ShowSimulator();
            }
            SimulatorForm.EnableTool = false;

            try
            {
                Interval = SimulatorForm.Interval;
                IsSimulator = true;
                RunSimulator();
            }
            catch (Exception ex)
            {
                SimulatorForm.AddLog(ex.Message);
                ColseSimulator();
            }
        }
        Random random = new Random();
        private void RunSimulator()
        {
            TaskHelper.Factory.StartNew(() =>
            {
                try
                {





                    for (int d = 0; d < Devices.Count; d++)
                    {
                        try
                        {


                            Scada.Model.IO_DEVICE Device = Devices[d];
                            TaskManager.Add(
                                               TaskHelper.Factory.StartNew(delegate
                          {

                          //将新值写入数据库
                          while (IsSimulator)
                              {
                                  TaskHelper.Factory.StartNew(() =>
                                  {
                                      Device.GetedValueDate = DateTime.Now;
                                  ///重新赋值
                                  for (int i = 0; i < Device.IOParas.Count; i++)
                                      {
                                          Device.IOParas[i].IORealData = new Scada.IOStructure.IOData() { ParaName = Device.IOParas[i].IO_NAME, ParaValue = random.Next(5000, 9000).ToString(), QualityStamp = Scada.IOStructure.QualityStamp.GOOD, Date = Device.GetedValueDate.Value };
                                      }
                                  ///将接收到的数据存储到当前缓存

                                  IOCenterManager.InfluxDbManager.DbWrite_RealPoints(new List<Scada.Model.IO_DEVICE>() { Device }, DateTime.Now);
                                      if (SimulatorForm != null)
                                      {
                                          SimulatorForm.AddLog(Device.IO_DEVICE_NAME + " IO点插入了模拟数据成功");
                                      }

                                      if (IsMakeAlarm)
                                      {
                                          IODeviceParaTriggerAlarm paraMaker = new IODeviceParaTriggerAlarm();
                                          List<IO_PARAALARM> alarms = new List<IO_PARAALARM>();

                                          for (int i = 0; i < Device.IOParas.Count; i++)
                                          {
                                              try
                                              {
                                                  IO_PARAALARM alarm = paraMaker.MakeAlarm(Device.IOParas, Device.IOParas[i], Device.IOParas[i].IORealData, Device.IO_DEVICE_LABLE);
                                                  if (alarm != null)
                                                  {
                                                      IOCenterManager.InfluxDbManager.DbWrite_AlarmPoints(alarm, DateTime.Now);
                                                  }
                                              }
                                              catch (Exception ex)
                                              {
                                                  this.SimulatorForm.AddLog("ERROR20003" + ex.Message);
                                              }

                                          }


                                      }

                                  });

                                  if (SimulatorForm != null && SimulatorForm.IsCustomTimer)
                                  {
                                      Thread.Sleep(Interval * 1000);
                                  }
                                  else
                                  {
                                      Thread.Sleep(Device.IO_DEVICE_UPDATECYCLE * 1000);

                                  }

                              }
                          }));

                        }
                        catch (Exception ex)
                        {
                            if (SimulatorForm != null)
                            {
                                SimulatorForm.AddLog(ex.Message);
                            }
                        }




                    }
                }
                catch (Exception ex)
                {

                    SimulatorForm.AddLog(ex.Message);
                    ColseSimulator();
                }
            });

        }

        /// <summary>
        /// 是否开启模拟报警
        /// </summary>

        public bool IsMakeAlarm
        { set; get; }
        /// <summary>
        /// 结束模拟器
        /// </summary>
        public void ColseSimulator()
        {
            Task.WaitAll();
            IsSimulator = false;
            if (SimulatorForm != null)
            {
                SimulatorForm.EnableTool = true;
                SimulatorForm.Dispose();
                SimulatorForm = null;
            }
            GC.Collect();


        }

        public override void Dispose()
        {
            Task.WaitAll();
            IsSimulator = false;





            Servers.Clear();
            Communications.Clear();
            Devices.Clear();
            base.Dispose();
            GC.Collect();
        }
    }
}
