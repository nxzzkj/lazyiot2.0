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
    using System.Collections;
    using System.Data;
    using System.Data.OracleClient;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class DbHelperOra
    {
        public string connectionString = "";

        private OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command1 = this.BuildQueryCommand(connection, storedProcName, parameters);
            command1.Parameters.Add(new OracleParameter("ReturnValue", OracleType.Int32, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command1;
        }

        private OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection) {
                CommandType = CommandType.StoredProcedure
            };
            foreach (OracleParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }
        public bool ExecuteTest()
        {
            bool res = false;
            try
            {
                using (OracleConnection connection = new OracleConnection(this.connectionString))
                {
                    connection.Open();
                    connection.Close();
                    res = true;
                }
            }
            catch
            {
                res = false;
            }
            return res;
        }
        public OracleDataReader ExecuteReader(string strSQL)
        {
            OracleDataReader reader;
            OracleConnection connection = new OracleConnection(this.connectionString);
            OracleCommand command = new OracleCommand(strSQL, connection);
            try
            {
                connection.Open();
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (OracleException exception1)
            {
                throw new Exception(exception1.Message);
            }
            return reader;
        }

        public OracleDataReader ExecuteReader(string SQLString, params OracleParameter[] cmdParms)
        {
            OracleDataReader reader;
            OracleConnection conn = new OracleConnection(this.connectionString);
            OracleCommand cmd = new OracleCommand();
            try
            {
                this.PrepareCommand(cmd, conn, null, SQLString, cmdParms);
                cmd.Parameters.Clear();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (OracleException exception1)
            {
                throw new Exception(exception1.Message);
            }
            return reader;
        }

        public int ExecuteSql(string SQLString)
        {
            int num;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand command = new OracleCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    num = command.ExecuteNonQuery();
                }
                catch (OracleException exception1)
                {
                    connection.Close();
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            return num;
        }

        public int ExecuteSql(string SQLString, string content)
        {
            int num;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand command = new OracleCommand(SQLString, connection);
                OracleParameter parameter = new OracleParameter("@content", OracleType.NVarChar) {
                    Value = content
                };
                command.Parameters.Add(parameter);
                try
                {
                    connection.Open();
                    num = command.ExecuteNonQuery();
                }
                catch (OracleException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return num;
        }

        public int ExecuteSql(string SQLString, params OracleParameter[] cmdParms)
        {
            int num;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                try
                {
                    this.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    cmd.Parameters.Clear();
                    num = cmd.ExecuteNonQuery();
                }
                catch (OracleException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }
            return num;
        }

        public int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            int num;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand command = new OracleCommand(strSQL, connection);
                OracleParameter parameter = new OracleParameter("@fs", OracleType.LongRaw) {
                    Value = fs
                };
                command.Parameters.Add(parameter);
                try
                {
                    connection.Open();
                    num = command.ExecuteNonQuery();
                }
                catch (OracleException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return num;
        }

        public void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                connection.Open();
                OracleCommand command = new OracleCommand {
                    Connection = connection
                };
                OracleTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        string str = SQLStringList[i].ToString();
                        if (str.Trim().Length > 1)
                        {
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (OracleException exception1)
                {
                    transaction.Rollback();
                    throw new Exception(exception1.Message);
                }
            }
        }

        public void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    OracleCommand cmd = new OracleCommand();
                    try
                    {
                        foreach (DictionaryEntry entry in SQLStringList)
                        {
                            string cmdText = entry.Key.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[]) entry.Value;
                            this.PrepareCommand(cmd, connection, transaction, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            transaction.Commit();
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool Exists(string strSql)
        {
            int num;
            object single = this.GetSingle(strSql);
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(single.ToString());
            }
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public bool Exists(string strSql, params OracleParameter[] cmdParms)
        {
            int num;
            object single = this.GetSingle(strSql, cmdParms);
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(single.ToString());
            }
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public int GetMaxID(string FieldName, string TableName)
        {
            string sQLString = "select max(" + FieldName + ")+1 from " + TableName;
            object single = this.GetSingle(sQLString);
            if (single == null)
            {
                return 1;
            }
            return int.Parse(single.ToString());
        }

        public object GetSingle(string SQLString)
        {
            object obj3;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand command = new OracleCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    object objA = command.ExecuteScalar();
                    if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
                    {
                        return null;
                    }
                    obj3 = objA;
                }
                catch (OracleException exception1)
                {
                    connection.Close();
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            return obj3;
        }

        public object GetSingle(string SQLString, params OracleParameter[] cmdParms)
        {
            object obj3;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                try
                {
                    this.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    object objA = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
                    {
                        return null;
                    }
                    obj3 = objA;
                }
                catch (OracleException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }
            return obj3;
        }

        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (OracleParameter parameter in cmdParms)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public DataSet Query(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    new OracleDataAdapter(SQLString, connection).Fill(dataSet, "ds");
                }
                catch (OracleException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                return dataSet;
            }
        }

        public DataSet Query(string SQLString, params OracleParameter[] cmdParms)
        {
            DataSet set2;
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                this.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        adapter.Fill(dataSet, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException exception1)
                    {
                        throw new Exception(exception1.Message);
                    }
                    set2 = dataSet;
                }
            }
            return set2;
        }

        public DataSet QueryPage(string SQLString, int pageindex, int pagesize)
        {
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    new OracleDataAdapter(SQLString, connection).Fill(dataSet, pagesize * pageindex, pagesize, "ds");
                }
                catch (SqlException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                return dataSet;
            }
        }

        public OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(this.connectionString);
            connection.Open();
            OracleCommand command1 = this.BuildQueryCommand(connection, storedProcName, parameters);
            command1.CommandType = CommandType.StoredProcedure;
            return command1.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                new OracleDataAdapter { SelectCommand = this.BuildQueryCommand(connection, storedProcName, parameters) }.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (OracleConnection connection = new OracleConnection(this.connectionString))
            {
                connection.Open();
                OracleCommand command = this.BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                return (int) command.Parameters["ReturnValue"].Value;
            }
        }
    }
}

