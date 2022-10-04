using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.Model;
using ScadaWeb.Common;

namespace ScadaWeb.IService
{
    public interface IWellOrganizeService : IBaseService<WellOrganizeModel>
    {
        
        /// <summary>
        /// 根据菜单获得列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        IEnumerable<WellOrganizeModel> GetListByWellId(long WellId);
        IEnumerable<WellOrganizeModel> GetListByOrganizeId(long OrganizeId);
        bool DeleteByWellId(long WellId);
    }
}
