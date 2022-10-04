

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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scada.BatchCommand
{
    [Serializable]
    public class BatchCommandMachineTrainTask : ISerializable
    {
        public BatchCommandMachineTrainTask()
        {

        }
        public BatchCommandMachineTrainTask(SerializationInfo info, StreamingContext context)
        {
          
          

            this.MachineTrainingTaskId = (string)info.GetValue("MachineTrainingTaskId", typeof(string));
            this.MachineTrainingTaskTitle = (string)info.GetValue("MachineTrainingTaskTitle", typeof(string));
            this.Expressions = (List<string>)info.GetValue("Expressions", typeof(List<string>));
        }
        public string MachineTrainingTaskId
        { set; get; }
        public string MachineTrainingTaskTitle
        { set; get; }
        
      
        public List<string> Expressions { set; get; } = new List<string>();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MachineTrainingTaskId", MachineTrainingTaskId);
            info.AddValue("Expressions", Expressions);
            info.AddValue("MachineTrainingTaskTitle", MachineTrainingTaskTitle);
        
           
        }

        public override string ToString()
        {
            return MachineTrainingTaskTitle.ToString();
        }

        public string GetDataString()
        {
            string str = "TaskId:" + MachineTrainingTaskId+ ",TaskTitle:"+ MachineTrainingTaskTitle+",";
            for(int i=0;i< Expressions.Count;i++)
            {
                str += Expressions[i]+";";
            }

            str = str.Substring(0, str.Length - 1);
            return str;
        }
        public void CreateFromDBString(string parastring)
        {

            if (!string.IsNullOrEmpty(parastring))
            {
                string[] arrays = parastring.Split(',');
                MachineTrainingTaskId = arrays[0].Split(':')[1];
                MachineTrainingTaskTitle= arrays[1].Split(':')[1];
                Expressions = new List<string>();
                if (arrays.Length > 2)
                {
                    Expressions.AddRange(arrays[2].Split(';'));

                }


            }

        }
    }
}
