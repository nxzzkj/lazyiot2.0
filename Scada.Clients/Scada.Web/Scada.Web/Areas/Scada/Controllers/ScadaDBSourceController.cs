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
    public class ScadaDBSourceController : BaseController
    {
        public IScadaDBSourceService DBSourceService { set; get; }
        // GET: Scada/ScadaDBSource
        public override ActionResult Index(long? id)
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
            ScadaDBSourceModel scadaDBSource = new ScadaDBSourceModel();
 
            return View(scadaDBSource);
        }
        [HttpGet]
        public JsonResult DeleteDBSource(int id)
        {
            try
            {
                var res = ErrorTip("删除数据源失败");
                var result = DBSourceService.DeleteById(id) ? SuccessTip("删除数据源成功") : ErrorTip("删除数据源失败");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(ErrorTip("删除数据源失败"), JsonRequestBehavior.AllowGet);
            }


       
        }
        [HttpGet]
        public JsonResult DBList()
        {
            //获取所有的设备
            var dblist = DBSourceService.GetAll();

            var result = new { code = 0, count = dblist.Count(), data = dblist };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditDBSource(long id)
        {
            var lkmodel = DBSourceService.GetById(id);
            if (lkmodel != null)
            {
               switch(lkmodel.DBType.Trim().ToLower())
                {
                    case "sqlserver":
                       return RedirectToAction("AddSqlServer",new { id=id});
                    case "oracle":
                        return RedirectToAction("AddOracle", new { id = id });
                    case "mysql":
                        return RedirectToAction("AddMysql", new { id = id });
                    case "sybase":
                        return RedirectToAction("AddSybase", new { id = id });
                }
            }
            return View();
        }
        public ActionResult AddSqlServer(long? id)
        {
            SqlServerModel model = new SqlServerModel();
            if(id!=null)
            {
                var lkmodel = DBSourceService.GetById(id.Value);
                if(lkmodel!=null)
                {
                    model.Id = lkmodel.Id;
                    model.SetConnectionString(lkmodel.ConnectorString);
                    model.DBTitle = lkmodel.DBTitle;
                }
            }
         
            return View(model);

        }
        [HttpPost]
        public JsonResult AddSqlServer(SqlServerModel model)
        {
            var result = ErrorTip("添加数据源失败");
            try
            {
                if (string.IsNullOrEmpty(model.Server))
                {
                    result = ErrorTip("添加数据源失败,服务器IP地址不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Database))
                {
                    result = ErrorTip("添加数据源失败,数据库名称不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.UserId))
                {
                    result = ErrorTip("添加数据源失败,用户名不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    result = ErrorTip("添加数据源失败,密码不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                ScadaDBSourceModel scadaDB = new ScadaDBSourceModel
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = Operator.UserId,
                    DBType = "SqlServer",
                    DBTitle = model.DBTitle,
                    Id = model.Id,
                    SortCode = 0,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = Operator.UserId,
                    ConnectorString = model.GetConnectionString()

                };

                if(scadaDB.Id>0)
                {
                    DBSourceService.UpdateById(scadaDB);
                }
                else
                {
                    DBSourceService.Insert(scadaDB);
                }
               
            }
            catch
            {
                result = ErrorTip("添加数据源失败");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = SuccessTip("添加数据源成功");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
  
        public ActionResult AddOracle(long? id)
        {


            OracleModel model = new OracleModel();
            if (id != null)
            {
                var lkmodel = DBSourceService.GetById(id.Value);
                if (lkmodel != null)
                {
                    model.Id = lkmodel.Id;
                    model.SetConnectionString(lkmodel.ConnectorString);
                    model.DBTitle = lkmodel.DBTitle;
                }
            }
            return View(model);
           
        }
        [HttpPost]
        public JsonResult AddOracle(OracleModel model)
        {
            var result = ErrorTip("添加数据源失败");
            try
            {
                if (string.IsNullOrEmpty(model.DataSource))
                {
                    result = ErrorTip("添加数据源失败,服务名不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.UserId))
                {
                    result = ErrorTip("添加数据源失败,用户名称不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
      
                if (string.IsNullOrEmpty(model.Password))
                {
                    result = ErrorTip("添加数据源失败,密码不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                ScadaDBSourceModel scadaDB = new ScadaDBSourceModel
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = Operator.UserId,
                    DBType = "Oracle",
                    DBTitle = model.DBTitle,
                    Id = model.Id,
                    SortCode = 0,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = Operator.UserId,
                    ConnectorString = model.GetConnectionString()

                };
                if(scadaDB.Id>0)
                {
                    DBSourceService.UpdateById(scadaDB);
                }
                else
                {
                    DBSourceService.Insert(scadaDB);
                }
            
            }
            catch
            {
                result = ErrorTip("添加数据源失败");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = SuccessTip("添加数据源成功");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        ///server=127.0.0.1;port=3306;uid=root;pwd=123456;database=ScadaWeb;charset=utf8;default command timeout=300;TreatTinyAsBoolean=false
 
        public ActionResult AddMysql(long? id)
        {
            MySqlModel model = new MySqlModel();
            if (id != null)
            {
                var lkmodel = DBSourceService.GetById(id.Value);
                if (lkmodel != null)
                {
                    model.Id = lkmodel.Id;
                    model.SetConnectionString(lkmodel.ConnectorString);
                    model.DBTitle = lkmodel.DBTitle;
                }
            }
            return View(model);
       
        }
        [HttpPost]
        public JsonResult AddMysql(MySqlModel model)
        {
            var result = ErrorTip("添加数据源失败");
            try
            {
                if (string.IsNullOrEmpty(model.Server))
                {
                    result = ErrorTip("添加数据源失败,服务器IP不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.UserId))
                {
                    result = ErrorTip("添加数据源失败,用户名称不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    result = ErrorTip("添加数据源失败,密码不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                ScadaDBSourceModel scadaDB = new ScadaDBSourceModel
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = Operator.UserId,
                    DBType = "Mysql",
                    DBTitle = model.DBTitle,
                    Id = model.Id,
                    SortCode = 0,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = Operator.UserId,
                    ConnectorString = model.GetConnectionString()

                };
                if(scadaDB.Id>0)
                {
                    DBSourceService.UpdateById(scadaDB);
                }
                else
                {
                    DBSourceService.Insert(scadaDB);
                }
            
            }
            catch
            {
                result = ErrorTip("添加数据源失败");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = SuccessTip("添加数据源成功");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Sybase
     
        public ActionResult AddSybase(long? id)
        {
            SybaseModel model = new SybaseModel();
            if (id != null)
            {
                var lkmodel = DBSourceService.GetById(id.Value);
                if (lkmodel != null)
                {
                    model.Id = lkmodel.Id;
                    model.SetConnectionString(lkmodel.ConnectorString);
                    model.DBTitle = lkmodel.DBTitle;
                }
            }
            return View(model);
      
        }
        [HttpPost]
        public JsonResult AddSybase(SybaseModel model)
        {
            var result = ErrorTip("添加数据源失败");
            try
            {
                if (string.IsNullOrEmpty(model.Server))
                {
                    result = ErrorTip("添加数据源失败,服务器IP不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.UserId))
                {
                    result = ErrorTip("添加数据源失败,用户名称不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    result = ErrorTip("添加数据源失败,密码不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                ScadaDBSourceModel scadaDB = new ScadaDBSourceModel
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = Operator.UserId,
                    DBType = "Sybase",
                    DBTitle = model.DBTitle,
                    Id = model.Id,
                    SortCode = 0,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = 0,
                    ConnectorString = model.GetConnectionString()

                };
                if(scadaDB.Id>0)
                {
                    DBSourceService.UpdateById(scadaDB);
                }
                else
                {
                    DBSourceService.Insert(scadaDB);
                }
          
            }
            catch
            {
                result = ErrorTip("添加数据源失败");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = SuccessTip("添加数据源成功");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}