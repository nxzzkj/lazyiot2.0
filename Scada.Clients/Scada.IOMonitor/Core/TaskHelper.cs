using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOMonitor.Core
{
    public abstract class TaskHelper
    {
        public static Action<string> TaskRunException;
        public static Action<string> TaskCanceled;
        public static Action<string> TaskRanToCompletion;
        public static LimitedTaskScheduler LimitedTaskScheduler = null;
        public static TaskFactory Factory = new TaskFactory(LimitedTaskScheduler=new LimitedTaskScheduler(new IOMonitorConfig().TaskMaxNumber)
        {



            TaskCanceled = TaskCanceled,
            TaskRanToCompletion = TaskRanToCompletion,
            TaskRunException = TaskRunException

           
        });
    }
}
