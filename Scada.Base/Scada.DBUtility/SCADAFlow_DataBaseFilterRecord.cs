namespace Scada.DBUtility
{

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
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class SCADAFlow_DataBaseFilterRecord : ISerializable, IDisposable
    {
        public SCADAFlow_DataBaseFilterRecord()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "");
            this.ValueRecord = new SCADAFlow_DataBaseRecord();
            this.TextRecord = new SCADAFlow_DataBaseRecord();
            this.FilterType = SCADAFlow_DataBaseFilterType.Key;
            this.TextItems = new List<string>();
            this.ValueItems = new List<string>();
            this.Label = "";
            this.DynamicRecord = new FilterDropDownRecord();
        }

        public SCADAFlow_DataBaseFilterRecord(SerializationInfo info, StreamingContext context)
        {
            this.Label = (string) info.GetValue("Label", typeof(string));
            this.DynamicRecord = (FilterDropDownRecord) info.GetValue("DynamicRecord", typeof(FilterDropDownRecord));
            this.TextItems = (List<string>) info.GetValue("TextItems", typeof(List<string>));
            this.ValueItems = (List<string>) info.GetValue("ValueItems", typeof(List<string>));
            this.ValueRecord = (SCADAFlow_DataBaseRecord) info.GetValue("ValueRecord", typeof(SCADAFlow_DataBaseRecord));
            this.TextRecord = (SCADAFlow_DataBaseRecord) info.GetValue("TextRecord", typeof(SCADAFlow_DataBaseRecord));
            this.FilterType = (SCADAFlow_DataBaseFilterType) info.GetValue("FilterType", typeof(SCADAFlow_DataBaseFilterType));
        }

        public string GetHtmlDataString() => 
            (" data-dbdynamiclist='json_" + this.ID + "' ");

        public string GetHtmlDataString(string id) => 
            (" data-dbdynamiclist='" + id + "' ");

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Label", this.Label);
            info.AddValue("DynamicRecord", this.DynamicRecord);
            info.AddValue("TextItems", this.TextItems);
            info.AddValue("ValueItems", this.ValueItems);
            info.AddValue("ValueRecord", this.ValueRecord);
            info.AddValue("TextRecord", this.TextRecord);
            info.AddValue("FilterType", this.FilterType);
        }

        public string GetObjectJson()
        {
            string str = ScadaJsonConvertor.ObjectToJson(this);
            string[] textArray1 = new string[] { " <script id='json_", this.ID, "' type='application/json'> ", str, "</script>" };
            return string.Concat(textArray1);
        }

        public string GetObjectJson(string id)
        {
            string str = ScadaJsonConvertor.ObjectToJson(this);
            string[] textArray1 = new string[] { " <script id='", id, "' type='application/json'> ", str, "</script>" };
            return string.Concat(textArray1);
        }

        public override string ToString()
        {
            switch (this.FilterType)
            {
                case SCADAFlow_DataBaseFilterType.StaticDropList:
                {
                    object[] objArray4 = new object[] { this.Label, " ", this.FilterType, " ", string.Join(";", this.TextItems.ToArray()) };
                    return string.Concat(objArray4);
                }
                case SCADAFlow_DataBaseFilterType.DynamicDropList:
                {
                    object[] objArray3 = new object[] { this.Label, " ", this.FilterType, " ", this.DynamicRecord.ToString() };
                    return string.Concat(objArray3);
                }
                case SCADAFlow_DataBaseFilterType.Key:
                {
                    object[] objArray1 = new object[] { this.Label, " ", this.FilterType, " ", this.ValueRecord.ToString() };
                    return string.Concat(objArray1);
                }
                case SCADAFlow_DataBaseFilterType.DateRange:
                {
                    object[] objArray2 = new object[] { this.Label, " ", this.FilterType, " ", this.ValueRecord.ToString() };
                    return string.Concat(objArray2);
                }
            }
            return "";
        }

        public void Dispose()
        {
            ValueRecord = null;
            TextRecord = null;
            TextItems = null;
            ValueItems = null;
            DynamicRecord = null;
        }

        public string ID { get; set; }

        public SCADAFlow_DataBaseRecord ValueRecord { get; set; }

        public SCADAFlow_DataBaseRecord TextRecord { get; set; }

        public SCADAFlow_DataBaseFilterType FilterType { get; set; }

        public List<string> TextItems { get; set; }

        public List<string> ValueItems { get; set; }

        public string Label { get; set; }

        public FilterDropDownRecord DynamicRecord { get; set; }
    }
}

