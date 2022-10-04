

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using Scada.BatchCommand;
using Scada.DBUtility;
using Scada.IOStructure;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Temporal.DbAPI;
using Temporal.Net.InfluxDb.Models.Responses;

namespace ScadaCenterServer.Core
{

    /// <summary>
    /// 自动控制命令管理器
    /// </summary>
    public class IOCenterBatchCommandTaskManager : IDisposable
    {

        public int interval = 500;//每500毫秒秒运行一次
        public bool RunStatus = false;
        /// <summary>
        /// 返回一个子项命令运行的结果
        /// </summary>
        public Func<BatchCommandTask, BatchCommandItem, Task> CommandItemRunResult;
        public Func<BatchCommandTask, Task> CommandTaskStarted;//任务开始的事件反馈
        public Func<BatchCommandTask, Task> CommandTaskEnded;//任务结束的事件反馈
        public Func<BatchCommandTask, string, Task> CommandTaskError;//错误报告输出事件
        public Func<BatchCommandTask, BatchCommandItem, string, Task> CommandRunErrorResult;//任务执行出现异常的时候反馈
        private List<BatchCommandTask> BatchCommandTasks = new List<BatchCommandTask>();

        private void BatchCommandTaskDetermineEnd(BatchCommandTask batchCommandTask)
        {
            bool isEnd = true;
            for (int i = 0; i < batchCommandTask.Items.Count; i++)
            {
                if (batchCommandTask.Items[i].ExecutedResult.IsExecuted == false)
                {
                    isEnd = false;
                    break;
                }

            }
            batchCommandTask.ExecutedResult.IsExecuted = isEnd;
            if (batchCommandTask.ExecutedResult.IsExecuted && CommandTaskEnded != null)
            {
                CommandTaskEnded(batchCommandTask);
            }


        }
        /// <summary>
        /// 此处只执行定时循环任务
        /// </summary>
        /// <returns></returns>
        public Task RunManager()
        {
          
            return TaskHelper.Factory.StartNew(() =>
            {


                RunStatus = true;
                while (RunStatus)
                {
                    if (IOCenterManager.IOProject == null)
                        break;
                    IOCenterManager.IOProject.BatchCommandTasks.ForEach(delegate (BatchCommandTaskModel taskModel)
                       {



                           using (BatchCommandTask batchCommandTask = new BatchCommandTask())
                           {
                               try
                               {
                                   batchCommandTask.CreateCommandTaskFromDBModel(taskModel);
                                   switch (batchCommandTask.TaskStartRunType)
                                   {
                                       case BatchCommandStartRunType.ManualTask://手动执行任务,通过用户在UI界面中手动命令任务
                                           break;
                                       case BatchCommandStartRunType.MachineTrainTask://机器预测学习任务，如果有最新的预测结果，则执行命令任务
                                           break;
                                       case BatchCommandStartRunType.TimedTask://定时循环任务
                                           {
                                               ExecuteTimedTask(taskModel);

                                           }
                                           break;
                                       case BatchCommandStartRunType.IOTriggerTask://根据采集数据进行触发执行，采集数据符合要求则触发该命令任务
                                           break;

                                   }
                               }
                               catch (Exception emx)
                               {

                                   if (CommandTaskError != null)
                                   {
                                       CommandTaskError(batchCommandTask, emx.Message);

                                   }
                               }
                           }
                           Thread.Sleep(interval);

                       });

                    Thread.Sleep(1000);
                }

            });


        }
        /// <summary>
        /// 执行定时循环任务
        /// </summary>
        /// <param name="taskModel"></param>
        public Task ExecuteTimedTask(Scada.Model.BatchCommandTaskModel taskModel)
        {

            return TaskHelper.Factory.StartNew(() =>
            {

                using (BatchCommandTask batchCommandTask = new BatchCommandTask())
                {
                    try
                    {
                        batchCommandTask.CreateCommandTaskFromDBModel(taskModel);
                        BatchCommandItem startItem = batchCommandTask.GetCommandItem(batchCommandTask.StartCommandItemID);
                        if (startItem != null)
                        {

                            if (CommandTaskStarted != null)
                            {
                                CommandTaskStarted(batchCommandTask);
                            }
                            ExecuteTimedTask(null, batchCommandTask, startItem);

                        }
                    }
                    catch (Exception emx)
                    {
                        if (CommandTaskError != null)
                        {
                            CommandTaskError(batchCommandTask, emx.Message);

                        }

                    }
                }


            });
        }
        //执行一个机器训练触发任务
        public Task ExecuteMachineTrainTask(string Label, Scada.Model.ScadaMachineTrainingModel trainingModel)
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                IOCenterManager.IOProject.BatchCommandTasks.ForEach(delegate (Scada.Model.BatchCommandTaskModel taskModel)
                {

                    using (BatchCommandTask batchCommandTask = new BatchCommandTask())
                    {
                        try
                        {
                            batchCommandTask.CreateCommandTaskFromDBModel(taskModel);
                            if (batchCommandTask.TaskStartRunType == BatchCommandStartRunType.MachineTrainTask
                            && batchCommandTask.MachineTrainingTaskId.Expressions.Contains(Label.Trim()))
                            {

                                if (batchCommandTask.MachineTrainingTaskId != null && batchCommandTask.MachineTrainingTaskId.MachineTrainingTaskId == trainingModel.Id.ToString())
                                {
                                    BatchCommandItem startItem = batchCommandTask.GetCommandItem(batchCommandTask.StartCommandItemID);
                                    if (startItem != null)
                                    {
                                        ExecuteTimedTask(null, batchCommandTask, startItem);
                                    }
                                }

                            }
                        }

                        catch (Exception emx)
                        {
                            if (CommandTaskError != null)
                            {
                                CommandTaskError(batchCommandTask, emx.Message);

                            }
                        }
                    };

                });
            });
        }
        /// <summary>
        /// 执行一个IO触发任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="ioValue"></param>
        /// <returns></returns>
        public Task ExecuteIOTriggerTask(float ioValue, IO_PARA para)
        {
            return TaskHelper.Factory.StartNew(() =>
            {
                
                
                    IOCenterManager.IOProject.BatchCommandTasks.ForEach(delegate (Scada.Model.BatchCommandTaskModel taskModel)
                    {

                        using (BatchCommandTask batchCommandTask = new BatchCommandTask())
                        {
                            batchCommandTask.CreateCommandTaskFromDBModel(taskModel);
                            if (batchCommandTask.TaskStartRunType == BatchCommandStartRunType.IOTriggerTask)
                            {
                                float value = batchCommandTask.IOStartConditionValue.GetExpressionValue(ioValue);
                                if (batchCommandTask.IOStartConditionValue != null
                                && batchCommandTask.IOStartConditionValue.IOParament.IO_ID == para.IO_ID
                                && value != -9999)
                                {

                                    BatchCommandItem startItem = batchCommandTask.GetCommandItem(batchCommandTask.StartCommandItemID);
                                    if (startItem != null)
                                    {
                                        ExecuteTimedTask(null, batchCommandTask, startItem);
                                    }
                                }

                            }

                        }
                    });
                
                
            });
        }
        public Task ExecuteManualTask(string taskId)
        {
            return TaskHelper.Factory.StartNew(() =>
            {
               
                    Scada.Model.BatchCommandTaskModel taskModel = IOCenterManager.IOProject.BatchCommandTasks.Find(x => x.Id == taskId);
                using (BatchCommandTask batchCommandTask = new BatchCommandTask())
                {
                    try
                    {
                        batchCommandTask.CreateCommandTaskFromDBModel(taskModel);
                        BatchCommandItem startItem = batchCommandTask.GetCommandItem(batchCommandTask.StartCommandItemID);
                        if (startItem != null)
                        {
                            ExecuteTimedTask(null, batchCommandTask, startItem);
                        }
                    }
                    catch (Exception emx)
                    {
                        if (CommandTaskError != null)
                        {
                            CommandTaskError(batchCommandTask, emx.Message);

                        }
                    }
                
                }
            });
        
        }
        private Task ExecuteTimedTask(Task preTask, BatchCommandTask batchCommandTask, BatchCommandItem command)
        {

            var task = TaskHelper.Factory.StartNew(() =>
             {
                 try
                 {
                     if (command != null)
                     {
                         command.ExecutedResult.IsExecuted = true;
                         IO_SERVER sendserver = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID == command.IOParaCommand.IOParament.IO_SERVER_ID);
                         IO_COMMUNICATION sendcomm = IOCenterManager.IOProject.Communications.Find(x => x.IO_SERVER_ID == command.IOParaCommand.IOParament.IO_SERVER_ID
                         && x.IO_COMM_ID == command.IOParaCommand.IOParament.IO_COMM_ID);
                         IO_DEVICE senddevice = IOCenterManager.IOProject.Devices.Find(x => x.IO_SERVER_ID == command.IOParaCommand.IOParament.IO_SERVER_ID
                          && x.IO_COMM_ID == command.IOParaCommand.IOParament.IO_COMM_ID && x.IO_DEVICE_ID == command.IOParaCommand.IOParament.IO_DEVICE_ID);
                         IO_COMMANDS dbcommand = new IO_COMMANDS()
                         {
                             COMMAND_DATE = command.CommandCreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             COMMAND_ID = GUIDToNormalID.GuidToLongID(),
                             COMMAND_RESULT = "FALSE",
                             COMMAND_USER = "",
                             COMMAND_VALUE = "0",
                             IO_COMM_NAME = sendcomm.IO_COMM_NAME+"["+ sendcomm.IO_COMM_LABEL + "]",
                             IO_COMM_ID = sendcomm.IO_COMM_ID,
                             IO_DEVICE_ID = senddevice.IO_DEVICE_ID,
                             IO_DEVICE_NAME = senddevice.IO_DEVICE_NAME+"["+ senddevice.IO_DEVICE_LABLE + "]",
                             IO_ID = command.IOParaCommand.IOParament.IO_ID,
                             IO_LABEL = command.IOParaCommand.IOParament.IO_LABEL,
                             IO_NAME = command.IOParaCommand.IOParament.IO_NAME,
                             IO_SERVER_ID = sendserver.SERVER_ID
                              

                         };
                         switch (command.CommandExecuteTime)
                         {
                             case BatchCommandItemExecuteTime.DelayedExecution:
                                 {
                                     if (command.Delayed.TimeType == ExecuteTimeType.Hours)
                                     {
                                         Thread.Sleep(command.Delayed.TimeSpan * 1000 * 60 * 60);
                                     }
                                     else if (command.Delayed.TimeType == ExecuteTimeType.Minutes)
                                     {
                                         Thread.Sleep(command.Delayed.TimeSpan * 1000 * 60);
                                     }
                                     else if (command.Delayed.TimeType == ExecuteTimeType.Seconds)
                                     {
                                         Thread.Sleep(command.Delayed.TimeSpan * 1000);
                                     }
                                     else if (command.Delayed.TimeType == ExecuteTimeType.Millisecond)
                                     {
                                         Thread.Sleep(command.Delayed.TimeSpan);
                                     }
                                     if (preTask != null)
                                         preTask.Wait();
                                     //获取写入值，根据类型
                                     float value = -9999;
                                     switch (command.CommandExecuteType)
                                     {
                                         case BatchCommandItemExecuteType.IOTrigger:
                                             {
                                                 IO_SERVER server = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID == command.IOTriggerParaValue.IOParament.IO_SERVER_ID);

                                                 IO_COMMUNICATION comm = IOCenterManager.IOProject.Communications.Find(x => x.IO_SERVER_ID == command.IOTriggerParaValue.IOParament.IO_SERVER_ID
                                                 && x.IO_COMM_ID == command.IOTriggerParaValue.IOParament.IO_COMM_ID);
                                                 IO_DEVICE device = IOCenterManager.IOProject.Devices.Find(x => x.IO_SERVER_ID == command.IOTriggerParaValue.IOParament.IO_SERVER_ID
                                                  && x.IO_COMM_ID == command.IOTriggerParaValue.IOParament.IO_COMM_ID && x.IO_DEVICE_ID == command.IOTriggerParaValue.IOParament.IO_DEVICE_ID);
                                                 string paraName = command.IOTriggerParaValue.IOParament.IO_NAME;
                                                 var result = IOCenterManager.InfluxDbManager.DbQuery_Real(server, comm, device).Result;
                                         
                                                 if (result != null && result.Count() > 0)
                                                 {
                                                     Serie s = result.Last();
                                                     if (s != null && s.Values.Count > 0)
                                                     {
                                                          
                                                         int valueindex = s.Columns.IndexOf("field_" + paraName.Trim().ToLower() + "_value");
                                                         if ( valueindex >= 0)
                                                         {
                                                             if(s.Values[0][valueindex]!=null&& s.Values[0][valueindex].ToString()!="")
                                                             {
                                                                 float serielvalue = Convert.ToSingle(s.Values[0][valueindex].ToString());
                                                                 //根据表达式判断要输入的值
                                                                 value= command.IOTriggerParaValue.GetExpressionValue(serielvalue);
                                                             }
                                                          

                                                         }
                                                       
                                                     }

                                                 }
                                             }
                                             break;
                                         case BatchCommandItemExecuteType.Unconditional:
                                             value = command.FixedValue;
                                             break;
                                     }
                                     if(value!=-9999)
                                     {
                                         dbcommand.COMMAND_VALUE = value.ToString();
                                         bool res = ExecuteCommand(dbcommand);
                                         command.ExecutedResult.Result = res ? BatchRunResultType.True : BatchRunResultType.False;
                                     }
                                  
                                 }
                                 break;
                             case BatchCommandItemExecuteTime.ExecuteNow:
                                 {
                                     if(preTask!=null)
                                     preTask.Wait();
                                     dbcommand.COMMAND_VALUE = command.FixedValue.ToString();
                                     bool res = ExecuteCommand(dbcommand);
                                     command.ExecutedResult.Result = res ? BatchRunResultType.True : BatchRunResultType.False;
                                 }
                                 break;
                             case BatchCommandItemExecuteTime.ParallelExecution://不需要等待前次命令执行
                                 {
                                     var subtask = TaskHelper.Factory.StartNew(() =>
                                     {
                                         dbcommand.COMMAND_VALUE = command.FixedValue.ToString();
                                         bool res = ExecuteCommand(dbcommand);
                                         command.ExecutedResult.Result = res ? BatchRunResultType.True : BatchRunResultType.False;
                                     });
                                     subtask.Wait();
                                 }

                                 break;
                         }
                     }
                 }
                 catch (Exception emx)
                 {
                     if (CommandTaskError != null)
                     {
                         CommandTaskError(batchCommandTask, emx.Message);
                     }
                     
                     if (CommandRunErrorResult != null)
                     {
                         CommandRunErrorResult(batchCommandTask, command, emx.Message);
                     }
                 }


             });

            if (command.ExecutedResult.IsExecuted)
            {
                if (CommandItemRunResult != null)
                { //返回每个子命令执行后的反馈
                    CommandItemRunResult(batchCommandTask, command);
                }

            }

            
            if (command != null && command.NextCommandItemIDList != null && command.NextCommandItemIDList.Count > 0)
            {
                command.NextCommandItemIDList.ForEach(delegate (string cmdid)
                {
                    ExecuteTimedTask(task, batchCommandTask, batchCommandTask.GetCommandItem(cmdid));
                });


            }
            
            BatchCommandTaskDetermineEnd(batchCommandTask);
            return task;
        }
        private bool ExecuteCommand(IO_COMMANDS command)
        {
            try
            {
                return IOCenterManager.SendCommand(command);
            }
            catch (Exception emx)
            {
                return false;
            }
        }
        public void Stop()
        {
            RunStatus = false;
        }
        public void Start()
        {
            RunStatus = true;
        }

        public void Dispose()
        {
            RunStatus = false;
            BatchCommandTasks.Clear();
            BatchCommandTasks = null;
        }
    }
}
