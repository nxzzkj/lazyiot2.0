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
	/// ScadaMachineTrainingCondition:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ScadaMachineTrainingCondition: IDisposable
	{
		public ScadaMachineTrainingCondition()
		{}
		#region Model
		private long _id;
		private long _taskid;
		private string _conditiontitle;
		private string _startdate;
		private string _enddate;
		private string _markdate;
		private string _marktitle;
		private int? _datalength;
		private string _conditions;
		private string _remark;
		private string _server_id;
		private string _server_name;
        private string _ConditionName;
		public string _DataFile = "";
	
		
		public string DataFile
		{
			set { _DataFile = value; }
			get { return _DataFile; }
		}

		public string ConditionName
        {
            set { _ConditionName = value; }
            get { return _ConditionName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public long TaskId
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConditionTitle
		{
			set{ _conditiontitle=value;}
			get{return _conditiontitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string StartDate
		{
			set{ _startdate=value;}
			get{return _startdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EndDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MarkDate
		{
			set{ _markdate=value;}
			get{return _markdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MarkTitle
		{
			set{ _marktitle=value;}
			get{return _marktitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DataLength
		{
			set{ _datalength=value;}
			get{return _datalength;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Conditions
		{
			set{ _conditions=value;}
			get{return _conditions;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
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
		private int _IsTrain = 0;
		public int IsTrain
        {
            set { _IsTrain = value; }
            get { return _IsTrain; }
        }
		#endregion Model

		public override string ToString()
        {
            return ConditionTitle + "["+  ConditionName + "]";
        }
		#region  序列化和反序列化
	 
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ScadaMachineTrainingCondition(SerializationInfo info, StreamingContext context)
		{

			#region 自定义属性
			this._server_id = (string)info.GetValue("_server_id", typeof(string));
			this._server_name = (string)info.GetValue("_server_name", typeof(string));
			this._id = (long)info.GetValue("_id", typeof(long));
			this._ConditionName = (string)info.GetValue("_ConditionName", typeof(string));
			this._conditions = (string)info.GetValue("_conditions", typeof(string));
			this._conditiontitle = (string)info.GetValue("_conditiontitle", typeof(string));
			this._datalength = (int?)info.GetValue("_datalength", typeof(int));

			this._enddate = (string)info.GetValue("_enddate", typeof(string));
			this._markdate = (string)info.GetValue("_markdate", typeof(string));
			this._marktitle = (string)info.GetValue("_marktitle", typeof(string));
			this._remark = (string)info.GetValue("_remark", typeof(string));
			this._startdate = (string)info.GetValue("_startdate", typeof(string));
			this._taskid = (long)info.GetValue("_taskid", typeof(long));
			this._DataFile = (string)info.GetValue("_DataFile", typeof(string));
			
			#endregion





		}

		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_server_id", this._server_id);
			info.AddValue("_server_name", this._server_name);
			info.AddValue("_id", this._id);
			info.AddValue("_ConditionName", this._ConditionName);
			info.AddValue("_conditions", this._conditions);
			info.AddValue("_conditiontitle", this._conditiontitle);
			info.AddValue("_datalength", this._datalength);
			info.AddValue("_enddate", this._enddate);
			info.AddValue("_markdate", this._markdate);
			info.AddValue("_marktitle", this._marktitle);
			info.AddValue("_remark", this._remark);
			info.AddValue("_startdate", this._startdate);
			info.AddValue("_taskid", this._taskid);
			info.AddValue("_DataFile", this._DataFile);
		
			


		}

		#endregion
		public ScadaMachineTrainingCondition Copy()
		{
			ScadaMachineTrainingCondition server = new ScadaMachineTrainingCondition()
			{
				ConditionName = this.ConditionName,
				SERVER_ID = this.SERVER_ID,
				Conditions = this.Conditions,
				SERVER_NAME = this.SERVER_NAME,
				ConditionTitle = this.ConditionTitle,
				DataLength = this.DataLength,
				EndDate = this.EndDate,
				Id = this.Id,
				MarkDate = this.MarkDate,
				MarkTitle = this.MarkTitle,
				Remark = this.Remark,
				StartDate = this.StartDate,
				TaskId = this.TaskId,
				DataFile = this.DataFile,
			

			};
			return server;
		}
		public string GetCommandString()
		{
			string str = "TABLE:ScadaMachineTrainingCondition#ConditionName:" + ConditionName;
			str += "#SERVER_ID:" + SERVER_ID;
			str += "#Conditions:" + Conditions;
			str += "#SERVER_NAME:" + SERVER_NAME;
			str += "#ConditionTitle:" + ConditionTitle;
			str += "#DataLength:" + DataLength;
			str += "#EndDate:" + EndDate;
			str += "#Id:" + Id;
			str += "#MarkDate:" + MarkDate;
			str += "#MarkTitle:" + MarkTitle;
			str += "#StartDate:" + StartDate;
			str += "#TaskId:" + TaskId;
		
			str += "#DataFile:" + DataFile;
			str += "#Remark:" + Remark.Replace("#", "//").Replace(":", "\\");
			return str;
		}

        public void Dispose()
        {
         
        }
    }
}

