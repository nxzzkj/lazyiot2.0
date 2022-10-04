using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Core
{
    public class IOMonitorRelationalDatabase: IDisposable
    {
        private string _ConnectString = "";
        public string ConnectString
        {   

            get
            {
                switch (Database_Type)
                {
                    case "SQL Server":
                        _ConnectString = "DataSourse=" + this.Database_IP + ";Initial Catalog=" + this.Database_SourceName + ";User ID=" + this.Database_User + ";Password=" + this.Database_Password + ";Trusted_Connection=False;Max Pool Size = 512;Connect Timeout =2";
                        break;
                    case "ORACLE":
                        _ConnectString = "Data Source="+this.Database_SourceName+";User Id="+this.Database_User+";Password="+this.Database_Password+ "; Connection Timeout=2";
                        break;
                    case "MySql":
                        _ConnectString = "server="+ this.Database_IP + ";database="+ Database_SourceName + "; uid="+ this.Database_User + ";pwd=" + this.Database_Password + ";charset=gb2312";
                        break;
                }
                return _ConnectString;

            }
        }
        /// <summary>
        /// 获取对应的sql语句
        /// </summary>
        /// <returns></returns>
        public string GetSql()
        {
            if (this.ValueRecordName.Trim() == "")
                return "";
            if (this.Database_SourceTable.Trim() == "")
                return "";
            if (this.Database_Type.Trim() == "")
                return "";
            if (this.DateRecordName.Trim() == "")
                return "";
            
            string selectValueRecord = this.ValueRecordName;
            string selectDateRecord = this.DateRecordName+" as datetime ";
            string groupby = "";
            string whereRecord = "";
            if (DateRecordName == "")
            {
                selectDateRecord = "";
            }
            
            if (selectDateRecord != "")
            {
                groupby = " group by " + DateRecordName;

            }
            if (this.Where != "")
            {
                whereRecord = Where;
            }
 
            switch (ValueRecordType)
            {
                case "NONE":
                    selectValueRecord = " " + selectValueRecord + " as value ";
                    break;
                case "MAX":
                    selectValueRecord = " Max(" + selectValueRecord + ") as value";
                    break;
                case "MIN":
                    selectValueRecord = " Min(" + selectValueRecord + ") as value";
                    break;
                case "AVG":
                    selectValueRecord = " Avg(" + selectValueRecord + ") as value";
                    break;
            }
            string sql = "select " + selectDateRecord + "," + selectValueRecord + " from " + Database_SourceTable + " where 1=1 ";
            DateTime dtEnd = DateTime.Now;
            DateTime dtStart = DateTime.Now;

            switch (this.DateRecordRange)
            {
                case "30分钟":
                    dtStart = dtStart.AddMinutes(-30);
                    break;
                case "1小时":
                    dtStart = dtStart.AddHours(-1);
                    break;
                case "2小时":
                    dtStart = dtStart.AddHours(-2);
                    break;
                case "3小时":
                    dtStart = dtStart.AddHours(-3);
                    break;
                case "4小时":
                    dtStart = dtStart.AddHours(-4);
                    break;
                case "5小时":
                    dtStart = dtStart.AddHours(-5);
                    break;
                case "6小时":
                    dtStart = dtStart.AddHours(-6);
                    break;
                case "12小时":
                    dtStart = dtStart.AddHours(-12);
                    break;
                case "1天":
                    dtStart = dtStart.AddDays(-1);
                    break;
                case "2天":
                    dtStart = dtStart.AddDays(-2);
                    break;
                case "最新日期":
                    dtStart = dtStart.AddMinutes(-5);
                    break;
                default:
                    dtStart = dtStart.AddMinutes(-5);
                    break;

            }


            switch (Database_Type)
            {
                case "SQL Server":
                    {
                        if (this.DateRecordName.Trim() != "")
                        {
                            sql += " and " + this.DateRecordName + " between '" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                    }
                    break;
                case "ORACLE":
                    {
                        if (this.DateRecordName.Trim() != "")
                        {
                            sql += " and " + this.DateRecordName + " between timestatmp'" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and timestatmp'" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                    }
                    break;
                case "MySql":
                    {
                        if (this.DateRecordName.Trim() != "")
                        {
                            sql += " and " + this.DateRecordName + " between '" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                    }
                    break;
            }
            if (whereRecord != "")
            {
                sql += " and " + whereRecord;
            }
            if (groupby != "")
            {
                sql += "  " + groupby;
            }
            if (this.OrderRecordName != ""&& this.OrderRecordType!="")
            {
                sql += "  order by " + this.OrderRecordName + " " + this.OrderRecordType;
            }
            return sql;

        }

        public void Dispose()
        {
            
        }

        public IOMonitorRelationalDatabase()
        {
            Database_IP = "";
            Database_SourceName = "";
            Database_Type = "";
            Database_User = "";
            Database_Password = "";
            Database_SourceTable = "";
            ValueRecordName = "";
            ValueRecordType = "";
            DateRecordName = "";
            DateRecordRange = "";
            OrderRecordName = "";
            OrderRecordType = "";
            Where = "";
        }
        public IOMonitorRelationalDatabase(string parastr)
        {
            Database_IP = "";
            Database_SourceName = "";
            Database_Type = "";
            Database_User = "";
            Database_Password = "";
            Database_SourceTable = "";
            ValueRecordName = "";
            ValueRecordType = "";
            DateRecordName = "";
            DateRecordRange = "";
            OrderRecordName = "";
            OrderRecordType = "";
            Where = "";

            if (parastr.Trim() == "")
                return;
            string[] datasorces = parastr.Split(',');
            if (datasorces.Length <= 5)
                return;
            try
            {


                Database_IP = datasorces[0];
                Database_SourceName = datasorces[1];
                Database_Type = datasorces[2];
                Database_User = datasorces[3];
                Database_Password = datasorces[4];
                Database_SourceTable = datasorces[5];
                ValueRecordName = datasorces[6];
                ValueRecordType = datasorces[7];
                DateRecordName = datasorces[8];
                DateRecordRange = datasorces[9];
                OrderRecordName = datasorces[10];
                OrderRecordType = datasorces[11];
                Where = datasorces[12];
            }
            catch 
            {

            }

        }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Database_Type
        {
            set;
            get;
        }
        //数据源IP
        public string Database_IP
        {
            set;get;
        }
        //数据源名称
        public string Database_SourceName
        {
            set; get;
        }
        //数据库账户
        public string Database_User
        {
            set; get;
        }
        /// <summary>
        /// 数据库密码
        /// </summary>
        public string Database_Password
        {
            set; get;
        }
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public string Database_SourceTable
        {
            set;get;
        }
        /// <summary>
        /// 要绑定的值字段
        /// </summary>
        public string ValueRecordName
        {
            set;get;
        }
        /// <summary>
        /// 要绑定的值字段统计类型
        /// </summary>
        public string ValueRecordType
        {
            set; get;
        }
        /// <summary>
        /// 要绑定的数据日期的字段
        /// </summary>
        public string DateRecordName
        {
            set; get;
        }
        /// <summary>
        /// 要绑定数据日期字段的筛选条件
        /// </summary>
        public string DateRecordRange
        {
            set; get;
        }
        /// <summary>
        /// 要排序的字段
        /// </summary>
        public string OrderRecordName
        {
            set;
            get;
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string OrderRecordType
        {
            set;
            get;
        }
        public string Where
        {
            set;
            get;
        }
    }
}
