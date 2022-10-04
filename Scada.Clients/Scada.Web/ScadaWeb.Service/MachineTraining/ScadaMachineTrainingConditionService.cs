using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.Common;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.IRepository;

namespace ScadaWeb.Service
{
    public class ScadaMachineTrainingConditionService : BaseService<ScadaMachineTrainingConditionModel>, IScadaMachineTrainingConditionService
    {
        public IScadaMachineTrainingConditionRepository ScadaMachineTrainingConditionRepository { get; set; }
 
        public dynamic GetListByFilter(ScadaMachineTrainingConditionModel filter, PageInfo pageInfo)
        {
            pageInfo.prefix = " ";
            string _where = " ScadaMachineTrainingCondition where 1=1 ";
            if (!string.IsNullOrEmpty(filter.ConditionTitle))
            {
                _where += string.Format(" and {0}ConditionTitle like '%{1}%' ", pageInfo.prefix, filter.ConditionTitle);
            }

            _where = CreateTimeWhereStr(filter.StartEndDate, _where, pageInfo.prefix);
            pageInfo.returnFields = string.Format("  * ", pageInfo.prefix);
            return GetPageUnite(filter, pageInfo, _where);
        }
        public IEnumerable<ScadaMachineTrainingConditionModel> GetByParentId(long parentid)
        {
            return GetByWhere(" where TaskId='" + parentid + "'");
        }
    }
}
