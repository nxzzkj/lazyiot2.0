﻿namespace Scada.DBUtility
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
    public class Oracle_Connection : ScadaConnectionBase
    {
        public string user;
        public string password;
        public string DataSource;

        public Oracle_Connection()
        {
            this.user = "";
            this.password = "";
            this.DataSource = "";
            base.DataBaseType = DataBaseType.Oracle;
            base.Icon = ScadaFlowRes.oracle;
        }

        public Oracle_Connection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.user = "";
            this.password = "";
            this.DataSource = "";
            this.user = (string) info.GetValue("user", typeof(string));
            this.password = (string) info.GetValue("password", typeof(string));
            this.DataSource = (string) info.GetValue("DataSource", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("user", this.user);
            info.AddValue("password", this.password);
            info.AddValue("DataSource", this.DataSource);
        }

        public override string ToString() => 
            (base.DataBaseType.ToString() + " " + DESEncrypt.Decrypt(this.DataSource));
    }
}

