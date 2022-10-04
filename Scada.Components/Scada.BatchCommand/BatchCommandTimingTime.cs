

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
    public class BatchCommandTimingTime : ISerializable
    {
        public BatchCommandTimingTimeType TimingTimeType = BatchCommandTimingTimeType.Day;
        public int Day = 1;
        public int Hour = 23;
        public int Minute = 59;
        public int Second = 59;
        public int ExecuteCycleTimes = 3;
        public override string ToString()
        {
            return "TimingTimeType:"+TimingTimeType.ToString()+ ",Day:" + Day+ ",Hour:" + Hour+ ",Minute:" + Minute+ ",Second:" + Second+ ",ExecuteCycleTimes:" + ExecuteCycleTimes;
        }
        public string GetDataString()
        {
            return "TimingTimeType:" + TimingTimeType.ToString() + ",Day:" + Day + ",Hour:" + Hour + ",Minute:" + Minute + ",Second:" + Second + ",ExecuteCycleTimes:" + ExecuteCycleTimes;

        }
            public  void  CreateFromDBString(string parastring)
        {
            if(!string.IsNullOrEmpty(parastring))
            {
                string[] arrays = parastring.Split(',');
                TimingTimeType= (BatchCommandTimingTimeType)Enum.Parse(typeof(BatchCommandTimingTimeType),arrays[0].Split(':')[1]);
                Day = Convert.ToInt32(arrays[1].Split(':')[1]);
                Hour = Convert.ToInt32(arrays[2].Split(':')[1]);
                Minute = Convert.ToInt32(arrays[3].Split(':')[1]);
                Second = Convert.ToInt32(arrays[4].Split(':')[1]);
                ExecuteCycleTimes = Convert.ToInt32(arrays[5].Split(':')[1]);
            }
        
        }
        public BatchCommandTimingTime()
        {
            Day = 1;
            Hour = 23;
            Minute = 59;
            Second = 59;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TimingTimeType", TimingTimeType);
            info.AddValue("Day", Day);
            info.AddValue("Hour", Hour);
            info.AddValue("Minute", Minute);
            info.AddValue("Second", Second);
            info.AddValue("ExecuteCycleTimes", ExecuteCycleTimes);
        }
        public BatchCommandTimingTime(SerializationInfo info, StreamingContext context)
        {
            this.ExecuteCycleTimes = (int)info.GetValue("ExecuteCycleTimes", typeof(int));
            this.Second = (int)info.GetValue("Second", typeof(int));
            this.Minute = (int)info.GetValue("Minute", typeof(int));
            this.Day = (int)info.GetValue("Day", typeof(int));
            this.Hour = (int)info.GetValue("Hour", typeof(int));
            this.TimingTimeType = (BatchCommandTimingTimeType)info.GetValue("TimingTimeType", typeof(BatchCommandTimingTimeType));

        }
    }
}
