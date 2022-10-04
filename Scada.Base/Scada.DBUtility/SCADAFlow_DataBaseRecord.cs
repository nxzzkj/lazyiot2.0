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
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class SCADAFlow_DataBaseRecord : ISerializable, IDisposable
    {
        public SCADAFlow_DataBaseRecord()
        {
            this.RecordType = SCADAFlow_DataBaseRecordType.Varchar;
            this.Format = "";
            this.DecimalPlaces = 0;
        }

        public SCADAFlow_DataBaseRecord(SerializationInfo info, StreamingContext context)
        {
            this.DecimalPlaces = (int) info.GetValue("DecimalPlaces", typeof(int));
            this.Title = (string) info.GetValue("Title", typeof(string));
            this.Record = (string) info.GetValue("Record", typeof(string));
            this.Format = (string) info.GetValue("Format", typeof(string));
            this.RecordType = (SCADAFlow_DataBaseRecordType) info.GetValue("RecordType", typeof(SCADAFlow_DataBaseRecordType));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DecimalPlaces", this.DecimalPlaces);
            info.AddValue("Format", this.Format);
            info.AddValue("Title", this.Title);
            info.AddValue("Record", this.Record);
            info.AddValue("RecordType", this.RecordType);
        }

        public override string ToString()
        {
            string[] textArray1 = new string[] { (this.Title != null) ? this.Title.ToString() : "", " ", this.Record, " ", this.RecordType.ToString(), " ", this.Format };
            return string.Concat(textArray1);
        }

        public void Dispose()
        {
           
        }

        public string Title { get; set; }

        public string Record { get; set; }

        public SCADAFlow_DataBaseRecordType RecordType { get; set; }

        public string Format { get; set; }

        public int DecimalPlaces { get; set; }
    }
}

