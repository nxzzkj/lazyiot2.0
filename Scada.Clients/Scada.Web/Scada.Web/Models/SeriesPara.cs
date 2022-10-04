using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaWeb.Web.Models
{
    public class SeriesPara
    {
        public string serverid { set; get; }
        public string communicateid { set; get; }
        public string deviceid { set; get; }

        public int charttype { set; get; }
        public string sdate { set; get; }
        public string edate { set; get; }
        public string method { set; get; }
        public string period { set; get; }
        private int _pagesize = 1000;
        public int pagesize
        {
            set { _pagesize = value; }
            get { return _pagesize; }
        }
        private int _updatecycle = 120;
        public int updatecycle { set { _updatecycle = value; } get { return _updatecycle; } }
        public int equipmentid { set; get; }

       
  
        public string selectserie { set; get; }
        public ApiSerieConfig serieConfig { set; get; }
    }
}