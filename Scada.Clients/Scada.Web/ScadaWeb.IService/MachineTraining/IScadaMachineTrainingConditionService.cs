using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.Model;
using ScadaWeb.Common;

namespace ScadaWeb.IService
{
    public interface IScadaMachineTrainingConditionService : IBaseService<ScadaMachineTrainingConditionModel>
    {
        IEnumerable<ScadaMachineTrainingConditionModel> GetByParentId(long parentid);
    }
}
