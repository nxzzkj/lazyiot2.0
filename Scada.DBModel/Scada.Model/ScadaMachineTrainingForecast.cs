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
	/// ScadaMachineTrainingForecast:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ScadaMachineTrainingForecast: IDisposable, ISerializable
	{
		public ScadaMachineTrainingForecast()
		{
			SERVER_NAME = "";
			COMM_NAME = "";
			DEVICE_NAME = "";
		}
		#region Model
	 
		private long _taskid;
 
		private string _algorithm;//当前的算法
		private DateTime _PredictedDate;//预测的时间
		private string _PredictedLabel;
 
		
		private string _remark;
		private string _server_id;
		private string _server_name;
		private string _comm_id;	
		public string COMM_NAME
        {
			set;get;
        }
		public string DEVICE_NAME
		{
			set; get;
		}

		public string COMM_ID
		{
			set { _comm_id = value; }
			get { return _comm_id; }
		}
		private string _device_id;
		public string DEVICE_ID
		{
			set { _device_id = value; }
			get { return _device_id; }
		}
		public string TaskName
		{
			set;get;
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
		public string Algorithm
		{
			set{ _algorithm=value;}
			get{return _algorithm;}
		}
		public string AlgorithmType
		{
			set;get;
		}
		private string _featuresName;// 数据的特征列名称
		public string FeaturesName
		{
			set { _featuresName = value; }
			get { return _featuresName; }
		}

		private string _featuresvalue;//输入值

		/// <summary>
		/// 
		/// </summary>
		public string FeaturesValue
		{
			set{ _featuresvalue = value;}
			get{return _featuresvalue; }
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime PredictedDate
		{
			set{ _PredictedDate = value;}
			get{return _PredictedDate; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string PredictedLabel
		{
			set{ _PredictedLabel = value;}
			get{return _PredictedLabel; }
		}
		public string Score
		{ set; get; }
		 
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
		public void Dispose()
		{

		}
		public ScadaMachineTrainingForecast(SerializationInfo info, StreamingContext context)
        {
			this.Algorithm = (string)info.GetValue("Algorithm", typeof(string));
			this.AlgorithmType = (string)info.GetValue("AlgorithmType", typeof(string));
			this.COMM_ID = (string)info.GetValue("COMM_ID", typeof(string));
			this.COMM_NAME = (string)info.GetValue("COMM_NAME", typeof(string));
			this.DEVICE_ID = (string)info.GetValue("DEVICE_ID", typeof(string));
			this.DEVICE_NAME = (string)info.GetValue("DEVICE_NAME", typeof(string));
			this.FeaturesName = (string)info.GetValue("FeaturesName", typeof(string));
			this.FeaturesValue = (string)info.GetValue("FeaturesValue", typeof(string));
			this.PredictedDate = (DateTime)info.GetValue("PredictedDate", typeof(DateTime));
			this.Remark = (string)info.GetValue("Remark", typeof(string));
			this.Score = (string)info.GetValue("Score", typeof(string));
			this.SERVER_ID = (string)info.GetValue("SERVER_ID", typeof(string));
			this.SERVER_NAME = (string)info.GetValue("SERVER_NAME", typeof(string));
			this.TaskId = (long)info.GetValue("TaskId", typeof(long));
			this.TaskName = (string)info.GetValue("TaskName", typeof(string));
	 

		}
		

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
			info.AddValue("Algorithm", Algorithm);
			info.AddValue("AlgorithmType", AlgorithmType);
			info.AddValue("COMM_ID", COMM_ID);
			info.AddValue("COMM_NAME", COMM_NAME);
			info.AddValue("DEVICE_ID", DEVICE_ID);
			info.AddValue("DEVICE_NAME", DEVICE_NAME);
			info.AddValue("FeaturesName", FeaturesName);
			info.AddValue("FeaturesValue", FeaturesValue);
			info.AddValue("PredictedDate", PredictedDate);
			info.AddValue("Remark", Remark);
			info.AddValue("Score", Score);
			info.AddValue("SERVER_ID", SERVER_ID);
			info.AddValue("SERVER_NAME", SERVER_NAME);
			info.AddValue("TaskId", TaskId);
			info.AddValue("TaskName", TaskName);
		}
        #endregion Model

    }
}

