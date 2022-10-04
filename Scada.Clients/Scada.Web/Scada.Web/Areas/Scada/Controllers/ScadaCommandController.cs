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
    //加载下置命令记录日志
    public class ScadaCommandController : BaseController
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
        
        
        public ActionResult GeneralHistoryCommand(int? id)
        {
            ScadaCommandModel model = new ScadaCommandModel();
            model.AllDeviceList = DeviceGroupService.GetAll();
            Session["AllDeviceList"] = model.AllDeviceList;
            base.Index(id);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetAllDevices()
        {

            IEnumerable<DeviceGroupModel> Items = DeviceGroupService.GetAll();
            return Json(Items.ToList(), JsonRequestBehavior.AllowGet);

        }


     
   
        public JsonResult GroupTreeListSelect()
        {
            var result = GroupService.GetGroupTreeSelect();
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public JsonResult GetGroupDevice(int groupId)
        {
            string idlist = GroupService.GetGroupNodeChildren(groupId);

            IEnumerable<DeviceGroupModel> Items = DeviceGroupService.GetListByGroupId(idlist);
            List<SelectOption> _select = new List<SelectOption>();


            if (Items != null && Items.Count() > 0)
            {

                foreach (var item in Items)
                {
                    SelectOption _option = new SelectOption
                    {
                        id = item.Id.ToString(),
                        name = item.ALIASNAME,
                        value = item.Id.ToString(),
                        value1 = item.IO_DEVICE_ID.ToString(),
                        value2 = item.IO_COMM_ID,
                        value3 = item.IO_SERVER_ID,
                        value4 = item.IOPARANAMES,
                        value5 = item.SerieType,

                    };
                    _select.Add(_option);
                }
            }
            string deviceids = "'1'";
            foreach (var item in Items)
            {
                deviceids += ",'" + item.IO_DEVICE_ID + "'";
            }
            var deviceItems = IO_DeviceServer.GetByWhere(" where  IO_DEVICE_ID in (" + deviceids + ")");
            foreach (var item in _select)
            {
                var searchs = deviceItems.Where(x => x.IO_DEVICE_ID.Trim().ToLower() == item.value1.Trim().ToLower());
                if (searchs.Count() > 0)
                {
                    var sItem = searchs.First();
                    if (string.IsNullOrWhiteSpace(sItem.IO_DEVICE_UPDATECYCLE))
                    {
                        item.value6 = "120";
                    }
                    else
                    {
                        item.value6 = sItem.IO_DEVICE_UPDATECYCLE;
                    }

                }
            }

            return Json(_select, JsonRequestBehavior.AllowGet);





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
        /// 读取通用历史下置命令信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GeneralQueryHistoryCommand(ScadaCommandModel model, PageInfo pageInfo)
        {

          
        
             
            var objs = (List<DeviceGroupModel>)Session["AllDeviceList"];
            if (objs == null)
                return null;
            List<ScadaCommandModel> events = new List<ScadaCommandModel>();
            InfluxDBHistoryResult realResult = null;

    
            realResult = mWebInfluxDbManager.DbQuery_Commands(model.IO_SERVER_ID, model.IO_COMM_ID,model.IO_DEVICE_ID, Convert.ToDateTime(model.StartDate), Convert.ToDateTime(model.EndDate),pageInfo.limit, pageInfo.page).Result;
            if (realResult != null&& realResult.Seres!=null && realResult.Seres.Count() > 0)
            {
                var s = realResult.Seres.First();
                for (int i = 0; i < s.Values.Count; i++)
                {
                    ScadaCommandModel mymodel = new ScadaCommandModel();
                    int index = s.Columns.IndexOf("time");
                    object time = s.Values[i][index];
                    mymodel.time = time != null ? time.ToString() : "";
                    index = -1;
                    index = s.Columns.IndexOf("field_command_date");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMMAND_DATE = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_command_result");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMMAND_RESULT = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_command_user");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMMAND_USER = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_command_value");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMMAND_VALUE = v != null ? v.ToString() : "";
                    }




                    index = -1;
                    index = s.Columns.IndexOf("field_command_id");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.COMMAND_ID = v != null ? v.ToString() : "";
                    }


                    index = -1;
                    index = s.Columns.IndexOf("tag_sid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_SERVER_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_cid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_COMM_ID = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_did");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_DEVICE_ID = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_ioid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_ID = v != null ? v.ToString() : "";
                    }
                    

                    index = -1;
                    index = s.Columns.IndexOf("field_name");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.IO_NAME = v != null ? v.ToString() : "";
                    }

                     



                    index = -1;
                    index = s.Columns.IndexOf("field_label");
                    if (index >= 0)
                    {
                        var search = objs.Find(x => x.IO_DEVICE_ID == mymodel.IO_DEVICE_ID);
                        if (search != null)
                        {
                            string[] titles = search.IOPARATITLES.Split(',');
                            List<string> ioids = search.IOPARAS.Split(',').ToList();
                            int nameindex = ioids.FindIndex(x => x == mymodel.IO_ID);
                            if (nameindex >= 0)
                            {
                                mymodel.IO_LABEL = titles[nameindex] + " " + mymodel.IO_LABEL;
                            }

                        }

                    }
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