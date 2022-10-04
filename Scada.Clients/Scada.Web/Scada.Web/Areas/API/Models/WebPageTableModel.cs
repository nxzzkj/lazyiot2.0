using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaWeb.Web.Areas.API.Models
{
    public class WebPageBaseModel
    {
        public string ELE_ID { set; get; }
    }
    public class WebPageTableColumn : WebPageBaseModel
    {
        public string value { set; get; }
        public string title { set; get; }
    }
    public class WebPageSelect : WebPageBaseModel
    {
      
        public string SELECT_SQL { set; get; }
        public string SELECT_VALUE_RECORD { set; get; }
        public string SELECT_TEXT_RECORD { set; get; }
        public string DATASOURCE { set; get; }
    }
    public class WebPageTableSelect : WebPageBaseModel
    {
        public bool SELECT_ENABLE { set; get; } = true;
        public string SELECT_SQL { set; get; }
        public string SELECT_VALUE_RECORD { set; get; }
        public string SELECT_TEXT_RECORD { set; get; }
        public string SELECT_TABLE_RECORD { set; get; }
        public string DATASOURCE { set; get; }
    }
    public class WebSelectResult
    {
        public string value { set; get; }
        public string title { set; get; }
    }
    public class WebPageTextBox: WebPageBaseModel
    {
        public bool TEXT_ENABLE { set; get; }
        public bool TABLE_RECORD { set; get; }
    }
    public class WebPageDateRange : WebPageBaseModel
    {
        public string ELE2_ID { set; get; }
        public bool TEXT_ENABLE { set; get; }
        public bool TABLE_RECORD { set; get; }
    }
    public class WebPageTableModel: WebPageBaseModel
    {
        public string DATASOURCE { set; get; }
        public string TABLE_KEY_ID_RECORD { set; get; }
        public int TABLE_HEIGHT { set; get; } = 500;
        public string TABLE_TITLE { set; get; } = "动态表格标题";
       
        public string BUTTON_ID { set; get; }
        public bool TABLE_SHOW_PAGE { set; get; } = true;
        public bool TABLE_SHOW_TOOL { set; get; } = true;
        public int TABLE_PAGESIZE { set; get; } = 100;
        public string TABLE_SQL { set; get; }
        public List<WebPageTableColumn> TABLE_COLIMNS { set; get; } = new List<WebPageTableColumn>();
        public WebPageTableSelect FILTER_SELECT1 { set; get; } = new WebPageTableSelect();
        public WebPageTableSelect FILTER_SELECT2 { set; get; } = new WebPageTableSelect();
        public bool FILTER_ENABLE { set; get; } = true;
        public string FILTER_SELECT1_SQL { set; get; }     
        public WebPageTextBox FILTER_KEY { set; get; }
        public WebPageDateRange FILTER_DATE_RANGE { set; get; }
        public bool FILTER_SELECT1_CHANGE_SELECT2 { set; get; }

        public WebPageTableFilter FILTER { set; get; } = new WebPageTableFilter();


    }
    public class WebPageTableResult
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int total { set; get; } = 0;
        public int limit { set; get; } = 0;
        public int page { set; get; } = 0;
        public int offset { set; get; } = 0;
        public object data { set; get; }
  

    }

    public class WebPageTableFilter
    {
        public int rows { set; get; } = 0;
        public int page { set; get; } = 0;
        public bool allowpage { set; get; } = true;
        public string sort { set; get; } = "";
        public string sortOrder { set; get; } = "asc";

        public string select1 { set; get; }
        public string select2 { set; get; }
        public string key { set; get; }
        public string startdate { set; get; }
        public string enddate { set; get; }
    }
}