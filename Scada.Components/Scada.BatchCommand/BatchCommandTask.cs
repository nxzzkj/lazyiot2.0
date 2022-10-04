

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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scada.BatchCommand
{

    [Serializable]
    public class BatchCommandTask : ISerializable, IDisposable
    {
        public BatchRunResult ExecutedResult = new BatchRunResult();
        /// <summary>
        /// 从数据库数据直接创建一个对象
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        public BatchCommandTask CreateCommandTaskFromDBModel(Scada.Model.BatchCommandTaskModel taskModel)
        {

            CommandTaskCreateTime = Convert.ToDateTime(taskModel.CommandTaskCreateTime);
            CommandTaskID = taskModel.Id;
            SERVER_ID = taskModel.SERVER_ID;
            CommandTaskRemark = taskModel.CommandTaskRemark;
            CommandTaskTitle = taskModel.CommandTaskTitle;
            ExecuteTaskTimingTime = new BatchCommandTimingTime();
            ExecuteTaskTimingTime.CreateFromDBString(taskModel.ExecuteTaskTimingTime);
            IOStartConditionValue = new BachCommand_IOPara();
            IOStartConditionValue.CreateFromDBString(taskModel.IOStartConditionValue);
            MachineTrainingTaskId = new BatchCommandMachineTrainTask();
            MachineTrainingTaskId.CreateFromDBString(taskModel.MachineTrainingTaskId);
            ManualTask = new BatchCommandManualTask();
            ManualTask.CreateFromDBString(taskModel.ManualTask);
            StartCommandItemID = taskModel.StartCommandItemID;
            TaskStartRunType = (BatchCommandStartRunType)Enum.Parse(typeof(BatchCommandStartRunType), taskModel.TaskStartRunType);
            Items = new List<BatchCommandItem>();
            if (taskModel.Items != null)
            {
                taskModel.Items.ForEach(delegate (BatchCommandTaskItemModel item)
                {
                    BatchCommandItem batch = new BatchCommandItem()
                    {
                        BatchTask = this,
                        CommandCreateTime = !string.IsNullOrEmpty(item.CommandCreateTime) ? Convert.ToDateTime(item.CommandCreateTime) : DateTime.Now,
                        CommandExecuteTime = (BatchCommandItemExecuteTime)Enum.Parse(typeof(BatchCommandItemExecuteTime), item.CommandExecuteTime),
                        CommandExecuteType = (BatchCommandItemExecuteType)Enum.Parse(typeof(BatchCommandItemExecuteType), item.CommandExecuteType),
                        CommandID = item.Id,
                        BatchCommandTaskId = item.CommandTaskID,

                        CommandItemTitle = item.CommandItemTitle,
                        CommandItemType = (BatchCommandItemType)Enum.Parse(typeof(BatchCommandItemType), item.CommandItemType),
                        NextCommandItemIDList = item.NextCommandItemIDList.Split(',').ToList(),
                        PreCommandItemID = item.PreCommandItemID,
                        Remark = item.Remark,
                        SERVER_ID = item.SERVER_ID,
                        IOParaCommand = new BachCommand_IOPara(),
                        Delayed = new BatchCommandTimeSpan(10),
                        FixedValue = 0,
                        IOTriggerParaValue = new BachCommand_IOPara(),
                        X = item.X,
                        Y = item.Y,
                        Width = item.Width,
                        Height = item.Height,
                        Expand = item.Expand

                    };
                    batch.IOParaCommand.CreateFromDBString(item.IOParaCommand);
                    batch.IOTriggerParaValue.CreateFromDBString(item.StartIOParaValue);
                    batch.Delayed.CreateFromDBString(item.Delayed);
                    batch.FixedValue = string.IsNullOrEmpty(item.FixedValue) ? 0 : Convert.ToSingle(item.FixedValue);
                    Items.Add(batch);


                });
            }
            return this;

        }
        public BatchCommandTaskModel CreateDBModelFromTask()
        {
            BatchCommandTaskModel model = new BatchCommandTaskModel()
            {
                CommandTaskCreateTime = this.CommandTaskCreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CommandTaskRemark = this.CommandTaskRemark,
                CommandTaskTitle = this.CommandTaskTitle,

                ExecuteTaskTimingTime = this.ExecuteTaskTimingTime != null ? this.ExecuteTaskTimingTime.GetDataString() : "",

                Id = this.CommandTaskID,
                IOStartConditionValue = this.IOStartConditionValue != null ? this.IOStartConditionValue.GetDataString() : "",
                MachineTrainingTaskId = this.MachineTrainingTaskId != null ? this.MachineTrainingTaskId.GetDataString() : "",
                ManualTask = this.ManualTask != null ? this.ManualTask.GetDataString() : "",
                SERVER_ID = this.SERVER_ID,
                TaskStartRunType = this.TaskStartRunType.ToString(),
                Items = new List<BatchCommandTaskItemModel>(),
                StartCommandItemID = this.StartCommandItemID

            };
            for (int i = 0; i < this.Items.Count; i++)
            {
                model.Items.Add(new BatchCommandTaskItemModel()
                {
                    CommandCreateTime = this.Items[i].CommandCreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    CommandExecuteTime = this.Items[i].CommandExecuteTime.ToString(),
                    CommandExecuteType = this.Items[i].CommandExecuteType.ToString(),

                    CommandItemTitle = this.Items[i].CommandItemTitle,
                    CommandItemType = this.Items[i].CommandItemType.ToString(),
                    CommandTaskID = this.Items[i].BatchCommandTaskId,
                    Delayed = this.Items[i].Delayed != null ? this.Items[i].Delayed.GetDataString() : "",
                    Expand = this.Items[i].Expand,
                    FixedValue = this.Items[i].FixedValue.ToString(),
                    Height = this.Items[i].Height,
                    Width = this.Items[i].Width,
                    X = this.Items[i].X,
                    Y = this.Items[i].Y,
                    Id = this.Items[i].CommandID,
                    IOParaCommand = this.Items[i].IOParaCommand != null ? this.Items[i].IOParaCommand.GetDataString() : "",

                    NextCommandItemIDList = string.Join(",", this.Items[i].NextCommandItemIDList),
                    PreCommandItemID = this.Items[i].PreCommandItemID,
                    Remark = this.Items[i].Remark,
                    SERVER_ID = this.Items[i].SERVER_ID,



                }); ;
            }

            return model;

        }
        public string SERVER_ID
        {
            set; get;
        }
        public string CommandTaskID { set; get; }
        public string CommandTaskTitle { set; get; }
        public string CommandTaskRemark { set; get; }
        public DateTime CommandTaskCreateTime { set; get; }

        //记录命令开始的第一个节点
        public string StartCommandItemID { set; get; }
        public BatchCommandTask()
        {
            CommandTaskID = GUIDToNormalID.GuidToLongID();


        }
        public BatchCommandStartRunType TaskStartRunType
        {
            set; get;
        } = BatchCommandStartRunType.TimedTask;
        //任务开始执行的时间
        public BatchCommandTimingTime ExecuteTaskTimingTime { set; get; } = new BatchCommandTimingTime();
        /// <summary>
        /// 如果本任务循环执行，则定义循环执行的次数,0表示在指定时间段内无限循环执行
        /// </summary>
        public BatchCommandManualTask ManualTask { set; get; } = new BatchCommandManualTask();

        /// <summary>
        /// 一个IO值触发任务
        /// </summary>
        public BachCommand_IOPara IOStartConditionValue
        { set; get; }
        /// <summary>
        /// 如果机器预测触发任务
        /// </summary>
        public BatchCommandMachineTrainTask MachineTrainingTaskId
        { set; get; }

        public List<BatchCommandItem> Items
        { set; get; }




        #region 序列化
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CommandTaskID", CommandTaskID);
            info.AddValue("CommandTaskTitle", CommandTaskTitle);
            info.AddValue("CommandTaskRemark", CommandTaskRemark);
            info.AddValue("CommandTaskCreateTime", CommandTaskCreateTime);
            info.AddValue("TaskStartRunType", TaskStartRunType);
            info.AddValue("ExecuteTaskTimingTime", ExecuteTaskTimingTime);
            info.AddValue("StartCommandItemID", StartCommandItemID);

            info.AddValue("Items", Items);
            info.AddValue("IOStartConditionValue", IOStartConditionValue);
            info.AddValue("SERVER_ID", SERVER_ID);
            info.AddValue("MachineTrainingTaskId", MachineTrainingTaskId);
        }


        public BatchCommandTask(SerializationInfo info, StreamingContext context)
        {
            this.CommandTaskID = (string)info.GetValue("CommandTaskID", typeof(string));
            this.CommandTaskTitle = (string)info.GetValue("CommandTaskTitle", typeof(string));
            this.CommandTaskRemark = (string)info.GetValue("CommandTaskRemark", typeof(string));
            this.SERVER_ID = (string)info.GetValue("SERVER_ID", typeof(string));
            this.MachineTrainingTaskId = (BatchCommandMachineTrainTask)info.GetValue("MachineTrainingTaskId", typeof(BatchCommandMachineTrainTask));
            this.CommandTaskCreateTime = (DateTime)info.GetValue("CommandTaskCreateTime", typeof(DateTime));
            this.TaskStartRunType = (BatchCommandStartRunType)info.GetValue("TaskStartRunType", typeof(BatchCommandStartRunType));
            this.ExecuteTaskTimingTime = (BatchCommandTimingTime)info.GetValue("ExecuteTaskTimingTime", typeof(BatchCommandTimingTime));
            this.StartCommandItemID = (string)info.GetValue("StartCommandItemID", typeof(string));

            this.Items = (List<BatchCommandItem>)info.GetValue("Items", typeof(List<BatchCommandItem>));
            this.IOStartConditionValue = (BachCommand_IOPara)info.GetValue("IOStartConditionValue", typeof(BachCommand_IOPara));
            if (this.Items != null)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.Items[i].BatchTask = this;
                }
            }
        }

        #endregion

        /// <summary>
        /// 根据命令ID返回一个命令
        /// </summary>
        /// <param name="commandid"></param>
        /// <returns></returns>
        public BatchCommandItem GetCommandItem(string commandid)
        {
            return Items.Find(x => x.CommandID == commandid);
        }
        public List<BatchCommandItem> GetCommandItemOrSubItem(string commandid)
        {
            List<BatchCommandItem> selectors = new List<BatchCommandItem>();
            BatchCommandItem item = GetCommandItem(commandid);
            if (item != null)
            {
                selectors.Add(item);
            }

            item.NextCommandItemIDList.ForEach(delegate (string str)
            {
                List<BatchCommandItem> nextitemx = GetCommandItemOrSubItem(str);
                if (nextitemx != null)
                {
                    selectors.AddRange(nextitemx);
                }
            });

            return selectors;




        }

        /// <summary>
        /// 增加一个命令项
        /// </summary>
        /// <param name="cmmd"></param>
        public void AddCommandItem(BatchCommandItem cmmd)
        {
            if (Items == null)
                Items = new List<BatchCommandItem>();
            cmmd.BatchCommandTaskId = this.CommandTaskID;
            cmmd.BatchTask = this;
            cmmd.SERVER_ID = this.SERVER_ID;
            Items.Add(cmmd);
        }
        /// <summary>
        /// 删除一个命令项及其相关联的项目
        /// </summary>
        /// <param name="commandid"></param>
        /// <returns></returns>
        public void DeleteCommandItem(string commandid)
        {
            List<BatchCommandItem> items = GetCommandItemOrSubItem(commandid);
            Items.RemoveAll(x => items.Contains(x));
        }

        public void SetCommandPre(string commandid, string precommandid)
        {
            BatchCommandItem item = GetCommandItem(commandid);
            item.PreCommandItemID = precommandid;

            if (item.NextCommandItemIDList.Count <= 0)
            {
                item.CommandItemType = BatchCommandItemType.End;
            }
            else
            {
                item.CommandItemType = BatchCommandItemType.Normal;
            }

        }
        public void SetCommandNext(string commandid, string nextcommandid)
        {
            BatchCommandItem item = GetCommandItem(commandid);

            if (!item.NextCommandItemIDList.Contains(nextcommandid))
            {
                item.NextCommandItemIDList.Add(nextcommandid);
            }

            if (item.NextCommandItemIDList.Count <= 0)
            {
                item.CommandItemType = BatchCommandItemType.End;
            }
            else
            {

                item.CommandItemType = BatchCommandItemType.Normal;
                if (string.IsNullOrEmpty(item.PreCommandItemID))
                {
                    item.CommandItemType = BatchCommandItemType.Start;
                }
            }

        }

        public void Dispose()
        {
            if (Items != null)
            {

                Items.Clear();
                Items = null;
            }

            IOStartConditionValue = null;
            MachineTrainingTaskId = null;
            ManualTask = null;
            ExecuteTaskTimingTime = null;
        }
    }
}
