using System;
using System.Runtime.Serialization;
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
namespace Scada.Model
{
	#region  
	/// <summary>
	/// BatchCommandTaskItem:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BatchCommandTaskItemModel: ISerializable, IDisposable
	{
		public BatchCommandTaskItemModel()
		{
			_commandcreatetime = DateTime.Now.ToString("yyyy-MM-dd HH-MM-ss");
		}
		public void Dispose()
		{
		 
		}
		private int _expand = 1;
		public int Expand
		{ set; get; }
		protected BatchCommandTaskItemModel(SerializationInfo info, StreamingContext context)
		{

		
			this._id = (string)info.GetValue("_id", typeof(string));
			this._commandtaskid = (string)info.GetValue("_commandtaskid", typeof(string));
			this._remark = (string)info.GetValue("_remark", typeof(string));
			this._commandcreatetime = (string)info.GetValue("_commandcreatetime", typeof(string));
			this._commandexecutetype = (string)info.GetValue("_commandexecutetype", typeof(string));
			this._commandexecutetime = (string)info.GetValue("_commandexecutetime", typeof(string));
			this._precommanditemid = (string)info.GetValue("_precommanditemid", typeof(string));
			this._server_id = (string)info.GetValue("_server_id", typeof(string));
			this._nextcommanditemidlist = (string)info.GetValue("_nextcommanditemidlist", typeof(string));
			this._x = (float)info.GetValue("_x", typeof(float));
			this._y = (float)info.GetValue("_y", typeof(float));
			this._width = (float)info.GetValue("_width", typeof(float));
			this._height = (float)info.GetValue("_height", typeof(float));
			this._expand = (int)info.GetValue("_expand", typeof(int));
			this._ioparacommand = (string)info.GetValue("_ioparacommand", typeof(string));
			this._commanditemtitle = (string)info.GetValue("_commanditemtitle", typeof(string));
			this._startioparavalue = (string)info.GetValue("_startioparavalue", typeof(string));
			this._delayed = (string)info.GetValue("_delayed", typeof(string));
			this._fixedvalue = (string)info.GetValue("_fixedvalue", typeof(string));
			this._commandItemexecuteresult = (string)info.GetValue("_commandItemexecuteresult", typeof(string));
		}
        public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_width", this._width);
			info.AddValue("_height", this._height);
			info.AddValue("_expand", this._expand);
			info.AddValue("_id", this._id);
			info.AddValue("_commandtaskid", this._commandtaskid);
			info.AddValue("_remark", this._remark);
			info.AddValue("_commandcreatetime", this._commandcreatetime);
			info.AddValue("_commandexecutetype", this._commandexecutetype);
			info.AddValue("_commandexecutetime", this._commandexecutetime);
			info.AddValue("_precommanditemid", this._precommanditemid);
			info.AddValue("_server_id", this._server_id);
			info.AddValue("_nextcommanditemidlist", this._nextcommanditemidlist);
			info.AddValue("_x", this._x);
			info.AddValue("_y", this._y);
			info.AddValue("_ioparacommand", this._ioparacommand);
			info.AddValue("_commanditemtitle", this._commanditemtitle);
			info.AddValue("_startioparavalue", this._startioparavalue);
			info.AddValue("_delayed", this._delayed);
			info.AddValue("_fixedvalue", this._fixedvalue);
			info.AddValue("_commandItemexecuteresult", this._commandItemexecuteresult);
			

		}
 
		private string _id;
		private string _commandtaskid;
		private string  _remark;
		private string _commandcreatetime;
		private string _commandexecutetype;
		private string _commandexecutetime;
		private string _precommanditemid;
		private string _server_id;
		private string _nextcommanditemidlist;
		private float _x;
		private float _y;
		private float _width=200;
		private float _height=150;
		private string _ioparacommand;
		private string _commanditemtitle;
		private string _startioparavalue;
		private string _delayed;
		private string _fixedvalue;
		private string _commandItemtype = "Normal";
		public string CommandItemType
        {
            set { _commandItemtype = value; }
            get { return _commandItemtype; }
        }

		private string _commandItemexecuteresult;
		public string CommandItemExecuteResult
        {
            set { _commandItemexecuteresult = value; }
            get { return _commandItemexecuteresult; }
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
		public string CommandTaskID
		{
			set{ _commandtaskid=value;}
			get{return _commandtaskid;}
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
		public string CommandCreateTime
		{
			set{ _commandcreatetime=value;}
			get{return _commandcreatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CommandExecuteType
		{
			set{ _commandexecutetype=value;}
			get{return _commandexecutetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CommandExecuteTime
		{
			set{ _commandexecutetime=value;}
			get{return _commandexecutetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PreCommandItemID
		{
			set{ _precommanditemid=value;}
			get{return _precommanditemid;}
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
		public string NextCommandItemIDList
		{
			set{ _nextcommanditemidlist=value;}
			get{return _nextcommanditemidlist;}
		}
		/// <summary>
		/// 
		/// </summary>
		public float X
		{
			set{ _x=value;}
			get{return _x;}
		}
		/// <summary>
		/// 
		/// </summary>
		public float Y
		{
			set{ _y=value;}
			get{return _y;}
		}

		public float Height
		{
			set { _height = value; }
			get { return _height; }
		}
		/// <summary>
		/// 
		/// </summary>
		public float Width
		{
			set { _width = value; }
			get { return _width; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string IOParaCommand
		{
			set{ _ioparacommand=value;}
			get{return _ioparacommand;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CommandItemTitle
		{
			set{ _commanditemtitle=value;}
			get{return _commanditemtitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string StartIOParaValue
		{
			set{ _startioparavalue=value;}
			get{return _startioparavalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Delayed
		{
			set{ _delayed=value;}
			get{return _delayed;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FixedValue
		{
			set{ _fixedvalue=value;}
			get{return _fixedvalue;}
		}

       
       

    }
}
#endregion