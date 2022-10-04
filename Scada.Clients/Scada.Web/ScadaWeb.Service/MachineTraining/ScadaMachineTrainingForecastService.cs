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
    public class ScadaMachineTrainingForecastService : BaseService<ScadaMachineTrainingForecastModel>, IScadaMachineTrainingForecastService
    {
        public IScadaMachineTrainingForecastRepository ScadaMachineTrainingForecastRepository { get; set; }
 
        public dynamic GetListByFilter(ScadaMachineTrainingForecastModel filter, PageInfo pageInfo)
        {
            return null;
        }
    }
}
