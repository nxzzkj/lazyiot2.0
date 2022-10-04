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
using Newtonsoft.Json;
using Scada.DBUtility;

namespace ScadaWeb.Web.Areas.Scada.Controllers
{
    public class ScadaAlarmController : BaseController
    {
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();

        public IWellService WellService { get; set; }
        public IIO_ServerService IO_Server { get; set; }
        public IIO_CommunicateService IO_CommunicateServer { get; set; }
        public IWellOrganizeService WellOrganizeServer { get; set; }
        public IIO_DeviceService IO_DeviceServer { get; set; }
        public IIO_ParaService IO_ParaServer { get; set; }
        public IOrganizeService OrganizeService { set; get; }
        public ISerieConfigService SerieServer
        {
            set;
            get;
        }
  
  
 
  
        public WebRealCache mWebRealCache = new WebRealCache();



        public ActionResult GeneralAlarm()
        {
            ScadaGreneralAlarmModel model = new ScadaGreneralAlarmModel();
            var paras = IO_ParaServer.GetAll();
            List<string> keys = new List<string>();
            foreach (var p in paras)
            {
                keys.Add(p.IO_SERVER_ID + "," + p.IO_COMM_ID + "," + p.IO_DEVICE_ID + "," + p.IO_ID + "," + p.IO_NAME + "," + (string.IsNullOrEmpty(p.IO_LABEL)? p.IO_NAME : p.IO_LABEL));

            }
            Session["AllAlarmKey"] = keys;
            return View(model);
        }
        public ActionResult GeneralHistoryAlarm()
        {
            ScadaGreneralAlarmModel model = new ScadaGreneralAlarmModel();
     
            return View(model);
        }
         
    
   
        [HttpGet]
        /// <summary>
        /// 读取通用实时报警
        /// </summary>
        /// <returns></returns>
        public JsonResult GeneralRealAlarm(ScadaGreneralAlarmModel model, PageInfo pageInfo)
        {
          
            var objs =(List<string>)Session["AllAlarmKey"];
            
            List<ScadaGreneralAlarmModel> alarms = new List<ScadaGreneralAlarmModel>();
            List<AlarmWebCacheDataItem> realResult = null;
            List<AlarmWebCacheDataItem> alarmCacheDatas = new List<AlarmWebCacheDataItem>();
            if (objs != null)
            {
                foreach (var item in objs)
                {
                    string[] strs = item.Split(',');
                    if (!string.IsNullOrEmpty(model.SERVER_ID))
                    {
                        if (strs[0].Trim() != model.SERVER_ID.Trim())
                        {
                            continue;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.COMM_ID))
                    {
                        if (strs[1] != model.COMM_ID.Trim())
                        {
                            continue;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.DEVICE_ID))
                    {
                        if (strs[2] != model.DEVICE_ID.Trim())
                        {
                            continue;
                        }
                    }
                    alarmCacheDatas.Add(new AlarmWebCacheDataItem
                    {
                        ServerId = strs[0],
                        CommunicationId = strs[1],
                        DeviceId = strs[2],
                        ParaLabel = strs[5],
                        ParaName = strs[4],
                        ParaId = strs[3]

                    });

                }
            }
            realResult = mWebRealCache.GetAlarms(alarmCacheDatas, model.IO_ALARM_TYPE, model.IO_ALARM_LEVEL);
            if (realResult != null && realResult.Count > 0)
            {

                for (int i = 0; i < realResult.Count; i++)
                {

                    AlarmWebCacheDataItem s = realResult[i];
                    if (DateTime.Now.AddHours(-6) <= Convert.ToDateTime(s.AlarmTime))
                    {
                        if(!string.IsNullOrEmpty(model.SERVER_ID))
                        {
                            if (s.ServerId.Trim()!= model.SERVER_ID.Trim())
                            {
                                continue;
                            }
                        }
                        if (!string.IsNullOrEmpty(model.COMM_ID))
                        {
                            if (s.CommunicationId.Trim() != model.COMM_ID.Trim())
                            {
                                continue;
                            }
                        }
                        if (!string.IsNullOrEmpty(model.DEVICE_ID))
                        {
                            if (s.DeviceId.Trim() != model.DEVICE_ID.Trim())
                            {
                                continue;
                            }
                        }
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
                        alarms.Add(mymodel);
                    }

                }

            }

            var result = Pager.Paging(alarms, alarms.Count);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        /// <summary>
        /// 读取通用历史报警
        /// </summary>
        /// <returns></returns>
        public JsonResult GeneralQueryHistoryAlarm(GeneralHistoryAlarmFormModel model, PageInfo pageInfo)
        {
            if (string.IsNullOrWhiteSpace(model.DeviceID))
                model.DeviceID = "";
            if (string.IsNullOrWhiteSpace(model.ServerID))
                model.ServerID = "";
            if (string.IsNullOrWhiteSpace(model.CommunicateID))
                model.ServerID = "";
 
           
            List<ScadaGreneralAlarmModel> alarms = new List<ScadaGreneralAlarmModel>();
            InfluxDBHistoryResult realResult = null;
            
     
            realResult = mWebInfluxDbManager.DbQuery_Alarms(model.ServerID, model.CommunicateID,model.DeviceID,Convert.ToDateTime( model.StartDate), Convert.ToDateTime(model.EndDate),model.IO_ALARM_TYPE, model.IO_ALARM_LEVEL, pageInfo.limit, pageInfo.page);


            if (realResult != null && realResult.Seres.Count() > 0)
            {
                var s = realResult.Seres.First();
                for (int i = 0; i < s.Values.Count; i++)
                {
                    ScadaGreneralAlarmModel mymodel = new ScadaGreneralAlarmModel();

                    int index = s.Columns.IndexOf("time");

                    object time = s.Values[i][index];
                    mymodel.time = time != null ? time.ToString() : "";



                    index = -1;
                    index = s.Columns.IndexOf("field_io_alarm_date");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ALARM_DATE = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_alarm_disposalidea");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ALARM_DISPOSALIDEA = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_alarm_disposaluser");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ALARM_DISPOSALUSER = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_alarm_level");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ALARM_LEVEL = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_alarm_type");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ALARM_TYPE = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_alarm_value");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ALARM_VALUE = v != null ? v.ToString() : "";
                    }




                    index = -1;
                    index = s.Columns.IndexOf("field_io_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_NAME = v != null ? v.ToString() : "";
                    }


                    index = -1;
                    index = s.Columns.IndexOf("tag_did");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_DEVICE_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_cid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_COMMUNICATE_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_sid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_SERVER_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_ioid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ID = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_device_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DEVICE_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_label");

                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_LABEL = v != null ? v.ToString() : "";
                    }

                  

                    alarms.Add(mymodel);
                }

            }

            var result = Pager.Paging(alarms, realResult.RecordCount);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
    }
}