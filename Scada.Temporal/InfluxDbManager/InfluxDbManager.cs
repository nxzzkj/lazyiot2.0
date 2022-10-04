using System;
using System.Collections.Generic;
using System.Linq;


using FluentAssertions;

using System.Configuration;
using System.Threading.Tasks;

using Scada.DBUtility;

using Scada.Model;
using Temporal.Net.Common.Enums;
using Temporal.Net.InfluxDb;
using Temporal.Net.InfluxDb.Models;
using AutoFixture;
using Temporal.Net.InfluxDb.Models.Responses;
using System.Net.Http;
using System.Globalization;
using Temporal.DBUtility;

namespace Temporal.DbAPI
{
    public delegate void ExceptionHandle(Exception ex);
    public class InfluxDbManager
    {

        public InfluxDbManager(string uri,string _dbname, string _user, string _password, string _version)
        {
            InfluxDbMeasurement.DbName = _dbname;
            InfluxDbMeasurement.Uri = uri;
            InfluxDbMeasurement.User = _user;
            InfluxDbMeasurement.Password = _password;
            InfluxDbMeasurement.Version = _version;
        }
        public event ExceptionHandle InfluxException;
      
        private int _AlarmConfigUpdateCryle = 10;//获取用户5秒前的报警设置信息
        private IInfluxDbClient _influx;
      
      
    

      
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
        public void  ShouldConnectInfluxDb()
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

                _influx.Should().NotBeNull();

                //如果不存在此数据库，则进行创建，否则不创建
                if (!ExistDatabases(InfluxDbMeasurement.DbName))
                {
                    var createResponse = _influx.Database.CreateDatabaseAsync(InfluxDbMeasurement.DbName).Result;
                    createResponse.Success.Should().BeTrue();

                }
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10002" + ex.Message));
            }



        }
        //清除数据库
        private async void PurgeDatabases()
        {
            try
            {
                var databasesResponse = _influx.Database.GetDatabasesAsync();
                var dbs = databasesResponse.Result;

                foreach (var db in dbs)
                {
                    if (db.Name.StartsWith(InfluxDbMeasurement.FakeDbPrefix))
                        await _influx.Database.DropDatabaseAsync(db.Name);
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
        protected void  FinalizeDropDatabase()
        {
            var deleteResponse = _influx.Database.DropDatabaseAsync(InfluxDbMeasurement.DbName).Result;

            deleteResponse.Success.Should().BeTrue();
        }


        //删除数据库
        private async Task ShouldDropInfluxDb(string dbName)
        {
            try
            {


                // Act

                var deleteResponse = await _influx.Database.DropDatabaseAsync(dbName);

                // Assert

                deleteResponse.Success.Should().BeTrue();
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
        private async Task ShouldCreateInfluxDb(string dbName)
        {

            try
            {
                // Act
                var createResponse = await _influx.Database.CreateDatabaseAsync(dbName);


                // Assert
                createResponse.Success.Should().BeTrue();
            }
            catch (Exception ex)
            {

                DisplayException(new Exception("ERR10006" + ex.Message));

            }

        }
      
    
        /// <summary>
        /// 返回已经存在的数据库列表
        /// </summary>
        /// <returns></returns>
        private async Task DbShowDatabases_OnDatabaseExists_ShouldReturnDatabaseList(string dbName)
        {

            try
            {
                // Act
                var databases = await _influx.Database.GetDatabasesAsync();

                // Assert
                databases
                    .Should()
                    .NotBeNullOrEmpty();

                databases
                    .Where(db => db.Name.Equals(dbName))
                    .Single()
                    .Should()
                    .NotBeNull();
            }
            catch (Exception ex)
            {

                DisplayException(new Exception("ERR10007" + ex.Message));


            }
        }
        /// <summary>
        /// 写入实时数据
        /// </summary>
        /// <returns></returns>
        public   Task DbWrite_RealPoints(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime? time)
        {
            try
            {

                if (server == null)
                {
                    DisplayException(new Exception("ERR10009_1 Server IS NULL"));
                }
                if (communication == null)
                {
                    DisplayException(new Exception("ERR10009_2 Communication IS NULL"));
                }
                if (device == null)
                {
                    DisplayException(new Exception("ERR10009_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateDevicePoint(server.SERVER_ID, communication.IO_COMM_ID, device, time);
                if (points != null)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                 
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;
        }
        private List<Point> GetPoints(List<IO_DEVICE> devices)
        {
            List<Point> points = new List<Point>();
            devices.ForEach(delegate (IO_DEVICE device)
            {
                var point = InfluxDbPointManager.CreateDevicePoint(device.IO_SERVER_ID, device.IO_COMM_ID, device, device.GetedValueDate);
                if (point != null)
                { points.Add(point); }
            });
            devices.Clear();
            devices = null;
            return points;
        }
        /// <summary>
        /// 批量多个实时数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public   Task DbWrite_RealPoints(List<IO_DEVICE> devices)
        {
            try
            {
                var points = GetPoints(devices);
             return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;
        }

        public   Task DbWrite_RealPoints(List<IO_DEVICE> devices, DateTime time)
        {
            try
            {


                var points = InfluxDbPointManager.CreateDevicePoint(devices, time);
                if(points!=null&& points.Count()>0)
                {
                    return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                  
                }
               
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }


        ///////////////////////报警写入
        /// <summary>
        /// 写入报警数据
        /// </summary>
        /// <returns></returns>
        public   Task DbWrite_AlarmPoints(IO_PARAALARM alarm, DateTime time)
        {
            string serverid = alarm.IO_SERVER_ID;
            string communicationid = alarm.IO_COMM_ID;
            try
            {

                 
                if (alarm == null)
                {
                    DisplayException(new Exception("ERR10009_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateAlarmPoint(serverid, communicationid, alarm, time);
                if (points != null)
                {
                    return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;
        }
        public   Task DbWrite_AlarmPoints(IO_PARAALARM alarm)
        {
            string serverid = alarm.IO_SERVER_ID;
            string communicationid = alarm.IO_COMM_ID;
            try
            {


                if (alarm == null)
                {
                    DisplayException(new Exception("ERR10009_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateAlarmPoint(serverid, communicationid, alarm, Convert.ToDateTime(alarm.IO_ALARM_DATE));
                if (points != null)
                {
              return   _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;
        }
        /// <summary>

        public   Task DbWrite_AlarmPoints(List<IO_PARAALARM> alarms, DateTime time)
        {
            try
            {


                var points = InfluxDbPointManager.CreateAlarmPoints( alarms, time);
                if(points!=null&& points.Count()>0)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                
                }
               
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        public   Task DbWrite_AlarmPoints(List<IO_PARAALARM> alarms)
        {
            try
            {


                var points = InfluxDbPointManager.CreateAlarmPoints(alarms);
                if (points != null && points.Count() > 0)
                {
                    return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                     
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        ////////////////////////结束报警写入
        //*****************************************************************
        ///////////////////////事件写入
        /// <summary>
        /// 写入事件数据
        /// </summary>
        /// <returns></returns>
        public   Task DbWrite_EventPoints(ScadaEventModel eventObj, DateTime time)
        {
            string serverid = eventObj.SERVER_ID;
            string communicationid = eventObj.COMM_ID;
            try
            {
                if (string.IsNullOrEmpty(eventObj.Id))
                    eventObj.Id = GUIDToNormalID.GuidToLongID().ToString();

                if (eventObj == null)
                {
                    DisplayException(new Exception("ERR10009_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateEventPoint( eventObj, time);
                if (points != null)
                {
                    return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                  
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;
        }
        public   Task DbWrite_EventPoints(ScadaEventModel eventObj)
        {
            if (string.IsNullOrEmpty(eventObj.Id))
                eventObj.Id = GUIDToNormalID.GuidToLongID().ToString();
            string serverid = eventObj.SERVER_ID;
            string communicationid = eventObj.COMM_ID;
            try
            {


                if (eventObj == null)
                {
                    DisplayException(new Exception("ERR10009_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateEventPoint( eventObj, Convert.ToDateTime(eventObj.Date));
                if (points != null)
                {
                   return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    
                }
                points = null;

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;
        }
        /// <summary>

        public   Task DbWrite_EventPoints(List<ScadaEventModel> events, DateTime time)
        {
            try
            {


                var points = InfluxDbPointManager.CreateEventPoints(events);
                if (points != null && points.Count() > 0)
                {


                 return    _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                 
                }
                points = null;

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        public   Task DbWrite_EventPoints(List<ScadaEventModel> events)
        {
            try
            {


                var points = InfluxDbPointManager.CreateEventPoints(events);
                if (points != null && points.Count() > 0)
                {
                    return  _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                     
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }

        //******************************************************************



        //*****************************************************************
        public   Task DbWrite_StatusPoints(ScadaStatusCacheObject eventObj, DateTime time)
        {
            if (eventObj == null)
            {
                DisplayException(new Exception("ERR10009_2 Device IS NULL"));
            }
            try
            {
                 
                
                var points = InfluxDbPointManager.CreateStatusPoint(eventObj);
                if (points != null)
                {
                 return   _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    
                }
              
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;

        }
 

        public   Task DbWrite_StatusPoints(List<ScadaStatusCacheObject> events)
        {
            try
            {


                var points = InfluxDbPointManager.CreateStatusPoints(events);
                if (points != null && points.Count() > 0)
                {
                    return   _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                   
                }
             
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        //******************************************************************


        //********************************机器预测*********************************
        public Task DbWrite_ForeastPoints(ScadaMachineTrainingForecastCacheObject foreastObj, DateTime time)
        {
            if (foreastObj == null)
            {
                DisplayException(new Exception("ERR10008_2 Device IS NULL"));
            }
            try
            {


                var points = InfluxDbPointManager.CreateForeastPoint(foreastObj);
                if (points != null)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

                }

                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;

        }


        public Task DbWrite_ForeastPoints(List<ScadaMachineTrainingForecastCacheObject> events)
        {
            try
            {


                var points = InfluxDbPointManager.CreateForeastPoints(events);
                if (points != null && points.Count() > 0)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

                }

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        //******************************************************************


        //********************************机器训练日志*********************************
        public Task DbWrite_MachineTrainPoints(MachineTrainInfluxDBModel influxdbObj, DateTime time)
        {
            if (influxdbObj == null)
            {
                DisplayException(new Exception("ERR10008_2 Device IS NULL"));
            }
            try
            {


                var points = InfluxDbPointManager.CreateMachineTrainPoint(influxdbObj);
                if (points != null)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

                }

                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;

        }


        public Task DbWrite_MachineTrainPoints(List<MachineTrainInfluxDBModel> events)
        {
            try
            {


                var points = InfluxDbPointManager.CreateMachineTrainPoints(events);
                if (points != null && points.Count() > 0)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

                }

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        //******************************************************************

        //********************************自动控制任务执行日志*********************************
        public Task DbWrite_BatchCommandPoints(BatchCommandInfluxDBModel influxdbObj, DateTime time)
        {
            if (influxdbObj == null)
            {
                DisplayException(new Exception("ERR10008_2 Device IS NULL"));
            }
            try
            {


                var points = InfluxDbPointManager.CreateBatchCommandPoint(influxdbObj);
                if (points != null)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

                }

                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10009" + ex.Message));


            }
            return null;

        }


        public Task DbWrite_BatchCommandPoints(List<BatchCommandInfluxDBModel> influxdbObjs)
        {
            try
            {


                var points = InfluxDbPointManager.CreateBatchCommandPoints(influxdbObjs);
                if (points != null && points.Count() > 0)
                {
                    return _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);

                }

            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }
            return null;
        }
        //******************************************************************


        ///////////////////////报警配置日志写入
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns></returns>
        public async Task DbWrite_AlarmConfigPoints(string serverid, string communicationid, IO_ALARM_CONFIG alarm, DateTime? time)
        {
            try
            {


                if (alarm == null)
                {
                    DisplayException(new Exception("ERR40009_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateAlarmConfigPoint(serverid, communicationid, alarm, time);
                if (points != null)
                {
                    var writeResponse = await _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    if (writeResponse != null)
                        writeResponse.Success.Should().BeTrue();
                }
             
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR40009" + ex.Message));


            }

        }
        /// <summary>

        public async Task DbWrite_AlarmConfigPoints(List<IO_PARAALARM> alarms, DateTime time)
        {
            try
            {


                var points = InfluxDbPointManager.CreateAlarmConfigPoints(alarms, time);
                if (points != null && points.Count() > 0)
                {
                    var writeResponse = await _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    if (writeResponse != null)
                        writeResponse.Success.Should().BeTrue();
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }

        }

        ////////////////////////结束报警写入

        //写入命令下置操作
        public async Task DbWrite_CommandPoint(string serverid, string communicationid, IO_COMMANDS command, DateTime? time)
        {
            try
            {


                if (command == null)
                {
                    DisplayException(new Exception("ERR50109_2 Device IS NULL"));
                }
                var points = InfluxDbPointManager.CreateCommandPoint(serverid, communicationid, command, time);
                if (points != null)
                {
                    var writeResponse = await _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    if (writeResponse != null)
                        writeResponse.Success.Should().BeTrue();
                }
              
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR50109" + ex.Message));


            }

        }
        public async Task DbWrite_CommandPoints(List<IO_COMMANDS> commands, DateTime time)
        {
            try
            {


                var points = InfluxDbPointManager.CreateCommandPoints(commands, time);
                if (points != null && points.Count() > 0)
                {
                    var writeResponse = await _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                    if (writeResponse != null)
                        writeResponse.Success.Should().BeTrue();
                }
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10010" + ex.Message));

            }

        }
        /// <summary>
        /// 写入数据库备份日志
        /// </summary>
        /// <param name="backup"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task DbWrite_BackupPoints(InfluxDBBackupLog backup, DateTime time)
        {
            try
            {


                var points = InfluxDbPointManager.CreateBackupPoint(backup, time);
                
                var writeResponse = await _influx.Client.WriteAsync(points, InfluxDbMeasurement.DbName, null, InfluxDbMeasurement.Precision);
                if (writeResponse != null)
                    writeResponse.Success.Should().BeTrue();
                points = null;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR13010" + ex.Message));

            }

        }
        ////////////////////////////////////////以下是读取相关的方法
        public static  string GetInfluxdbValue(object obj)
        {
            if (obj == null)
                return "";
            if(obj.GetType()==typeof(string))
            {
                return obj == null ? "" : obj.ToString();
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                DateTime dt = DateTime.Now;
                if(DateTime.TryParse(obj == null ? "" : obj.ToString(), out dt))
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
        //读取数据库备份日志,分页读取
        public async Task<InfluxDBHistoryData> DbQuery_Backup(int PageSize,int PageIndex)
        {
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            InfluxDBHistoryData datas = new InfluxDBHistoryData();
            try
            {
          
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                try
                {
                    string queryCount = "SELECT COUNT(field_device_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.InfluxDBBackupMeasurement + "  ";
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.InfluxDBBackupMeasurement + "    ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
                }
                return datas;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10015" + ex.Message));
                return null;

            }
        }

        //结束命令下置操作

        /// <summary>
        /// 读取一个设备的系列数据,并将读取的数据保存的Device
        /// </summary>
        /// <param name="server">采集站</param>
        /// <param name="communication">通道</param>
        /// <param name="device">设备</param>
        /// <returns></returns>
        public async Task<IEnumerable<Serie>> DbQuery_Real(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device)
        {
            try
            {

                string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + device.TableName + " where   time> '" + DateTime.Now.AddMilliseconds(-device.IO_DEVICE_UPDATECYCLE).ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY time DESC ";
                var readerResponse = await _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                return readerResponse;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10011" + ex.Message));
                return null;

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
        public async Task<InfluxDBHistoryData> DbQuery_History(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device,DateTime SDate, DateTime EDate,int PageSize,int PageIndex)
        {
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                try
                {
                    string queryCount = "SELECT COUNT(field_device_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + device.TableName + " where   time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY time DESC  ";
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + device.TableName + " where   time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "'    ORDER BY time DESC   LIMIT " + PageSize + " OFFSET "+ (PageIndex-1)* PageSize + "    ";
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
                    if(datas.PageCount==0)
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        /// 判断一个点是值类型的点还是字符串类型的点
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private bool IsNumberIOPoint(IO_PARA para)
        {
            //"模拟量",
            //"开关量",
            //"字符串量",
            //"计算值",
            //"关系数据库值",
            //"常量值"

                if (para.IO_POINTTYPE == "模拟量" ||
                para.IO_POINTTYPE == "开关量" ||
                para.IO_POINTTYPE == "计算值" ||
                 para.IO_POINTTYPE == "常量值")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 读取所有设备要做预测的时间段的数据,此处是求取这一段时间的平均值用来做预测
        /// </summary>
        /// <param name="devices">所有的预测设备</param>
        /// <param name="foreastDate">预测时间</param>
        /// <param name="foreastInterval"> 平均值间隔(分钟)</param>
        /// <returns></returns>
        public Task<List<IEnumerable<Serie>>> DbQuery_MachineTrainForeastSourceRealData(List<Scada.Model.ScadaMachineTrainingModel> trainingModels,List<IO_DEVICE> devices, DateTime foreastDate)
        {
            return Task<List<IEnumerable<Serie>>>.Run<List<IEnumerable<Serie>>>(async () =>
            {
                try
                {
                    List<string> Querys = new List<string>();
                    trainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel trainModel)
                    {

                        IO_DEVICE tempDevice = devices.Find(x => x.IO_SERVER_ID == trainModel.SERVER_ID
                        && x.IO_COMM_ID == trainModel.COMM_ID && x.IO_DEVICE_ID == trainModel.DEVICE_ID);
                        if (tempDevice != null)
                        {
                            string record = " time ";
                            for (int i = 0; i < trainModel.Properties.Split(',').Length; i++)
                            {
                                string ioName = trainModel.Properties.Split(',')[i].Trim().ToLower();
                                IO_PARA para = tempDevice.IOParas.Find(x=>x.IO_NAME.Trim().ToLower().Trim()== ioName.Trim().ToLower().Trim());
                                if(IsNumberIOPoint(para))
                                {
                                    record += ",MEAN(field_" + ioName + "_value) as " + ioName;
                                }
                                else
                                {
                                    record += ",MEAN(null)  as " + ioName;
                                }
                               

                            }
                            string query = "select " + record + " from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + trainModel.SERVER_ID.Trim() + "_" + trainModel.DEVICE_ID.Trim().ToLower() + " where   time>'" + foreastDate.AddMinutes(-trainModel.ForecastPriod.Value).ToString("yyyy-MM-dd HH:mm:00") + "' and time<'" + foreastDate.ToString("yyyy-MM-dd HH:mm:00") + "'  group by time(" + trainModel.ForecastPriod.Value + "m)     ORDER BY time asc    limit 1  ";
                            Querys.Add(query);
                        }


                    });
                    if (Querys != null && Querys.Count > 0)
                    {
                       //批量读取influxdb数据库，防止频繁读取操作
                       var readerResponse = await _influx.Client.MultiQueryAsync(Querys, InfluxDbMeasurement.DbName);
                        int index = 0;
                        trainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel trainModel)
                        {
                            if (trainModel.Conditions != null && trainModel.Conditions.Count > 0)
                            {
                                Serie s = readerResponse.ElementAt(index).First();
                                if (s != null)
                                {
                                    readerResponse.ElementAt(index).First().Tag = trainModel;

                                }
                                index++;
                            }
                        });
                        return readerResponse.ToList();
                    }
                }
                catch (Exception emx)
                {
                    DisplayException(new Exception(emx.Message));

                }
                return null;
            });

        }
        /// <summary>
        /// 读取某个设备的统计数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryData> DbQuery_RealDataHistoryStatics(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex,string selected,string timespan)
        {
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                try
                {
                    string queryCount = "SELECT COUNT(*) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + device.TableName + " where    time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  group by time("+ timespan + ")";
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select "+ selected + " from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + device.TableName + " where    time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "'      group by time(" + timespan + ")   ORDER BY time DESC  LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
 
                    if (readerCountResponse != null && readerCountResponse.Count() > 0)
                    {
                        Serie s = readerCountResponse.Last();
                        if (s != null)
                        {
                           
                                datas.RecordCount = int.Parse(s.Values.Count.ToString());
                          

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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        public async Task<InfluxDBHistoryData> DbQuery_Alarms(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime SDate, DateTime EDate, string AlarmType,string  AlarmLevel, int PageSize, int PageIndex)
        {
          
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='"+ server.SERVER_ID + "' and tag_cid='"+ communication.IO_COMM_ID + "' and tag_did='"+ device .IO_DEVICE_ID+ "'";
                if(AlarmType!="")
                {
                    where = " and  tag_type='"+ AlarmType + "'";
                }
                if (AlarmLevel != "")
                {
                    where = " and  tag_level='" + AlarmLevel + "'";
                }
                try
                {
                    string queryCount = "SELECT COUNT(field_io_alarm_date) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmMeasurement + " where   "+ where+ " ORDER BY time DESC  ";
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmMeasurement + " where  "+ where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        /// 读取某个设备的历史 事件数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryData> DbQuery_Events(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime SDate, DateTime EDate, string EventType,int PageSize, int PageIndex)
        {
        
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss");

                if(server!=null)
                {
                    where += "' and  tag_sid='" + server.SERVER_ID + "' ";
                }
              

                if(communication!=null)
                {
                    where += " and tag_cid ='" + communication.IO_COMM_ID + "'";
                }
                if (device != null)
                {
                    where += " and tag_did ='" + device.IO_DEVICE_ID + "'";
                }

                if (!string.IsNullOrEmpty(EventType))
                {
                    where = " and  tag_event='" + EventType + "'";
                }
              
                try
                {
                    string queryCount = "SELECT COUNT(field_id) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement + " where   " + where + " ORDER BY time DESC  ";
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        public async Task<InfluxDBHistoryData> DbQuery_Commands(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
          
            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + server.SERVER_ID + "' and tag_cid='" + communication.IO_COMM_ID + "' and tag_did='" + device.IO_DEVICE_ID + "'";
               
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        /// 读取某个机器训练任务执行日志数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryData> DbQuery_MachineTrain(ScadaMachineTrainingModel machineTrain, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {

            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + machineTrain.SERVER_ID + "' and tag_cid='" + machineTrain.COMM_ID + "' and tag_did='" + machineTrain.DEVICE_ID + "'  and   tag_taskid='" + machineTrain.Id + "'";

                try
                {
                    string queryCount = "SELECT COUNT(field_createtime) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.MachineTrainMeasurement + " where   " + where;
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.MachineTrainMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        /// 获取机器训练预测的数据
        /// </summary>
        /// <param name="machineTrain"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryData> DbQuery_MachineTrainForeast(ScadaMachineTrainingModel machineTrain, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {

            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + machineTrain.SERVER_ID + "' and tag_cid='" + machineTrain.COMM_ID + "' and tag_did='" + machineTrain.DEVICE_ID + "'  and   tag_taskid='" + machineTrain.Id + "'";

                try
                {
                    string queryCount = "SELECT COUNT(field_predicteddate) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement + " where   " + where;
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        /// 获取自动控制命令日志数据
        /// </summary>
        /// <param name="machineTrain"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryData> DbQuery_BatchCommandTask(BatchCommandTaskModel commandTask, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {

            //条件限制最大页是10000条，超过此数量，强制归为最大数量
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + commandTask.SERVER_ID + "'  and   tag_taskid='" + commandTask.Id + "'";

                try
                {
                    string queryCount = "SELECT COUNT(field_createtime) FROM " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.BatchCommandMeasurement + " where   " + where;
                    var readerCountResponse = await _influx.Client.QueryAsync(queryCount, InfluxDbMeasurement.DbName);
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.BatchCommandMeasurement + " where  " + where + "     ORDER BY time DESC   LIMIT " + PageSize + " OFFSET " + (PageIndex - 1) * PageSize + "    ";
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
                catch (Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        ///  
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex">从1开始</param>
        /// <returns></returns>
        public async Task<InfluxDBHistoryData> DbQuery_AlarmConfigs(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
          
            if (PageSize > 10000)
                PageSize = 10000;
            try
            {
                InfluxDBHistoryData datas = new InfluxDBHistoryData();
                datas.PageSize = PageSize;
                datas.PageIndex = PageIndex;
                string where = "time>='" + SDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<='" + EDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and  tag_sid='" + server.SERVER_ID + "' and tag_cid='" + communication.IO_COMM_ID + "' and tag_did='" + device.IO_DEVICE_ID + "'";

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
                catch(Exception emx)
                {
                    datas.ReturnResult = false;
                    datas.Msg = "查询异常";
                    DisplayException(new Exception(emx.Message));
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
        /// 读取用户重新设置的报警配置信息
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Serie>> DbQuery_AlarmConfig()
        {
            try
            {

                string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmConfigMeasurement + " where   time> '" + DateTime.Now.AddSeconds(-this._AlarmConfigUpdateCryle).ToString("yyyy-MM-dd HH:mm:ss") + "'";


                var writeResponse = await _influx.Client.QueryAsync(query, InfluxDbMeasurement.DbName);
                return writeResponse;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10011" + ex.Message));
                return null;

            }



        }

        /// <summary>
        /// 读取多个设备数据并返回
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IEnumerable<Serie>>> MultiQueryAsync(List<IO_DEVICE> devices)
        {
            try
            {
                List<string> querys = new List<string>();
                for (int i = 0; i < devices.Count; i++)
                {
                    string query = "select * from " + InfluxDbMeasurement.FakeMeasurementPrefix + "_" + devices[i].TableName + " where   time>='" + DateTime.Now.AddSeconds(0 - devices[i].IO_DEVICE_UPDATECYCLE).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    querys.Add(query);

                }

                var writeResponse = await _influx.Client.MultiQueryAsync(querys, InfluxDbMeasurement.DbName, null);
                return writeResponse;
            }
            catch (Exception ex)
            {
                DisplayException(new Exception("ERR10012" + ex.Message));
                return null;

            }
        }
       
      
    }
}
