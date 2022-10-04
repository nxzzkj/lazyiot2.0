using Scada.DBUtility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTest
{
    public enum ScadaTaskType
    {
        LongTermEffectiveness,//长期执行
        Immediate//立即执行
    }
    /// <summary>
    /// 集成一个Task
    /// </summary>
    public class ScadaTask : Task
    {
        private ScadaTaskType taskType = ScadaTaskType.Immediate;
        public ScadaTaskType TaskType { get { return taskType; } }
        public ScadaTask(Action action, ScadaTaskType _taskType = ScadaTaskType.Immediate) : base(action)
        {
            TaskId = GUIDToNormalID.GuidToLongID();
            taskType = _taskType;
        }
        public string TaskId { set; get; }
        public override string ToString()
        {
            return TaskId.ToString();
        }
    }
    /// <summary>
    /// 定义一个Taks的管理类,用于管理任务的运行和注销已经其他
    /// </summary>
    public class ScadaTaskManager
    {
        public TaskScheduler TaskScheduler { set; get; }
        public Action<string> TaskRunException;
        public Action<string> TaskCanceled;
        public Action<string> TaskRanToCompletion;
        Stopwatch stopwatch = new Stopwatch();
        /// <summary>
        /// 表示同时并发的任务数，防止出现cpu100%,所以在此处要限制Task的数量
        /// </summary>
        public int MaxTaskNumber = 50;//最大并行运行任务
        private System.Threading.Timer monitorTimer = null;

        public ScadaTaskManager()
        {
            monitorTimer = new Timer(delegate {

                RunTask();


            }, null, 1000, 5);
        }
        public ConcurrentBag<ScadaTask> Tasks { get; set; } = new ConcurrentBag<ScadaTask>() { };
        /// <summary>
        /// 创建一个任务但并不运行,在外部运行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="_taskType"></param>
        /// <returns></returns>
        public ScadaTask StartNew(Action action, ScadaTaskType _taskType = ScadaTaskType.Immediate)
        {
            var _task = new ScadaTask(action, _taskType);

            if (_taskType == ScadaTaskType.Immediate)
            {
                AddTask(_task);
            }


            TaskMonitor(_task);
            return _task;

        }
        private void TaskMonitor(ScadaTask _task)
        {
            ///如果任务异常则，在此处输出异常
            _task.ContinueWith(r =>
            {

                if (TaskRunException != null && _task.Exception != null)
                {
                    string Exception = Convert.ToString(_task.Exception);
                    TaskRunException(Exception);
                }
                ScadaTask result = (ScadaTask)r;
                bool res = Tasks.TryTake(out result);//删除执行完成的任务
            }, TaskContinuationOptions.OnlyOnFaulted);
            //任务取消的时候返回信息
            _task.ContinueWith(r =>
            {

                if (TaskCanceled != null)
                {

                    TaskCanceled(r.ToString());
                }
                ScadaTask result = (ScadaTask)r;
                bool res = Tasks.TryTake(out result);//删除执行完成的任务

            }, TaskContinuationOptions.OnlyOnCanceled);
            ///任务执行完成的时候返回的信息
            _task.ContinueWith(r =>
            {

                if (TaskRanToCompletion != null)
                {
                    TaskRanToCompletion(r.ToString());

                }
                ScadaTask result = (ScadaTask)r;
                bool res = Tasks.TryTake(out result);//删除执行完成的任务

            }, TaskContinuationOptions.OnlyOnRanToCompletion);

        }
        private void AddTask(ScadaTask _task)
        {
         
            Tasks.Add(_task);

        }
        /// <summary>
        /// 直接创建并运行任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="_taskType"></param>
        /// <returns></returns>
        public ScadaTask Run(Action action, ScadaTaskType _taskType = ScadaTaskType.Immediate)
        {
            var _task = new ScadaTask(action, _taskType);
            if (_taskType == ScadaTaskType.Immediate)
            {
                AddTask(_task);
            }
            TaskMonitor(_task);

        
            return _task;
        }
        /// <summary>
        /// 判断是否可以插入一个任务
        /// </summary>
        /// <returns></returns>
        private bool CanInsertTask()
        {


            return Tasks.Count > MaxTaskNumber ? false : true;
        }
        //每次并发执行设置的任务数
        private void RunTask()
        {
            List<ScadaTask> mtasks = new List<ScadaTask>();
            for (int i = 0; i < this.MaxTaskNumber; i++)
            {
                ScadaTask result = null;
                bool res = Tasks.TryTake(out result);//删除执行完成的任务
                if (res && result != null)
                {
                    result.Start();
                    mtasks.Add(result);
                }
            }

            Task.WhenAll(mtasks);
        }
    }
}
