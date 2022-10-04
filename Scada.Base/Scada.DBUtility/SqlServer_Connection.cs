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
    using System.Runtime.Serialization;

    [Serializable]
    public class SqlServer_Connection : ScadaConnectionBase
    {
        public string Database;
        public string userid;
        public string password;
        public int ConnectTimeout;
        public string Server;

        public SqlServer_Connection()
        {
            this.Database = "";
            this.userid = "";
            this.password = "";
            this.ConnectTimeout = 30;
            this.Server = "";
            base.DataBaseType = DataBaseType.SQLServer;
            base.Icon = ScadaFlowRes.sqlserver;
        }

        public SqlServer_Connection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Database = "";
            this.userid = "";
            this.password = "";
            this.ConnectTimeout = 30;
            this.Server = "";
            this.Database = (string) info.GetValue("Database", typeof(string));
            this.userid = (string) info.GetValue("userid", typeof(string));
            this.password = (string) info.GetValue("password", typeof(string));
            this.ConnectTimeout = (int) info.GetValue("ConnectTimeout", typeof(int));
            this.Server = (string) info.GetValue("Server", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Database", this.Database);
            info.AddValue("userid", this.userid);
            info.AddValue("password", this.password);
            info.AddValue("ConnectTimeout", this.ConnectTimeout);
            info.AddValue("Server", this.Server);
        }

        public override string ToString()
        {
            string[] textArray1 = new string[] { base.DataBaseType.ToString(), " ", DESEncrypt.Decrypt(this.Server), " ", DESEncrypt.Decrypt(this.Database) };
            return string.Concat(textArray1);
        }
    }
}

