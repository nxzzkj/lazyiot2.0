

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
   
    public   class ScadaEventModel
    {

        public ScadaEventModel()
        {
            Id = "";
            Date = "";
            Event = "";
            SERVER_ID = "";
            COMM_ID = "";
            COMM_NAME = "";
            DEVICE_ID = "";
            DEVICE_NAME = "";
            IO_ID = "";
            IO_NAME = "";
            IO_LABEL = "";
            Content = "";
         
        
            StartDate = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
            EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public string time { set; get; }


    
        public IEnumerable<DeviceGroupModel> AllDeviceList { set; get; }

  
        public string SearchKey
        { set; get; }
      
        public string StartDate
        {
            set;
            get;
        }
        public string EndDate
        {
            set;
            get;
        }
        public string Content
        {
            set; get;
        }
        public string Id
        {
            set; get;
        }
        public string Date
        { set; get; }
        public string Event
        {
            set; get;
        }
        public string SERVER_ID { set; get; }
        public string COMM_ID { set; get; }
        public string COMM_NAME { set; get; }
        public string DEVICE_ID { set; get; }
        public string DEVICE_NAME { set; get; }
        public string IO_ID { set; get; }
        public string IO_NAME { set; get; }
        public string IO_LABEL { set; get; }
     
        
    }
}
