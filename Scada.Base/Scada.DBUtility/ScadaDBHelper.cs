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
    using System.Data;

    public class ScadaDBHelper
    {
        private ScadaConnectionBase ConnectionBasic;

        public ScadaDBHelper(ScadaConnectionBase connectionBase)
        {
            this.ConnectionBasic = connectionBase;
        }
        public bool ExecuteTest()
        {
            switch (this.ConnectionBasic.DataBaseType)
            {
                case DataBaseType.SQLServer:
                    {
                        DbHelperSQL rsql1 = new DbHelperSQL
                        {
                            connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                        };
                        return rsql1.ExecuteTest();
                    }
                case DataBaseType.Oracle:
                    {
                        DbHelperOra ora1 = new DbHelperOra
                        {
                            connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                        };
                        return ora1.ExecuteTest();
                    }
                case DataBaseType.MySQL:
                    {
                        DbHelperMySQL ysql1 = new DbHelperMySQL
                        {
                            connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                        };
                        return ysql1.ExecuteTest();
                    }
                case DataBaseType.SyBase:
                    {
                        DbHelperSyBase base1 = new DbHelperSyBase
                        {
                            connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                        };
                        return base1.ExecuteTest();
                    }
                case DataBaseType.SQLite:
                    {
                        return false;
                    }
            }
            return false;
        }
        public int ExecuteSql(string sql)
        {
            switch (this.ConnectionBasic.DataBaseType)
            {
                case DataBaseType.SQLServer:
                {
                    DbHelperSQL rsql1 = new DbHelperSQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return rsql1.ExecuteSql(sql);
                }
                case DataBaseType.Oracle:
                {
                    DbHelperOra ora1 = new DbHelperOra {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ora1.ExecuteSql(sql);
                }
                case DataBaseType.MySQL:
                {
                    DbHelperMySQL ysql1 = new DbHelperMySQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ysql1.ExecuteSql(sql);
                }
                case DataBaseType.SyBase:
                {
                    DbHelperSyBase base1 = new DbHelperSyBase {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return base1.ExecuteSql(sql);
                }
                case DataBaseType.SQLite:
                {
                    DbHelperSQLiteEx ex1 = new DbHelperSQLiteEx {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ex1.ExecuteSql(sql);
                }
            }
            return 0;
        }

        public object GetSingle(string sql)
        {
            switch (this.ConnectionBasic.DataBaseType)
            {
                case DataBaseType.SQLServer:
                {
                    DbHelperSQL rsql1 = new DbHelperSQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return rsql1.GetSingle(sql);
                }
                case DataBaseType.Oracle:
                {
                    DbHelperOra ora1 = new DbHelperOra {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ora1.GetSingle(sql);
                }
                case DataBaseType.MySQL:
                {
                    DbHelperMySQL ysql1 = new DbHelperMySQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ysql1.GetSingle(sql);
                }
                case DataBaseType.SyBase:
                {
                    DbHelperSyBase base1 = new DbHelperSyBase {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return base1.GetSingle(sql);
                }
                case DataBaseType.SQLite:
                {
                    DbHelperSQLiteEx ex1 = new DbHelperSQLiteEx {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ex1.GetSingle(sql);
                }
            }
            return 0;
        }

        public DataSet Query(string sql)
        {
            switch (this.ConnectionBasic.DataBaseType)
            {
                case DataBaseType.SQLServer:
                {
                    DbHelperSQL rsql1 = new DbHelperSQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return rsql1.Query(sql);
                }
                case DataBaseType.Oracle:
                {
                    DbHelperOra ora1 = new DbHelperOra {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ora1.Query(sql);
                }
                case DataBaseType.MySQL:
                {
                    DbHelperMySQL ysql1 = new DbHelperMySQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ysql1.Query(sql);
                }
                case DataBaseType.SyBase:
                {
                    DbHelperSyBase base1 = new DbHelperSyBase {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return base1.Query(sql);
                }
                case DataBaseType.SQLite:
                {
                    DbHelperSQLiteEx ex1 = new DbHelperSQLiteEx {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ex1.Query(sql);
                }
            }
            return new DataSet();
        }

        public DataSet QueryPage(string sql, int pageindex, int pagesize)
        {
            if (pageindex <= 0)
            {
                pageindex = 1;
            }
            pageindex--;
            switch (this.ConnectionBasic.DataBaseType)
            {
                case DataBaseType.SQLServer:
                {
                    DbHelperSQL rsql1 = new DbHelperSQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return rsql1.QueryPage(sql, pageindex, pagesize);
                }
                case DataBaseType.Oracle:
                {
                    DbHelperOra ora1 = new DbHelperOra {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ora1.QueryPage(sql, pageindex, pagesize);
                }
                case DataBaseType.MySQL:
                {
                    DbHelperMySQL ysql1 = new DbHelperMySQL {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ysql1.QueryPage(sql, pageindex, pagesize);
                }
                case DataBaseType.SyBase:
                {
                    DbHelperSyBase base1 = new DbHelperSyBase {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return base1.QueryPage(sql, pageindex, pagesize);
                }
                case DataBaseType.SQLite:
                {
                    DbHelperSQLiteEx ex1 = new DbHelperSQLiteEx {
                        connectionString = DESEncrypt.Decrypt(this.ConnectionBasic.ConnectionString)
                    };
                    return ex1.QueryPage(sql, pageindex, pagesize);
                }
            }
            return new DataSet();
        }
    }
}

