

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
    public class BachCommandBoolExpression : ISerializable
    {
        public float DefaultValue { set; get; } = 0;
        public string  Label { set; get; } = "";
        public BachCommandBoolExpression()
        {

        }
        public BachCommandBoolExpression(SerializationInfo info, StreamingContext context)
        {
            this.Label = (string)info.GetValue("Label", typeof(string));
            this.OpSymbol = (string)info.GetValue("OpSymbol", typeof(string));
            this.Value = (float)info.GetValue("Value", typeof(float));
            this.DefaultValue = (float)info.GetValue("DefaultValue", typeof(float));
        }

        public string OpSymbol { set; get; } = "=";
  
        public float Value { set; get; } = 0;
        public string GetHtmlString()
        {

            return OpSymbol + ":" + Value.ToString();
        }
        public string GetDataString()
        {

            if (Label == "")
                Label = " ";

            return "Symbol:" + OpSymbol  +";Value:"+ Value.ToString()+ ";DefaultValue:" + DefaultValue+ ";Label:" + Label;
        }
        public BachCommandBoolExpression CreateFromDBString(string parastring)
        {
            if (!string.IsNullOrEmpty(parastring))
            {
                string[] arrays = parastring.Split(';');
                OpSymbol = arrays[0].Split(':')[1];
                Value = Convert.ToSingle( arrays[1].Split(':')[1]);
                DefaultValue = Convert.ToSingle(arrays[2].Split(':')[1]);
                Label = arrays[3].Split(':')[1];
            }
            return this;
        }


        public override string ToString()
        {
            return OpSymbol + Value.ToString() + ",写入值:" + DefaultValue+",标签:"+ Label;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OpSymbol", this.OpSymbol);
            info.AddValue("DefaultValue", this.DefaultValue);
            info.AddValue("Value", this.Value);
            info.AddValue("Label", this.Label);
        }



    }

 
     
}
