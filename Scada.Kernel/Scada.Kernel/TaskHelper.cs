using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Kernel
{
   
    public abstract  class KernelTaskHelper
    {
     public static   TaskFactory Factory = new TaskFactory(new LimitedTaskScheduler(new IOMonitorConfig().TaskMaxNumber));
    }
}
