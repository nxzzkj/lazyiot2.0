using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScadaWeb.Common;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.Web.Controllers;
using Temporal.WebDbAPI;
using Temporal.Net.InfluxDb.Models.Responses;
using System.Collections;
using ScadaWeb.Web.Areas.Scada.Models;
using System.Reflection;
using ScadaWeb.Service;
using System.Dynamic;
using Newtonsoft.Json;
using ScadaWeb.Web.Areas.Permissions.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.Net;
using System.Net.WebSockets;
using System.Net.Sockets;
using System.Threading;

using System.Threading.Tasks;
using Scada.MDSCore;
using Scada.DBUtility;
using Scada.MDSCore.Client;
using Scada.MDSCore.Communication.Messages;
using Scada.MDSCore.Settings;

namespace ScadaWeb.Web.Areas.Scada.Controllers
{
    /// <summary>
    /// 通用SCADA系统系统的控制模块
    /// </summary>
    public class ScadaDialogController : BaseController
    {
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();
        public IIO_ParaService ParaBll { set; get; }

        public IScadaFlowProjectService ProjectServer { get; set; }
        public IScadaFlowViewService ViewServer { get; set; }
       
       
        public override ActionResult Index(long? id)
        {
            
            ////////////////////////////
            string vid = Request["vid"];
            if (vid == null || vid.ToString().Trim() == "")
                vid = "";
            ScadaFlowModel model = new ScadaFlowModel();
            if (vid == "")
            {
         

                string para = Request.QueryString["id"].Split('?')[0];
                string idstr = Request.QueryString["id"].Split('?')[1].Split('=')[1];
                base.Index(int.Parse(idstr));
                if (para != null && para != "")
                    id = int.Parse(para);
            }
            ScadaFlowProjectModel Project = ProjectServer.GetById(id.Value);
            if (Project != null && vid == "")
            {
                ScadaFlowViewModel view = ViewServer.GetByWhere(" where ProjectId='" + Project.ProjectId + "'").First();
                model.Project = Project;
                model.MainView = view;
                base.Index(Project.Id);
            }
            else if (vid != "")
            {
                ScadaFlowViewModel view = ViewServer.GetByWhere(" where  ViewId='" + vid + "'").First();
                if (view != null)
                {
                    Project = ProjectServer.GetByWhere(" where ProjectId='" + view.ProjectId + "'").First();
                    model.Project = Project;
                    model.MainView = view;
                    base.Index(Project.Id);
                }

            }
            return View(model);
        }
        #region 下置命令相关
        /// <summary>
        /// 发送一个下置命令消息到消息列表
        /// </summary>
        /// <param name="command"></param>
        private bool SendNetMQ(IO_COMMANDS command)
        {
            bool res = false;
            try
            {
                MDSConfig MDSServerConfig = MDSManager.WebCleintConfig;
                MDSClient serviceConsumer = MDSManager.WebClient;
               if (serviceConsumer!=null)
                {
                    byte[] commandbytes = Encoding.UTF8.GetBytes(command.ToString());
                    try
                    {
                        //首先将数据发送到数据中心，让数据中心知道web端向监视器发送了下置命令
                        //如果正常发送数据成功，则不会出现异常报错,否则会出现异常报错
                        //此处只是通知数据中心有数据下置，并不执行任何操作
                        var message = serviceConsumer.CreateMessage();
                     
                        message.MessageData = SplitPackage.AssembleBytes(commandbytes, command.IO_SERVER_ID, MDSCommandType.下置命令, ScadaClientType.WebSystem);
                        message.DestinationApplicationName = MDSServerConfig.CenterAppName;//首先将命令发送到数据中心，由数据中心统一将命令发送到每个对应的采集站
                        message.TransmitRule = MessageTransmitRules.StoreAndForward;
                        message.Send(10000);//默认超时是10秒


                    }
                    catch (Exception emx)
                    {

                    }
                    try
                    {
                       //此处要知道是向那个采集站发送的
                        var message = serviceConsumer.CreateMessage();
                        MDSSettings mDSSettings = new MDSSettings(Server.MapPath("ScadaCenterServer") + "/MDSSettings.xml");
                        IOStationInfoItem station = mDSSettings.Stations.Find(x => x.PhysicalMAC.Trim().ToLower() == command.IO_SERVER_ID.Trim().ToLower());
                        if(station!=null)
                        {
                            message.MessageData = SplitPackage.AssembleBytes(commandbytes, command.IO_SERVER_ID, MDSCommandType.下置命令, ScadaClientType.WebSystem);
                            message.DestinationApplicationName = MDSServerConfig.MonitorAppPrefix + "_" + station.PhysicalAddress.Replace(".","").Replace("。","");//首先将命令发送到数据中心，由数据中心统一将命令发送到每个对应的采集站
                            message.TransmitRule = MessageTransmitRules.StoreAndForward;
                            var response = message.SendAndGetResponse(10000);//默认超时是10秒
                            response.Acknowledge();//确认返回的消息
                                                   //解析返回的执行结果
                            TcpData tcpData = new TcpData();
                            tcpData.BytesToTcpItem(SplitPackage.RemoveIdentificationBytes(response.MessageData).Datas);
                            string IO_COMM_ID = tcpData.GetItemValue("IO_COMM_ID");
                            string IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                            string COMMAND_RESULT = tcpData.GetItemValue("COMMAND_RESULT");
                            if (IO_COMM_ID == command.IO_COMM_ID && IO_SERVER_ID == command.IO_SERVER_ID)
                            {
                                if (COMMAND_RESULT.ToUpper() == "TRUE")
                                    res = true;
                            }
                           
                        }
                    }
                    catch (Exception emx)
                    {

                    }

                    finally
                    {
                        
                    }
                    return res;


                }
            }
            catch
            {
                return false;
            }
            return res;

        }
        /// <summary>
        /// 用户下置命令
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCommand(WellCommandModel model)
        {
            
            bool res = false;
           string    msg = "";
            if (model.io.Trim() == "")
                return null;
            IOParaModel para = ParaBll.GetByWhere(" where IO_SERVER_ID='"+ model.io.Split(',')[0] + "' and IO_COMM_ID='"+ model.io.Split(',')[1] + "' and IO_DEVICE_ID='"+ model.io.Split(',')[2] + "' and IO_ID='"+ model.io.Split(',')[3] + "'  ").First();
            IO_COMMANDS command = new IO_COMMANDS();
            command.COMMAND_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            command.IO_SERVER_ID = model.io.Split(',')[0].Trim();
            command.IO_COMM_ID = model.io.Split(',')[1].Trim();
            command.IO_DEVICE_ID = model.io.Split(',')[2].Trim();
            command.IO_ID = model.io.Split(',')[3].Trim();
            if (Operator == null)
            {
                command.COMMAND_SEND_USER = "WebSystem";
                command.COMMAND_SEND_USERNAME = "WebSystem";
            }
            else
            {
                command.COMMAND_SEND_USER = base.Operator.UserId.ToString();
                command.COMMAND_SEND_USERNAME = base.Operator.Account;
            }
      
  
            if (para != null)
            {
                try
                {
                    if (SendNetMQ(command))
                    {

                        res = true;
                        msg = res ? "命令下置成功" : "下置命令失败";
                        command.IO_NAME = para.IO_NAME;
                        command.IO_LABEL = para.IO_LABEL;
                        command.COMMAND_ID = Guid.NewGuid().ToString();
                        command.COMMAND_RESULT = res.ToString().ToLower();
                        command.COMMAND_USER = "";
                        command.COMMAND_VALUE = model.writevalue;

                        //写入下置命令日志
                     
                    }
                    else
                    {
                        res = false;
                        msg = res ? "命令下置成功" : "下置命令失败";
                        command.IO_NAME = para.IO_NAME;
                        command.IO_LABEL = para.IO_LABEL;
                        command.COMMAND_ID = Guid.NewGuid().ToString();
                        command.COMMAND_RESULT = res.ToString().ToLower();
                        command.COMMAND_USER = "";
                        command.COMMAND_VALUE = model.writevalue;
                      

                    }
                    //保存命令下置结果
                    mWebInfluxDbManager.DbWrite_CommandPoints(new List<IO_COMMANDS> { command }, DateTime.Now).GetAwaiter();
                }
                catch(Exception emx)
                {
                    res = false;
                    msg = "命令下置失败,"+emx.Message;

                }
            }
            var result = new
            {
                result = res,
                msg = msg

            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion

    }
}