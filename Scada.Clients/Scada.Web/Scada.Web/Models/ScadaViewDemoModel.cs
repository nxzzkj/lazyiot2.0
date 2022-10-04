using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaWeb.Web.Models
{
    public class ScadaViewDemoModel
    {
        public string ShapeID { set; get; } = "";
        public string IoPath { set; get; } = "";
        public string IoName { set; get; } = "";
        public string DataType { set; get; } = "";
        public string Unit { set; get; } = "";
        public string Format { set; get; } = "";
        public string Value { set; get; } = "";
        public string DateTime { set; get; } = "";
        public string Status { set; get; } = "0";
        public string QualityStamp { set; get; } = "BAD";
        public string IOStr
        {
            get
            {


                string str = string.Join(",", new List<string> { ShapeID, IoPath, IoName, DataType, Unit, Format });
                return str;
            }
        }
    }
}