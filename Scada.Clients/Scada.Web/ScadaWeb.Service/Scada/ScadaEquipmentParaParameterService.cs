using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.IRepository;

namespace ScadaWeb.Service
{

    public class ScadaEquipmentParaParameterService : BaseService<ScadaEquipmentParaParameterModel>, IScadaEquipmentParaParameterService
    {
        public IScadaEquipmentParaParameterRepository ScadaEquipmentParaParameterRepository { get; set; }
        public dynamic GetListByFilter(ScadaEquipmentParaParameterModel filter, PageInfo pageInfo)
        {
            throw new NotImplementedException();
        }


    }
}
