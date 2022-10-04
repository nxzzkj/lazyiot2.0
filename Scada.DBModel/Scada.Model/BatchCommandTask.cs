using System;
using System.Collections.Generic;
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
	/// BatchCommandTask:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BatchCommandTaskModel : ISerializable,IDisposable
	{
		public List<Scada.Model.BatchCommandTaskItemModel> Items { set; get; }
		public BatchCommandTaskModel()
		{
			_commandtaskcreatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Items = new List<BatchCommandTaskItemModel>();

        }
		protected BatchCommandTaskModel(SerializationInfo info, StreamingContext context)
		{

			#region 自定义属性
			this._id = (string)info.GetValue("_id", typeof(string));
			this._commandtasktitle = (string)info.GetValue("_commandtasktitle", typeof(string));
			this._commandtaskcreatetime = (string)info.GetValue("_commandtaskcreatetime", typeof(string));
			this._commandtaskremark = (string)info.GetValue("_commandtaskremark", typeof(string));
			this._taskstartruntype = (string)info.GetValue("_taskstartruntype", typeof(string));
			this._executetasktimingtime = (string)info.GetValue("_executetasktimingtime", typeof(string));
	 
			this._server_id = (string)info.GetValue("_server_id", typeof(string));
			this._iostartconditionvalue = (string)info.GetValue("_iostartconditionvalue", typeof(string));
			this._machinetrainingtaskId = (string)info.GetValue("_machinetrainingtaskId", typeof(string));
			this._manualtask = (string)info.GetValue("_manualtask", typeof(string));
            this._enable = (int)info.GetValue("_enable", typeof(int));
            


            this.Items = (List<Scada.Model.BatchCommandTaskItemModel>)info.GetValue("Items", typeof(List<Scada.Model.BatchCommandTaskItemModel>));
            #endregion

            if (Items == null)
                Items = new List<BatchCommandTaskItemModel>();




        }
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_id", this._id);
			info.AddValue("_commandtasktitle", this._commandtasktitle);
			info.AddValue("_commandtaskcreatetime", this._commandtaskcreatetime);
			info.AddValue("_commandtaskremark", this._commandtaskremark);
			info.AddValue("_taskstartruntype", this._taskstartruntype);
			info.AddValue("_executetasktimingtime", this._executetasktimingtime);
			info.AddValue("_server_id", this._server_id);
			info.AddValue("_iostartconditionvalue", this._iostartconditionvalue);
			info.AddValue("Items", this.Items);
			info.AddValue("_machinetrainingtaskId", this._machinetrainingtaskId);
			info.AddValue("_manualtask", this._manualtask);
            info.AddValue("_enable", this._enable);
            


        }

		public void Dispose()
		{
			if(Items!=null)
			Items.Clear();
			Items = null;
		}
		#region Model
		private string _id;
		private string _commandtasktitle;
		private string _commandtaskcreatetime;
		private string _commandtaskremark;
		private string _taskstartruntype;
		private string _executetasktimingtime;
		private string _server_id;
		private string _iostartconditionvalue;

		private string _manualtask;
		private string _machinetrainingtaskId;

		private string _startcommanditemid;
		public string StartCommandItemID
        {
			set { _startcommanditemid = value; }
			get { return _startcommanditemid; }
		}
		public string ManualTask
        {
            set { _manualtask = value; }
            get { return _manualtask; }
        }
		public string MachineTrainingTaskId
        {
            set { _machinetrainingtaskId = value; }
			get { return _machinetrainingtaskId; }
		}
        public override string ToString()
        {
            return _commandtasktitle.ToString();
        }

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
		public string CommandTaskTitle
		{
			set{ _commandtasktitle=value;}
			get{return _commandtasktitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CommandTaskCreateTime
		{
			set{ _commandtaskcreatetime=value;}
			get{return _commandtaskcreatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CommandTaskRemark
		{
			set{ _commandtaskremark=value;}
			get{return _commandtaskremark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TaskStartRunType
		{
			set{ _taskstartruntype=value;}
			get{return _taskstartruntype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ExecuteTaskTimingTime
		{
			set{ _executetasktimingtime=value;}
			get{return _executetasktimingtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		 
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
		public string IOStartConditionValue
		{
			set{ _iostartconditionvalue=value;}
			get{return _iostartconditionvalue;}
		}

        public int _enable = 1;
        public int Enable { set { _enable = value; } get { return _enable; } }

		#endregion Model

	}
}

