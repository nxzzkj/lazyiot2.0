using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScadaWeb.Common;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.Web.Controllers;


namespace ScadaWeb.Web.Areas.Permissions.Controllers
{
    public class FlowProjectController : BaseController
    {
        public IScadaFlowProjectService FlowProjectService { get; set; }
        public IScadaFlowViewService FlowViewService { get; set; }
        // GET: Permissions/User
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
            var datas = FlowProjectService.GetAll();

            var result = new { code = 0, count = datas.Count(), data = datas };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
       
        public ActionResult Edit(int id)
        {
           
            var model = FlowProjectService.GetById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ScadaFlowProjectModel model)
        {
            var newmodel = FlowProjectService.GetById(model.Id);
            newmodel.UpdateTime = DateTime.Now;
            newmodel.UpdateUserId = Operator.UserId;
            newmodel.CreateTime = DateTime.Now;
            newmodel.CreateUserId = Operator.UserId;
            newmodel.Title = model.Title;
        
            var result = FlowProjectService.UpdateById(newmodel) ? SuccessTip("修改成功") : ErrorTip("修改失败");
            return Json(result);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            var existModel = FlowProjectService.GetById(id);
            if(existModel==null)
            {
                return Json(ErrorTip("删除失败,工程不存在"), JsonRequestBehavior.AllowGet);
            }
            bool res = FlowProjectService.DeleteById(id);
            if(res)
            {
                FlowViewService.DeleteByWhere(" where ProjectId='"+ existModel.ProjectId + "'");
            }
            var result = res ? SuccessTip("删除成功") : ErrorTip("删除失败");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
      
 
    }
}