using System;
using System.Collections.Generic;
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
	/// ScadaMachineTrainingModel:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ScadaMachineTrainingModel : IDisposable
	{
		public ScadaMachineTrainingModel()
		{ }
		#region Model
		private long _id;
		private string _taskname;
		private string _algorithm;
		private string _algorithmtype;
		private int? _trainingcycle;
		private int? _forecastpriod;
		private string _properties;
		private string _remark;
		private string _server_name;
		private string _server_id;
		public int _isTrain = 0;//是否已经训练
		public string _detection;

		public string Detection
        {
            set { _detection = value; }
            get { return _detection; }
        }
		public string GetDetection(float v)
        {
			if(v<=0.5f)
            {
				return Detection.Split(',')[0];

			}
			else if (v > 0.5f&&v<=0.6f)
			{
				return Detection.Split(',')[1];

			}
			else if (v > 0.6f && v <= 0.7f)
			{
				return Detection.Split(',')[2];

			}
			else if (v > 0.7f && v <= 0.8f)
			{
				return Detection.Split(',')[3];

			}
			else if (v > 0.8f && v <= 0.9f)
			{
				return Detection.Split(',')[4];

			}
			else if (v > 0.9f && v <= 1f)
			{
				return Detection.Split(',')[5];

			}
			return "";
		}
		/// <summary>
		/// 
		/// </summary>
		public long Id
		{
			set { _id = value; }
			get { return _id; }
		}
		public string AlgorithmType
		{
			set { _algorithmtype = value; }
			get { return _algorithmtype; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string TaskName
		{
			set { _taskname = value; }
			get { return _taskname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Algorithm
		{
			set { _algorithm = value; }
			get { return _algorithm; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TrainingCycle
		{
			set { _trainingcycle = value; }
			get { return _trainingcycle; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ForecastPriod
		{
			set { _forecastpriod = value; }
			get { return _forecastpriod; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Properties
		{
			set { _properties = value; }
			get { return _properties; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set { _remark = value; }
			get { return _remark; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SERVER_NAME
		{
			set { _server_name = value; }
			get { return _server_name; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SERVER_ID
		{
			set { _server_id = value; }
			get { return _server_id; }
		}
		public override string ToString()
		{
			return TaskName.ToString();
		}
		private string _comm_id = "";
		public string COMM_ID
		{
			set { _comm_id = value; }
			get { return _comm_id; }

		}
		private string _device_id = "";
		public string DEVICE_ID
		{
			set { _device_id = value; }
			get { return _device_id; }
		}
		#endregion Model

		private List<ScadaMachineTrainingCondition> _Conditions = new List<ScadaMachineTrainingCondition>();
		public List<ScadaMachineTrainingCondition> Conditions
		{
			set { _Conditions = value; }
			get { return _Conditions; }
		}
		public DateTime LastTrainTime = DateTime.Now;

		#region  序列化和反序列化

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ScadaMachineTrainingModel(SerializationInfo info, StreamingContext context)
		{

			#region 自定义属性
			this._server_id = (string)info.GetValue("_server_id", typeof(string));
			this._server_name = (string)info.GetValue("_server_name", typeof(string));
			this._id = (long)info.GetValue("_id", typeof(long));
			this._algorithm = (string)info.GetValue("_algorithm", typeof(string));
			this._comm_id = (string)info.GetValue("_comm_id", typeof(string));
			this._Conditions = (List<ScadaMachineTrainingCondition>)info.GetValue("_Conditions", typeof(List<ScadaMachineTrainingCondition>));
			this._device_id = (string)info.GetValue("_device_id", typeof(string));

			this._forecastpriod = (int)info.GetValue("_forecastpriod", typeof(int));
			this._properties = (string)info.GetValue("_properties", typeof(string));
			this._remark = (string)info.GetValue("_remark", typeof(string));
			this._taskname = (string)info.GetValue("_taskname", typeof(string));
			this._trainingcycle = (int)info.GetValue("_trainingcycle", typeof(int));
			this._algorithmtype = (string)info.GetValue("_algorithmtype", typeof(string));
			this._truetext = (string)info.GetValue("_truetext", typeof(string));
			this._falsetext = (string)info.GetValue("_falsetext", typeof(string));
			this._detection = (string)info.GetValue("_detection", typeof(string));
			this._isTrain = (int)info.GetValue("_isTrain", typeof(int));
			
			#endregion





		}

		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_server_id", this._server_id);
			info.AddValue("_server_name", this._server_name);
			info.AddValue("_id", this._id);
			info.AddValue("_algorithm", this._algorithm);
			info.AddValue("_comm_id", this._comm_id);
			info.AddValue("_Conditions", this._Conditions);
			info.AddValue("_device_id", this._device_id);
			info.AddValue("_forecastpriod", this._forecastpriod);
			info.AddValue("_properties", this._properties);
			info.AddValue("_remark", this._remark);
			info.AddValue("_taskname", this._taskname);
			info.AddValue("_trainingcycle", this._trainingcycle);
			info.AddValue("_algorithmtype", this._algorithmtype);
			info.AddValue("_falsetext", this._falsetext);
			info.AddValue("_truetext", this._truetext);
			info.AddValue("_isTrain", this._isTrain);
			info.AddValue("_detection", this._detection);
			
		}

		#endregion
		public ScadaMachineTrainingModel Copy()
		{
			ScadaMachineTrainingModel server = new ScadaMachineTrainingModel()
			{
				Algorithm = this.Algorithm,
				AlgorithmType = this.AlgorithmType,
				SERVER_ID = this.SERVER_ID,
				Conditions = this.Conditions,
				SERVER_NAME = this.SERVER_NAME,
				COMM_ID = this.COMM_ID,
				DEVICE_ID = this.DEVICE_ID,
				ForecastPriod = this.ForecastPriod,
				Id = this.Id,
				Properties = this.Properties,
				TaskName = this.TaskName,
				Remark = this.Remark,
				TrainingCycle = this.TrainingCycle,
				TrueText = this.TrueText,
				FalseText = this.FalseText,
				 IsTrain=this.IsTrain,
				 Detection=this.Detection
			};
			return server;
		}
		public string GetCommandString()
		{
			string str = "TABLE:ScadaMachineTrainingModel#Algorithm:" + Algorithm;
			str += "#SERVER_ID:" + SERVER_ID;
			str += "#SERVER_NAME:" + SERVER_NAME;
			str += "#COMM_ID:" + COMM_ID;
			str += "#DEVICE_ID:" + DEVICE_ID;
			str += "#ForecastPriod:" + ForecastPriod;
			str += "#Id:" + Id;
			str += "#Properties:" + Properties;
			str += "#TaskName:" + TaskName;
			str += "#TrueText:" + TrueText;
			str += "#FalseText:" + FalseText;
			str += "#IsTrain:" + this.IsTrain;
			str += "#Detection:" + this.Detection;
			
			str += "#TrainingCycle:" + TrainingCycle;
			str += "#AlgorithmType:" + AlgorithmType;
			str += "#Remark:" + Remark.Replace("#", "//").Replace(":", "\\");
			return str;
		}
		private string _truetext = "是";
		private string _falsetext = "否";
		public string TrueText { set { _truetext = value; } get { return _truetext; } }
		public string FalseText { set { _falsetext = value; } get { return _falsetext; } }

		public int IsTrain
		{
			set { _isTrain = value; }
			get { return _isTrain; }
		}
        
		public void Dispose()
		{
			this.Conditions = null;
		}
		public string Classification
		{
			get
			{
				List<string> cols = new List<string>();
				string classification = "";
				if (Conditions != null)
				{
					Conditions.ForEach(delegate (ScadaMachineTrainingCondition p) {

						string[] ss = p.Conditions.Split(new char[1] { ',' }, StringSplitOptions.None);
						if(ss.Length>0)
					   cols.AddRange(ss);
					});
				}
				return classification;
			}

		}

	}
}

