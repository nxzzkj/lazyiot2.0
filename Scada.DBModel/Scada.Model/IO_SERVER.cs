using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

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
    /// IO_SERVER:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
	public partial class IO_SERVER:ISerializable, IDisposable
    {
		public IO_SERVER()
		{}
		#region Model
		private string _server_id="";
		private string _server_name = "";
		private int _server_status=1;
		private string _server_ip = "";
		private string _server_createdate=DateTime.Today.ToString("yyyy-MM-dd");
		private string _server_remark = "";
        private string _CENTER_IP = "";
        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IO_SERVER(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性
            this._server_id = (string)info.GetValue("_server_id", typeof(string));
            this._server_name = (string)info.GetValue("_server_name", typeof(string));
            this._server_status = (int)info.GetValue("_server_status", typeof(int));
            this._server_ip = (string)info.GetValue("_server_ip", typeof(string));
            this._server_createdate = (string)info.GetValue("_server_createdate", typeof(string));
            this._server_remark = (string)info.GetValue("_server_remark", typeof(string));
            this._CENTER_IP = (string)info.GetValue("_CENTER_IP", typeof(string));
            #endregion





        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public   void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_server_id", this._server_id);
            info.AddValue("_server_name", this._server_name);
            info.AddValue("_server_status", this._server_status);
            info.AddValue("_server_ip", this._server_ip);
            info.AddValue("_server_createdate", this._server_createdate);
            info.AddValue("_server_remark", this._server_remark);
            info.AddValue("_CENTER_IP", this._CENTER_IP);
            
        }

        #endregion
        public IO_SERVER Copy()
        {
            IO_SERVER server = new IO_SERVER() {
                 SERVER_CREATEDATE= this.SERVER_CREATEDATE,
                 SERVER_ID = this.SERVER_ID,
                 SERVER_IP = this.SERVER_IP,
                 SERVER_NAME = this.SERVER_NAME,
                 SERVER_REMARK = this.SERVER_REMARK,
                 SERVER_STATUS = this.SERVER_STATUS,
                CENTER_IP=this.CENTER_IP

            };
            return server;
        }
        public string GetCommandString()
        {
            string str = "TABLE:IO_SERVER#SERVER_CREATEDATE:" + SERVER_CREATEDATE;
            str += "#SERVER_ID:" + SERVER_ID;
            str += "#SERVER_IP:" + SERVER_IP;
            str += "#SERVER_NAME:" + SERVER_NAME;
            str += "#SERVER_REMARK:" + SERVER_REMARK.Replace("#", "//").Replace(":", "\\");
            str += "#SERVER_STATUS:" + SERVER_STATUS;
            str += "#CENTER_IP:" + CENTER_IP;
            return str;
        }
   
        /// <summary>
        /// 
        /// </summary>
        public string SERVER_ID
		{
			set{ _server_id=value;}
			get{return _server_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SERVER_NAME
		{
			set{ _server_name=value;}
			get{return _server_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SERVER_STATUS
		{
			set{ _server_status=value;}
			get{return _server_status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SERVER_IP
		{
			set{ _server_ip=value;}
			get{return _server_ip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SERVER_CREATEDATE
		{
			set{ _server_createdate=value;}
			get{return _server_createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SERVER_REMARK
		{
			set{ _server_remark=value;}
			get{return _server_remark;}
		}
        public string CENTER_IP
        { set { _CENTER_IP = value; }
            get { return _CENTER_IP; }
        }
        #endregion Model

        public override string ToString()
        {
            return SERVER_ID.ToString()+"["+SERVER_NAME+"]";
        }

        public void Dispose()
        {
          
        }

        /// <summary>
        /// 当前保存的实际监视器客户端连接,用于下置命令
        /// </summary>
        //public EndPoint ThisEndPoint { set; get; }
        public EndPoint MonitorEndPoint { set; get; }
    }
}

