using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temporal.DBUtility
{
    public class InfluxDbMeasurement
    {
        public static string AlarmMeasurement = "DeviceParaAlarm";//报警数据表
        public static string EventMeasurement = "ScadaEvent";//事件数据表
        public static string StatusMeasurement = "ScadaDeviceStatus";//设备状态数据表
        public static string AlarmConfigMeasurement = "DeviceParaAlarmConfig";//用户修改报警数据表
        public static string ForeastMeasurement = "TrainingForeast";//机器学习算法预测的数据表
        public static string CommandsMeasurement = "DeviceParaCommands";//下置命令数据表
        public static string BatchCommandMeasurement = "BatchCommandTask";//自动控制命令日志表
        public static string MachineTrainMeasurement = "MachineTrainTask";//机器训练日志表
        public static string InfluxDBBackupMeasurement = "InfluxDBBackupLog";//备份信息表
        public static string DbName = String.Empty;
        public static readonly string FakeDbPrefix = "SCADADB";
        public static  readonly string FakeMeasurementPrefix = "SCADA";
        public static string User = "root";
        public static string Password = "root";
        public static string Version = "v_1_3";
        public static string Uri = "";
        public static string Precision = "ms";


    }
}
