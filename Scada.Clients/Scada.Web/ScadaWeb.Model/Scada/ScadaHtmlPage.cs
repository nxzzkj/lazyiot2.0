using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
    [Table("ScadaHtmlPage")]
    public  class ScadaHtmlPageModel: Entity
    {
        public string PageTitle { set; get; }
        public string PageUrl { set; get; }
        public string Remark { set; get; }
        public string PageUid { set; get; }
        public string LayoutData { set; get; }
        public string JsContent { set; get; }
        [Computed]
        public string html { set; get; }
    }
}
