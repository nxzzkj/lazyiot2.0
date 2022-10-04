
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
//在调试过程中如果发现相关的bug或者代码错误等问题可直接微信联系作者。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
using System;
using System.ComponentModel;

namespace Scada.Model
{
    public class MachineTrainInfluxDBModel: IDisposable
    {
        public MachineTrainInfluxDBModel()
        {

        }
        [Description("任务所在的采集站")]
        public string SERVER_ID
        { set; get; } = "";
        [Description("任务所在的通道")]
        public string COMM_ID
        { set; get; } = "";
        [Description("任务所在的通道名称")]
        public string COMM_NAME
        { set; get; } = "";
        [Description("任务所在的设备")]
        public string DEVICE_ID
        { set; get; } = "";
        [Description("任务所在的设备名称")]
        public string DEVICE_NAME
        { set; get; } = "";
        [Description("任务ID")]
        public string TaskID
        {
            set; get;
        } = "";
        [Description("任务标题")]
        public string TaskTitle
        {
            set; get;
        } = "";
        [Description("任务执行时间")]
        public string CreateTime
        { set; get; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        [Description("任务运行的结果，True或者False")]
        public string Result
        {
            set; get;
        } = "False";
        [Description("如果任务运行出错，则记录错误信息")]
        public string Remark
        { set; get; } = "";

        public void Dispose()
        {
           
        }
    }
}
