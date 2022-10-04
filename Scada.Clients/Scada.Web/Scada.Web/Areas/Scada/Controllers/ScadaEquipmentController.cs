using Scada.DBUtility;
using ScadaWeb.Common;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.Service;
using ScadaWeb.Web.Areas.Scada.Models;
using ScadaWeb.Web.Controllers;
using ScadaWeb.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Temporal.WebDbAPI;
using static ScadaWeb.Web.Areas.API.Controllers.PageController;

namespace ScadaWeb.Web.Areas.Scada.Controllers
{
    public class ScadaEquipmentController : BaseController
    {
        public IIO_ParaService ParaService { set; get; }
        public IDeviceGroupService DeviceGroupService { set; get; }
        public IScadaGroupService GroupService { get; set; }
        public IScadaEquipmentParaParameterService ScadaEquipmentParaParameterService { get; set; }
        public IScadaEquipmentService ScadaEquipmentService { get; set; }

        public IIO_DeviceService DeviceService { set; get; }
        public IIO_CommunicateService CommunicateService { set; get; }
        public IIO_ServerService StationService { set; get; }
        public ISerieConfigService serieConfigService { set; get; }
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();



        public ActionResult AddGroup()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddGroup(ScadaGroupModel model)
        {
            model.CreateTime = DateTime.Now;
            model.CreateUserId = Operator.UserId;
            model.UpdateTime = DateTime.Now;
            model.UpdateUserId = Operator.UserId;
            model.ParentId = 0;
            var result = GroupService.Insert(model) ? SuccessTip("添加成功") : ErrorTip("添加失败");
            return Json(result);
        }
        public ActionResult EditGroup(int id)
        {

            var model = GroupService.GetById(id);
            return View(model);
        }
      
        [HttpPost]
        public ActionResult EditGroup(ScadaGroupModel model)
        {
            model.UpdateTime = DateTime.Now;
            model.UpdateUserId = Operator.UserId;
            var result = GroupService.UpdateById(model) ? SuccessTip("修改成功") : ErrorTip("修改失败");
            return Json(result);
        }
        [HttpGet]
        public JsonResult DeleteGroup(int id)
        {
            var result = ErrorTip("删除失败");
            if (GroupService.DeleteById(id))
            {
                //此处删除设备模型
               // DeviceGroupService.DeleteByWhere(" where  GroupId=" + id);
                result = SuccessTip("删除成功");
            }
            else
            {
                result = ErrorTip("删除失败");
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
   

       
       
    
        #region 2022-2-12年新修改的模块方法
        /// <summary>
        /// 设备
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EquipmentGroups(long? Id)
        {
            var _menuId = Id == null ? 0 : Id.Value;
            if (Operator != null)
            {
                var _roleId = Operator.RoleId;
                if (Id != null)
                {
                    var buttons = ButtonService.GetButtonListByRoleIdModuleId(_roleId, _menuId);
                    var finder = buttons.FirstOrDefault(x => x.EnCode == "add");
                    if (finder!=null&& finder.Id>0)
                    {
                        ViewBag.RoleAdd = true;
                    }
                    else
                    {
                        ViewBag.RoleAdd = false;
                    }
                    finder = buttons.FirstOrDefault(x => x.EnCode == "del");
                    if (finder != null && finder.Id > 0)
                    {
                        ViewBag.RoleDel = true;
                    }
                    else
                    {
                        ViewBag.RoleDel = false;
                    }
                    finder = buttons.FirstOrDefault(x => x.EnCode == "edit");
                    if (finder != null && finder.Id > 0)
                    {
                        ViewBag.RoleEdit = true;
                    }
                    else
                    {
                        ViewBag.RoleEdit = false;
                    }
                }

           
            }
            base.Index(Id);
            return View();

          

        }
        Random rd = new Random();
        /// <summary>
        /// 加载所有分组信息,分组信息不进行分页显示
        /// </summary>
        [HttpGet]
        public JsonResult EquipmentGroupList(string key)
        {
            //获取所有的设备
            var equipments = ScadaEquipmentService.GetAll();
            string where = "";
            if(!string.IsNullOrEmpty(key))
            {
                where += " where  GroupTitle like '%"+ key + "%'";
            }
            var list = GroupService.GetByWhere(where);
            foreach (var item in list)
            {
                item.ModelCount = equipments.Count(x => x.GroupId == item.Id);
            }
            var result = new { code = 0, count = list.Count(), data = list };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载所有分组信息,分组信息不进行分页显示
        /// </summary>
        [HttpGet]
        public JsonResult GetGroup()
        {
        
           
            var list = GroupService.GetByWhere(null);
            
            var result = new { code = 0, count = list.Count(), data = list };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult Equipments(long? id, long GroupId)
        {
            var _menuId = id == null ? 0 : id.Value;
            if (Operator != null)
            {
                var _roleId = Operator.RoleId;
                if (id != null)
                {
                    var buttons = ButtonService.GetButtonListByRoleIdModuleId(_roleId, _menuId);
                    var finder = buttons.FirstOrDefault(x => x.EnCode == "add");
                    if (finder != null && finder.Id > 0)
                    {
                        ViewBag.RoleAdd = true;
                    }
                    else
                    {
                        ViewBag.RoleAdd = false;
                    }
                    finder = buttons.FirstOrDefault(x => x.EnCode == "del");
                    if (finder != null && finder.Id > 0)
                    {
                        ViewBag.RoleDel = true;
                    }
                    else
                    {
                        ViewBag.RoleDel = false;
                    }
                    finder = buttons.FirstOrDefault(x => x.EnCode == "edit");
                    if (finder != null && finder.Id > 0)
                    {
                        ViewBag.RoleEdit = true;
                    }
                    else
                    {
                        ViewBag.RoleEdit = false;
                    }
                }


            }
            base.Index(id);
            ScadaGroupModel scadaGroup = GroupService.GetById(GroupId);
            if (scadaGroup == null)
                scadaGroup = new ScadaGroupModel();
            return View(scadaGroup);

        }
        [HttpGet]
        public JsonResult EquipmentList(string key,int groupid,int pageindex,int pagesize)
        {
            string where = "where GroupId="+ groupid;
            if (!string.IsNullOrEmpty(key))
            {
                where += " and   ModelTitle like '%" + key + "%'";
            }
            ScadaGroupModel scadaGroup = GroupService.GetById(groupid);
            if (scadaGroup == null)
                scadaGroup = new ScadaGroupModel();
            //获取所有的设备
            ScadaEquipmentModel filter = new ScadaEquipmentModel();
            filter.GroupId = groupid;
            filter.ModelTitle = key;
            PageInfo page = new PageInfo();
            page.prefix = "";
            page.limit = pagesize;
            page.page = pageindex;

            long total = 0;
            var equipments = ScadaEquipmentService.GetListObjectByFilter(filter, page,out total);
            var paras = ScadaEquipmentParaParameterService.GetAll();
            var devices= DeviceService.GetAll();
            var communications = CommunicateService.GetAll();
            var stations = StationService.GetAll();
            foreach (var item in equipments)
            {
                item.Paras = paras.Where(x => x.EquipmentId == item.Id).ToList();
                var server = stations.First(x => x.SERVER_ID == item.ServerId);
                var communication = communications.First(x => x.IO_SERVER_ID == item.ServerId&&x.IO_COMM_ID== item.CommunicationId);
                var device = devices.First(x => x.IO_SERVER_ID == item.ServerId && x.IO_COMM_ID == item.CommunicationId&&x.IO_DEVICE_ID== item.DeviceId);
                item.ServerName = server != null ? server.SERVER_NAME + "[" + server.SERVER_ID + "]":"采集站缺失";
                item.CommunicationName = communication != null ? communication.IO_COMM_LABEL + "[" + communication.IO_COMM_NAME + "]" : "通道缺失";
                item.DeviceName = device != null ? device.IO_DEVICE_LABLE + "[" + device.IO_DEVICE_NAME + "]" : "设备缺失";
                item.ScadaGroup = scadaGroup;
            }
            var result = new { code = 0, count = total, data = equipments };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取是IO信息
        [HttpGet]
        public JsonResult GetStations()
        {
            var stations = StationService.GetAll();
            var result = new { code = 0, count = stations.Count(), data = stations };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCommunications(string serverid)
        {
            var communications = CommunicateService.GetByWhere(" where IO_SERVER_ID='"+ serverid + "'");
            var result = new { code = 0, count = communications.Count(), data = communications };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDevices(string serverid, string communicationid)
        {
            var devices = DeviceService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='"+ communicationid + "'");
            var result = new { code = 0, count = devices.Count(), data = devices };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetParas(string serverid, string communicationid,string deviceid)
        {
            var paras = ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicationid + "' and IO_DEVICE_ID='"+ deviceid + "'");
            List<ParaSelectItem> Items = new List<ParaSelectItem>();
            foreach (var item in paras)
            {
                Items.Add(new ParaSelectItem { 
                  title= item.IO_NAME+"["+ item.IO_LABEL + "]",
                 value= item.IO_NAME,
                 id= item.IO_ID

                });
            }
            var result = new { code = 0, count = Items.Count(), data = Items };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSerieTypes()
        {
            var items = serieConfigService.GetByWhere(null, null, "  distinct SerieClassify ");
            var result = new { code = 0, count = items.Count(), data = items };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSerieStyles(string SerieType)
        {
            var items = serieConfigService.GetByWhere(" where SerieClassify='"+ SerieType.Trim() + "'");
            var result = new { code = 0, count = items.Count(), data = items };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
       
        #endregion

        public ActionResult AddEquipment(int Id)
        {
            ViewBag.GroupId = Id;
            return View();

        }
          
        [HttpPost]
        public ActionResult AddEquipment(ScadaEquipmentModel model)
        {
            model.CreateTime = DateTime.Now;
            model.CreateUserId = Operator.UserId;
            model.UpdateTime = DateTime.Now;
            model.UpdateUserId = Operator.UserId;
          if(string.IsNullOrEmpty(model.Remark))
            {
                model.Remark = "";
            }
            var result= ErrorTip("添加失败"); 
            if(model.GroupId<=0)
            {
                result = ErrorTip("添加失败,请选择模型分组!");
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.ModelTitle))
            {
                result = ErrorTip("添加失败,请输入模型名称!");
                return Json(result);
            }
            if (model.Paras==null|| model.Paras.Count<=0)
            {
                result = ErrorTip("添加失败,请输入模型参数!");
                return Json(result);
            }
            long res = ScadaEquipmentService.InsertReturnId(model);
            if(res>0)
            {
                model.Id = res;
                try
                {
                    for (int i = 0; i < model.Paras.Count; i++)
                    {
                        ScadaEquipmentParaParameterModel parameterModel = model.Paras[i];
                        parameterModel.Id = 0;
                        parameterModel.CreateTime = DateTime.Now;
                        parameterModel.CreateUserId = Operator.UserId;
                        parameterModel.UpdateTime = DateTime.Now;
                        parameterModel.UpdateUserId = Operator.UserId;
                        parameterModel.EquipmentId = Convert.ToInt32(model.Id);
                        ScadaEquipmentParaParameterService.Insert(parameterModel);
                    }
                    result = SuccessTip("添加成功");
                }
                catch(Exception emx)
                {
                    ScadaEquipmentService.DeleteById(model.Id);
                    ScadaEquipmentParaParameterService.DeleteByWhere("where EquipmentId="+ model.Id);
                    result = ErrorTip("添加失败 "+ emx.Message);
                }
            }
            else
            {
                result = ErrorTip("添加失败");
            }
       
            return Json(result);
        }

        public JsonResult DeleteEquipment(int id)
        {
            var result = ErrorTip("删除失败");
            if (ScadaEquipmentService.DeleteById(id))
            {
                //此处删除设备模型
                ScadaEquipmentParaParameterService.DeleteByWhere("where EquipmentId=" + id);
                result = SuccessTip("删除成功");
            }
            else
            {
                result = ErrorTip("删除失败");
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region 获取实时数据
        Random random = new Random();
        public WebRealCache mWebRealCache = new WebRealCache();
        [HttpPost]
        public JsonResult GetReadData(List<JsIOPara> ioparas)
        {
            if (ioparas == null || ioparas.Count < 0)
                return null;

            List<JsIOPara> noCFioparas = new List<JsIOPara>();


            for (int i = ioparas.Count - 1; i >= 0; i--)
            {
                if (!noCFioparas.Exists(x => x.ServerID == ioparas[i].ServerID
                && x.CommunicateID == ioparas[i].CommunicateID && x.DeviceID == ioparas[i].DeviceID && x.ParaID == ioparas[i].ParaID))
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
                    var result = Pager.Paging(new List<JsIOPara>(), 0);
                    //读取以下10行的实时数据，从influxDB中读取
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }

        }

        #endregion
        #region 编辑设备模型
        public ActionResult EditEquipment(int Id)
        {
            ScadaEquipmentModel model = ScadaEquipmentService.GetById(Id);
            if (model != null)
            {
                model.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (model.Paras == null)
                    model.Paras = new List<ScadaEquipmentParaParameterModel>();
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }


        }
        [HttpPost]
        public ActionResult EditEquipment(ScadaEquipmentModel model)
        {
            model.CreateTime = DateTime.Now;
            model.CreateUserId = Operator.UserId;
            model.UpdateTime = DateTime.Now;
            model.UpdateUserId = Operator.UserId;
            if (string.IsNullOrEmpty(model.Remark))
            {
                model.Remark = "";
            }
            var result = ErrorTip("添加失败");
            if (model.GroupId <= 0)
            {
                result = ErrorTip("添加失败,请选择模型分组!");
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.ModelTitle))
            {
                result = ErrorTip("添加失败,请输入模型名称!");
                return Json(result);
            }
            if (model.Paras == null || model.Paras.Count <= 0)
            {
                result = ErrorTip("添加失败,请输入模型参数!");
                return Json(result);
            }
            bool res = ScadaEquipmentService.UpdateById(model);
            if (res)
            {
               
                try
                {
                   if( ScadaEquipmentParaParameterService.DeleteByWhere("where EquipmentId=" + model.Id))
                    {
                        for (int i = 0; i < model.Paras.Count; i++)
                        {
                            ScadaEquipmentParaParameterModel parameterModel = model.Paras[i];
                            parameterModel.Id = 0;
                            parameterModel.CreateTime = DateTime.Now;
                            parameterModel.CreateUserId = Operator.UserId;
                            parameterModel.UpdateTime = DateTime.Now;
                            parameterModel.UpdateUserId = Operator.UserId;
                            parameterModel.EquipmentId = Convert.ToInt32(model.Id);
                            ScadaEquipmentParaParameterService.Insert(parameterModel);
                        }
                      
                    }
                    result = SuccessTip("添加成功");
                }
                catch (Exception emx)
                {
                    ScadaEquipmentService.DeleteById(model.Id);
                    ScadaEquipmentParaParameterService.DeleteByWhere("where EquipmentId=" + model.Id);
                    result = ErrorTip("添加失败 " + emx.Message);
                }
            }
            else
            {
                result = ErrorTip("添加失败");
            }

            return Json(result);
        }
        #endregion
        #region 实时曲线
        public ActionResult EquipmentRealSerie(int Id)
        {
            ScadaEquipmentModel model = ScadaEquipmentService.GetById(Id);
            if (model != null)
            {
                model.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (model.Paras == null)
                    model.Paras = new List<ScadaEquipmentParaParameterModel>();
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 用户自由选择任意井进行专门的历史曲线查询模块的实际查询数据的功能
        /// </summary>
        /// <param name="wellid"></param>
        /// <param name="series"></param>
        /// <param name="charttype"></param>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <param name="serieclassify"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult  EquipmentHistorySeriesData(SeriesPara para)
        {
       
            string serverid = para.serverid;
            string communicateid = para.communicateid;
            string deviceid = para.deviceid;
            int charttype = para.charttype;
            string sdate = para.sdate;
            string edate = para.edate;
            int pagesize = para.pagesize;
            int equipmentid = para.equipmentid;
            //曲线指标列表
            string[] serienames = para.selectserie.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ApiSerieConfig serieConfig = new ApiSerieConfig();
            serieConfig.IO_DEVICE_ID = deviceid;
            serieConfig.Series = new List<ApiSerieConfigModel>();
            var paras= ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + equipmentid).ToList();
            var seriecharts = serieConfigService.GetAll();
            for (int i = 0; i < serienames.Length; i++)
            {
                var eqpara = paras.Find(x => x.ParaName.Trim().ToLower() == serienames[i].Trim().ToLower());
                if(eqpara!=null)
                {
                    var eqserie = seriecharts.First(x => x.SerieClassify.Trim() == eqpara.SerieType.Trim() && x.SerieName.Trim().ToLower() == eqpara.SerieName.Trim().ToLower());
                    if(eqserie!=null)
                    {
                        ApiSerieConfigModel apiSerie = new ApiSerieConfigModel();
                        apiSerie.IO_ID = eqpara.ParaName;
                        apiSerie.SerieColor = eqserie.SerieColor;
                        apiSerie.SerieName = eqpara.ParaName;
                        apiSerie.SerieTitle = eqpara.ParaTitle;
                        apiSerie.SerieType = eqserie.SerieType;
                        apiSerie.SerieWidth = eqserie.SerieWidth;
                        apiSerie.ShowLegend = eqserie.ShowLegend;
                        apiSerie.ShowSymbol = eqserie.ShowSymbol;
                        apiSerie.SymbolColor = eqserie.SymbolColor;
                        apiSerie.SymbolSize = eqserie.SymbolSize;
                        apiSerie.SymbolStep = eqserie.SymbolStep;
                        apiSerie.SymbolType = eqserie.SymbolType;
                        
                        serieConfig.Series.Add(apiSerie);
                    }

                }
             
           

            }
            return GetEquipmentHistorySeriesData(serieConfig, serverid, communicateid, deviceid, charttype, sdate, edate, pagesize);
        }
        private JsonResult GetEquipmentHistorySeriesData(ApiSerieConfig serieConfig, string serverid, string communicateid, string deviceid, int charttype = 1, string sdate = "", string edate = "", int pagesize = 1000)
        {

            //获取对应的曲线配置信息

            List<ApiSerieConfigModel> Items = serieConfig.Series;
            ///初始化曲线对象
            EChartOption chartOption = new EChartOption();
            chartOption.xAxis = new Axis[1];
            chartOption.xAxis[0] = new Axis();
            chartOption.xAxis[0].gridIndex = 1;
            chartOption.xAxis[0].type = "time";
            chartOption.xAxis[0].name = "时间";
            List<Axis> yaxis = new List<Axis>();
            int index = 0;
            string[] legend = new string[Items.Count()];

            foreach (var item in Items)
            {
                yaxis.Add(new Axis() { gridIndex = index, name = item.SerieTitle, type = "value" ,data=new string[0]});
                legend[index] = item.SerieTitle;
                index++;
            }
            chartOption.legend.data = legend;
            chartOption.yAxis = yaxis.ToArray();
            //初始化对象结束
            if (sdate == null || sdate == "")
                sdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            if (edate == null || edate == "")
                edate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chartOption.series = new Series[Items.Count()];
            int snum = 0;
            foreach (var item in Items)
            {
                chartOption.series[snum] = new Series();
                chartOption.series[snum].id = item.IO_ID;
                chartOption.series[snum].name = item.SerieTitle;
                chartOption.series[snum].lineStyle.color = item.SerieColor;
                chartOption.series[snum].lineStyle.width = int.Parse(item.SerieWidth);
                
                chartOption.series[snum].data = new double[0];
                chartOption.series[snum].type = item.SerieType;
                chartOption.series[snum].showSymbol = item.ShowSymbol == "1" ? true : false;
                chartOption.series[snum].symbol = item.SymbolType;
                chartOption.series[snum].symbolSize = int.Parse(item.SymbolSize);
                snum++;
            }
            try
            {
                InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(serverid, communicateid, deviceid, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), 10000, 1, " ASC ");
                if (realResult != null)
                {

                    var datas = realResult.Seres;
                    if (datas != null && datas.Count() > 0)
                    {
                        var onedata = datas.First();
                        int sindex = 0;
                        foreach (var item in Items)
                        {
                            chartOption.series[sindex] = new Series();
                            chartOption.series[sindex].id = item.IO_ID;
                            chartOption.series[sindex].name = item.SerieTitle;
                            chartOption.series[sindex].lineStyle.color = item.SerieColor;
                            chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);
                           
                            chartOption.series[sindex].data = new double[onedata.Values.Count];
                            chartOption.series[sindex].type = item.SerieType;
                            chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                            chartOption.series[sindex].symbol = item.SymbolType;
                            chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
                            sindex++;
                        }
                        string[] axisData = new string[onedata.Values.Count];
                        //获取的数据按照时间先后
                        int dataindex = onedata.Values.Count() - 1;

                        foreach (var value in onedata.Values)
                        {
                            //获取采集时间
                            object objx = onedata.Values[dataindex][onedata.Columns.IndexOf("time")];
                            axisData[dataindex] = objx != null ? objx.ToString() : "";
                            //////////////////////////
                            sindex = 0;
                            foreach (var item in Items)
                            {

                                try
                                {



                                    int recordindex = onedata.Columns.IndexOf("field_" + item.IO_ID.Trim().ToLower() + "_value");
                                    if (recordindex >= 0)
                                    {
                                        object objy = onedata.Values[dataindex][recordindex];
                                        chartOption.series[sindex].data[dataindex] = Convert.ToDouble(objy);
                                        chartOption.series[sindex].id = item.IO_ID;
                                    }


                                }
                                catch
                                {

                                }

                                sindex++;
                            }
                            dataindex--;
                        }
                        chartOption.xAxis[0].data = axisData;//设置x轴数据，time格式的数据必须在Axis轴上进行设置


                    }


                }


            }
            catch
            {


            }
            //读取以下的实时数据，从influxDB中读取
            return Json(chartOption, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EquipmentRealSeriesData(SeriesPara para)
        {
            //曲线指标列表
            string[] serienames = para.selectserie.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ApiSerieConfig serieConfig = new ApiSerieConfig();
            serieConfig.IO_DEVICE_ID = para.deviceid;
            serieConfig.Series = new List<ApiSerieConfigModel>();
            var paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + para.equipmentid).ToList();
            var seriecharts = serieConfigService.GetAll();
            for (int i = 0; i < serienames.Length; i++)
            {
                var eqpara = paras.Find(x => x.ParaName.Trim().ToLower() == serienames[i].Trim().ToLower());
                if (eqpara != null)
                {
                    var eqserie = seriecharts.First(x => x.SerieClassify.Trim() == eqpara.SerieType.Trim() && x.SerieName.Trim().ToLower() == eqpara.SerieName.Trim().ToLower());
                    if (eqserie != null)
                    {
                        ApiSerieConfigModel apiSerie = new ApiSerieConfigModel();
                        apiSerie.IO_ID = eqpara.ParaId;
                        apiSerie.SerieColor = eqserie.SerieColor;
                        apiSerie.SerieName = eqserie.SerieName;
                        apiSerie.SerieTitle = eqpara.ParaTitle;
                        apiSerie.SerieType = eqserie.SerieType;
                        apiSerie.SerieWidth = eqserie.SerieWidth;
                        apiSerie.ShowLegend = eqserie.ShowLegend;
                        apiSerie.ShowSymbol = eqserie.ShowSymbol;
                        apiSerie.SymbolColor = eqserie.SymbolColor;
                        apiSerie.SymbolSize = eqserie.SymbolSize;
                        apiSerie.SymbolStep = eqserie.SymbolStep;
                        apiSerie.SymbolType = eqserie.SymbolType;

                        serieConfig.Series.Add(apiSerie);
                    }

                }



            }
            para.serieConfig = serieConfig;
            RealSerieData resDatas = new RealSerieData();
            string serverid = para.serverid, communicateid = para.communicateid, deviceid = para.deviceid;
            int updatecycle = para.updatecycle;
            if (Configs.GetValue("SystemRunModel") == "Debug")
            {
                RealWebCacheDataItem realItem =  new RealWebCacheDataItem() { CommunicationId = communicateid, DeviceId = deviceid, ServerId = serverid };
                realItem.ParaReals = new List<WebCacheIOData>();
                List<string> fs = new List<string>();
                foreach (var s in para.serieConfig.Series)
                {
                    fs.Add(s.SerieName);

                }
           
                for (int i = 0; i < serienames.Length; i++)
                {

                    realItem.ParaReals.Add(new WebCacheIOData
                    {

                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ParaID = serienames[i],
                        ParaName = serienames[i],
                        ParaValue = random.Next(100).ToString(),
                        QualityStamp = "GOOD"
                    });
                }
           
     
                if (realItem != null)
                {

                    realItem.ParaReals.ForEach(delegate (WebCacheIOData iOData)
                    {   ///删除重复项

                        PointData rdata = new PointData() { Name = iOData.ParaName, Status = 1, Value = iOData.ParaValue.ToString() };
                        resDatas.Date = iOData.Date;
                        resDatas.Data.Add(rdata);

                    });


                }
            }
            else
            {

                RealWebCacheDataItem realItem = mWebRealCache.GetReal(new WebCacheDataItem() { CommunicationId = communicateid, DeviceId = deviceid, ServerId = serverid });

                List<string> fs = new List<string>();
                foreach (var s in para.serieConfig.Series)
                {
                    fs.Add(s.SerieName);
                }
       
                if (realItem != null)
                {

                    realItem.ParaReals.ForEach(delegate (WebCacheIOData iOData)
                    {   ///删除重复项

                    PointData rdata = new PointData() { Name = iOData.ParaName, Status = 1, Value = iOData.ParaValue.ToString() };
                        resDatas.Date = iOData.Date;
                        resDatas.Data.Add(rdata);

                    });


                }
            }

            //读取以下的实时数据，从influxDB中读取
            return Json(resDatas, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 历史数据查询
        public ActionResult EquipmentHistoryQuery(int Id)
        {
            ScadaEquipmentModel model = ScadaEquipmentService.GetById(Id);
            if (model != null)
            {
           
                model.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (model.Paras == null)
                    model.Paras = new List<ScadaEquipmentParaParameterModel>();
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public JsonResult EquipmentHistoryData(ScadaEquipmentModel model, PageInfo pageInfo)
        {
            ScadaEquipmentModel serachemodel = ScadaEquipmentService.GetById(model.Id);
            if(serachemodel != null)
            {
                serachemodel.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (serachemodel.Paras == null)
                    serachemodel.Paras = new List<ScadaEquipmentParaParameterModel>();
            }
        if(serachemodel==null)
                return Json(null, "application/text", JsonRequestBehavior.AllowGet);
            string items = "[";

            List<string> columns = new List<string>();
        
            foreach (var para in serachemodel.Paras)
            {
                columns.Add( para.ParaName);
    

            }

   
            if (!string.IsNullOrWhiteSpace(serachemodel.DeviceId))
            {
                try
                {
                    string sdate = model.StartDate;
                    string edate = model.EndDate;
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(model.ServerId, model.CommunicationId, model.DeviceId, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pageInfo.limit, pageInfo.page, " DESC ");
                    foreach (var s in realResult.Seres)
                    {

                        List<int> indexs = new List<int>();
                        for (int i = 0; i < s.Values.Count; i++)
                        {
                            string jsonrow = "";
                            int index = s.Columns.IndexOf("time");
                            object time = s.Values[i][index];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";

                            foreach (string str in columns)
                            {
                                try
                                {

                                    index = -1;
                                    index = s.Columns.IndexOf("field_" + str.Trim().ToLower().ToString() + "_value");
                                    if (index >= 0)
                                    {
                                        object v = s.Values[i][index];

                                        jsonrow += ",\"" + str + "\":\"" + (v != null ? v.ToString() : "") + "\"";
                                    }

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items += jsonrow;

                        }
                    }
                    items += "]";
                    var result = Pager.Paging2(items, realResult.RecordCount);
                    //读取以下的实时数据，从influxDB中读取
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(null, "application/text", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var result = Pager.Paging2("", 0);
                return Json(result, "application/text", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 历史曲线
        public ActionResult EquipmentHistorySerie(int Id)
        {
            ScadaEquipmentModel model = ScadaEquipmentService.GetById(Id);
            if (model != null)
            {
                model.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (model.Paras == null)
                    model.Paras = new List<ScadaEquipmentParaParameterModel>();
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        #endregion
        #region 历史统计
        public ActionResult EquipmentHistorySummaryQuery(int Id)
        {
            ScadaEquipmentModel model = ScadaEquipmentService.GetById(Id);
            if (model != null)
            {

                model.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (model.Paras == null)
                    model.Paras = new List<ScadaEquipmentParaParameterModel>();
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        private bool IsNumberIOPoint(IOParaModel para)
        {
            //"模拟量",
            //"开关量",
            //"字符串量",
            //"计算值",
            //"关系数据库值",
            //"常量值"

            if (para.IO_POINTTYPE == "模拟量" ||
            para.IO_POINTTYPE == "开关量" ||
            para.IO_POINTTYPE == "计算值" ||
             para.IO_POINTTYPE == "常量值")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        public JsonResult EquipmentHistorySummaryData(ScadaEquipmentSummaryPageModel model, PageInfo pageInfo)
        {
            ScadaEquipmentModel serachemodel = ScadaEquipmentService.GetById(model.Id);
            if (serachemodel != null)
            {
                serachemodel.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (serachemodel.Paras == null)
                    serachemodel.Paras = new List<ScadaEquipmentParaParameterModel>();
            }
            if (serachemodel == null)
                return Json(null, "application/text", JsonRequestBehavior.AllowGet);
            string items = "[";

            List<string> columns = new List<string>();

            foreach (var para in serachemodel.Paras)
            {
                columns.Add(para.ParaName);


            }


            if (!string.IsNullOrWhiteSpace(serachemodel.DeviceId))
            {
                try
                {
                    List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serachemodel.ServerId + "' and IO_COMM_ID='" + serachemodel.CommunicationId + "' and IO_DEVICE_ID='" + serachemodel.DeviceId + "'").ToList();

                    string returnFields = "  time";
                    #region
                    {
                      
                        foreach (var item in serachemodel.Paras)
                        {
                            var para = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.ParaName.Trim().ToLower().ToString());
                            if (para != null && IsNumberIOPoint(para))
                            {
                                returnFields += "," + model.Method + "(field_" + item.ParaName.Trim().ToLower().ToString() + "_value) as field_" + item.ParaName.Trim().ToLower().ToString() + "_value";
                            }
                            else
                            {
                                returnFields += ",mean(null) as field_" + item.ParaName.Trim().ToLower().ToString() + "_value";
                            }

                        }


                    }
                    #endregion
                    string sdate = model.StartDate;
                    string edate = model.EndDate;
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_HistoryStatics(model.ServerId, model.CommunicationId, model.DeviceId, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pageInfo.limit, pageInfo.page, " DESC ", model.Period, returnFields);
                    foreach (var s in realResult.Seres)
                    {

                        List<int> indexs = new List<int>();
                        for (int i = 0; i < s.Values.Count; i++)
                        {
                            string jsonrow = "";
                            int index = s.Columns.IndexOf("time");
                            object time = s.Values[i][index];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";

                            foreach (string str in columns)
                            {
                                try
                                {

                                    index = -1;
                                    index = s.Columns.IndexOf("field_" + str.Trim().ToLower().ToString() + "_value");
                                    if (index >= 0)
                                    {
                                        object v = s.Values[i][index];

                                        jsonrow += ",\"" + str + "\":\"" + (v != null ? v.ToString() : "") + "\"";
                                    }

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items += jsonrow;

                        }
                    }
                    items += "]";
                    var result = Pager.Paging2(items, realResult.RecordCount);
                    //读取以下的实时数据，从influxDB中读取
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(null, "application/text", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var result = Pager.Paging2("", 0);
                return Json(result, "application/text", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 历史统计曲线
        public ActionResult EquipmentHistorySummarySerie(int Id)
        {
            ScadaEquipmentModel model = ScadaEquipmentService.GetById(Id);
            if (model != null)
            {
                model.Paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + model.Id).ToList();
                if (model.Paras == null)
                    model.Paras = new List<ScadaEquipmentParaParameterModel>();
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
 
        [HttpPost]
        public JsonResult EquipmentHistorySummarySeriesData(SeriesPara para)
        {

            string serverid = para.serverid;
            string communicateid = para.communicateid;
            string deviceid = para.deviceid;
            int charttype = para.charttype;
            string sdate = para.sdate;
            string edate = para.edate;
            int pagesize = para.pagesize;
            int equipmentid = para.equipmentid;
            //曲线指标列表
            string[] serienames = para.selectserie.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ApiSerieConfig serieConfig = new ApiSerieConfig();
            serieConfig.IO_DEVICE_ID = deviceid;
            serieConfig.Series = new List<ApiSerieConfigModel>();
            var paras = ScadaEquipmentParaParameterService.GetByWhere("where EquipmentId=" + equipmentid).ToList();
            var seriecharts = serieConfigService.GetAll();
            for (int i = 0; i < serienames.Length; i++)
            {
                var eqpara = paras.Find(x => x.ParaName.Trim().ToLower() == serienames[i].Trim().ToLower());
                if (eqpara != null)
                {
                    var eqserie = seriecharts.First(x => x.SerieClassify.Trim() == eqpara.SerieType.Trim() && x.SerieName.Trim().ToLower() == eqpara.SerieName.Trim().ToLower());
                    if (eqserie != null)
                    {
                        ApiSerieConfigModel apiSerie = new ApiSerieConfigModel();
                        apiSerie.IO_ID = eqpara.ParaName;
                        apiSerie.SerieColor = eqserie.SerieColor;
                        apiSerie.SerieName = eqpara.ParaName;
                        apiSerie.SerieTitle = eqpara.ParaTitle;
                        apiSerie.SerieType = eqserie.SerieType;
                        apiSerie.SerieWidth = eqserie.SerieWidth;
                        apiSerie.ShowLegend = eqserie.ShowLegend;
                        apiSerie.ShowSymbol = eqserie.ShowSymbol;
                        apiSerie.SymbolColor = eqserie.SymbolColor;
                        apiSerie.SymbolSize = eqserie.SymbolSize;
                        apiSerie.SymbolStep = eqserie.SymbolStep;
                        apiSerie.SymbolType = eqserie.SymbolType;

                        serieConfig.Series.Add(apiSerie);
                    }

                }



            }
            return GetEquipmentHistorySummarySeriesData(serieConfig, serverid, communicateid, deviceid, charttype, sdate, edate, pagesize, para.period, para.method);
        }
        private JsonResult GetEquipmentHistorySummarySeriesData(ApiSerieConfig serieConfig, string serverid, string communicateid, string deviceid, int charttype = 1, string sdate = "", string edate = "", int pagesize = 1000, string timespan="1m",string method= "MEAN")
        {

            //获取对应的曲线配置信息

            List<ApiSerieConfigModel> Items = serieConfig.Series;
            ///初始化曲线对象
            EChartOption chartOption = new EChartOption();
            chartOption.xAxis = new Axis[1];
            chartOption.xAxis[0] = new Axis();
            chartOption.xAxis[0].gridIndex = 1;
            chartOption.xAxis[0].type = "time";
            chartOption.xAxis[0].name = "时间";
            List<Axis> yaxis = new List<Axis>();
            int index = 0;
            string[] legend = new string[Items.Count()];

            foreach (var item in Items)
            {
                yaxis.Add(new Axis() { gridIndex = index, name = item.SerieTitle, type = "value", data = new string[0] });
                legend[index] = item.SerieTitle;
                index++;
            }
            chartOption.legend.data = legend;
            chartOption.yAxis = yaxis.ToArray();
            //初始化对象结束
            if (sdate == null || sdate == "")
                sdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            if (edate == null || edate == "")
                edate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chartOption.series = new Series[Items.Count()];
            int snum = 0;
            foreach (var item in Items)
            {
                chartOption.series[snum] = new Series();
                chartOption.series[snum].id = item.IO_ID;
                chartOption.series[snum].name = item.SerieTitle;
  
                chartOption.series[snum].lineStyle.color = item.SerieColor;
                chartOption.series[snum].lineStyle.width = int.Parse(item.SerieWidth);
               
                chartOption.series[snum].data = new double[0];
                chartOption.series[snum].type = item.SerieType;
                chartOption.series[snum].showSymbol = item.ShowSymbol == "1" ? true : false;
                chartOption.series[snum].symbol = item.SymbolType;
                chartOption.series[snum].symbolSize = int.Parse(item.SymbolSize);

                snum++;
            }
            try
            {
                List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicateid + "' and IO_DEVICE_ID='" + deviceid + "'").ToList();

                string returnFields = "  time";
                #region
                {

                    foreach (var item in Items)
                    {
                        var para = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.IO_ID.Trim().ToLower().ToString());
                        if (para != null && IsNumberIOPoint(para))
                        {
                            returnFields += "," + method + "(field_" + item.IO_ID.Trim().ToLower().ToString() + "_value) as field_" + item.IO_ID.Trim().ToLower().ToString() + "_value";
                        }
                        else
                        {
                            returnFields += ",mean(null) as field_" + item.IO_ID.Trim().ToLower().ToString() + "_value";
                        }

                    }


                }
                #endregion
                InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_HistoryStatics(serverid, communicateid, deviceid, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), 10000, 1, " ASC ",timespan, returnFields);
                if (realResult != null)
                {

                    var datas = realResult.Seres;
                    if (datas != null && datas.Count() > 0)
                    {
                        var onedata = datas.First();
                        int sindex = 0;
                        foreach (var item in Items)
                        {
                            chartOption.series[sindex] = new Series();
                            chartOption.series[sindex].id = item.IO_ID;
                            chartOption.series[sindex].name = item.SerieTitle;
                            chartOption.series[sindex].lineStyle.color = item.SerieColor;
                            chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);
                        
                            chartOption.series[sindex].data = new double[onedata.Values.Count];
                            chartOption.series[sindex].type = item.SerieType;
                            chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                            chartOption.series[sindex].symbol = item.SymbolType;
                            chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
                            sindex++;
                        }
                        string[] axisData = new string[onedata.Values.Count];
                        //获取的数据按照时间先后
                        int dataindex = onedata.Values.Count() - 1;

                        foreach (var value in onedata.Values)
                        {
                            //获取采集时间
                            object objx = onedata.Values[dataindex][onedata.Columns.IndexOf("time")];
                            axisData[dataindex] = objx != null ? objx.ToString() : "";
                            //////////////////////////
                            sindex = 0;
                            foreach (var item in Items)
                            {

                                try
                                {



                                    int recordindex = onedata.Columns.IndexOf("field_" + item.IO_ID.Trim().ToLower() + "_value");
                                    if (recordindex >= 0)
                                    {
                                        object objy = onedata.Values[dataindex][recordindex];
                                        chartOption.series[sindex].data[dataindex] = Convert.ToDouble(objy);
                                        chartOption.series[sindex].id = item.SerieName;
                                    }


                                }
                                catch
                                {

                                }

                                sindex++;
                            }
                            dataindex--;
                        }
                        chartOption.xAxis[0].data = axisData;//设置x轴数据，time格式的数据必须在Axis轴上进行设置


                    }


                }


            }
            catch
            {


            }
            //读取以下的实时数据，从influxDB中读取
            return Json(chartOption, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}