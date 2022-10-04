using ScadaWeb.IService;
using ScadaWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Service
{
    public class WellOrganizeService : BaseService<WellOrganizeModel>, IWellOrganizeService
    {
        public bool DeleteByWellId(long WellId)
        {
            string where = "where WellId="+ WellId;
            return DeleteByWhere(where);
        }

        public dynamic GetListByFilter(WellOrganizeModel filter, PageInfo pageInfo)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取组织机构下的所有关联的井
        /// </summary>
        /// <param name="OrganizeId"></param>
        /// <returns></returns>
        public IEnumerable<WellOrganizeModel> GetListByOrganizeId(long OrganizeId)
        {
            string where = "where OrganizeId=@OrganizeId";
            return GetByWhere(where, new { OrganizeId = OrganizeId });
        }
        /// <summary>
        /// 获取井关联的组织机构
        /// </summary>
        /// <param name="WellId"></param>
        /// <returns></returns>

        public IEnumerable<WellOrganizeModel> GetListByWellId(long WellId)
        {
            string where = "where WellId=@WellId";
            return GetByWhere(where, new { WellId = WellId });
        }
    }
}
