

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOMonitor.Core
{
    /// <summary>
    /// scada通道和设备状态监控
    /// </summary>
    public class IOMonitorStatusManager : IDisposable
    {
        public System.Threading.Timer ScadaStatusTimer = null;
        public Func<Task> Monitor;
        public int intervalTimes = 240;//秒
        public IOMonitorStatusManager()
        {

        }
        public IOMonitorStatusManager(int interval)
        {
            intervalTimes = interval;
        }
        public void Start()
        {
            ScadaStatusTimer = new System.Threading.Timer(p => StatusMonitor(), null, 60000, intervalTimes * 1000);
        }
        private Task StatusMonitor()
        {
            if (Monitor != null)
              return  Monitor();
            return null;
        }

        public void Close()
        {
            if (ScadaStatusTimer != null)
                ScadaStatusTimer.Dispose();
            ScadaStatusTimer = null;

        }
        public void Dispose()
        {
            if (ScadaStatusTimer != null)
                ScadaStatusTimer.Dispose();
            ScadaStatusTimer = null;
        }
    }
}
