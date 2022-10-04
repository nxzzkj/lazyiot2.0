using Newtonsoft.Json;
using Scada.DBUtility;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.Web.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Temporal.WebDbAPI;

namespace ScadaWeb.Web.Areas.Scada.Controllers
{
    public class MachineTrainingController : BaseController
    {
        public IScadaMachineTrainingService ScadaMachineTrainingService { get; set; }
        public IScadaMachineTrainingForecastService ScadaMachineTrainingForecastService { get; set; }
        public IScadaMachineTrainingConditionService ScadaMachineTrainingConditionService { get; set; }
        public IIO_ServerService IO_Server { get; set; }
        public IIO_CommunicateService IO_CommunicateServer { get; set; }

        public IIO_DeviceService IO_DeviceServer { get; set; }
        public IIO_ParaService IO_ParaServer { get; set; }

        public IIO_ParaService ParaService { set; get; }
        public IDeviceGroupService DeviceGroupService { set; get; }
        public IScadaGroupService GroupService { get; set; }
        public ISerieConfigService SerieServer
        {
            set;
            get;
        }
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();
        /// <summary>
        /// 获取训练模型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MachineTrainingList(ScadaMachineTrainingModel model, PageInfo pageInfo)
        {


            var result = ScadaMachineTrainingService.GetListByFilter(model, pageInfo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MachineTrainingDelete(long id)
        {
            var result = ErrorTip("删除失败");
            if (ScadaMachineTrainingService.DeleteByWhere(" where  Id=" + id + ""))
            {
                ScadaMachineTrainingConditionService.DeleteByWhere(" where  TaskId=" + id + "");
                try
                {
                  
                    string path = Server.MapPath("~") + "ScadaCenterServer\\MachineTrainingData\\" + id + "\\";
                    Directory.Delete(path);  
                   
                }
                catch
                {

                }
                result = SuccessTip("删除成功");
            }
            else
            {
                result = ErrorTip("删除失败");
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult MachineTrainingConditionDelete(long id)
        {
            var model = ScadaMachineTrainingConditionService.GetById(id);
            var result = ErrorTip("删除失败");
            if (ScadaMachineTrainingConditionService.DeleteByWhere(" where  Id=" + id + ""))
            {
                if (model != null)
                {
                    try
                    {
                        string fill = Server.MapPath("~") + "ScadaCenterServer\\MachineTrainingData\\" + model.TaskId + "\\" + model.DataFile;
                        FileInfo f = new FileInfo(fill);
                        f.Delete();
                    }
                    catch
                    {

                    }


                }
               
                result = SuccessTip("删除成功");
            }
            else
            {
                result = ErrorTip("删除失败");
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        public override ActionResult Index(long? id)
        {
            ScadaMachineTrainingModel scada = new ScadaMachineTrainingModel();
            base.Index(id);
            return View(scada);
        }
        /// <summary>
        /// 编辑和添加任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MachineTrainingAdd(long id)
        {

            var model = ScadaMachineTrainingService.GetById(id);

            if (model == null)
            {
                model = new ScadaMachineTrainingModel();
                model.Detection = "正常,疑似异常,轻度异常,中度异常,重度异常,严重异常";
                model.Detection5 = "正常";
                model.Detection6 = "疑似异常";
                model.Detection7 = "轻度异常";
                model.Detection8 = "中度异常";
                model.Detection9 = "重度异常";
                model.Detection10 = "严重异常";

            }
            else
            {
                string[] cols = model.Detection.Split(',');
                if (cols.Length == 6)
                {
                    model.Detection5 = cols[0];
                    model.Detection6 = cols[1];
                    model.Detection7 = cols[2];
                    model.Detection8 = cols[3];
                    model.Detection9 = cols[4];
                    model.Detection10 = cols[5];
                }

            }
        
            if (model.UpdateTime == null)
            {
                model.UpdateTime = DateTime.Now;
                model.UpdateUserId = base.Operator.UserId;
            }
            if (model.CreateTime == null)
            {
                model.CreateTime = DateTime.Now;
                model.CreateUserId = base.Operator.UserId;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult MachineTrainingSave(ScadaMachineTrainingModel model)
        {
            model.CreateTime = DateTime.Now;
            model.CreateUserId = Operator.UserId;
            model.UpdateTime = DateTime.Now;
            model.UpdateUserId = Operator.UserId;
            model.IsTrain = 0;
            model.Detection = model.Detection5 + "," + model.Detection6 + "," + model.Detection7 + "," + model.Detection8 + "," + model.Detection9 + "," + model.Detection10;
            model.AlgorithmType = GetEnumCategory<ScadaMachineTrainingAlgorithm>((ScadaMachineTrainingAlgorithm)Enum.Parse(typeof(ScadaMachineTrainingAlgorithm), model.Algorithm));

            if (model.Id == 0)
            {
                model.Id = GUIDToNormalID.GuidToInt();
                var result = ScadaMachineTrainingService.InsertAndId(model) ? SuccessTip("添加成功") : ErrorTip("添加失败");
                return Json(result);

            }
            else
            {
                var result = ScadaMachineTrainingService.UpdateById(model) ? SuccessTip("修改成功") : ErrorTip("修改失败");
                return Json(result);
            }

         

        }
        [HttpGet]
        public JsonResult GetIOServer()
        {
            IEnumerable<IOServerModel> servers = IO_Server.GetAll(null);


            List<SelectOption> _select = new List<SelectOption>();


            if (servers != null && servers.Count() > 0)
            {

                foreach (var item in servers)
                {

                    SelectOption _option = new SelectOption
                    {
                        id = item.SERVER_ID.ToString(),
                        name = item.SERVER_NAME,
                        value = item.SERVER_ID.ToString(),



                    };
                    _select.Add(_option);
                }
            }


            return Json(_select, JsonRequestBehavior.AllowGet);





        }

        [HttpGet]
        public JsonResult GetIOCommunication(string server)
        {
            IEnumerable<IOCommunicateModel> comms = IO_CommunicateServer.GetByWhere(" where  IO_SERVER_ID='" + server + "'   ");


            List<SelectOption> _select = new List<SelectOption>();


            if (comms != null && comms.Count() > 0)
            {

                foreach (var item in comms)
                {

                    SelectOption _option = new SelectOption
                    {
                        id = item.IO_COMM_ID.ToString(),
                        name = item.IO_COMM_NAME + "[" + item.IO_COMM_LABEL + "]",
                        value = item.IO_COMM_ID.ToString(),
                        value1 = item.IO_SERVER_ID.ToString()


                    };
                    _select.Add(_option);
                }
            }


            return Json(_select, JsonRequestBehavior.AllowGet);





        }

        [HttpGet]
        public JsonResult GetIODevice(string server, string communication)
        {
            IEnumerable<IODeviceModel> devices = IO_DeviceServer.GetByWhere(" where  IO_SERVER_ID='" + server + "' and IO_COMM_ID='" + communication + "' ");
            List<SelectOption> _select = new List<SelectOption>();
            if (devices != null && devices.Count() > 0)
            {

                foreach (var item in devices)
                {

                    SelectOption _option = new SelectOption
                    {
                        id = item.IO_DEVICE_ID.ToString(),
                        name = item.IO_DEVICE_NAME + "[" + item.IO_DEVICE_LABLE + "]",
                        value = item.IO_DEVICE_ID.ToString(),
                        value1 = item.IO_SERVER_ID.ToString(),
                        value2 = item.IO_COMM_ID.ToString(),



                    };
                    _select.Add(_option);
                }
            }


            return Json(_select, JsonRequestBehavior.AllowGet);





        }

        [HttpGet]
        public JsonResult GetIOPara(string server, string communication, string device)
        {
            IEnumerable<IOParaModel> paras = ParaService.GetByWhere(" where  IO_SERVER_ID='" + server + "' and IO_COMM_ID='" + communication + "' and IO_DEVICE_ID='" + device + "' ");
            List<SelectOption> _select = new List<SelectOption>();
            if (paras != null && paras.Count() > 0)
            {

                foreach (var item in paras)
                {

                    SelectOption _option = new SelectOption
                    {
                        id = item.IO_NAME.ToString(),
                        name = item.IO_NAME + "[" + item.IO_LABEL + "]",
                        value = item.IO_NAME.ToString(),
                        value1 = item.IO_SERVER_ID.ToString(),
                        value2 = item.IO_COMM_ID.ToString(),
                        value3 = item.IO_DEVICE_ID.ToString()


                    };
                    _select.Add(_option);
                }
            }


            return Json(_select, JsonRequestBehavior.AllowGet);





        }
        [HttpGet]
        public JsonResult GetAlgorithm()
        {
            List<SelectOption> _select = new List<SelectOption>();
            foreach (ScadaMachineTrainingAlgorithm item in Enum.GetValues(typeof(ScadaMachineTrainingAlgorithm)))
            {
                SelectOption _option = new SelectOption
                {
                    id = item.ToString(),
                    name = item.ToString() + "[" + GetEnumDesc<ScadaMachineTrainingAlgorithm>(item) + "]",
                    value = item.ToString()
                };
                _select.Add(_option);


            }


            return Json(_select, JsonRequestBehavior.AllowGet);





        }

        /// <summary>
        /// 获取 enum 的描述信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tField"></param>
        /// <returns></returns>
        public static string GetEnumDesc<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(DescriptionAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (DescriptionAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Description ?? string.Empty;
            }
            return description;
        }
        public static string GetEnumCategory<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(CategoryAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (CategoryAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Category ?? string.Empty;
            }
            return description;
        }
        public ActionResult MachineTrainingCondition(long id)
        {
            var model = ScadaMachineTrainingService.GetById(id);
            return View(model);
        }




        [HttpGet]
        public JsonResult MachineTrainingConditionList(long Id)
        {
            var list = ScadaMachineTrainingConditionService.GetByWhere(" where TaskId=" + Id);
            var result = Pager.Paging(list, list.Count());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑一个样本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MachineTrainingConditionEdit(long id, long taskid)
        {
            var taskmodel = ScadaMachineTrainingService.GetById(taskid);
            var scada = ScadaMachineTrainingConditionService.GetById(id);
            if (scada == null)
                scada = new ScadaMachineTrainingConditionModel()
                {
                    Id = 0,
                    ConditionTitle = "未命名",
                    CreateTime = DateTime.Now,
                    CreateUserId = base.Operator.UserId,
                    DataLength = 0,
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now,
                    Enable = 0,
                    MarkDate = DateTime.Now,
                    MarkTitle = "",
                    Remark = "",
                    Conditions = "",
                    TaskId = taskmodel.Id,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = base.Operator.UserId,
                    DataFile = ""
                };

            scada.TaskId = taskid;
            scada.Properties = taskmodel.Properties;
            scada.ServerID = taskmodel.SERVER_ID;
            scada.CommunicateID = taskmodel.COMM_ID;
            scada.DeviceID = taskmodel.DEVICE_ID;
            scada.TrueText = taskmodel.TrueText;
            scada.FalseText = taskmodel.FalseText;
 
            scada.AlgorithmClassify = taskmodel.AlgorithmType;
            return View(scada);
        }
       
        /// <summary>
        /// 获取某个样本的数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MachineTrainingConditionDataList(long id, long taskid, string startdate, string enddate, int his)
        {
            var model = ScadaMachineTrainingConditionService.GetById(id);
            if (model == null)
            {
                model = new ScadaMachineTrainingConditionModel()
                {
                    StartDate = Convert.ToDateTime(startdate),
                    EndDate = Convert.ToDateTime(enddate),
                    CreateTime = DateTime.Now,
                    CreateUserId = base.Operator.UserId,
                    DataLength = 0,
                    Conditions = "",
                    Enable = 0,
                    DataFile = "demo",
                    Id = GUIDToNormalID.GuidToInt(),
                    TaskId = taskid,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = base.Operator.UserId,
                    ConditionTitle = "",
                    Remark = "",
                    MarkTitle = ""

                };
            }
            model.StartDate = Convert.ToDateTime(startdate);
            model.EndDate = Convert.ToDateTime(enddate);
            var taskmodel = ScadaMachineTrainingService.GetById(taskid);
            model.AlgorithmClassify = taskmodel.AlgorithmType;
            if (taskmodel != null)
            {
                if (model.Id<=0|| his==1)
                {
                    #region 从时序数据库读取
                    string sdate = model.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
                    string edate = model.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                    //最大数据选取10000万行
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(taskmodel.SERVER_ID, taskmodel.COMM_ID, taskmodel.DEVICE_ID, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), 10000, 1, " ASC ");
                    StringBuilder items = new StringBuilder();
                    if (realResult.Seres != null)
                    {
                        items.Append("[");
                        foreach (var s in realResult.Seres)
                        {
                            string[] columns = taskmodel.Properties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            List<int> indexs = new List<int>();
                            for (int i = 0; i < s.Values.Count; i++)
                            {
                                string jsonrow = "";
                                int index = s.Columns.IndexOf("time");
                                object time = s.Values[i][index];
                                jsonrow += "{";
                                jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";
                                if (model.AlgorithmClassify == "二元分类")
                                    jsonrow += ",\"MarkLabel\":\"" + "false\"";
                                else
                                    jsonrow += ",\"MarkLabel\":\"" + "\"";
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

                                items.Append(jsonrow);

                            }
                        }
                        items.Append("]");
                        var result = Pager.Paging2(items.ToString(), realResult.RecordCount);
                        //读取以下的实时数据，从influxDB中读取
                        return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("", "application/text", JsonRequestBehavior.AllowGet);

                    }
                    #endregion
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.DataFile))
                    {
                        #region 从文本文件读取
                        DataTable dataTable = TextTrainReaderEx.Read(Server.MapPath("~") + "ScadaCenterServer\\MachineTrainingData\\"+ model.TaskId+"\\" + model.DataFile, taskmodel.Properties.Split(','));
                        StringBuilder items = new StringBuilder();
                        items.Append("[");
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            object[] objs = dr.ItemArray;
                            string jsonrow = "";

                            object time = dr["DateStampTime"];
                            object label = dr["MarkLabel"];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";
                            jsonrow += ",\"MarkLabel\":\"" + (label != null ? label.ToString() : "") + "\"";

                            foreach (string str in taskmodel.Properties.Split(','))
                            {
                                try
                                {
                                    object cvalue = dr[str].ToString();
                                    jsonrow += ",\"" + str + "\":\"" + (cvalue != null ? cvalue.ToString() : "") + "\"";

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items.Append(jsonrow);
                        }
                        items.Append("]");
                        var result = Pager.Paging2(items.ToString(), dataTable.Rows.Count);
                        //读取以下的实时数据，从influxDB中读取
                        return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                        #endregion
                    }
                }
                return Json("", "application/text", JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json("", "application/text", JsonRequestBehavior.AllowGet);
            }

        }

       public class TrainCondition
        {
            public ScadaMachineTrainingConditionModel model { set; get; }
            public string DataTableJSON { set; get; }
        }

        [HttpPost]
        public ActionResult MachineTrainingConditionSave(TrainCondition tmodel)
        {
            if(tmodel==null)
            {
                return Json(ErrorTip("修改失败"));
            }
            try
            {
           
                ScadaMachineTrainingConditionModel model = tmodel.model;
                var taskmodel = ScadaMachineTrainingService.GetById(model.TaskId);
                model.ServerID = taskmodel.SERVER_ID;
                model.TaskId = taskmodel.Id;
                model.IsTrain = 0;
                DataTable dataTable = ConvertJsonToTable(tmodel.DataTableJSON);
                var result = false;
                model.DataLength = dataTable.Rows.Count;
                List<string> labels = new List<string>();

                if (dataTable.Rows.Count > 0)
                {
                    model.Conditions = "";
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (dataTable.Rows[i]["MarkLabel"] != null && dataTable.Rows[i]["MarkLabel"].ToString() != "")
                        {
                           if(!labels.Contains(dataTable.Rows[i]["MarkLabel"].ToString().Trim()))
                            {
                                labels.Add(dataTable.Rows[i]["MarkLabel"].ToString().Trim());
                            }
                        }
                    }
                    ///建立标签的数值标记
                    
                    for(int i=0;i< labels.Count;i++)
                    {
                        labels[i] = labels[i] + ":" + (i);
                    }
                    model.Conditions = string.Join(",", labels);
                     
                }
                if (model.Id > 0)
                {
                    result = ScadaMachineTrainingConditionService.UpdateById(model);
                }
                else
                {
                    model.Id = GUIDToNormalID.GuidToInt();
                    model.DataFile = "Train_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff") + ".txt";
                    result = ScadaMachineTrainingConditionService.InsertAndId(model);

                }
                if(result)
                {
                    
                 
                    if(taskmodel!=null)
                    {
                     
                        taskmodel.IsTrain = 0;
                        ScadaMachineTrainingService.UpdateById(taskmodel);
                    }
                }
                if (result)
                {
                    string fill = Server.MapPath("~") + "ScadaCenterServer\\MachineTrainingData\\" + model .TaskId+"\\"+ model.DataFile;
                    TextTrainReaderEx.Write(fill, dataTable);
                }

                var res = result ? SuccessTip("修改成功") : ErrorTip("修改失败");
                return Json(res);
            }
            catch
            {
                return Json(ErrorTip("修改失败"));
            }
        }

        public   DataTable ConvertJsonToTable(string jsonValue)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(jsonValue.Replace("\"", "'").Replace("{'MachineTraining':", "").Replace("}]}", "}]"));
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            dataTable.Columns.Remove("LAY_TABLE_INDEX");
            return result;
        }
        /// <summary>
        /// 历史工况查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GeneralHistoryMachineTraining(int? id)
        {
            ScadaMachineTrainingForecastModel scadaMachine = new ScadaMachineTrainingForecastModel();
            return View(scadaMachine);
        }
      
        [HttpGet]
        public JsonResult GeneralHistoryMachineTrainingList(ScadaMachineTrainingForecastModel model, PageInfo pageInfo  )
        {
           
            List<ScadaMachineTrainingForecastModel> events = new List<ScadaMachineTrainingForecastModel>();
            InfluxDBHistoryResult realResult = null;


            realResult = mWebInfluxDbManager.DbQuery_MachineTraingingHistoryForeast(Convert.ToDateTime(model.StartDate), Convert.ToDateTime(model.EndDate),  model.ServerId, model.CommunicationId, model.DeviceId, pageInfo.limit, pageInfo.page);
            if (realResult != null && realResult.Seres != null && realResult.Seres.Count() > 0)
            {
                var s = realResult.Seres.First();
                for (int i = 0; i < s.Values.Count; i++)
                {
                    ScadaMachineTrainingForecastModel mymodel = new ScadaMachineTrainingForecastModel();
                    int index = s.Columns.IndexOf("time");
                    object time = s.Values[i][index];
                    mymodel.ScadaTime = time != null ? time.ToString() : "";
                   

                    index = -1;
                    index = s.Columns.IndexOf("tag_sid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ServerId = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_cid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.CommunicationId = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_did");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DeviceId = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_taskid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.TaskId = v != null ? long.Parse(v.ToString()) : 0;
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_PredictedDate");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastDate = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_PredictedLabel");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastLabel = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_Score");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastScore = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_FeaturesName");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastColumnNames = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_FeaturesValue");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastColumnValues = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_Algorithm");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Algorithm = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_AlgorithmType");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.AlgorithmType = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_SERVER_NAME");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ServerName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_COMM_NAME");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.CommunicationName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_DEVICE_NAME");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DeviceName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_TaskName");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.TaskName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_Remark");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Remark = v != null ? v.ToString() : "";
                    }

                    events.Add(mymodel);
                }

            }

            var result = Pager.Paging(events, realResult.RecordCount);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 实时工况监控
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GeneralMachineTrain(int? id)
        {
            ScadaMachineTrainingForecastModel model = new ScadaMachineTrainingForecastModel();
         
            return View(model);
            
        }
        [HttpGet]
        public JsonResult GeneralRealMachineTrainList(ScadaMachineTrainingForecastModel model, PageInfo pageInfo)
        {
       
         
            List<ScadaMachineTrainingForecastModel> trainmodels = new List<ScadaMachineTrainingForecastModel>();
            InfluxDBHistoryResult realResult = null;
         
            realResult = mWebInfluxDbManager.DbQuery_MachineTraingingForeast("6h", model.ServerId, model.CommunicationId, model.DeviceId, pageInfo.limit, pageInfo.page);

            if (realResult != null && realResult.Seres.Count() > 0)
            {
                var s = realResult.Seres.First();
                for (int i = 0; i < s.Values.Count; i++)
                {
                    ScadaMachineTrainingForecastModel mymodel = new ScadaMachineTrainingForecastModel();

                    int index = s.Columns.IndexOf("time");

                    object time = s.Values[i][index];
                    mymodel.ScadaTime = time != null ? time.ToString() : "";



                    index = -1;
                    index = s.Columns.IndexOf("tag_sid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ServerId = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_cid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.CommunicationId = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("tag_did");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DeviceId = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("tag_taskid");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.TaskId = v != null ? long.Parse(v.ToString()) : 0;
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_PredictedDate");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastDate = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_PredictedLabel");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastLabel = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_Score");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastScore = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_FeaturesName");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastColumnNames = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_FeaturesValue");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ForecastColumnValues = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_Algorithm");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Algorithm = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_AlgorithmType");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.AlgorithmType = v != null ? v.ToString() : "";
                    }

                    index = -1;
                    index = s.Columns.IndexOf("field_SERVER_NAME");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.ServerName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_COMM_NAME");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.CommunicationName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_DEVICE_NAME");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.DeviceName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_TaskName");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.TaskName = v != null ? v.ToString() : "";
                    }
                    index = -1;
                    index = s.Columns.IndexOf("field_Remark");
                    if (index >= 0)
                    {
                        object v = s.Values[i][index];
                        mymodel.Remark = v != null ? v.ToString() : "";
                    }

                    trainmodels.Add(mymodel);
                }

            }

            var result = Pager.Paging(trainmodels, realResult.RecordCount);
            //读取以下10行的实时数据，从influxDB中读取
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}