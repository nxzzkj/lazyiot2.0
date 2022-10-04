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
    public class ScadaMachineTrainingService : BaseService<ScadaMachineTrainingModel>, IScadaMachineTrainingService
    {
        public IScadaMachineTrainingRepository ScadaMachineTrainingRepository { get; set; }
        public dynamic GetListByFilter(ScadaMachineTrainingModel filter, PageInfo pageInfo)
        {
            pageInfo.prefix = " ";
            string _where = " ScadaMachineTrainingModel where 1=1 ";
            if (!string.IsNullOrEmpty(filter.TaskName))
            {
                _where += string.Format(" and {0}TaskName like '%{1}%' ", pageInfo.prefix, filter.TaskName);
            }
            
            _where = CreateTimeWhereStr(filter.StartEndDate, _where, pageInfo.prefix);
            pageInfo.returnFields = string.Format("{0}Id,{0}TaskName,{0}Detection,{0}Algorithm,{0}AlgorithmType,{0}TrainingCycle,{0}ForecastPriod,{0}Properties,{0}CreateTime,{0}Remark,{0}SERVER_NAME,{0}TrueText,{0}FalseText,{0}IsTrain", pageInfo.prefix);
            return GetPageUnite(filter, pageInfo, _where);
        }
    }
}
