using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCenterServer.Core
{
    public abstract class TaskHelper
    {
        public static Action<string> TaskRunException;
        public static Action<string> TaskCanceled;
        public static Action<string> TaskRanToCompletion;
        public static TaskFactory Factory = new TaskFactory(new LimitedTaskScheduler(new CenterServerConfig().TaskMaxNumber)
        {

            TaskCanceled = TaskCanceled,
            TaskRanToCompletion = TaskRanToCompletion,
            TaskRunException = TaskRunException
        });
    }
}
