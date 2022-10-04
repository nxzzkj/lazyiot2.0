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
    public class SyBase_Connection : ScadaConnectionBase
    {
        public int Port;
        public string DataSource;
        public string servername;
        public string database;
        public string UID;
        public string PWD;

        public SyBase_Connection()
        {
            this.Port = 0x1838;
            this.DataSource = "";
            this.servername = "";
            this.database = "";
            this.UID = "";
            this.PWD = "";
            base.DataBaseType = DataBaseType.SyBase;
            base.Icon = ScadaFlowRes.sybase;
        }

        public SyBase_Connection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Port = 0x1838;
            this.DataSource = "";
            this.servername = "";
            this.database = "";
            this.UID = "";
            this.PWD = "";
            this.Port = (int) info.GetValue("Port", typeof(int));
            this.DataSource = (string) info.GetValue("DataSource", typeof(string));
            this.servername = (string) info.GetValue("servername", typeof(string));
            this.database = (string) info.GetValue("database", typeof(string));
            this.UID = (string) info.GetValue("UID", typeof(string));
            this.PWD = (string) info.GetValue("PWD", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Port", this.Port);
            info.AddValue("DataSource", this.DataSource);
            info.AddValue("servername", this.servername);
            info.AddValue("database", this.database);
            info.AddValue("UID", this.UID);
            info.AddValue("PWD", this.PWD);
        }

        public override string ToString()
        {
            string[] textArray1 = new string[] { base.DataBaseType.ToString(), " ", DESEncrypt.Decrypt(this.DataSource), " ", DESEncrypt.Decrypt(this.servername), " ", DESEncrypt.Decrypt(this.database) };
            return string.Concat(textArray1);
        }
    }
}

