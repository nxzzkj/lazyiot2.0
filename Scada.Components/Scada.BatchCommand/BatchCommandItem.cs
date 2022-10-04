

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
using Scada.DBUtility;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scada.BatchCommand
{
    [Serializable]
    /// <summary>
    /// 定义一个命令执行子项
    /// </summary>
    public class BatchCommandItem : ISerializable
    {
        public BatchCommandItem()
        {
            NextCommandItemIDList = new List<string>();
            CommandID = GUIDToNormalID.GuidToLongID();
            CommandCreateTime = DateTime.Now;
        }
        /// <summary>
        /// 该命令执行的结果
        /// </summary>
        public BatchRunResult ExecutedResult { set; get; } = new BatchRunResult(false, BatchRunResultType.False);

        public string BatchCommandTaskId{set;get;}
        public string Remark { set; get; } = "";
        public string CommandItemTitle
        {
            set; get;
        } = "";
        public DateTime CommandCreateTime { set; get; } = DateTime.Now;

        public BatchCommandTask BatchTask
        { set; get; } = null;
        /// <summary>
        /// 命令的ID号
        /// </summary>
        public string CommandID
        {
            set; get;
        }
        public string SERVER_ID
        {
            set;get;
        }
        /// <summary>
		/// 
		/// </summary>
		public float X
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            set;get;
        }

        public float Height
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            set; get;
        }
        public int Expand { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string CommandContent
        {

            get {

                string str = "";
                str += "\r\n采集站:"+ SERVER_ID;
                str += "\r\n下置固定值:" + FixedValue;
                str += "\r\n下置方式:" + CommandExecuteType;
                str += "\r\n执行方式:" + CommandExecuteTime;
                str += "\r\n延迟时间:" + Delayed.ToDescString() ;
                if(IOParaCommand!=null)
                str += "\r\n下置IO参数:" + IOParaCommand.GetDataString();
                if(IOTriggerParaValue != null)
                str += "\r\n:触发IO参数" + IOTriggerParaValue.GetDataString();
                return str;
            }
        }
        [Description("启动方式,包含根据某个IO触发启动,无条件启动")]
        public BatchCommandItemExecuteType CommandExecuteType
        {
            set; get;
        } = BatchCommandItemExecuteType.Unconditional;//无条件执行


        [Description("启动时间:上次命令执行完后立即执行此次命令，上次命令执行完后延迟执行此次命令")]
        /// <summary>
        /// 启动时间，
        /// </summary>
        public BatchCommandItemExecuteTime CommandExecuteTime
        {
            set; get;
        } = BatchCommandItemExecuteTime.DelayedExecution;

        [Description("如果是延迟执行，则要设置延迟时间")]

        public BatchCommandTimeSpan Delayed = new BatchCommandTimeSpan(0);//延迟多少毫秒开始执行

      
        [Description("如果是IO条件触发，则需要设置IO 参数")]
        public BachCommand_IOPara IOTriggerParaValue
        { set; get; } = null;

        [Description("节点类型:是开始节点还是终端节点或者是中间节点")]
        public BatchCommandItemType CommandItemType { set; get; } = BatchCommandItemType.Start;
        /// <summary>
        /// 当前命令的前置命令
        /// </summary>
        public string PreCommandItemID
        {
            set; get;
        } = "";
        /// <summary>
        /// 当前命令要执行的下一个命令
        /// </summary>
        public List<string> NextCommandItemIDList
        {
            set; get;
        } = new List<string>();
        /// <summary>
        /// 下置命令参数
        /// </summary>
        public BachCommand_IOPara IOParaCommand
        { set; get; } = null;
        //启动条件的参数

      
        public Image Icon { set; get; }

      
        public void RefreshItemType()
        {
            if (this.NextCommandItemIDList.Count <= 0)
            {
                this.CommandItemType = BatchCommandItemType.End;
            }
            else
            {

                this.CommandItemType = BatchCommandItemType.Normal;
                if (string.IsNullOrEmpty(this.PreCommandItemID))
                {
                    this.CommandItemType = BatchCommandItemType.Start;
                }
            }
            switch (this.CommandItemType)
            {
                case BatchCommandItemType.Normal:
                    this.Icon = BatchCommandResource.Normal;
                    break;
                case BatchCommandItemType.Start:
                    this.Icon = BatchCommandResource.Start;
                    break;
                case BatchCommandItemType.End:
                    this.Icon = BatchCommandResource.End;
                    break;
            }

        }
        public float FixedValue { set; get; } = 0;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Remark", Remark);
            info.AddValue("CommandItemTitle", CommandItemTitle);
            info.AddValue("CommandCreateTime", CommandCreateTime);
            info.AddValue("CommandID", CommandID);
            info.AddValue("CommandExecuteType", CommandExecuteType);
            info.AddValue("CommandExecuteTime", CommandExecuteTime);
            info.AddValue("Delayed", Delayed);
            info.AddValue("PreCommandItemID", PreCommandItemID);
            info.AddValue("NextCommandItemIDList", NextCommandItemIDList);
            info.AddValue("IOTriggerParaValue", IOTriggerParaValue);
            info.AddValue("IOParaCommand", IOParaCommand);
      
            info.AddValue("FixedValue", FixedValue);
            info.AddValue("SERVER_ID", SERVER_ID);
            info.AddValue("CommandItemType", CommandItemType);
            info.AddValue("Icon", Icon);
            info.AddValue("BatchCommandTaskId", BatchCommandTaskId);
            info.AddValue("X", X);
            info.AddValue("Y", Y);
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
            info.AddValue("Expand", Expand);

        }
        public BatchCommandItem(SerializationInfo info, StreamingContext context)
        {
            this.X = (float)info.GetValue("X", typeof(float));
            this.Y = (float)info.GetValue("Y", typeof(float));
            this.Height = (float)info.GetValue("Height", typeof(float));
            this.Width = (float)info.GetValue("Width", typeof(float));
            this.Expand = (int)info.GetValue("Expand", typeof(int));

            this.BatchCommandTaskId = (string)info.GetValue("BatchCommandTaskId", typeof(string));
            this.CommandID = (string)info.GetValue("CommandID", typeof(string));
            this.Remark = (string)info.GetValue("Remark", typeof(string));
            this.CommandItemTitle = (string)info.GetValue("CommandItemTitle", typeof(string));
            this.CommandCreateTime = (DateTime)info.GetValue("CommandCreateTime", typeof(DateTime));
            this.CommandExecuteType = (BatchCommandItemExecuteType)info.GetValue("CommandExecuteType", typeof(BatchCommandItemExecuteType));
            this.CommandExecuteTime = (BatchCommandItemExecuteTime)info.GetValue("CommandExecuteTime", typeof(BatchCommandItemExecuteTime));
            this.Delayed = (BatchCommandTimeSpan)info.GetValue("Delayed", typeof(BatchCommandTimeSpan));
            this.NextCommandItemIDList = (List<string>)info.GetValue("NextCommandItemIDList", typeof(List<string>));
            this.PreCommandItemID = (string)info.GetValue("PreCommandItemID", typeof(string));
            this.IOParaCommand = (BachCommand_IOPara)info.GetValue("IOParaCommand", typeof(BachCommand_IOPara));
            this.IOTriggerParaValue = (BachCommand_IOPara)info.GetValue("IOTriggerParaValue", typeof(BachCommand_IOPara));
            this.FixedValue = (float)info.GetValue("FixedValue", typeof(float));
            this.SERVER_ID = (string)info.GetValue("SERVER_ID", typeof(string));
            this.Icon = (Image)info.GetValue("Icon", typeof(Image)); 
            this.CommandItemType = (BatchCommandItemType)info.GetValue("CommandItemType", typeof(BatchCommandItemType));
          

        }
    }
}
