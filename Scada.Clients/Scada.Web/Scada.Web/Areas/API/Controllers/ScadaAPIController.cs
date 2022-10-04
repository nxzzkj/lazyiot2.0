using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScadaWeb.Web.Areas.SysSet.Models;
using ScadaWeb.Model;
using ScadaWeb.Common;
using System.IO;
using ScadaWeb.IService;
using ScadaWeb.Web.Controllers;
using ScadaWeb.Web.Areas.API.Models;
using Scada.DBUtility;
using DESEncrypt = Scada.DBUtility.DESEncrypt;
using System.Data;
using ScadaWeb.Web.Models;

namespace ScadaWeb.Web.Areas.API.Controllers
{
    /// <summary>
    /// 系统web端口相关的API 所有方法返回均是Json
    /// </summary>
    public class ScadaAPIController : Controller
    {
        public ActionResult Index()
        {

            return View(new WebModel().GetWebInfo());
        }
        public IUserService UserService { get; set; }
        public ILogonLogService LogonLogService { get; set; }
        public IScadaFlowProjectService ProjectServer { get; set; }
        public IScadaFlowViewService ViewServer { get; set; }
        public sealed class loginModel
        {
            public string username { set; get; }
            public string password { set; get; }
            public string viewId { set; get; }
            public string ReturnViewId { set; get; }
            public string projId { set; get; }


        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(loginModel model)
        {
            string username = model.username, password = model.password, viewId = model.viewId, projId = model.projId;
            AjaxResult error = new AjaxResult { state = ResultType.error.ToString(), message = "登录失败" };
            AjaxResult success = new AjaxResult { state = ResultType.success.ToString(), message = "登录成功" };
            var result = error.SetMsg("禁止登录");
            LogonLogModel logEntity = new LogonLogModel();
            logEntity.LogType = DbLogType.Login.ToString();
            try
            {
                ScadaFlowProjectModel Project = null;
                if (viewId != null && viewId != "" && viewId != "0")
                {
                    ScadaFlowViewModel view = ViewServer.GetByWhere(" where  ViewId='" + viewId + "'").First();
                    if (view != null)
                    {
                        Project = ProjectServer.GetByWhere(" where ProjectId='" + view.ProjectId + "'").First();
                        model.projId = Project.Id.ToString();
                        model.viewId = view.ViewId;
                    }
                    string nickname = "";
                    bool isUser = ProjectServer.LoginOn(username, password, Project.Id.ToString(), out nickname);
                    if (isUser == true)
                    {

                        OperatorModel operatorModel = new OperatorModel();
                        operatorModel.UserId = 0;
                        operatorModel.Account = username;
                        operatorModel.RealName = nickname;
                        operatorModel.HeadIcon = "";
                        operatorModel.RoleId = 0;
                        operatorModel.LoginIPAddress = Net.Ip;
                        operatorModel.LoginIPAddressName = Net.GetLocation(Net.Ip);
                        OperatorFlowProvider.Provider.AddCurrent(operatorModel);
                        logEntity.Account = username;
                        logEntity.RealName = nickname;
                        logEntity.Description = Project.Title + "(" + Project.Id + ")工程的 " + nickname + "登陆成功！";
                        LogonLogService.WriteDbLog(logEntity);
                        result = success.SetMsg(nickname + "登陆成功");
                        result.data = Json(model).Data;

                        return Json(result);
                    }
                    else
                    {
                        result = error.SetMsg("用户名或密码错误");
                        return Json(result);

                    }
                }
                else
                {
                    logEntity.Account = username;
                    logEntity.RealName = username;
                    logEntity.Description = "登录失败，登录页面不存在";
                    LogonLogService.WriteDbLog(logEntity);
                    result = error.SetMsg(logEntity.Description);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                logEntity.Account = username;
                logEntity.RealName = username;
                logEntity.Description = "登录失败，" + ex.Message;
                LogonLogService.WriteDbLog(logEntity);
                result = error.SetMsg(logEntity.Description);

                return Json(result);


            }
        }
        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LoginOut()
        {
            AjaxResult error = new AjaxResult { state = ResultType.error.ToString(), message = "退出登录失败" };
            AjaxResult success = new AjaxResult { state = ResultType.success.ToString(), message = "退出登录成功" };
            LogonLogService.WriteDbLog(new LogonLogModel
            {
                LogType = DbLogType.Exit.ToString(),
                Account = OperatorFlowProvider.Provider.GetCurrent().Account,
                RealName = OperatorFlowProvider.Provider.GetCurrent().RealName,
                Description = "安全退出系统",
            });
            Session.Abandon();
            Session.Clear();
            OperatorFlowProvider.Provider.RemoveCurrent();
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        public IScadaDBSourceService DBSourceService { set; get; }
        //获取通用select查询的
        [HttpGet]
        public JsonResult SelectQuery(WebPageSelect model)
        {
            AjaxResult error = new AjaxResult { state = ResultType.error.ToString(), message = "获取数据异常" };
            AjaxResult success = new AjaxResult { state = ResultType.success.ToString(), message = "获取数据成功" };
            if (Configs.GetValue("SystemRunModel") == "Debug")
            {
                List<WebSelectResult> results = new List<WebSelectResult>();


                for (int i = 0; i < 10; i++)
                {
                    WebSelectResult selectResult = new WebSelectResult();

                    selectResult.title = "第" + (i + 1) + "项";
                    selectResult.value = i.ToString();
                    results.Add(selectResult);

                }

                success.data = results;
                return Json(success, JsonRequestBehavior.AllowGet);
            }
            else
            {





                if (model == null)
                {
                    return Json(error, JsonRequestBehavior.AllowGet);
                }

                if (model == null)
                {
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.SELECT_SQL))
                {
                    error.message = "下拉框未配置SQL查询语句";
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.DATASOURCE))
                {
                    error.message = "未配置查询数据源";
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.SELECT_TEXT_RECORD))
                {
                    error.message = "未配置显示文本字段";
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.SELECT_VALUE_RECORD))
                {
                    error.message = "未配置显示文本字段";
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    var dbModel = DBSourceService.GetById(long.Parse(model.DATASOURCE));
                    if (dbModel == null)
                    {
                        error.message = "未找到数据源";
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                    DataBaseType dataBaseType = DataBaseType.MySQL;
                    switch (dbModel.DBType.Trim().ToLower())
                    {
                        case "mysql":
                            dataBaseType = DataBaseType.MySQL;
                            break;
                        case "oarcle":
                            dataBaseType = DataBaseType.Oracle;
                            break;
                        case "sqlserver":
                            dataBaseType = DataBaseType.SQLServer;
                            break;
                        case "sybase":
                            dataBaseType = DataBaseType.SyBase;
                            break;

                    }

                    ScadaDBHelper scadaDBHelper = new ScadaDBHelper(new ScadaConnectionBase
                    {
                        ConnectionString = DESEncrypt.Encrypt(dbModel.ConnectorString),
                        DataBaseType = dataBaseType,
                        Icon = null,
                        UID = ""



                    });
                    List<WebSelectResult> results = new List<WebSelectResult>();
                    DataSet ds = scadaDBHelper.Query(model.SELECT_SQL);
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            WebSelectResult selectResult = new WebSelectResult();
                            if (ds.Tables[0].Rows[i][model.SELECT_TEXT_RECORD] != null)
                                selectResult.title = ds.Tables[0].Rows[i][model.SELECT_TEXT_RECORD].ToString();
                            if (ds.Tables[0].Rows[i][model.SELECT_VALUE_RECORD] != null)
                                selectResult.value = ds.Tables[0].Rows[i][model.SELECT_VALUE_RECORD].ToString();
                            results.Add(selectResult);
                        }
                    }
                    success.data = results;
                    return Json(success, JsonRequestBehavior.AllowGet);
                }
                catch (Exception emx)
                {
                    error.message = emx.Message;
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpGet]
        public JsonResult TableQuery(WebPageTableModel model)
        {
            AjaxResult error = new AjaxResult { state = ResultType.error.ToString(), message = "获取数据异常" };
            AjaxResult success = new AjaxResult { state = ResultType.success.ToString(), message = "获取数据成功" };

            if (model == null)
            {
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.DATASOURCE))
            {
                error.message = "未配置数据源";
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.TABLE_SQL))
            {
                error.message = "未配置SQL查询语句";
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            if (model.TABLE_COLIMNS==null|| model.TABLE_COLIMNS.Count<=0)
            {
                error.message = "未配置要显示的列";
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            var dbModel = DBSourceService.GetById(long.Parse(model.DATASOURCE));
            if (dbModel == null)
            {
                error.message = "未找到数据源";
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            DataBaseType dataBaseType = DataBaseType.MySQL;
            switch (dbModel.DBType.Trim().ToLower())
            {
                case "mysql":
                    dataBaseType = DataBaseType.MySQL;
                    break;
                case "oarcle":
                    dataBaseType = DataBaseType.Oracle;
                    break;
                case "sqlserver":
                    dataBaseType = DataBaseType.SQLServer;
                    break;
                case "sybase":
                    dataBaseType = DataBaseType.SyBase;
                    break;

            }

            ScadaDBHelper scadaDBHelper = new ScadaDBHelper(new ScadaConnectionBase
            {
                ConnectionString = DESEncrypt.Encrypt(dbModel.ConnectorString),
                DataBaseType = dataBaseType,
                Icon = null,
                UID = ""



            });
            DataSet ds = scadaDBHelper.Query(model.TABLE_SQL);
            if (ds != null && ds.Tables.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                }
            }
         
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        Random random = new Random();
        [HttpPost]
        public JsonResult ScadaViewReal(List<ScadaViewDemoModel> paras)
        {
            for(int i=0;i< paras.Count;i++)
            {
                paras[i].Value = random.Next(0, 100).ToString();
                paras[i].Status ="1";
                paras[i].QualityStamp = "GOOD";
                paras[i].DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            var list= Pager.Paging(paras, paras.Count);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ScadaViewCommand()
        {
            AjaxResult result = new AjaxResult { state = ResultType.error.ToString(), message = "下置命令成功" };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult ScadaViewConnect(SystemLic lic)
        {
            var eDate = Convert.ToDateTime("2022-4-30");
            var sDate = Convert.ToDateTime("2022-3-1");
            var currDate = DateTime.Now;
            AjaxResult result = new AjaxResult { state = ResultType.error.ToString(), message = "您没有授权许可" };
            if(lic!=null&& lic.AuthorizationKey== "AF2856202BF349D7822878E5CD500294S")
            {
                if(currDate>= sDate&& currDate<= eDate)
                {
                    result = new AjaxResult { state = ResultType.success.ToString(), message = "临时授权用户" };
                }
                else
                {
                    result = new AjaxResult { state = ResultType.error.ToString(), message = "您使用的版本已经过期！" };
                }

            }
            else
            {
                result = new AjaxResult { state = ResultType.error.ToString(), message = "您没有授权许可" };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}