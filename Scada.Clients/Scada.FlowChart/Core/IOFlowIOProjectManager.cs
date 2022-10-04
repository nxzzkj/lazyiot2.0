
using Scada.FlowGraphEngine.GraphicsCusControl;
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
namespace ScadaFlowDesign.Core
{
    public class IOFlowIOProjectManager
    {


        //当前服务器加载的采集站
        public   Scada.Model.IO_SERVER IOServer = null;
        //当前服务器加载的通道
        public   List<Scada.Model.IO_COMMUNICATION> IOCommunications = null;
        //当前服务器加载的设备
        public   List<Scada.Model.IO_DEVICE> IODevices = null;
        public List<Scada.Model.ScadaMachineTrainingModel> MachineTrains = null;
        public List<Scada.Model.BatchCommandTaskModel> BatchCommandTasks = null;
        public   int ProgressMaxNum = 0;
        private   string mServerID = "";
        public   event FlowDesignException OnFlowExceptionHanped;
        public   event FlowDesignLogger OnFlowDesignLogger;
        #region 异常处理，统一都输出到主任何界面
        private     void AddLogToMainLog(string msg)
        {
            if (OnFlowDesignLogger != null)
            {
                OnFlowDesignLogger(msg);
            }

        }
        /// <summary>
        /// 异常信息在日志端显示
        /// </summary>
        /// <param name="ex"></param>
        private   void ThrowExceptionToMain(Exception ex)
        {
            if (OnFlowExceptionHanped != null)
            {
                OnFlowExceptionHanped(ex);
            }

        }


        #endregion
        public   string ServerID
        {
            get
            {
                if (mServerID == "")
                {
                    DbHelperSQLite.connectionString = "Data Source=" + Application.StartupPath + "\\IOProject\\Station.station";
                    IO_SERVER serverBll = new IO_SERVER();
                    AddLogToMainLog("读取采集站信息......");
                    IOServer = serverBll.GetModel();
                    mServerID = IOServer.SERVER_ID;
                    return mServerID;
                }
                return mServerID;
            }
        }

        public void InitBaseModel()
        {
            Task.Run(() =>
            {
                try
                {
                    DbHelperSQLite.connectionString = "Data Source=" + Application.StartupPath + "\\IOProject\\Station.station";
                    IO_SERVER serverBll = new IO_SERVER();
                    AddLogToMainLog("读取采集站信息......");
                    IOServer = serverBll.GetModel(ComputerInfo.GetInstall().ServerID);
                    if (IOServer == null)
                        return;
                    mServerID = IOServer.SERVER_ID;
                    //加载通道
                    AddLogToMainLog("读取采集站通道信息......");
                    IO_COMMUNICATION commBll = new IO_COMMUNICATION();
                    IOCommunications = commBll.GetModelList(" IO_SERVER_ID='" + IOServer.SERVER_ID + "'");
                    AddLogToMainLog("读取采集站通道下的所有设备信息......");
                    IO_DEVICE deviceBll = new IO_DEVICE();
                    IODevices = deviceBll.GetModelList(" IO_SERVER_ID='" + IOServer.SERVER_ID + "'");
                    AddLogToMainLog("读取该采集站下机器训练任务......");
                    ScadaMachineTrainingModel trainBll = new ScadaMachineTrainingModel();
                    MachineTrains = trainBll.GetModelList(" SERVER_ID='" + IOServer.SERVER_ID + "'");
                    BatchCommandTaskModel batchCommandTaskBll = new BatchCommandTaskModel();
                    BatchCommandTaskItemModel batchCommandItemBll = new BatchCommandTaskItemModel();
                    BatchCommandTasks = batchCommandTaskBll.GetModelList(" SERVER_ID='" + IOServer.SERVER_ID + "'");
                    List<Scada.Model.BatchCommandTaskItemModel> commandItems= batchCommandItemBll.GetModelList(" SERVER_ID='" + IOServer.SERVER_ID + "'");
                   
                    AddLogToMainLog("数据处理中.....");
                    for (int i = 0; i < BatchCommandTasks.Count; i++)
                    {
                        BatchCommandTasks[i].Items = commandItems.FindAll(x => x.CommandTaskID == BatchCommandTasks[i].Id && x.SERVER_ID == BatchCommandTasks[i].SERVER_ID);
                    }
                    for (int i = 0; i < IOCommunications.Count; i++)
                    {
                        AddLogToMainLog("处理 " + IOCommunications[i].IO_COMM_NAME + "[" + IOCommunications[i].IO_COMM_LABEL + "]");
                        IOCommunications[i].Devices = IODevices.FindAll(x => x.IO_COMM_ID == IOCommunications[i].IO_COMM_ID && x.IO_SERVER_ID == IOCommunications[i].IO_SERVER_ID);


                    }
                    FlowGraphEngineProject.IOServer = IOServer;
                    FlowGraphEngineProject.IOCommunications = IOCommunications;
                    FlowGraphEngineProject.IODevices = IODevices;

                    FlowGraphEngineProject.MachineTrains = MachineTrains;
                    FlowGraphEngineProject.BatchCommandTasks = BatchCommandTasks;
                    AddLogToMainLog("正在创建驱动.....");

                    AddLogToMainLog("读取工程完成!");
                    ProgressMaxNum = IOCommunications.Count + IODevices.Count;
                   
                }
                catch (Exception ex)
                {
                    ThrowExceptionToMain(ex);
                }
            });

        }
    }
}
