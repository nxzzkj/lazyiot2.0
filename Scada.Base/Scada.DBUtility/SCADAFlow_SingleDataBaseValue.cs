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
    public class SCADAFlow_SingleDataBaseValue : ISerializable, IDisposable
    {
        private string m_ConnectionString;
        public int Decimalplace;

        public SCADAFlow_SingleDataBaseValue()
        {
            this.m_ConnectionString = "";
            this.SqlString = "";
            this.elementId = "";
            this.ValueType = SCADAFlow_DataBaseType.Single;
            this.UpdateCycle = 60;
            this.DateFormat = "yyyy-MM-dd HH:mm:ss";
            this.DateFormat = "yyyy-MM-dd HH:mm:ss";
            this.Value = "0.0";
        }

        public SCADAFlow_SingleDataBaseValue(string mDefault)
        {
            this.m_ConnectionString = "";
            this.SqlString = "";
            this.Value = mDefault;
        }

        protected SCADAFlow_SingleDataBaseValue(SerializationInfo info, StreamingContext context)
        {
            this.m_ConnectionString = "";
            this.Connection = (ScadaConnectionBase) info.GetValue("Connection", typeof(ScadaConnectionBase));
            this.SqlString = (string) info.GetValue("SqlString", typeof(string));
            this.UpdateCycle = (int) info.GetValue("UpdateCycle", typeof(int));
            this.Value = (string) info.GetValue("Value", typeof(string));
            this.DateFormat = (string) info.GetValue("DateFormat", typeof(string));
            this.ConnectionString = (string) info.GetValue("ConnectionString", typeof(string));
            this.Decimalplace = (int) info.GetValue("Decimalplace", typeof(int));
            this.ValueType = (SCADAFlow_DataBaseType) info.GetValue("ValueType", typeof(SCADAFlow_DataBaseType));
            this.Record = (SCADAFlow_DataBaseRecord) info.GetValue("Record", typeof(SCADAFlow_DataBaseRecord));
        }

        public SCADAFlow_SingleDataBaseValue(float mDefault, string format)
        {
            this.m_ConnectionString = "";
            this.SqlString = "";
            this.Value = mDefault.ToString(format);
        }

        public string GetHtmlDataString(string uid)
        {
            if (this.Connection != null)
            {
                this.ConnectionString = this.Connection.ConnectionString;
            }
            object[] objArray1 = new object[] { " data-dbsingle='json_", uid, "' data-datetime='' data-updatecycle='", this.UpdateCycle, "' data-decimalplace='", this.Decimalplace, "'" };
            return string.Concat(objArray1);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Connection", this.Connection);
            info.AddValue("SqlString", this.SqlString);
            info.AddValue("ValueType", this.ValueType);
            info.AddValue("Record", this.Record);
            info.AddValue("UpdateCycle", this.UpdateCycle);
            info.AddValue("Value", this.Value);
            info.AddValue("DateFormat", this.DateFormat);
            info.AddValue("Decimalplace", this.Decimalplace);
            info.AddValue("ConnectionString", this.ConnectionString);
        }

        public string GetObjectJson(string uid)
        {
            string str = ScadaJsonConvertor.ObjectToJson(this);
            string[] textArray1 = new string[] { " <script id='json_", uid, "' type='application/json'> ", str, "</script>" };
            return string.Concat(textArray1);
        }

        public override string ToString()
        {
            if (this.Connection == null)
            {
                string[] textArray1 = new string[] { (this.Record != null) ? this.Record.ToString() : "", " ", this.UpdateCycle.ToString(), " ", this.Value };
                return string.Concat(textArray1);
            }
            return this.Connection.ToString();
        }

        public void Dispose()
        {
            Record = null;
            Connection = null;
            SqlString = null;
        }

        public string elementId { get; set; }

        public SCADAFlow_DataBaseType ValueType { get; set; }

        public SCADAFlow_DataBaseRecord Record { get; set; }

        public int UpdateCycle { get; set; }

        public string DateFormat { get; set; }

        public string ConnectionString
        {
            get
            {
                return this.m_ConnectionString;
            }
            set
            {
                this.m_ConnectionString = value;
            }
        }

        public string Value { get; set; }

        public ScadaConnectionBase Connection { get; set; }

        public string SqlString { get; set; }
    }
}

