using Temporal.Net.Common.Enums;
using Temporal.Net.InfluxDb;
using Temporal.Net.InfluxDb.Models;
using Temporal.Net.InfluxDb.Models.Responses;
using ScadaWeb.Common;
using ScadaWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Temporal.DBUtility;

namespace Temporal.WebDbAPI
{
    public class InfluxDBHistoryResult
    {
        public IEnumerable<Serie> Seres = null;
        public int PageSize = 5000;
        public int PageCount = 0;
        public int PageIndex = 1;
        public int RecordCount = 0;
        /// <summary>
        /// 查询成功与否的条件
        /// </summary>
        public bool ReturnResult = false;
        /// <summary>
        /// 返回的相关说明
        /// </summary>
        public string Msg = "";
    }
    public class InfluxDBQueryPara
    {
        /// <summary>
        /// 通讯状态
        /// </summary>
        public int DeviceStatus { set; get; }
        public string IOServerID
        {
            set;
            get;
        }
        public string IOCommunicateID
        {
            set;
            get;
        }
        public string IODeviceID
        {
            set;
            get;
        }
        public string TableName
        {
            get { return    IOServerID + "_" + IODeviceID; }
        }
        public string SDate
        {
            set;
            get;
        }
        public string EDate
        {
            set;
            get;
        }
        public int UpdateCycle
        {
            set;
            get;
        }
        public string Fields = "";
    }
    public class InfluxDBEMachineTrainPara 
    {
        public string IOServerID
        {
            set;
            get;
        }
        public string IOCommunicateID
        {
            set;
            get;
        }
        public string IODeviceID
        {
            set;
            get;
        }

        public string TaskId
        {
            set;
            get;
        }
        public int ForecastPriod
        {
            set;get;
        }
    }
    public delegate void ExceptionHandle(Exception ex);
    public class WebInfluxDbManager
    {
        public WebInfluxDbManager(string _uri, string _dbname, string _user, string _password, string _version)
        {
            InfluxDbMeasurement.DbName = _dbname;
            InfluxDbMeasurement.Uri = _uri;
            InfluxDbMeasurement.User = _user;
            InfluxDbMeasurement.Password = _password;
            InfluxDbMeasurement.Version = _version;

            ShouldConnectInfluxDb();
        }
        public WebInfluxDbManager()
        {
            InfluxDbMeasurement.DbName = Configs.GetValue("InfluxDataBase");
            InfluxDbMeasurement.Uri = Configs.GetValue("InfluxHttpAddress");
            InfluxDbMeasurement.User = Configs.GetValue("InfluxUser");
            InfluxDbMeasurement.Password = Configs.GetValue("InfluxPassword");
            InfluxDbMeasurement.Version = Configs.GetValue("InfluxDBVersion");
            ShouldConnectInfluxDb();
        }
        public event ExceptionHandle InfluxException;
       
        private IInfluxDbClient _influx;
  
     
        public string RealDataTablePrefix
        {
            get { return InfluxDbMeasurement.FakeMeasurementPrefix; }
        }
      
        private void DisplayException(Exception ex)
        {
            if (InfluxException != null)
            {
                InfluxException(ex);
            }
        }
        /// <summary>
        /// 配置数据库连接
        /// </summary>
        /// <returns></returns>
        public bool ShouldConnectInfluxDb()
        {


            try
            {


                //TODO: 使这个可写入的，以便它可以用不同的数据从测试服务器执行
                InfluxDbVersion influxVersion;
                if (!Enum.TryParse(InfluxDbMeasurement.Version, out influxVersion))
                    influxVersion = InfluxDbVersion.v_1_3;

                _influx = new InfluxDbClient(
                   InfluxDbMeasurement.Uri,
                   InfluxDbMeasurement.User,
                  InfluxDbMeasurement.Password,
                    influxVersion, QueryLocation.FormData);

                //如果不存在此数据库，则进行创建，否则不创建
                if (!ExistDatabases(InfluxDbMeasurement.DbName))
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10002" + ex.Message));
            }

            return false;

        }

        //清除数据库
        private void PurgeDatabases()
        {
            try
            {
                var databasesResponse = _influx.Database.GetDatabasesAsync();
                var dbs = databasesResponse.Result;

                foreach (var db in dbs)
                {
                    if (db.Name.StartsWith(InfluxDbMeasurement.FakeDbPrefix))
                        _influx.Database.DropDatabaseAsync(db.Name);
                }
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10003" + ex.Message));
            }
        }

        private bool ExistDatabases(string dbName)
        {
            try
            {
                var databasesResponse = _influx.Database.GetDatabasesAsync();
                var dbs = databasesResponse.Result;

                foreach (var db in dbs)
                {
                    if (db.Name.Trim() == dbName)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                DisplayException(new Exception("ERR10004" + ex.Message));
                return false;
            }

        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private bool BackupDatabases(string dbName)
        {
            try
            {
                var databasesResponse = _influx.Database.GetDatabasesAsync();
                var dbs = databasesResponse.Result;

                foreach (var db in dbs)
                {
                    if (db.Name.Trim() == dbName)
                    {

                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {

                DisplayException(new Exception("ERR10004" + ex.Message));
                return false;
            }

        }
        /// <summary>
        /// 完成数据库的卸载(删除时序数据库)
        /// </summary>
        /// <returns></returns>
        private bool FinalizeDropDatabase()
        {
            var deleteResponse = _influx.Database.DropDatabaseAsync(InfluxDbMeasurement.DbName).Result;

            return deleteResponse.Success;
        }

        //删除数据库
        private void ShouldDropInfluxDb(string dbName)
        {
            try
            {


                // Act
                var deleteResponse = _influx.Database.DropDatabaseAsync(dbName);
                // Assert

            }
            catch (Exception ex)
            {

                DisplayException(new Exception("ERR10005" + ex.Message));

            }

        }
        /// <summary>
        /// 创建新的数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private void ShouldCreateInfluxDb(string dbName)
        {

            try
            {
                // Act
                var createResponse = _influx.Database.CreateDatabaseAsync(dbName);
                // Assert

            }
            catch (Exception ex)
            {

                DisplayException(new Exception("ERR10006" + ex.Message));

            }

        }

        //Web端只负责查询处理，不负责influxDB实时数据的写入
        ////////////////////////////////////////以下是读取相关的方法
       
        public static string GetInfluxdbValue(object obj)
        {
            if (obj == null)
                return "";
            if (obj.GetType() == typeof(string))
            {
                return obj == null ? "" : obj.ToString();
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(obj == null ? "" : obj.ToString(), out dt))
                {
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return obj == null ? "-9999" : obj.ToString();
            }

        }


       

        /// <summary>
        /// 读取某个设备的历史数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public InfluxDBHistoryResult  DbQuery_History(string serverid, string communicationid, string deviceid, DateTime SDate, DateTime EDate, int PageSize, int PageIndex, string orderAction)
        {
            string tablename = serverid + "_" + deviceid;
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                try
                {
                    string queryCount = "SELECT COUNT(field_device_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + tablename + " where   time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName).Result;
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + tablename + " where   time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "'    ORDER BY time " + orderAction + "   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
                    if (readerCountResponse != null && readerCountResponse.Count() > 0)
                    {
                        Serie s = readerCountResponse.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName).Result;
                    datas.Seres = readerResponse;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch 
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
        /// <summary>
        /// 读取某个设备的历史统计数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public InfluxDBHistoryResult  DbQuery_HistoryStatics(string serverid, string communicationid, string deviceid, DateTime SDate, DateTime EDate, int PageSize, int PageIndex, string orderAction, string timespan, string returnfields)
        {
            string tablename = serverid + "_" + deviceid;
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                try
                {
                    string queryCount = "SELECT COUNT(*) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + tablename + " where   time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  group by time(" + timespan + ")";
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName).Result;
                    string query = "select " + returnfields + " from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + tablename + " where   time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "'    group by time(" + timespan + ")   ORDER BY time " + orderAction + "   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
                    if (readerCountResponse != null && readerCountResponse.Count() > 0)
                    {
                        Serie s = readerCountResponse.Last();
                        if (s != null && s.Values.Count >= 1)
                        {

                            datas.RecordCount = s.Values.Count;



                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName).Result;
                    datas.Seres = readerResponse;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch  
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
 
 


        /// <summary>
        /// 读取某个设备的历史报警数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public InfluxDBHistoryResult DbQuery_Alarms(string serverid, string communicationid, string deviceid, DateTime SDate, DateTime EDate, string AlarmType, string AlarmLevel, int PageSize, int PageIndex)
        {
            if (AlarmType == "0")
                AlarmType = "";
            if (AlarmLevel == "0")
                AlarmLevel = "";
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + serverid + "' ";
                if (!string.IsNullOrWhiteSpace(serverid))
                {
                    where += " and tag_sid='" + serverid + "'";
                }
                if (!string.IsNullOrWhiteSpace(communicationid))
                {
                    where += " and tag_cid='" + communicationid + "'";
                }
                if (!string.IsNullOrWhiteSpace(deviceid))
                {
                    where += " and tag_did='" + deviceid + "'";
                }
                if (!string.IsNullOrWhiteSpace(AlarmType))
                {
                    where += " and tag_level='" + AlarmType + "'";
                }
                if (!string.IsNullOrWhiteSpace(AlarmLevel))
                {
                    where += " and tag_type='" + AlarmLevel + "'";
                }
                try
                {
                    string queryCount = "SELECT COUNT(field_io_alarm_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmMeasurement + " where   " + where;
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName).Result;
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
                    if (readerCountResponse != null && readerCountResponse.Count() > 0)
                    {
                        Serie s = readerCountResponse.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse.Result;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch 
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }

        /// <summary>
        /// 读取某个设备的下置命令历史记录
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryResult> DbQuery_Commands(string serverid, string communicationid, string deviceid, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
         
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss");
                
                if(!string.IsNullOrEmpty(deviceid))
                {
                    where += " and tag_did='" + deviceid + "'";
                }
                if (!string.IsNullOrEmpty(communicationid))
                {
                    where += " and tag_cid='" + communicationid + "'";
                }
                if (!string.IsNullOrEmpty(serverid))
                {
                    where += " and tag_sid='" + serverid + "'";
                }

               

                try
                {
                    string queryCount = "SELECT COUNT(field_command_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.CommandsMeasurement + " where   " + where;
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.CommandsMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
                    if (readerCountResponse != null && readerCountResponse.Count() > 0)
                    {
                        Serie s = readerCountResponse.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = await _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch 
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
        /// <summary>
        /// 读取某个设备报警配置记录
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryResult> DbQuery_AlarmConfigs(string serverid, string communicationid, string deviceid, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {

            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + serverid + "' and tag_cid='" + communicationid + "' and tag_did='" + deviceid + "'";

                try
                {
                    string queryCount = "SELECT COUNT(field_command_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmConfigMeasurement + " where   " + where;
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmConfigMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
                    if (readerCountResponse != null && readerCountResponse.Count() > 0)
                    {
                        Serie s = readerCountResponse.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = await _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch 
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
       

        /// <summary>
        /// 读取多个采集站的事件内容并返回
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<Serie>> MultiQueryRealEvent(List<InfluxDBQueryPara> services,int timespan)
        {

            try
            {
                List<string> querys = new List<string>();
                for (int i = 0; i < services.Count; i++)
                {
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement + " where tag_sid='"+ services[i].IOServerID + "'   time>='" + DateTime.Now.AddSeconds(0 - timespan).ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY time DESC ";
                    querys.Add(query);
                }

                var writeResponse = _influx.Client.MultiQueryAsync(querys, InfluxDbMeasurement.DbName, null).Result;
                return writeResponse;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10012" + ex.Message));
                return null;

            }
        }

 
        /// <summary>
        /// 读取多个采集站设备状态内容并返回 timespan 如果当前时间没有数据表示最新的状态数据未上传，否则返回每个设备的最新状态
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<Serie>> MultiQueryRealMachineTrain(List<InfluxDBEMachineTrainPara> trains)
        {

            try
            {
                List<string> querys = new List<string>();
                for (int i = 0; i < trains.Count; i++)
                {
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement + " where tag_sid='" + trains[i].IOServerID + "' and tag_cid='" + trains[i].IOCommunicateID + "' and tag_did='" + trains[i].IODeviceID + "'  and tag_taskid='" + trains[i].TaskId + "' and   time>='" + DateTime.Now.AddMinutes(0- trains[i].ForecastPriod).ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY time DESC ";

                    querys.Add(query);
                }

                var writeResponse = _influx.Client.MultiQueryAsync(querys, InfluxDbMeasurement.DbName, null).Result;
                return writeResponse;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10012" + ex.Message));
                return null;

            }
        }


        public bool DbUpdate_AlarmPoints(ScadaAlarmModel alarmModel)
        {
            try
            {


                var points = CreateAlarmPoint(alarmModel.IO_SERVER_ID, alarmModel.IO_COMMUNICATE_ID, alarmModel);
                return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, "ms").Result.Success;

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return false;

        }
        private Point CreateAlarmPoint(string serverid, string communicationid, ScadaAlarmModel alarm)
        {

            Point point = new Point();
            point.Fields = CreateParaAlarmFields(alarm);
            point.Tags = CreateParaAlarmTags(serverid, communicationid, alarm);
            point.Timestamp = Convert.ToDateTime(alarm.time);
            point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmMeasurement;

            return point;
        }

        /// <summary>
        /// 创建实时数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private Dictionary<string, object> CreateParaAlarmTags(string serverid, string communicationid, ScadaAlarmModel alarm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                dict.Add("tag_did", alarm.IO_DEVICE_ID.ToString());
                dict.Add("tag_cid", communicationid.ToString());
                dict.Add("tag_sid", serverid.ToString());
                dict.Add("tag_ioid", alarm.IO_ID.ToString());
                dict.Add("tag_type", (string)alarm.IO_ALARM_TYPE);
                dict.Add("tag_level", (string)alarm.IO_ALARM_LEVEL);
                dict.Add("tag_device_name", (string)alarm.DEVICE_NAME);
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10013" + ex.Message));
            }

            return dict;
        }
        /// <summary>
        /// 创建实时数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private Dictionary<string, object> CreateParaAlarmFields(ScadaAlarmModel alarm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("field_io_alarm_date", (string)alarm.IO_ALARM_DATE);
            dict.Add("field_io_alarm_disposalidea", (string)alarm.IO_ALARM_DISPOSALIDEA);
            dict.Add("field_io_alarm_disposaluser", (string)alarm.IO_ALARM_DISPOSALUSER);
            dict.Add("field_io_alarm_level", (string)alarm.IO_ALARM_LEVEL);
            dict.Add("field_io_alarm_type", (string)alarm.IO_ALARM_TYPE);
            dict.Add("field_io_alarm_value", (string)alarm.IO_ALARM_VALUE);
            dict.Add("field_io_label", (string)alarm.IO_LABEL);
            dict.Add("field_io_name", (string)alarm.IO_NAME);

            return dict;
        }
        #region 下置命令类
        public async Task DbWrite_CommandPoints(List<IO_COMMANDS> commands, DateTime time)
        {
            try
            {


                var points = this.CreateCommandPoints(commands, time);
                var writeResponse = await _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, "ms");

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }

        }
        private Point[] CreateCommandPoints(List<IO_COMMANDS> commands, DateTime time)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < commands.Count; i++)
            {
                Point np = CreateCommandPoint(commands[i].IO_SERVER_ID, commands[i].IO_COMM_ID, commands[i], time);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }
        private Point CreateCommandPoint(string serverid, string communicationid, IO_COMMANDS command, DateTime? time)
        {

            Point point = new Point();
            point.Fields = this.CreateCommandFields(command);
            point.Tags = this.CreateCommandTags(serverid, communicationid, command);
            point.Timestamp = time;
            point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.CommandsMeasurement;

            return point;
        }
        ///
        /// <summary>
        /// 创建实时数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private Dictionary<string, object> CreateCommandTags(string serverid, string communicationid, IO_COMMANDS command)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                dict.Add("tag_did", command.IO_DEVICE_ID.ToString());
                dict.Add("tag_cid", communicationid.ToString());
                dict.Add("tag_sid", serverid.ToString());
                dict.Add("tag_ioid", command.IO_ID.ToString());
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR51013" + ex.Message));
            }

            return dict;
        }
        /// <summary>
        /// 创建实时数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private Dictionary<string, object> CreateCommandFields(IO_COMMANDS command)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("field_command_date", (string)command.COMMAND_DATE);
            dict.Add("field_command_id", command.COMMAND_ID.ToString());
            dict.Add("field_command_result", (string)command.COMMAND_RESULT);
            dict.Add("field_command_user", (string)command.COMMAND_USER);
            dict.Add("field_command_value", (string)command.COMMAND_VALUE.ToString());
            dict.Add("field_label", (string)command.IO_LABEL.ToString());
            dict.Add("field_send_user", (string)command.COMMAND_SEND_USER.ToString());
            dict.Add("field_send_username", (string)command.COMMAND_SEND_USERNAME.ToString());
     
            return dict;
        }
        #endregion
        #region 事件查询
        /// <summary>
        /// 读取某个设备的实时报警数据,实时数据默认是当天的报警
        /// </summary>
        public InfluxDBHistoryResult DbQuery_Events(string timespan = "1d", string EventType = "",List<string> devices=null, int PageSize = 2000, int PageIndex = 1)
        {

            ///查询的数量最大为10000条，influxdb系统限制最大查询数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                string sql = "SELECT * FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement;

                string where = "  WHERE time > now() - " + timespan;
                if (!string.IsNullOrEmpty(EventType) && EventType != "0")
                {
                    where += " and tag_event='" + EventType + "'";
                }
                if (devices != null && devices.Count > 0)
                {
                    string destr = "";
                    foreach (string str in devices)
                    {
                        destr += " tag_did='" + str + "' or";

                    }


                    if (destr != "")
                    {
                        destr = destr.Remove(destr.Length - 2, 2);
                        where += " and (" + destr + ")";
                    }
                 
                }
               
                sql += where;
                sql += "   ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize;
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;

                try
                {
                    string queryCount = "SELECT COUNT(field_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement + "    " + where;
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = sql;
                    if (readerCountResponse != null && readerCountResponse.Result.Count() > 0)
                    {
                        Serie s = readerCountResponse.Result.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse.Result;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
        /// <summary>
        /// 查询某个采集站下的所有历史事件
        /// </summary>
        /// <returns></returns>
        public InfluxDBHistoryResult DbQuery_Events(DateTime SDate, DateTime EDate,string EventType = "", string serverid = "", string commid = "", string deviceid = "", int PageSize = 2000, int PageIndex = 1)
        {
            if (EventType == "0")
                EventType = "";
          

            ///查询的数量最大为10000条，influxdb系统限制最大查询数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                string where = " WHERE  time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                string sql = "SELECT * FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement;

          
                if (!string.IsNullOrEmpty(EventType) && EventType != "0")
                {
                    where += " and tag_event='" + EventType + "'";
                }
                if (!string.IsNullOrEmpty(serverid) && serverid != "0")
                {
                    where += " and tag_sid='" + serverid + "'";
                }
                if (!string.IsNullOrEmpty(commid) && commid != "0")
                {
                    where += " and tag_cid='" + commid + "'";
                }
                if (!string.IsNullOrEmpty(deviceid) && deviceid != "0")
                {
                    where += " and tag_did='" + deviceid + "'";
                }
                 
                sql += where;
                sql += "   ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize;
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;

                try
                {
                    string queryCount = "SELECT COUNT(field_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement + "    " + where;
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = sql;
                    if (readerCountResponse != null && readerCountResponse.Result.Count() > 0)
                    {
                        Serie s = readerCountResponse.Result.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse.Result;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
        public InfluxDBHistoryResult DbQuery_Events(string timespan = "1d", string EventType = "", string serverid = "", string commid = "", string deviceid = "", int PageSize = 2000, int PageIndex = 1)
        {
            if (EventType == "0")
                EventType = "";


            ///查询的数量最大为10000条，influxdb系统限制最大查询数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                string where = "  WHERE time > now() - " + timespan;
                string sql = "SELECT * FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement;


                if (!string.IsNullOrEmpty(EventType) && EventType != "0")
                {
                    where += " and tag_event='" + EventType + "'";
                }
                if (!string.IsNullOrEmpty(serverid) && serverid != "0")
                {
                    where += " and tag_sid='" + serverid + "'";
                }
                if (!string.IsNullOrEmpty(commid) && commid != "0")
                {
                    where += " and tag_cid='" + commid + "'";
                }
                if (!string.IsNullOrEmpty(deviceid) && deviceid != "0")
                {
                    where += " and tag_did='" + deviceid + "'";
                }

                sql += where;
                sql += "   ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize;
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;

                try
                {
                    string queryCount = "SELECT COUNT(field_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement + "    " + where;
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = sql;
                    if (readerCountResponse != null && readerCountResponse.Result.Count() > 0)
                    {
                        Serie s = readerCountResponse.Result.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse.Result;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }


        #endregion

        #region 工况查询
        /// <summary>
        /// 读取某个设备的实时工况数据,实时数据默认是当天的
        /// </summary>
        public InfluxDBHistoryResult DbQuery_MachineTraingingForeast(string timespan = "1d", string serverid = "", string commid = "", string deviceid = "", int PageSize = 2000, int PageIndex = 1)
        {

            ///查询的数量最大为10000条，influxdb系统限制最大查询数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                string sql = "SELECT * FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement;
                string where = "  WHERE time > now() - " + timespan;
                if (!string.IsNullOrEmpty(serverid) && serverid != "0")
                {
                    where += " and tag_sid='" + serverid + "'";
                }
                if (!string.IsNullOrEmpty(commid) && commid != "0")
                {
                    where += " and tag_cid='" + commid + "'";
                }
                if (!string.IsNullOrEmpty(deviceid) && deviceid != "0")
                {
                    where += " and tag_did='" + deviceid + "'";
                }
                sql += where;
                sql += "   ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize;
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;

                try
                {
                    string queryCount = "SELECT COUNT(time) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement + "    " + where;
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = sql;
                    if (readerCountResponse != null && readerCountResponse.Result.Count() > 0)
                    {
                        Serie s = readerCountResponse.Result.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse.Result;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }
        /// <summary>
        /// 查询某个采集站下的所有历史事件
        /// </summary>
        /// <returns></returns>
        public InfluxDBHistoryResult DbQuery_MachineTraingingHistoryForeast(DateTime SDate, DateTime EDate, string serverid = "", string commid = "", string deviceid = "", int PageSize = 2000, int PageIndex = 1)
        {
            

            ///查询的数量最大为10000条，influxdb系统限制最大查询数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                string where = " WHERE  time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                string sql = "SELECT * FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement;


                
                if (!string.IsNullOrEmpty(serverid) && serverid != "0")
                {
                    where += " and tag_sid='" + serverid + "'";
                }
                if (!string.IsNullOrEmpty(commid) && commid != "0")
                {
                    where += " and tag_cid='" + commid + "'";
                }
                if (!string.IsNullOrEmpty(deviceid) && deviceid != "0")
                {
                    where += " and tag_did='" + deviceid + "'";
                }

                sql += where;
                sql += "   ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize;
                InfluxDBHistoryResult datas = new InfluxDBHistoryResult();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;

                try
                {
                    string queryCount = "SELECT COUNT(field_TaskName) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement + "    " + where;
                    var readerCountResponse = _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = sql;
                    if (readerCountResponse != null && readerCountResponse.Result.Count() > 0)
                    {
                        Serie s = readerCountResponse.Result.Last();
                        if (s != null && s.Values.Count == 1)
                        {
                            if (s.Values[0][1] != null)
                            {
                                datas.RecordCount = int.Parse(s.Values[0][1].ToString());
                            }

                        }
                    }
                    datas.PageCount = datas.RecordCount / datas.PageSize;
                    if (datas.RecordCount % datas.PageSize != 0)
                    {
                        datas.PageCount++;
                    }
                    if (datas.PageCount == 0)
                    {
                        datas.PageIndex = 0;

                    }

                    var readerResponse = _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                    datas.Seres = readerResponse.Result;
                    datas.ReturnResult = true;
                    if (+datas.RecordCount > 0)
                        datas.Msg = "共计有" + datas.RecordCount + "条数据,当前" + datas.PageIndex + "/" + datas.PageCount + ",每页显示" + datas.PageSize;
                    else
                        datas.Msg = "没有符合查询条件的数据";
                }
                catch
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }



        }


        #endregion
    }
}
