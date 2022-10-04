
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
//在调试过程中如果发现相关的bug或者代码错误等问题可直接微信联系作者。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Temporal.Net.InfluxDb.Models;

namespace Temporal.DBUtility
{
   public class InfluxDbPointManager
    {
        #region 预测数据点属性字段创建


        public static Point CreateForeastPoint(ScadaMachineTrainingForecastCacheObject eventObj)
        {

            try
            {
                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateForeastFields(eventObj.MachineTrainingForecast);
                point.Tags = InfluxDbFieldManager.CreateForeastTags(eventObj.MachineTrainingForecast);
                point.Timestamp = DateTime.Now;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.ForeastMeasurement;
                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point[] CreateForeastPoints(List<ScadaMachineTrainingForecastCacheObject> events)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < events.Count; i++)
            {
                Point np = CreateForeastPoint(events[i]);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }


        #endregion
        #region 机器训练日志表属性字段创建


        public static Point CreateMachineTrainPoint(MachineTrainInfluxDBModel eventObj)
        {

            try
            {
                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateMachineTrainFields(eventObj);
                point.Tags = InfluxDbFieldManager.CreateMachineTrainTags(eventObj);
                point.Timestamp = DateTime.Now;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.MachineTrainMeasurement;
                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point[] CreateMachineTrainPoints(List<MachineTrainInfluxDBModel> events)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < events.Count; i++)
            {
                Point np = CreateMachineTrainPoint(events[i]);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }


        #endregion
        #region 自动控制任务日志表属性字段创建


        public static Point CreateBatchCommandPoint(BatchCommandInfluxDBModel eventObj)
        {

            try
            {
                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateBatchCommandFields(eventObj);
                point.Tags = InfluxDbFieldManager.CreateBatchCommandTags(eventObj);
                point.Timestamp = DateTime.Now;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.BatchCommandMeasurement;
                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point[] CreateBatchCommandPoints(List<BatchCommandInfluxDBModel> events)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < events.Count; i++)
            {
                Point np = CreateBatchCommandPoint(events[i]);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }


        #endregion
        #region 报警配置表日志
        public static Point CreateAlarmConfigPoint(string serverid, string communicationid, IO_ALARM_CONFIG alarm, DateTime? time)
        {

            Point point = new Point();
            point.Fields = InfluxDbFieldManager.CreateParaAlarmConfigFields(alarm);
            point.Tags = InfluxDbFieldManager.CreateParaAlarmConfigTags(serverid, communicationid, alarm);
            point.Timestamp = time;
            point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmConfigMeasurement;

            return point;
        }
        public static Point[] CreateAlarmConfigPoints(List<IO_PARAALARM> alarms, DateTime time)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < alarms.Count; i++)
            {
                Point np = CreateAlarmPoint(alarms[i].IO_SERVER_ID, alarms[i].IO_COMM_ID, alarms[i], time);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }
        #endregion
        #region 下置命令执行
        public static Point CreateCommandPoint(string serverid, string communicationid, IO_COMMANDS command, DateTime? time)
        {

            Point point = new Point();
            point.Fields = InfluxDbFieldManager.CreateCommandFields(command);
            point.Tags = InfluxDbFieldManager.CreateCommandTags(serverid, communicationid, command);
            point.Timestamp = time;
            point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.CommandsMeasurement;

            return point;
        }
        public static Point[] CreateCommandPoints(List<IO_COMMANDS> commands, DateTime time)
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
        #endregion
        #region 备份
        public static Point CreateBackupPoint(InfluxDBBackupLog backup, DateTime? time)
        {

            Point point = new Point();
            point.Fields = InfluxDbFieldManager.CreateBackupFields(backup);
            point.Tags = InfluxDbFieldManager.CreateBackupTags(backup);
            point.Timestamp = time;
            point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.InfluxDBBackupMeasurement;

            return point;
        }
        public static Point[] CreateBackupPoints(List<IO_COMMANDS> commands, DateTime time)
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
        #endregion
        /// <summary>
        /// 创建一个事实数据表
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>

        //写入设备数据表数据，如果没有相关设备的数据表，则对应建立相关结构
        public static Point CreateDevicePoint(string serverid, string communicationid, IO_DEVICE device, DateTime? time)
        {
            try
            {

                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateDeviceParaFields(device);
                point.Tags = InfluxDbFieldManager.CreateDeviceParaTags(serverid, communicationid, device);
                point.Timestamp = device.GetedValueDate;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + device.TableName;

                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point CreateDevicePoint(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, DateTime time)
        {


            return CreateDevicePoint(server.SERVER_ID, communication.IO_COMM_ID.ToString(), device, time);
        }
        //写入设备数据表数据，如果没有相关设备的数据表，则对应建立相关结构
        public static Point[] CreateDevicePoint(List<IO_DEVICE> devices, DateTime time)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < devices.Count; i++)
            {
                Point np = CreateDevicePoint(devices[i].IO_SERVER_ID, devices[i].IO_COMM_ID, devices[i], time);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }
        public static Point CreateAlarmPoint(string serverid, string communicationid, IO_PARAALARM alarm, DateTime time)
        {

            try
            {
                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateParaAlarmFields(alarm);
                point.Tags = InfluxDbFieldManager.CreateParaAlarmTags(serverid, communicationid, alarm);
                point.Timestamp = time;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.AlarmMeasurement;

                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point[] CreateAlarmPoints(List<IO_PARAALARM> alarms, DateTime time)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < alarms.Count; i++)
            {
                Point np = CreateAlarmPoint(alarms[i].IO_SERVER_ID, alarms[i].IO_COMM_ID, alarms[i], time);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }
        public static Point[] CreateAlarmPoints(List<IO_PARAALARM> alarms)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < alarms.Count; i++)
            {
                Point np = CreateAlarmPoint(alarms[i].IO_SERVER_ID, alarms[i].IO_COMM_ID, alarms[i], Convert.ToDateTime(alarms[i].IO_ALARM_DATE));
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }

        #region 事件字段属性创建
        public static Point CreateEventPoint(ScadaEventModel eventObj, DateTime time)
        {

            try
            {
                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateEventFields(eventObj);
                point.Tags = InfluxDbFieldManager.CreateEventTags(eventObj);
                point.Timestamp = time;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.EventMeasurement;

                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point[] CreateAlarmPoints(List<ScadaEventModel> events)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < events.Count; i++)
            {
                Point np = CreateEventPoint(events[i], Convert.ToDateTime(events[i].Date));
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }
        public static Point[] CreateEventPoints(List<ScadaEventModel> events)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < events.Count; i++)
            {
                Point np = CreateEventPoint(events[i], Convert.ToDateTime(events[i].Date));
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }
        #endregion

        #region 状态属性字段创建


        public static Point CreateStatusPoint(ScadaStatusCacheObject eventObj)
        {

            try
            {
                Point point = new Point();
                point.Fields = InfluxDbFieldManager.CreateStatusFields(eventObj);
                point.Tags = InfluxDbFieldManager.CreateStatusTags(eventObj);
                point.Timestamp = DateTime.Now;
                point.Name = InfluxDbMeasurement.FakeMeasurementPrefix + "_" + InfluxDbMeasurement.StatusMeasurement;
                return point;
            }
            catch
            {
                return null;
            }
        }
        public static Point[] CreateStatusPoints(List<ScadaStatusCacheObject> events)
        {
            List<Point> Points = new List<Point>();
            for (int i = 0; i < events.Count; i++)
            {
                Point np = CreateStatusPoint(events[i]);
                if (np != null)
                    Points.Add(np);
            }
            return Points.ToArray();
        }


        #endregion

    }
}
