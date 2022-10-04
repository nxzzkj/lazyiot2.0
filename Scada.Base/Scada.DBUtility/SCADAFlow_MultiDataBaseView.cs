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
    public class SCADAFlow_MultiDataBaseView : ISerializable, IDisposable
    {
        private string m_ConnectionString;

        public SCADAFlow_MultiDataBaseView()
        {
            this.m_ConnectionString = "";
            this.SqlString = "";
            this.UpdateCycle = 20;
            this.ViewFilter = new SCADAFlow_MultiDataBaseViewFilter();
            this.ViewPage = new SCADAFlow_MultiDataBaseViewPage();
            this.ValueType = SCADAFlow_DataBaseType.Table;
            this.AutoPage = false;
        }

        protected SCADAFlow_MultiDataBaseView(SerializationInfo info, StreamingContext context)
        {
            this.m_ConnectionString = "";
            this.Connection = (ScadaConnectionBase) info.GetValue("Connection", typeof(ScadaConnectionBase));
            this.SqlString = (string) info.GetValue("SqlString", typeof(string));
            this.m_ConnectionString = (string) info.GetValue("m_ConnectionString", typeof(string));
            this.UpdateCycle = (int) info.GetValue("UpdateCycle", typeof(int));
            this.AutoPage = (bool) info.GetValue("AutoPage", typeof(bool));
            this.ValueType = (SCADAFlow_DataBaseType) info.GetValue("ValueType", typeof(SCADAFlow_DataBaseType));
            this.ViewFilter = (SCADAFlow_MultiDataBaseViewFilter) info.GetValue("ViewFilter", typeof(SCADAFlow_MultiDataBaseViewFilter));
            this.ViewPage = (SCADAFlow_MultiDataBaseViewPage) info.GetValue("ViewPage", typeof(SCADAFlow_MultiDataBaseViewPage));
        }

        public string GetHtmlDataString(string uid)
        {
            if (this.Connection != null)
            {
                this.ConnectionString = this.Connection.ConnectionString;
            }
            object[] objArray1 = new object[] { " data-dbmultiple='json_", uid, "' data-datetime='' data-updatecycle='", this.UpdateCycle, "' " };
            return string.Concat(objArray1);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_ConnectionString", this.m_ConnectionString);
            info.AddValue("Connection", this.Connection);
            info.AddValue("SqlString", this.SqlString);
            info.AddValue("ValueType", this.ValueType);
            info.AddValue("ViewFilter", this.ViewFilter);
            info.AddValue("ViewPage", this.ViewPage);
            info.AddValue("UpdateCycle", this.UpdateCycle);
            info.AddValue("AutoPage", this.AutoPage);
        }

        public string GetObjectJson(string uid)
        {
            string str = ScadaJsonConvertor.ObjectToJson(this);
            string[] textArray1 = new string[] { " <script id='json_", uid, "' type='application/json'> ", str, "</script>" };
            return string.Concat(textArray1);
        }

        public override string ToString()
        {
            if (this.Connection != null)
            {
                return (this.Connection.ToString() + " " + this.ViewFilter.ToString());
            }
            return "";
        }

        public void Dispose()
        {
            ViewFilter = null;
            Connection = null;
        }

        public SCADAFlow_DataBaseType ValueType { get; set; }

        public string SqlString { get; set; }

        public int UpdateCycle { get; set; }

        public bool AutoPage { get; set; }

        public SCADAFlow_MultiDataBaseViewFilter ViewFilter { get; set; }

        public SCADAFlow_MultiDataBaseViewPage ViewPage { get; set; }

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

        public ScadaConnectionBase Connection { get; set; }
    }
}

