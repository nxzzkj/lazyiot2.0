
using Scada.Business;
using Scada.DBUtility;
using Scada.MDSCore;
using System;
using System.Collections.Generic;
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
    public class IOCenterIOProjectManager : ScadaTask
    {
        public IOCenterIOProjectManager()
        {
            ServerConfig = new CenterServerConfig();
        }
        public event CenterServerLogHappened CenterServerLog;
        /// <summary>
        /// 错误日志
        /// </summary>
        public event CenterServerExceptionHappened CenterServerException;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseFileName = AppDomain.CurrentDomain.BaseDirectory + "\\IOProject\\IOCenterServer.station";
        public CenterServerConfig ServerConfig = null;
        public List<Scada.Model.IO_SERVER> Servers = new List<Scada.Model.IO_SERVER>();
        public List<Scada.Model.IO_COMMUNICATION> Communications = new List<Scada.Model.IO_COMMUNICATION>();
        public List<Scada.Model.IO_DEVICE> Devices = new List<Scada.Model.IO_DEVICE>();
        IO_SERVER serverBll = new IO_SERVER();
        IO_COMMUNICATION commBll = new IO_COMMUNICATION();
        IO_DEVICE deviceBll = new IO_DEVICE();

          Scada.Business.BatchCommandTaskModel mBatchCommandTaskBll = new Scada.Business.BatchCommandTaskModel();
          Scada.Business.BatchCommandTaskItemModel mBatchCommandTaskItemBll = new Scada.Business.BatchCommandTaskItemModel();

        Scada.Business.ScadaMachineTrainingModel mMachineTrainingBll = new Scada.Business.ScadaMachineTrainingModel();
        /// <summary>
        /// 自动控制任务
        /// </summary>
        public List<Scada.Model.BatchCommandTaskModel> BatchCommandTasks = new List<Scada.Model.BatchCommandTaskModel>();
       
        /// <summary>
        /// 机器训练任务
        /// </summary>
        public List<Scada.Model.ScadaMachineTrainingModel> MachineTrainingModels = new List<Scada.Model.ScadaMachineTrainingModel>();
        public void LoadProject()
        {

            DataBaseFileName = AppDomain.CurrentDomain.BaseDirectory + "\\IOProject\\IOCenterServer.station";
            //设置数据库数据源
            DbHelperSQLite.connectionString = "Data Source=" + DataBaseFileName;

            AddLog("初始化采集站数据......");
            Servers = serverBll.GetModelList("");
            AddLog("采集站数据已经完成！");
            AddLog("加载通道数据......");
            Communications = commBll.GetModelList("");
            AddLog("通道数据已经完成!");
            AddLog("加载设备及其IO点表......");
            Devices = deviceBll.GetModelList("");
            AddLog("加载设备及其IO点表已经完成");

          
            for (int i = 0; i < Communications.Count; i++)
            {
                Communications[i].Devices = Devices.FindAll(x => x.IO_COMM_ID == Communications[i].IO_COMM_ID && x.IO_SERVER_ID == Communications[i].IO_SERVER_ID);
            }
            AddLog("加载自动控制任务......");
            BatchCommandTasks = mBatchCommandTaskBll.GetModelList("");
            List<Scada.Model.BatchCommandTaskItemModel> BatchCommandTaskItems = mBatchCommandTaskItemBll.GetModelList("");
            for (int i = 0; i < BatchCommandTasks.Count; i++)
            {
                BatchCommandTasks[i].Items = BatchCommandTaskItems.FindAll(x => x.CommandTaskID == BatchCommandTasks[i].Id && x.SERVER_ID == BatchCommandTasks[i].SERVER_ID);
            }
            AddLog("加载自动控制任务已经完成");
            AddLog("加载机器训练任务......");
            MachineTrainingModels = mMachineTrainingBll.GetModelList("");
            AddLog("加载机器训练任务已经完成");
        }
        public void ReLoadProject(string IO_SERVER_ID)
        {

            //删除当前的采集站信息
            Servers.RemoveAll(x => x.SERVER_ID == IO_SERVER_ID);
            Communications.RemoveAll(x => x.IO_SERVER_ID == IO_SERVER_ID);
            Devices.RemoveAll(x => x.IO_SERVER_ID == IO_SERVER_ID);
            BatchCommandTasks.RemoveAll(x => x.SERVER_ID == IO_SERVER_ID);
            MachineTrainingModels.RemoveAll(x => x.SERVER_ID == IO_SERVER_ID);
            Scada.Model.IO_SERVER newServer = serverBll.GetModel(IO_SERVER_ID);
            if (newServer != null)
            {
                Servers.Add(newServer);
                AddLog("采集站 重新加载" + IO_SERVER_ID + "工程....");
                List<Scada.Model.IO_COMMUNICATION> newCommunications = commBll.GetModelList(" IO_SERVER_ID='" + IO_SERVER_ID + "'");
                AddLog("采集站 读取该采集站下" + newCommunications.Count + "个通道....");
                List<Scada.Model.IO_DEVICE> newDevices = deviceBll.GetModelList(" IO_SERVER_ID='" + IO_SERVER_ID + "'");
                AddLog("采集站 读取该采集站下" + newDevices.Count + "网关设备....");
                Devices.AddRange(newDevices);
                for (int i = 0; i < newCommunications.Count; i++)
                {
                    newCommunications[i].Devices = newDevices.FindAll(x => x.IO_COMM_ID == newCommunications[i].IO_COMM_ID && x.IO_SERVER_ID == newCommunications[i].IO_SERVER_ID);
                    AddLog("采集站 读取该采集站下" + newCommunications[i].IO_COMM_NAME + "通道下" + newCommunications[i].Devices.Count + "个网关设备....");
                }
                Communications.AddRange(newCommunications);

            }
            AddLog("加载自动控制任务......");
            List<Scada.Model.BatchCommandTaskModel> batchCommandTaskModels = mBatchCommandTaskBll.GetModelList(" SERVER_ID='" + IO_SERVER_ID + "'");
               List<Scada.Model.BatchCommandTaskItemModel> mBatchCommandTaskItems = mBatchCommandTaskItemBll.GetModelList(" SERVER_ID='" + IO_SERVER_ID + "'");
            for (int i = 0; i < batchCommandTaskModels.Count; i++)
            {
                batchCommandTaskModels[i].Items = mBatchCommandTaskItems.FindAll(x => x.CommandTaskID == batchCommandTaskModels[i].Id && x.SERVER_ID == batchCommandTaskModels[i].SERVER_ID);
            }
            BatchCommandTasks.AddRange(batchCommandTaskModels);
            AddLog("加载自动控制任务已经完成!");
            AddLog("加载机器训练任务......");
             List<Scada.Model.ScadaMachineTrainingModel>  mMachineTrainingModels = mMachineTrainingBll.GetModelList(" SERVER_ID='"+ IO_SERVER_ID + "'");
            MachineTrainingModels.AddRange(mMachineTrainingModels);
            AddLog("加载机器训练任务已经完成");
            AddLog("采集站 重新加载指定采集站工程成功");
        }
        /// <summary>
        /// 卸载并重新加载资源
        /// </summary>
        /// <param name="IO_SERVER_ID"></param>
        /// <returns></returns>
        public void PublishReloadProject(string IO_SERVER_ID, string stationName)
        {
            TaskHelper.Factory.StartNew(() =>
            {
                IOCenterManager.PublishRestart = true;
             
                IOCenterManager.IOCenterServer.MDSServerStatus = MDSServerStatus.暂停;
                try
                {
                  
                      
                    AddLog(stationName + "采集站 重新加载IO工程....");
                    ReLoadProject(IO_SERVER_ID);
                    AddLog(stationName + "采集站 重新加载查询目录树....");
                   IOCenterManager.QueryFormManager.LoadQueryTreeProject(IO_SERVER_ID);

                    AddLog(stationName + "采集站 重新加载机器训练任务....");
                    IOCenterManager.QueryFormManager.LoadQueryMachineTreeTask(IO_SERVER_ID);

                    AddLog(stationName + "采集站 重新加载自动控制任务....");
                    IOCenterManager.QueryFormManager.LoadQueryBatchCommandTaskTreeTask(IO_SERVER_ID);
                    AddLog(stationName + "采集站 采集站工程发布成功....");
                    Scada.MDSCore.TcpData sendData = new TcpData();
                    sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "true" });
                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "采集站工程发布成功,正在重新初始化数据中心服务器，请耐心等待......" });
                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                    sendData.ChangedToBytes();
                    IOCenterManager.IOCenterClient.Send(sendData.Datas, IO_SERVER_ID, stationName, MDSCommandType.发布工程成功, ScadaClientType.IoManager);

                }
                catch (Exception ex)
                {
                    DisplyException(new Exception("更新服务器失败" + ex.Message));
                    TcpData sendData = new TcpData();
                    sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "true" });
                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "更新服务器失败" });
                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                    sendData.ChangedToBytes();
                    IOCenterManager.IOCenterClient.Send(sendData.Datas, IO_SERVER_ID, stationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);

                }
                IOCenterManager.IOCenterServer.MDSServerStatus = MDSServerStatus.运行;//暂停TCP服务
                IOCenterManager.PublishRestart = false;
            });
        }
        /// <summary>
        /// 异常信息在日志端显示
        /// </summary>
        /// <param name="ex"></param>
        public void DisplyException(Exception ex)
        {

            if (CenterServerException != null)
            {
                CenterServerException(ex.Message);
            }
            Scada.Logger.Logger.GetInstance().Debug(ex.Message);

        }
        public void AddLog(string log)
        {
            if (CenterServerLog != null)
            {
                CenterServerLog(log);
            }
            Scada.Logger.Logger.GetInstance().Debug(log);
        }
        public override void Dispose()
        {
            if (Servers != null)
            {
                Servers.Clear();
            }

            if (Communications != null)
            {
                Communications.Clear();
            }

            if (Devices != null)
            {
                Devices.Clear();
            }
            if (BatchCommandTasks != null)
                BatchCommandTasks.Clear();

            if (MachineTrainingModels != null)
                MachineTrainingModels.Clear();
            BatchCommandTasks = null;

            Communications = null;
            Devices = null;
            Servers = null;
            ServerConfig = null;
            base.Dispose();
        }
    }
}
