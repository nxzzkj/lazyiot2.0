using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScadaWeb.Model
{
    [Table("ScadaDBSource")]
    public  class ScadaDBSourceModel: Entity
    {
        public string DBType { set; get; }
        public string ConnectorString { set; get; }
        public string DBTitle { set; get; }
    }
}
