using Newtonsoft.Json;
using Scada.DBUtility;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.Web.Areas.Scada.Models;
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
    public class ScaddDesignerController : BaseController
    {
        public IScadaMachineTrainingService ScadaMachineTrainingService { get; set; }
        public IScadaMachineTrainingForecastService ScadaMachineTrainingForecastService { get; set; }
        public IScadaMachineTrainingConditionService ScadaMachineTrainingConditionService { get; set; }
        public IIO_ServerService  StationServer { get; set; }
        public IIO_CommunicateService CommunicateServer { get; set; }
        public IScadaHtmlPageService ScadaHtmlPageServer { set; get; }
        public IIO_DeviceService DeviceServer { get; set; }
        public IIO_ParaService ParaService { set; get; }
        public IDeviceGroupService DeviceGroupService { set; get; }
        public ISerieConfigService SerieServer
        {
            set;
            get;
        }

        public IScadaHtmlPageService PageServer { set; get; }
        public IScadaDBSourceService DBSourceService { set; get; }

        [HttpGet]
        public JsonResult GetIOServer()
        {
            IEnumerable<IOServerModel> servers = StationServer.GetAll(null);


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
            IEnumerable<IOCommunicateModel> comms = CommunicateServer.GetByWhere(" where  IO_SERVER_ID='" + server + "'   ");


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
            IEnumerable<IODeviceModel> devices = DeviceServer.GetByWhere(" where  IO_SERVER_ID='" + server + "' and IO_COMM_ID='" + communication + "' ");
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
        [HttpGet]
        public JsonResult GetPages()
        {
            var pages = ScadaHtmlPageServer.GetAll();
            var result = new { code = 0, count = pages.Count(), data = pages };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPage(int id)
        {
            var page = ScadaHtmlPageServer.GetById(id);
            return Json(page, JsonRequestBehavior.AllowGet);

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveJsContent(ScadaHtmlPageModel page)
        {
            try
            {
                var expage = ScadaHtmlPageServer.GetById(page.Id);
                expage.JsContent = page.JsContent.Replace("\\n", "\n").Replace("\\\"", "\"");
                if (ScadaHtmlPageServer.UpdateById(expage))
                {
                    WriteJs(expage);
                    var result = new { code = 0, count = 0 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = 1, count = 0 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                var result = new { code = 1, count = 0 };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        private void WriteFile(ScadaHtmlPageModel page)
        {
            using (StreamWriter sw = new StreamWriter(Server.MapPath("/")+ "WebTemplate/"+ page.PageUid+".htm",false, System.Text.Encoding.UTF8))
            {

                if (!string.IsNullOrEmpty(page.html))
                {
                    string headstr = @"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
    <meta name='title' content='"+ page .PageTitle+ @"'>
    <meta name='description' content='" + page.PageTitle + @"'>
    <meta name='keywords' content='" + page.PageTitle + @"'>
    <title>" + page.PageTitle + @"</title>
    <!-- Le styles -->
    <link href='../Content/css/bootstrap-combined.min.css' rel='stylesheet'>
    <link href='../Content/css/layoutit.css' rel='stylesheet'>

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src='../Content/js/html5shiv.js'></script>
        <![endif]-->


    <script type='text/javascript' src='../Content/js/jquery-3.4.1.js'></script>
    <!--[if lt IE 9]>
    <script type='text/javascript' src='http://code.jquery.com/jquery-1.9.1.min.js'></script>
    <![endif]-->
    <script type='text/javascript' src='../Content/js/bootstrap.min.js'></script>
    <script type='text/javascript' src='../Content/js/jquery-ui.js'></script>
    <script type='text/javascript' src='../Content/js/jquery.ui.touch-punch.min.js'></script>
    <script type='text/javascript' src='../Content/js/jquery.htmlClean.js'></script>
    <script type='text/javascript' src='../Content/lib/echarts/echarts.min.js'></script>
   <script type='text/javascript' src='js/" + page.PageUid + @".js'></script>
</head>
<body style='min-height: 460px; cursor: auto;' >
<div class='container-fluid'>";
                    string endstr = @"</div>
</body>
</html>";
                    sw.Write(headstr+" "+page.html.Replace("\\n", "\n").Replace("\\\"", "\"")+" "+ endstr);

                }
                  
            }
         
        }
        private void WriteJs(ScadaHtmlPageModel page)
        {
            using (StreamWriter sw = new StreamWriter(Server.MapPath("/") + "WebTemplate/js/" + page.PageUid + ".js", false, Encoding.UTF8))
            {
                if (!string.IsNullOrEmpty(page.JsContent))
                    sw.Write(page.JsContent.Replace("\\n", "\n").Replace("\\\"", "\""));
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SavePage(ScadaHtmlPageModel page)
        {
            var expage = ScadaHtmlPageServer.GetById(page.Id);
            if(expage != null)
            {
                expage.LayoutData = page.LayoutData;
                expage.html= page.html.Replace("\\n", "\n").Replace("\\\"", "\"");
                if (ScadaHtmlPageServer.UpdateById(expage))
                {
                    WriteFile(expage);
                    WriteJs(expage);
                    var result = new { code = 0, count = 0 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = 1, count = 0 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = new { code = 1, count = 0};
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public JsonResult AddPage(string title, string remark)
        {
            try
            {
                ScadaHtmlPageModel scadaHtmlPage = new ScadaHtmlPageModel
                {

                    CreateTime = DateTime.Now,
                    CreateUserId = Operator.UserId,
                    UpdateUserId = Operator.UserId,
                    UpdateTime = DateTime.Now,
                    LayoutData = "",
                    PageTitle = title,
                    PageUrl = "",
                    Remark = remark,
                    PageUid = GUIDToNormalID.GuidToLongID()

                };
                var res = ScadaHtmlPageServer.InsertReturnId(scadaHtmlPage);
                WriteFile(scadaHtmlPage);
                scadaHtmlPage.Id = res;
                var result = new { code = 0, count = 1, data = scadaHtmlPage };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { code =1, count =0};
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DataSourceTest(long? id)
        {
            var result = ErrorTip("请选择数据");
            if (id == null)
                return Json(result, JsonRequestBehavior.AllowGet);

            var datasource = DBSourceService.GetById(id.Value);
            if (datasource == null)
            {
                result = ErrorTip("您选择的数据源不存在");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            switch(datasource.DBType.Trim().ToLower())
            {

                case "oracle":
                    {
                        ScadaDBHelper scadaDBHelper = new ScadaDBHelper(new ScadaConnectionBase
                        {
                            ConnectionString = DESEncrypt.Encrypt(datasource.ConnectorString),
                            DataBaseType = DataBaseType.Oracle


                        });
                        result = scadaDBHelper.ExecuteTest() ? SuccessTip("数据源链接测试成功"): ErrorTip("数据源配置不正确,无法链接");
                    }
                    break;
                case "sqlserver":
                    {
                        ScadaDBHelper scadaDBHelper = new ScadaDBHelper(new ScadaConnectionBase
                        {
                            ConnectionString = DESEncrypt.Encrypt( datasource.ConnectorString),
                            DataBaseType = DataBaseType.SQLServer


                        });
                        result = scadaDBHelper.ExecuteTest() ? SuccessTip("数据源链接测试成功") : ErrorTip("数据源配置不正确,无法链接");
                    }
                    break;
                case "mysql":
                    {
                        ScadaDBHelper scadaDBHelper = new ScadaDBHelper(new ScadaConnectionBase
                        {
                            ConnectionString = DESEncrypt.Encrypt(datasource.ConnectorString),
                            DataBaseType = DataBaseType.MySQL


                        });
                        result = scadaDBHelper.ExecuteTest() ? SuccessTip("数据源链接测试成功") : ErrorTip("数据源配置不正确,无法链接");
                    }
                    break;
                case "sybase":
                    {
                        ScadaDBHelper scadaDBHelper = new ScadaDBHelper(new ScadaConnectionBase
                        {
                            ConnectionString = DESEncrypt.Encrypt(datasource.ConnectorString),
                            DataBaseType = DataBaseType.SyBase


                        });
                        result = scadaDBHelper.ExecuteTest() ? SuccessTip("数据源链接测试成功") : ErrorTip("数据源配置不正确,无法链接");
                    }
                    break;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [ValidateInput(false)]
        public override ActionResult Index(long? id)
        {
            ViewBag.PageId = 0;
            WebDesignerModel webDesigner = new WebDesignerModel
            {
                DataSources = DBSourceService.GetAll().ToList(),
                Pages = PageServer.GetAll().ToList()


            };
              base.Index(id);
            return View(webDesigner);

        }
    }
}