using System;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.IRepository;
using System.Collections.Generic;

namespace ScadaWeb.Service
{
    public class ScadaDBSourceService : BaseService<ScadaDBSourceModel>, IScadaDBSourceService
    {
        public IScadaDBSourceRepository ScadaEquipmentRepository { get; set; }
        public dynamic GetListByFilter(ScadaDBSourceModel filter, PageInfo pageInfo)
        {
            string _where = " where 1=1";
            
            _where = CreateTimeWhereStr(filter.StartEndDate, _where);
            return GetListByFilter(filter, pageInfo, _where);
        }

        
    }
}
