using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.Model;

namespace ScadaWeb.IService
{
    public interface IScadaEquipmentService : IBaseService<ScadaEquipmentModel>
    {
        IEnumerable<ScadaEquipmentModel> GetListObjectByFilter(ScadaEquipmentModel filter, PageInfo pageInfo, out long total);

    }
}
