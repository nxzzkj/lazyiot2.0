using System;
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
	/// Classify_DRIVER:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public   class Classify_DRIVER: ISerializable, IDisposable
	{
		public Classify_DRIVER()
		{}
		#region Model
		private int _id;
		private string _classifyname;
		private string _createtime;
		private string _updatetime;
		private string _description;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ClassifyName
		{
			set{ _classifyname=value;}
			get{return _classifyname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}

        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Classify_DRIVER(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性
            this._id = (int)info.GetValue("_id", typeof(int));
            this._classifyname = (string)info.GetValue("_classifyname", typeof(string));
            this._createtime = (string)info.GetValue("_createtime", typeof(string));
            this._description = (string)info.GetValue("_description", typeof(string));
            this._updatetime = (string)info.GetValue("_updatetime", typeof(string));
            this._description = (string)info.GetValue("_description", typeof(string));

            #endregion





        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_id", this._id);
            info.AddValue("_classifyname", this._classifyname);
            info.AddValue("_createtime", this._createtime);
            info.AddValue("_description", this._description);
            info.AddValue("_updatetime", this._updatetime);
            info.AddValue("_description", this._description);
        }

        #endregion
        public Classify_DRIVER Copy()
        {
            Classify_DRIVER driver = new Classify_DRIVER()
            {
                _classifyname = this._classifyname,
                _id = this._id,
                _createtime = this._createtime,
                _description = this._description,
                _updatetime = this._updatetime

            };
            return driver;
        }

        public void Dispose()
        {
            
        }
        #endregion Model

    }
}

