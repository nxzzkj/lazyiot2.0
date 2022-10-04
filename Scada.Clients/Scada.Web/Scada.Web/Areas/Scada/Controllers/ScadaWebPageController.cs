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
    public class ScadaWebPageController : BaseController
    {
        
        public IScadaHtmlPageService PageServer { set; get; }
 
        // GET: Scada/ScadaWebPage
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
            return View();
        }
        [HttpGet]
        public JsonResult List()
        {
            //获取所有的设备
            var dblist = PageServer.GetAll();

            var result = new { code = 0, count = dblist.Count(), data = dblist };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(long? id)
        {
            if (id == null)
                id = 0;
            ScadaHtmlPageModel pageModel = PageServer.GetById(id.Value);
            if (pageModel == null)
                pageModel = new ScadaHtmlPageModel()
                {
                    PageUid = GUIDToNormalID.GuidToLongID(),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    CreateUserId = Operator.UserId,
                    UpdateUserId = Operator.UserId,
                    Id = 0,
                    PageUrl = ""

                };
            return View(pageModel);
        }
      
        [HttpPost]
        public JsonResult Add(ScadaHtmlPageModel model)
        {
            var result = ErrorTip("添加页面失败");
            try
            {
                if (string.IsNullOrEmpty(model.PageTitle))
                {
                    result = ErrorTip("页面标题不能为空");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                ScadaHtmlPageModel scadaDB = PageServer.GetById(model.Id);
                if(scadaDB!=null)
                {
                    scadaDB = new ScadaHtmlPageModel
                    {
                        CreateTime = DateTime.Now,
                        CreateUserId = Operator.UserId,
                        PageUid = model.PageUid,
                        PageTitle = model.PageTitle,
                        Id = model.Id,
                        SortCode = 0,
                        UpdateTime = DateTime.Now,
                        UpdateUserId = Operator.UserId
                    };
                }
                else
                {
                    scadaDB.PageTitle = model.PageTitle;
                    scadaDB.Remark = model.Remark;
                }
             

                if (scadaDB.Id > 0)
                {
                    PageServer.UpdateById(scadaDB);
                }
                else
                {
                    PageServer.Insert(scadaDB);
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
        [HttpGet]
        public JsonResult Delete(int id)
        {
            try
            {
                var res = ErrorTip("删除页面失败");
                var result = PageServer.DeleteById(id) ? SuccessTip("删除页面成功") : ErrorTip("删除页面失败");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(ErrorTip("删除页面失败"), JsonRequestBehavior.AllowGet);
            }



        }
    }
}