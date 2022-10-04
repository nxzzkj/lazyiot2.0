

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Model
{
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
    /// <summary>
    /// 定义一个事件模型
    /// </summary>
    [Serializable]
  public  class ScadaEventModel: ISerializable, IDisposable
    {
        public ScadaEventModel()
        {
            Id = "";
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
        }
        public string Content
        {
            set; get;
        }
        public string Id
        {
            set;get;
        }
        public string Date
        { set; get; }
        public string Event
        {
            set;get;
        }
        public string SERVER_ID { set; get; }
        public string COMM_ID { set; get; }
        public string COMM_NAME { set; get; }
        public string DEVICE_ID { set; get; }
        public string DEVICE_NAME { set; get; }
        public string IO_ID { set; get; }
        public string IO_NAME { set; get; }
        public string IO_LABEL { set; get; }
        public string GetCommandString()
        {
            try
            {
                string str = "TABLE:ScadaEventModel#Id:" + Id;
                str += "#Date:" + Date.Replace("#", "//").Replace(":", "\\");
                str += "#Event:" + Event;
                str += "#SERVER_ID:" + SERVER_ID;
                str += "#COMM_ID:" + COMM_ID;
                str += "#COMM_NAME:" + COMM_NAME;
                str += "#DEVICE_ID:" + DEVICE_ID;
                str += "#DEVICE_NAME:" + DEVICE_NAME;
                str += "#IO_ID:" + IO_ID;
                str += "#IO_NAME:" + IO_NAME;
                str += "#IO_LABEL:" + IO_LABEL;
                str += "#Content:" + Content.Replace("#", "//").Replace(":", "\\").Replace("^"," ");
                
                return str;
            }
            catch
            {
                return "";
            }
        }
     
      
        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ScadaEventModel(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性
            this.Id = (string)info.GetValue("Id", typeof(string));
            this.Date = (string)info.GetValue("Date", typeof(string));
            this.Event = (string)info.GetValue("Event", typeof(string));
            this.SERVER_ID = (string)info.GetValue("SERVER_ID", typeof(string));
            this.COMM_ID = (string)info.GetValue("COMM_ID", typeof(string));
            this.COMM_NAME = (string)info.GetValue("COMM_NAME", typeof(string));
            this.DEVICE_ID = (string)info.GetValue("DEVICE_ID", typeof(string));
            this.DEVICE_NAME = (string)info.GetValue("DEVICE_NAME", typeof(string));
            this.IO_ID = (string)info.GetValue("IO_ID", typeof(string));
            this.IO_NAME = (string)info.GetValue("IO_NAME", typeof(string));
            this.IO_LABEL = (string)info.GetValue("IO_LABEL", typeof(string));
            this.Content = (string)info.GetValue("Content", typeof(string));
            #endregion





        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
          
            info.AddValue("Id", this.Id);
            info.AddValue("Date", this.Date);
            info.AddValue("Event", this.Event);
            info.AddValue("SERVER_ID", this.SERVER_ID);
            info.AddValue("COMM_ID", this.COMM_ID);
            info.AddValue("COMM_NAME", this.COMM_NAME);
            info.AddValue("DEVICE_ID", this.DEVICE_ID);
            info.AddValue("DEVICE_NAME", this.DEVICE_NAME);
            info.AddValue("IO_ID", this.IO_ID);
            info.AddValue("IO_NAME", this.IO_NAME);
            info.AddValue("IO_LABEL", this.IO_LABEL);
            info.AddValue("Content", this.Content);
        }

        #endregion
        public ScadaEventModel Copy()
        {
            ScadaEventModel scadaevent = new ScadaEventModel()
            {
                Id = this.Id,
                Date = this.Date,
                Event = this.Event,
                SERVER_ID = this.SERVER_ID,
                COMM_ID = this.COMM_ID,
                COMM_NAME = this.COMM_NAME,
                DEVICE_ID = this.DEVICE_ID,
                DEVICE_NAME = this.DEVICE_NAME,
                IO_ID = this.IO_ID,
                IO_NAME = this.IO_NAME,
                IO_LABEL = this.IO_LABEL,
                Content = this.Content

            };
            return scadaevent;
        }

        public void Dispose()
        {
            
        }
    }
}
