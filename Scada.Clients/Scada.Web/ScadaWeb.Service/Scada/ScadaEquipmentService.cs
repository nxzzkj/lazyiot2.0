using System;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.IRepository;
using System.Collections.Generic;

namespace ScadaWeb.Service
{
    public class ScadaEquipmentService : BaseService<ScadaEquipmentModel>, IScadaEquipmentService
    {
        public IScadaEquipmentRepository ScadaEquipmentRepository { get; set; }
        public dynamic GetListByFilter(ScadaEquipmentModel filter, PageInfo pageInfo)
        {
            return null;
        }

        public IEnumerable<ScadaEquipmentModel> GetListObjectByFilter(ScadaEquipmentModel filter, PageInfo pageInfo, out long total)
        {
            pageInfo.field = "";
            pageInfo.order = " CreateTime  asc ";
            pageInfo.prefix = " ";
            string _where = " ScadaEquipment  where GroupId=" + filter.GroupId;
            if (!string.IsNullOrEmpty(filter.ModelTitle))
            {
                _where += string.Format(" and {0}ModelTitle like '%" + filter.ModelTitle.Trim() + "%'", pageInfo.prefix);
            }
            pageInfo.returnFields = string.Format(" * ", pageInfo.prefix);
            return (IEnumerable<ScadaEquipmentModel>)base.GetPageOjectsUnite(filter, pageInfo, out total, _where);
        }
    }
}
