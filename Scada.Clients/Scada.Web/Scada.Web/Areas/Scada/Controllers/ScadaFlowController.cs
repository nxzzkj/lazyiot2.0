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
using Scada.DBUtility;
using System.Data;
using Microsoft.CSharp.RuntimeBinder;
using System.Runtime.CompilerServices;

namespace ScadaWeb.Web.Areas.Scada.Controllers
{
    public   class JsIOPara
    {
        public JsIOPara()
        {
            ServerID = "";
            CommunicateID = "";
            DeviceID = "";
            ParaID = "";
            DataType = "";
            Format = "";
            Unit = "";
            UpdateCycle = "120";
            IoName = "";
            Value = "";
            DateTime = "";
            Status = 0;
            QualityStamp = "BAD";
            ShapeID = "";
        }
        public string ShapeID
        {
            set; get;
        }
        public string IOStr
        {
            get { return ServerID + "," + CommunicateID + "," + DeviceID + "," + ParaID + "," + DataType + "," + Format + "," + Unit + "," + UpdateCycle + "," + IoName; }
        }
        public string IOEquipmentStr
        {
            get { return ServerID + "/" + CommunicateID + "/" + DeviceID + "/" + ParaID + "/" + IoName; }
        }
        public string ServerID
        {
            set; get;
        }
        public string CommunicateID
        {
            set; get;
        }
        public string DeviceID
        {
            set; get;
        }
        public string ParaID
        {
            set; get;
        }
        public string DataType
        {
            set;
            get;
        }
        public string Format
        {
            set;
            get;
        }
        public string Unit
        {
            set;
            get;
        }
        public string UpdateCycle
        {
            set; get;
        }
        public string IoName
        {
            set;
            get;
        }
        /// <summary>
        /// 对应获取的实时值
        /// </summary>
        public string Value
        {
            set;
            get;
        }
        /// <summary>
        /// 对应获取的数据时间戳
        /// </summary>
        public string DateTime
        {
            set;
            get;
        }
        public int Status
        {
            set;
            get;
        }
        public string QualityStamp
        {
            set;
            get;
        }

    }
    public   class JsEventContent
    {
        public string ShapeID
        {
            set; get;
        }
        public string Id
        {
            set; get;
        }
        /// <summary>
        /// 对应获取的实时值
        /// </summary>
        public string Value
        {
            set;
            get;
        }
        /// <summary>
        /// 对应获取的数据时间戳
        /// </summary>
        public string DateTime
        {
            set;
            get;
        }
    }
    public   class JsEventPara
    {
        public string ShapeID
        {
            set; get;
        }
        public JsEventPara()
        {
            ServerID = "";



        }

        public string ServerID
        {
            set; get;
        }

        private List<JsEventContent> mItems = new List<JsEventContent>();

        public List<JsEventContent> Items
        {
            set { mItems = value; }
            get { return mItems; }
        }


    }
    public   class JsStatusPara
    {
        public string ShapeID
        {
            set; get;
        }
        public JsStatusPara()
        {
            ServerID = "";
            CommunicateID = "";
            DeviceID = "";
            Value = "";


        }
        public string ServerID
        {
            set; get;
        }
        public string CommunicateID
        {
            set; get;
        }
        public string DeviceID
        {
            set; get;
        }

        public string Value
        { set; get; }

        public string DateTime { set; get; }

        public string IOStr
        {
            get { return ServerID + "," + CommunicateID + "," + DeviceID; }
        }


    }

    public   class JsMacineTrain
    {
        public JsMacineTrain()
        {
            ShapeType = "single";
            ShapeID = "";
        }
        public string ShapeID
        {
            set; get;
        }
        public string ShapeType
        {
            set; get;
        }
        #region Model

        private string _taskname;
        private string _algorithm;
        private string _algorithmtype;

        private int? _forecastpriod;
        public string DateTime { set; get; }

        private string _server_name;
        public string ForecastLabel { set; get; }
        public string ForecastInputValues { set; get; }
        public string ForecastIOParaNames { set; get; }
        public string ForecastScore { set; get; }

        public string ForecastDate { set; get; }
        /// <summary>
        /// 
        /// </summary>

        public string AlgorithmType
        {
            set { _algorithmtype = value; }
            get { return _algorithmtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskName
        {
            set { _taskname = value; }
            get { return _taskname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Algorithm
        {
            set { _algorithm = value; }
            get { return _algorithm; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? ForecastPriod
        {
            set { _forecastpriod = value; }
            get { return _forecastpriod; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string SERVER_NAME
        {
            set { _server_name = value; }
            get { return _server_name; }
        }
        public string COMM_NAME
        {
            set; get;
        } = "";
        public string DEVICE_NAME
        {
            set; get;
        } = "";

        #endregion Model
    }
    public class JsMachineTrainPara
    {
        public string ShapeType
        {
            set; get;
        }
        public string ShapeID
        {
            set; get;
        }
        public JsMachineTrainPara()
        {
            ServerID = "";
            CommunicateID = "";
            DeviceID = "";
            ShapeType = "single";
            ForecastPriod = 10;//默认是10分钟


        }
        public string TaskId
        { set; get; }

        public string ServerID
        {
            set; get;
        }
        public string CommunicateID
        {
            set; get;
        }
        public string DeviceID
        {
            set; get;
        }





        public virtual string IOStr
        {
            get { return ServerID + "," + CommunicateID + "," + DeviceID + "," + TaskId + "," + ForecastPriod; }
        }
        /// <summary>
        /// 预测周期
        /// </summary>
        public int ForecastPriod
        { set; get; }




    }
    public class JsMachineTrainResult : JsMachineTrainPara
    {
       
        public JsMachineTrainResult()
        {

            Train = null;



        }


        public JsMacineTrain Train
        { set; get; }







    }
    public sealed class JsIOAlarm
    {
        List<JsIOPara> mList = new List<JsIOPara>();
        public List<JsIOPara> List
        {
            set { mList = value; }
            get { return mList; }
        }
        private int mPageSize = 0;
        public int PageSize
        {
            set { mPageSize = value; }
            get { return mPageSize; }
        }
        private int mPageIndex = 0;
        public int PageIndex
        {
            set { mPageIndex = value; }
            get { return mPageIndex; }
        }
        public string ShapeID
        {
            set; get;
        }
    }
    /// <summary>
    /// 通用SCADA系统的控制模块
    /// </summary>
    public class ScadaFlowController : Controller
    {
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();
        public WebRealCache mWebRealCache=new  WebRealCache();  
        public IScadaFlowProjectService ProjectServer { get; set; }
        public IScadaFlowViewService ViewServer { get; set; }
        public   ActionResult Index(int? id)
        {

            string vid = Request["vid"];
            if (vid == null || vid.ToString().Trim() == "")
                vid = "";
            ScadaFlowModel model = new ScadaFlowModel();
            if (vid == "")
            {


                string para = Request.QueryString["id"].Split('?')[0];
                string[] idarray = Request.QueryString["id"].Split('?').Length >= 2 ? Request.QueryString["id"].Split('?')[1].Split('=') : null;
                string idstr = "0";
                if (idarray != null && idarray.Length >= 2)
                {
                    idstr = Request.QueryString["id"].Split('?')[1].Split('=')[1];
                }
                else
                {
                    idstr = id.ToString();
                }
           
                if (para != null && para != "")
                    id = int.Parse(para);
            }
            ScadaFlowProjectModel Project = ProjectServer.GetById(id.Value);
            if (Project != null && vid == "")
            {
                ScadaFlowViewModel view = ViewServer.GetByWhere(" where ProjectId='" + Project.ProjectId + "'").First();
                model.Project = Project;
                model.MainView = view;
            }
            else if (vid != "")
            {
                ScadaFlowViewModel view = ViewServer.GetByWhere(" where  ViewId='" + vid + "'").First();
                if (view != null)
                {
                    Project = ProjectServer.GetByWhere(" where ProjectId='" + view.ProjectId + "'").First();
                    model.Project = Project;
                    model.MainView = view;
                }

            }
            return View(model);
        }
        
    
        [HttpPost]
        public JsonResult GetReadData(List<JsIOPara> ioparas)
        {
            if (ioparas == null || ioparas.Count < 0)
                return null;

            List<JsIOPara> noCFioparas = new List<JsIOPara>();


            for (int i = ioparas.Count - 1; i >= 0; i--)
            {
                if (!noCFioparas.Exists(x => x.ServerID == ioparas[i].ServerID
                && x.CommunicateID == ioparas[i].CommunicateID && x.DeviceID == ioparas[i].DeviceID && x.ParaID == ioparas[i].ParaID&&x.DataType== ioparas[i].DataType))
                {
                    noCFioparas.Add(ioparas[i]);
                }
                if (string.IsNullOrWhiteSpace(ioparas.ElementAt(i).ServerID) || string.IsNullOrWhiteSpace(ioparas.ElementAt(i).DeviceID) || string.IsNullOrWhiteSpace(ioparas.ElementAt(i).ParaID) || string.IsNullOrWhiteSpace(ioparas.ElementAt(i).CommunicateID))
                {
                    ioparas.Remove(ioparas.ElementAt(i));
                }


            }
            List<WebCacheDataItem> inparas = new List<WebCacheDataItem>();
            noCFioparas.ForEach(delegate (JsIOPara p)
            {
                inparas.Add(new WebCacheDataItem()
                {
                    CommunicationId = p.CommunicateID,
                    DeviceId = p.DeviceID,
                    ServerId = p.ServerID,
                     

                });
            });



            ///删除重复项
            if (Configs.GetValue("SystemRunModel") == "Debug")
            {
              

                for (int i = 0; i < noCFioparas.Count; i++)
                {
                    noCFioparas[i].Value = random.Next(100).ToString();
                    noCFioparas[i].DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    noCFioparas[i].Status = 1;
                    noCFioparas[i].QualityStamp = "GOOD";

                }
                var result = Pager.Paging(noCFioparas, noCFioparas.Count);
                //读取以下10行的实时数据，从influxDB中读取
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                ///测试数据

                if (noCFioparas != null)
                {
                    //读取缓存的历史数据
                    var cacheResult = mWebRealCache.GetReals(inparas);
                    cacheResult.ForEach(delegate (RealWebCacheDataItem real)
                    {
                        var finders = noCFioparas.FindAll(x => x.ServerID == real.ServerId && x.CommunicateID == real.CommunicationId && x.DeviceID == real.DeviceId);
                        var cacheFinder = cacheResult.Find(x => x.ServerId == real.ServerId && x.CommunicationId == real.CommunicationId && x.DeviceId == real.DeviceId);
                        if (finders != null && finders.Count > 0 && cacheFinder != null)
                        {
                            finders.ForEach(delegate (JsIOPara para)
                            {
                                var realData = cacheFinder.ParaReals.Find(x => x.ParaID == para.ParaID);
                                if (realData != null)
                                {
                                    para.DateTime = realData.Date;
                                    para.QualityStamp = realData.QualityStamp;
                                    para.Value = realData.ParaValue;
                                    DateTime dt;
                                    if (DateTime.TryParse(para.DateTime, out dt))
                                    {
                                        para.DateTime = Convert.ToDateTime(para.DateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    else
                                    {
                                        para.QualityStamp = "BAD";
                                    }
                                    if (para.Value == "" || para.Value == "-9999")
                                    {
                                        para.QualityStamp = "BAD";

                                    }
                                    para.QualityStamp = "GOOD";

                                }



                            });


                        }
                    });
                    var result = Pager.Paging(noCFioparas, noCFioparas.Count);
                    //读取以下10行的实时数据，从influxDB中读取
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = Pager.Paging(new List<JsIOPara>(),0);
                    //读取以下10行的实时数据，从influxDB中读取
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

        }
        /// <summary>
        /// 获取系统事件
        /// </summary>
        /// <param name="ioparas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetReadEvent(List<JsEventPara> IoEventParas)
        {
            List<JsEventPara> results = new List<JsEventPara>();
            if (IoEventParas != null)
            {
                ///删除重复项

                for (int i = IoEventParas.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(IoEventParas.ElementAt(i).ServerID) || string.IsNullOrWhiteSpace(IoEventParas.ElementAt(i).ServerID))
                    {
                        IoEventParas.Remove(IoEventParas.ElementAt(i));
                    }

                }
                //读取所有的实时数据，并加载 mWebInfluxDbManager
                List<InfluxDBQueryPara> services = new List<InfluxDBQueryPara>();
                foreach (JsEventPara res in IoEventParas)
                {
                    InfluxDBQueryPara data = new InfluxDBQueryPara();
                    data.IOServerID = res.ServerID;
                    if (!services.Exists(x => x.IOServerID == data.IOServerID))
                        services.Add(data);
                }


                IEnumerable<IEnumerable<Serie>> realResult = mWebInfluxDbManager.MultiQueryRealEvent(services.ToList(), 10);
                for (int i = 0; i < services.Count; i++)
                {
                    string server = services[i].IOServerID;
                    JsEventPara eventPara = IoEventParas.Find(x => x.ServerID == server);
                    try
                    {
                        eventPara.Items = new List<JsEventContent>();
                        var series = realResult.ElementAt(i);
                        foreach (Serie s in series)
                        {
                            JsEventContent eventContent = new JsEventContent();
                            eventContent.Value = "采集站:" + services[i];
                            eventContent.Id = GUIDToNormalID.GuidToLongID();
                            if (s != null)
                            {
                                int timeindex = s.Columns.IndexOf("time");
                                if (timeindex >= 0)
                                {
                                    eventContent.Value += " 时间:" + s.Values[i][timeindex].ToString();
                                    eventContent.DateTime = s.Values[i][timeindex].ToString();
                                    int valueindex = s.Columns.IndexOf("field_event");
                                    if (valueindex >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(s.Values[i][valueindex].ToString()))
                                            eventContent.Value += " 事件类型:" + s.Values[i][valueindex].ToString();

                                    }

                                    valueindex = -1;
                                    valueindex = s.Columns.IndexOf("field_comm_name");
                                    if (valueindex >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(s.Values[i][valueindex].ToString()))
                                            eventContent.Value += " 通道:" + s.Values[i][valueindex].ToString();

                                    }
                                    valueindex = -1;
                                    valueindex = s.Columns.IndexOf("field_io_name");
                                    if (valueindex >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(s.Values[i][valueindex].ToString()))
                                            eventContent.Value += " 设备:" + s.Values[i][valueindex].ToString();

                                    }

                                    valueindex = -1;
                                    valueindex = s.Columns.IndexOf("field_device_name");
                                    if (valueindex >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(s.Values[i][valueindex].ToString()))
                                            eventContent.Value += " IO点:" + s.Values[i][valueindex].ToString();

                                    }
                                    valueindex = -1;
                                    valueindex = s.Columns.IndexOf("field_content");
                                    if (valueindex >= 0)
                                    {
                                        if (!string.IsNullOrEmpty(s.Values[i][valueindex].ToString()))
                                            eventContent.Value += " 事件内容:" + s.Values[i][valueindex].ToString();

                                    }

                                }
                                eventPara.Items.Add(eventContent);

                            }

                        }

                    }
                    catch
                    {

                    }
                }
                if (results.Count <= 0)
                {
                    IoEventParas.ForEach(delegate (JsEventPara p)
                    {
                        p.Items = new List<JsEventContent>();
                        p.Items.Add(new JsEventContent()
                        {
                            ShapeID = p.ShapeID,
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Value = random.Next(1) == 0 ? "异常" : "正常"



                        });

                        results.Add(p);

                    });
                }
            }
            else
            {
                results = new List<JsEventPara>();
            }
            var result = Pager.Paging(results, results.Count);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
 
        [HttpPost]
        public JsonResult GetReadStatus(List<JsStatusPara> IoStatusParas)
        {
            List<JsStatusPara> noCFioparas = new List<JsStatusPara>();
           
            if (IoStatusParas != null)
            {
                ///删除重复项

                for (int i = IoStatusParas.Count - 1; i >= 0; i--)
                {
                    if (!noCFioparas.Exists(x => x.ServerID == IoStatusParas[i].ServerID
                  && x.CommunicateID == IoStatusParas[i].CommunicateID && x.DeviceID == IoStatusParas[i].DeviceID))
                    {
                        noCFioparas.Add(IoStatusParas.ElementAt(i));
                    }
                    if (string.IsNullOrWhiteSpace(IoStatusParas.ElementAt(i).ServerID)
                        || string.IsNullOrWhiteSpace(IoStatusParas.ElementAt(i).CommunicateID)
                        || string.IsNullOrWhiteSpace(IoStatusParas.ElementAt(i).DeviceID))
                    {
                        
                        IoStatusParas.Remove(IoStatusParas.ElementAt(i));
                    }

                }
                List<WebCacheDataItem> inparas = new List<WebCacheDataItem>();
                noCFioparas.ForEach(delegate (JsStatusPara p)
                {
                    inparas.Add(new WebCacheDataItem()
                    {
                        CommunicationId = p.CommunicateID,
                        DeviceId = p.DeviceID,
                        ServerId = p.ServerID,


                    });
                });


                var cacheResult = mWebRealCache.GetStatus(inparas);
                cacheResult.ForEach(delegate (StatusWebCacheDataItem real)
                {
                    var finder = noCFioparas.Find(x => x.ServerID == real.ServerId && x.CommunicateID == real.CommunicationId && x.DeviceID == real.DeviceId);
                    var cacheFinder = cacheResult.Find(x => x.ServerId == real.ServerId && x.CommunicationId == real.CommunicationId && x.DeviceId == real.DeviceId);
                    if (finder != null && cacheFinder != null)
                    {
                        finder.DateTime = real.DateTime;
                        finder.Value = (real.Status.ToLower() == "true" || real.Status == "1") ? "通讯正常" : "通讯异常";

                    }
                });

            }
            
            var result = Pager.Paging(noCFioparas, noCFioparas.Count);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        Random random = new Random();
        /// <summary>
        /// 获取机器训练预测数据
        /// </summary>
        /// <param name="ioparas"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult GetReadMachineTrain(List<JsMachineTrainPara> IoMachineTrainParas)
        {
            List<JsMachineTrainPara> noCFioparas = new List<JsMachineTrainPara>();
            if (IoMachineTrainParas != null)
            {
                ///删除重复项

                for (int i = IoMachineTrainParas.Count - 1; i >= 0; i--)
                {
                    if (!noCFioparas.Exists(x => x.ServerID == IoMachineTrainParas[i].ServerID&& x.TaskId == IoMachineTrainParas[i].TaskId
                  && x.CommunicateID == IoMachineTrainParas[i].CommunicateID && x.DeviceID == IoMachineTrainParas[i].DeviceID))
                    {
                        noCFioparas.Add(IoMachineTrainParas.ElementAt(i));
                    }
                    if (string.IsNullOrWhiteSpace(IoMachineTrainParas.ElementAt(i).ServerID)
                        || string.IsNullOrWhiteSpace(IoMachineTrainParas.ElementAt(i).CommunicateID)
                        || string.IsNullOrWhiteSpace(IoMachineTrainParas.ElementAt(i).DeviceID)
                        || string.IsNullOrWhiteSpace(IoMachineTrainParas.ElementAt(i).TaskId))
                    {
                        IoMachineTrainParas.Remove(IoMachineTrainParas.ElementAt(i));
                    }

                }

                List<MachineTrainForecastWebCacheDataItem> inparas = new List<MachineTrainForecastWebCacheDataItem>();
                noCFioparas.ForEach(delegate (JsMachineTrainPara p)
                {
                    inparas.Add(new MachineTrainForecastWebCacheDataItem()
                    {
                        CommunicationId = p.CommunicateID,
                        DeviceId = p.DeviceID,
                        ServerId = p.ServerID,
                        TaskId=p.TaskId
                      
                    });
                });


                var cacheResult = mWebRealCache.GetTrainForecast(inparas);

                List<JsMachineTrainResult> outresults = new List<JsMachineTrainResult>();
 
                for (int i = 0; i < cacheResult.Count; i++)
                {
                    var s = cacheResult.ElementAt(i);
                    string server = s.ServerId;
                    string commid = s.CommunicationId;
                    string deviceid = s.DeviceId;
                    string taskid = s.TaskId;
                    JsMachineTrainPara forecastPara = noCFioparas.Find(x => x.ServerID == server
                    && x.CommunicateID == commid
                    && x.DeviceID == deviceid&&x.TaskId== taskid);
                    if (forecastPara != null)
                    {

                        JsMachineTrainResult res = new JsMachineTrainResult();
                        res.CommunicateID = s.CommunicationId;
                        res.DeviceID = s.DeviceId;
                        res.ServerID = s.ServerId;
                        res.TaskId = s.TaskId;
                        res.ShapeID = forecastPara.ShapeID;
                        res.ShapeType = forecastPara.ShapeType;
                        res.ForecastPriod = forecastPara.ForecastPriod;
                        res.Train = new JsMacineTrain();
                        res.Train.DateTime = s.PredictedDate.ToString("yyyy-MM-dd HH:mm:ss");
                        res.Train.Algorithm = s.Algorithm;
                        res.Train.AlgorithmType = s.AlgorithmType;
                        res.Train.COMM_NAME = s.COMM_NAME;
                        res.Train.DEVICE_NAME = s.DEVICE_NAME;
                        res.Train.ForecastDate = s.PredictedDate.ToString("yyyy-MM-dd HH:mm:ss");
                        res.Train.ForecastLabel = s.PredictedLabel;
                        res.Train.ForecastInputValues = s.FeaturesValue;
                        res.Train.ForecastIOParaNames = s.FeaturesName;
                        res.Train.ForecastScore = s.Score;
                        res.Train.SERVER_NAME = s.SERVER_NAME;
                        res.Train.TaskName = s.TaskName;
                        outresults.Add(res);
                    }

                    var result = Pager.Paging(outresults, noCFioparas.Count);
                    //读取以下10行的实时数据，从influxDB中读取
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
              
            }
         
            //读取以下10行的实时数据，从influxDB中读取
            return Json(noCFioparas, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取实时报警数据
        /// </summary>
        /// <param name="alarms"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetReadAlarm(JsIOAlarm alarm)
        {
            List<JsIOPara> alarms = alarm.List;
            int pagesize = alarm.PageSize;
            int pageindex = alarm.PageIndex;
            List<AlarmWebCacheDataItem> inputAlarm = new List<AlarmWebCacheDataItem>();

            List<ScadaGreneralAlarmModel> results = new List<ScadaGreneralAlarmModel>();

            ///删除重复项

            for (int i = alarms.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(alarms.ElementAt(i).ServerID) || string.IsNullOrWhiteSpace(alarms.ElementAt(i).DeviceID) || string.IsNullOrWhiteSpace(alarms.ElementAt(i).ParaID) || string.IsNullOrWhiteSpace(alarms.ElementAt(i).CommunicateID))
                {
                    alarms.Remove(alarms.ElementAt(i));
                }
            }

            //读取所有的实时数据，并加载 mWebInfluxDbManager


            foreach (JsIOPara res in alarms)
            {
                if (!inputAlarm.Exists(x => x.DeviceId == res.DeviceID && x.ParaId == res.ParaID))
                {

                    inputAlarm.Add(new AlarmWebCacheDataItem
                    {
                        ServerId = res.ServerID,
                        CommunicationId = res.CommunicateID,
                        DeviceId = res.DeviceID,
                        ParaId = res.ParaID,
                        ParaName = res.IoName

                    });
                }
            }
            var realResult = mWebRealCache.GetAlarms(inputAlarm);
            if (realResult != null && realResult.Count() > 0)
            {

                for (int i = 0; i < realResult.Count; i++)
                {
                    AlarmWebCacheDataItem s = realResult[i];
                    ScadaGreneralAlarmModel mymodel = new ScadaGreneralAlarmModel();
                    mymodel.time = s.AlarmTime;
                    mymodel.IO_ALARM_DATE = s.AlarmTime;
                    mymodel.IO_ALARM_DISPOSALIDEA = s.Disposalidea;
                    mymodel.IO_ALARM_DISPOSALUSER = s.Disposaluser;
                    mymodel.IO_ALARM_LEVEL = s.Alarmlevel;
                    mymodel.IO_ALARM_TYPE = s.AlarmType;
                    mymodel.IO_ALARM_VALUE = s.AlarmValue;
                    mymodel.IO_NAME = s.ParaName;
                    mymodel.IO_DEVICE_ID = s.DeviceId;
                    mymodel.IO_COMMUNICATE_ID = s.CommunicationId;
                    mymodel.IO_SERVER_ID = s.ServerId;
                    mymodel.IO_ID = s.ParaId;
                    mymodel.DEVICE_NAME = s.DeviceName;
                    results.Add(mymodel);
                }
            }
            var result = Pager.Paging(results, results.Count);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDBMultiDBValues(SCADAFlow_MultiDataBaseView obj, PageInfo pageInfo)
        {
            try
            {
                if (obj.Connection != null)
                {
                    DataSet set = new ScadaDBHelper(obj.Connection).Query(obj.SqlString);
                    object obj2 = Pager.ScadaDataTablePaging(set.Tables[0].ToJson(), (long)set.Tables[0].Rows.Count, base.Request["elementId"].Trim());
                    
                    return Json( obj2, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetDBSingleValues(SCADAFlow_SingleDataBaseValue dbvalue)
        {
            try
            {
                if (dbvalue.Connection != null)
                {
                    SCADAFlow_SingleDataBaseValue value2 = dbvalue;
                    DataSet set = new ScadaDBHelper(value2.Connection).Query(value2.SqlString);
                    object obj2 = Pager.ScadaDataTablePaging(set.Tables[0].ToJson(), (long)set.Tables[0].Rows.Count, base.Request["elementId"].Trim());

                    return Json(obj2, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                return base.Json(exception.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}