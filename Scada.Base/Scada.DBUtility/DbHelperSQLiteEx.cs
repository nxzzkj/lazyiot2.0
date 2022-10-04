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
    using System.Data.SqlClient;
    using System.Data.SQLite;

    public class DbHelperSQLiteEx
    {
        public string connectionString = "";

        public void Compress()
        {
            SQLiteConnection conn = new SQLiteConnection(this.connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                this.PrepareCommand(cmd, conn, null, "VACUUM");
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException exception1)
            {
                throw new Exception(exception1.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public SQLiteDataReader ExecuteReader(string strSQL)
        {
            SQLiteDataReader reader;
            SQLiteConnection connection = new SQLiteConnection(this.connectionString);
            SQLiteCommand command = new SQLiteCommand(strSQL, connection);
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch (SQLiteException exception1)
            {
                throw new Exception(exception1.Message);
            }
            return reader;
        }

        public SQLiteDataReader ExecuteReader(string SQLString, params SQLiteParameter[] cmdParms)
        {
            SQLiteDataReader reader;
            SQLiteConnection conn = new SQLiteConnection(this.connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                this.PrepareCommand(cmd, conn, null, SQLString, cmdParms);
                cmd.Parameters.Clear();
                reader = cmd.ExecuteReader();
            }
            catch (SQLiteException exception1)
            {
                throw new Exception(exception1.Message);
            }
            return reader;
        }

        public int ExecuteSql(string SQLString)
        {
            int num;
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(SQLString, connection);
                try
                {
                    connection.Open();
              
                    num = command.ExecuteNonQuery();
        
                }
                catch (SQLiteException exception1)
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
                    connection.Close();
                }
            }
            return num;
        }

        public int ExecuteSql(string SQLString, string content)
        {
            int num;
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(SQLString, connection);
                SQLiteParameter parameter = new SQLiteParameter("@content", DbType.String) {
                    Value = content
                };
                command.Parameters.Add(parameter);
                try
                {
                    connection.Open();
                    num = command.ExecuteNonQuery();
                }
                catch (SQLiteException exception1)
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

        public int ExecuteSql(string SQLString, params SQLiteParameter[] cmdParms)
        {
            int num;
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    try
                    {
                        this.PrepareCommand(command, connection, null, SQLString, cmdParms);
                        command.Parameters.Clear();
                        command.Dispose();
                        num = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException exception1)
                    {
                        throw new Exception(exception1.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return num;
        }

        public int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            int num;
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(strSQL, connection);
                SQLiteParameter parameter = new SQLiteParameter("@fs", DbType.Binary) {
                    Value = fs
                };
                command.Parameters.Add(parameter);
                try
                {
                    connection.Open();
                    num = command.ExecuteNonQuery();
                }
                catch (SQLiteException exception1)
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
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection
                };
                SQLiteTransaction transaction = connection.BeginTransaction();
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
                catch (SQLiteException exception1)
                {
                    transaction.Rollback();
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    try
                    {
                        foreach (DictionaryEntry entry in SQLStringList)
                        {
                            string cmdText = entry.Key.ToString();
                            SQLiteParameter[] cmdParms = (SQLiteParameter[]) entry.Value;
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
                    finally
                    {
                        connection.Close();
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

        public bool Exists(string strSql, params SQLiteParameter[] cmdParms)
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
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(SQLString, connection);
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
                catch (SQLiteException exception1)
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

        public object GetSingle(string SQLString, params SQLiteParameter[] cmdParms)
        {
            object obj3;
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    try
                    {
                        this.PrepareCommand(command, connection, null, SQLString, cmdParms);
                        object objA = command.ExecuteScalar();
                        command.Parameters.Clear();
                        if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
                        {
                            return null;
                        }
                        obj3 = objA;
                    }
                    catch (SQLiteException exception1)
                    {
                        throw new Exception(exception1.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return obj3;
        }

        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText)
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
        }

        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
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
                foreach (SQLiteParameter parameter in cmdParms)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public DataSet Query(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    new SQLiteDataAdapter(SQLString, connection).Fill(dataSet, "ds");
                }
                catch (SQLiteException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                finally
                {
                    connection.Close();
                }
                return dataSet;
            }
        }

        public DataSet Query(string SQLString, params SQLiteParameter[] cmdParms)
        {
            DataSet set2;
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                this.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        adapter.Fill(dataSet, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SQLiteException exception1)
                    {
                        throw new Exception(exception1.Message);
                    }
                    connection.Close();
                    set2 = dataSet;
                }
            }
            return set2;
        }

        public DataSet QueryPage(string SQLString, int pageindex, int pagesize)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    new SQLiteDataAdapter(SQLString, connection).Fill(dataSet, pagesize * pageindex, pagesize, "ds");
                }
                catch (SqlException exception1)
                {
                    throw new Exception(exception1.Message);
                }
                return dataSet;
            }
        }
    }
}

