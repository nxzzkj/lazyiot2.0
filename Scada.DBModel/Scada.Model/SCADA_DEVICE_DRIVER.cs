using System;
using System.Runtime.Serialization;

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
	/// SCADA_DEVICE_DRIVER:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SCADA_DEVICE_DRIVER: ISerializable, IDisposable
	{
		public SCADA_DEVICE_DRIVER()
		{}
        public override string ToString()
        {
            return _title.ToString();
        }
        #region Model
        private string _id;
		private string _devicename;
		private string _title;
		private string _namespace;
		private string _devicefullname;
		private string _fillname;
		private string _dll_guid;
		private string _dll_name;
		private string _dll_title;
		/// <summary>
		/// 
		/// </summary>
		public string Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DeviceName
		{
			set{ _devicename=value;}
			get{return _devicename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Namespace
		{
			set{ _namespace=value;}
			get{return _namespace;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DeviceFullName
		{
			set{ _devicefullname=value;}
			get{return _devicefullname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FillName
		{
			set{ _fillname=value;}
			get{return _fillname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Dll_GUID
		{
			set{ _dll_guid=value;}
			get{return _dll_guid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Dll_Name
		{
			set{ _dll_name=value;}
			get{return _dll_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Dll_Title
		{
			set{ _dll_title=value;}
			get{return _dll_title;}
		}

       
        #endregion Model

        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SCADA_DEVICE_DRIVER(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性
            this._id = (string)info.GetValue("_id", typeof(string));
            this._devicefullname = (string)info.GetValue("_devicefullname", typeof(string));
            this._devicename = (string)info.GetValue("_devicename", typeof(string));
            this._dll_guid = (string)info.GetValue("_dll_guid", typeof(string));
            this._dll_name = (string)info.GetValue("_dll_name", typeof(string));
            this._dll_title = (string)info.GetValue("_dll_title", typeof(string));
            this._fillname = (string)info.GetValue("_fillname", typeof(string));
            this._namespace = (string)info.GetValue("_namespace", typeof(string));
            this._title = (string)info.GetValue("_title", typeof(string));
           
            #endregion





        }
 
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_id", this._id);
            info.AddValue("_devicefullname", this._devicefullname);
            info.AddValue("_devicename", this._devicename);
            info.AddValue("_dll_guid", this._dll_guid);
            info.AddValue("_dll_name", this._dll_name);
            info.AddValue("_title", this._title);
            info.AddValue("_dll_title", this._dll_title);
            info.AddValue("_namespace", this._namespace);
            info.AddValue("_fillname", this._fillname);
           

        }

        #endregion
        public SCADA_DEVICE_DRIVER Copy()
        {
            SCADA_DEVICE_DRIVER driver = new SCADA_DEVICE_DRIVER()
            {
                _devicefullname = this._devicefullname,
                _id = this._id,
                _devicename = this._devicename,
                _dll_guid = this._dll_guid,
                _dll_name = this._dll_name,
                _title = this._title,
                _dll_title = this._dll_title,
                _namespace = this._namespace,
                _fillname = this._fillname
             

            };
            return driver;
        }

        public void Dispose()
        {
        
        }
    }
}

