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

namespace Scada.MachineTraining
{
    public class LookupMap
    {
        public LookupMap()
        { }
        public LookupMap(float value, string category)
        {
            Value = value;
            Category = category;
        }

        public float Value { get; set; }
        public string Category { get; set; }
    }
    public class LookupBinaryMap
    {
        public bool Value { get; set; }
        public string Category { get; set; }
    }
    public class LookupRandomizedPcaMap
    {
        public float Min { set; get; }
        public float Max { set; get; }
        public float Value { get; set; }
        public string Category
        {
            get
            {

                if (Value < 0.5f)
                {
                    return Condtions[0];
                }
                else if (Value < 0.6f&& Value>=0.5f)
                {
                    return Condtions[1];
                }
                else if (Value < 0.7f && Value >= 0.6f)
                {
                    return Condtions[2];
                }
                else if (Value < 0.8f && Value >= 0.7f)
                {
                    return Condtions[3];
                }
                else if (Value < 0.9f && Value >= 0.8f)
                {
                    return Condtions[4];
                }
                else if (Value < 1f && Value >= 0.9f)
                {
                    return Condtions[5];
                }
                return "正常";
            }
        }

        public string[] Condtions { set; get; }
    }
}
