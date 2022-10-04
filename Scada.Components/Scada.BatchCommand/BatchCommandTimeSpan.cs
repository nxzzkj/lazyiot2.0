

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
using System;
using System.Runtime.Serialization;

namespace Scada.BatchCommand
{
 
    [Serializable]
    public class BatchCommandTimeSpan: ISerializable
    {
        public BatchCommandTimeSpan(int timeSpan, ExecuteTimeType timeType= ExecuteTimeType.Seconds)
        {
            TimeType = timeType;

            TimeSpan = timeSpan;
        }
        public ExecuteTimeType TimeType { set; get; } = ExecuteTimeType.Seconds;

        public int  TimeSpan = 3;

        public override string ToString()
        {

            return "TimeType:" + TimeType.ToString()+ ",TimeSpan:" + TimeSpan+"";
        }
        public string GetDataString()
        {
            return "TimeType:" + TimeType.ToString() + ",TimeSpan:" + TimeSpan + "";

        }
        public BatchCommandTimeSpan CreateFromDBString(string parastring)
        {

            if (!string.IsNullOrEmpty(parastring))
            {
                string[] arrays = parastring.Split(',');

                TimeType = (ExecuteTimeType)Enum.Parse(typeof(ExecuteTimeType), arrays[0].Split(':')[1]);
                TimeSpan = Convert.ToInt32(arrays[1].Split(':')[1]);
            }
            return this;

        }
        public   string ToDescString()
        {
            string unit = "秒";
            if (TimeType == ExecuteTimeType.Hours)
            {
                unit = "时";
            }
            else if (TimeType == ExecuteTimeType.Minutes)
            {
                unit = "分";
            }
            else if (TimeType == ExecuteTimeType.Seconds)
            {
                unit = "秒";
            }
            return "前置执行后" + TimeSpan.ToString() + unit + "执行";
        }
        #region 序列化
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TimeType", TimeType);
            info.AddValue("TimeSpan", TimeSpan);
        }

        public BatchCommandTimeSpan(SerializationInfo info, StreamingContext context)
        {
            this.TimeType = (ExecuteTimeType)info.GetValue("TimeType", typeof(ExecuteTimeType));
            this.TimeSpan = (int)info.GetValue("TimeSpan", typeof(int));
            
        }

        #endregion
    }
}
