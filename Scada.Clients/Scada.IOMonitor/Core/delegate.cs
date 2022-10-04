
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada.Model;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Core
{
    public enum TaskOperator
    {
        运行,
        暂停,
        停止,
        关闭

    }

    public delegate void MonitorException(Exception ex);
    public delegate void MonitorLog(string log);
    public delegate void MonitorOperator(TaskOperator Operator);
    /// <summary>
    /// 实时接收数据的事件
    /// </summary>
    /// <param name="server"></param>
    /// <param name="comm"></param>
    /// <param name="device"></param>
    public delegate void MonitorReceive(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, byte[] sourceBytes);
    /// <summary>
    /// 生成报警的事件
    /// </summary>
    /// <param name="server"></param>
    /// <param name="comm"></param>
    /// <param name="device"></param>
    /// <param name="arlarm"></param>
    public delegate void MonitorMakeAlarm(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARAALARM arlarm);
    public delegate void MonitorSenCommand(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARAALARM arlarm);
    public delegate void MonitorUploadListView(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, string uploadresult);





}
