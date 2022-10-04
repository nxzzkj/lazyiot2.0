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
    public class ScadaEventController : BaseController
    {
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();
 
        public IIO_ServerService IO_Server { get; set; }
        public IIO_CommunicateService IO_CommunicateServer { get; set; }
  
        public IIO_DeviceService IO_DeviceServer { get; set; }
        public IIO_ParaService IO_ParaServer { get; set; }
        public IOrganizeService OrganizeService { set; get; }
        public ISerieConfigService SerieServer
        {
            set;
            get;
        }
        public IDeviceGroupService DeviceGroupService { set; get; }
        public IScadaGroupService GroupService { get; set; }
        /// <summary>
        /// 默认首页是实时事件监控界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  ActionResult GeneralEvent(int? id)
        {
            ScadaEventModel model = new ScadaEventModel();
       
            base.Index(id);
            return View(model);

        }
        public ActionResult GeneralHistoryEvent(int? id)
        {
            ScadaEventModel model = new ScadaEventModel();
 
            base.Index(id);
            return View(model);
        }

    

 
        [HttpGet]
        public JsonResult GetGroupDevicePara(int id,string serverid,string communicateid,string deviceid)
        {
 

            
            DeviceGroupModel deviceModel = DeviceGroupService.GetById(id);
            List<SelectOption> _select = new List<SelectOption>();


            if (deviceModel != null )
            {

                string[] titles = deviceModel.IOPARATITLES.Split(',');
                string[] ionames = deviceModel.IOPARANAMES.Split(',');
                string[] ioids = deviceModel.IOPARAS.Split(',');
                for (int i=0;i< titles.Length;i++)
                {
                    SelectOption _option = new SelectOption
                    {
                        id = ioids[i],
                        name = titles[i],
                        value = ioids[i],
                        value1 ="/"+ deviceModel.GroupId+"/" + deviceModel.IO_SERVER_ID+"/" + deviceModel.IO_COMM_ID+"/"+ deviceModel.IO_DEVICE_ID+"/"+ ioids[i]+"/"+ deviceModel.UpdateCycle,
                    };
                    _select.Add(_option);
                }
                
            }
            
            return Json(_select, JsonRequestBehavior.AllowGet);





        }
        [HttpGet]
        /// <summary>
        /// 读取通用实时报警
        /// </summary>
        /// <returns></returns>
        public JsonResult GeneralRealEvent(ScadaEventModel model, PageInfo pageInfo)
        {
        
    
            List<ScadaEventModel> events = new List<ScadaEventModel>();
            InfluxDBHistoryResult realResult = null;

 
            realResult = mWebInfluxDbManager.DbQuery_Events("6h",  model.Event, model.SERVER_ID, model.COMM_ID, model.DEVICE_ID, pageInfo.limit, pageInfo.page);

            if (realResult != null && realResult.Seres !=null&& realResult.Seres.Count() > 0)
            {
                var s = realResult.Seres.First();
                for (int i = 0; i < s.Values.Count; i++)
                {
                    ScadaEventModel mymodel = new ScadaEventModel();

                    int index = s.Columns.IndexOf("time");

                    object time = s.Values[i][index];
                    mymodel.time = time != null ? time.ToString() : "";



                    index = -1;
                    index = s.Columns.IndexOf("field_date");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Date = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_sid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.SERVER_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_cid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMM_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_did");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DEVICE_ID = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_ioid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ID = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_event");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Event = v != null ? v.ToString() : "";
                    }


                    index = -1;
                    index = s.Columns.IndexOf("field_comm_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMM_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_device_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DEVICE_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_label");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_LABEL = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_content");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Content = v != null ? v.ToString() : "";
                    }



 
                    index = -1;
                    index = s.Columns.IndexOf("field_io_label");
                    
                    events.Add(mymodel);
                }

            }

            var result = Pager.Paging(events, realResult.RecordCount);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        /// <summary>
        /// 读取通用历史事件信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GeneralQueryHistoryEvent(ScadaEventModel model, PageInfo pageInfo)
        {

          
        
             
          
            List<ScadaEventModel> events = new List<ScadaEventModel>();
            InfluxDBHistoryResult realResult = null;

    
            realResult = mWebInfluxDbManager.DbQuery_Events(Convert.ToDateTime(model.StartDate), Convert.ToDateTime(model.EndDate), model.Event, model.SERVER_ID, model.COMM_ID,model.DEVICE_ID, pageInfo.limit, pageInfo.page);
            if (realResult != null&& realResult.Seres!=null && realResult.Seres.Count() > 0)
            {
                var s = realResult.Seres.First();
                for (int i = 0; i < s.Values.Count; i++)
                {
                    ScadaEventModel mymodel = new ScadaEventModel();
                    int index = s.Columns.IndexOf("time");
                    object time = s.Values[i][index];
                    mymodel.time = time != null ? time.ToString() : "";
                    index = -1;
                    index = s.Columns.IndexOf("field_date");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Date = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_sid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.SERVER_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_cid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMM_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_did");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DEVICE_ID = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_ioid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ID = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_event");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Event = v != null ? v.ToString() : "";
                    }


                    index = -1;
                    index = s.Columns.IndexOf("field_comm_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMM_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_device_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DEVICE_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_NAME = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_io_label");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_LABEL = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_content");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Content = v != null ? v.ToString() : "";
                    }




                    index = -1;
                    index = s.Columns.IndexOf("field_io_label");
                    
                    events.Add(mymodel);
                }

            }

            var result = Pager.Paging(events, realResult.RecordCount);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取事件所有类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEventItem()
        {
            List<SelectOption> _select = new List<SelectOption>();
            foreach (var item in Enum.GetValues(typeof(ScadaEvent)))
                {
                    SelectOption _option = new SelectOption
                    {
                        id = item.ToString(),
                        name = item.ToString(),
                        value = item.ToString(),
                       
                    };
                    _select.Add(_option);
                }

            return Json(_select, JsonRequestBehavior.AllowGet);

        }


    }
}