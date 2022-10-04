using ScadaWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaWeb.Web.Areas.Scada.Models
{
    public class WebDesignerModel
    {
        public List<ScadaDBSourceModel> DataSources { set; get; }
        public List<ScadaHtmlPageModel> Pages { set; get; }
    }
}