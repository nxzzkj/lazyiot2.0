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
    public class ScadaHtmlPageService : BaseService<ScadaHtmlPageModel>, IScadaHtmlPageService
    {
        public IScadaGroupRepository ScadaGroupRepository { get; set; }
        public dynamic GetListByFilter(ScadaHtmlPageModel filter, PageInfo pageInfo)
        {
            throw new NotImplementedException();
        }
  

    }
}
