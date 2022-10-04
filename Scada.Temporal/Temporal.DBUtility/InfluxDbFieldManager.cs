
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
using Scada.DBUtility;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temporal.DBUtility
{
 public   class InfluxDbFieldManager
    {
        /// <summary>
        /// 创建实时数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateDeviceParaTags(string serverid, string communicationid, IO_DEVICE device)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                if (device != null)
                {
                    dict.Add("tag_did", device.IO_DEVICE_ID.ToString());
                    dict.Add("tag_cid", communicationid.ToString());
                    dict.Add("tag_sid", serverid.ToString());
                }



            }
            catch (Exception ex)
            {
               
            }

            return dict;
        }
        /// <summary>
        /// 创建实时数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateDeviceParaFields(IO_DEVICE device)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                if (device != null)
                {
                    //新增加一个时间点的
                    dict.Add("field_device_date", device.GetedValueDate == null ? "" : device.GetedValueDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));//创建数据采集时间,总时间，这个时间是所有数据统一获取的时间

                    for (int i = 0; i < device.IOParas.Count; i++)
                    {
                        IO_PARA npara = device.IOParas[i];
                        if (npara.IORealData == null)
                            continue;
                        if (string.IsNullOrEmpty(npara.IORealData.ParaValue))
                            continue;
                        if (npara.IORealData.ParaValue == "-9999")
                            continue;


                        npara.IORealData.ServerID = device.IO_SERVER_ID;
                        npara.IORealData.CommunicationID = device.IO_COMM_ID;
                        npara.IORealData.ID = device.IO_DEVICE_ID;

                        if (dict.ContainsKey("field_" + npara.IO_NAME.ToLower().Trim() + "_datetime"))
                        {
                            dict.Remove("field_" + npara.IO_NAME.ToLower().Trim() + "_datetime");
                        }


                        if (dict.ContainsKey("field_" + npara.IO_NAME.ToLower().Trim() + "_qualitystamp"))
                        {
                            dict.Remove("field_" + npara.IO_NAME.ToLower().Trim() + "_qualitystamp");

                        }


                        if (dict.ContainsKey("field_" + npara.IO_NAME.ToLower().Trim() + "_value"))
                        {
                            dict.Remove("field_" + npara.IO_NAME.ToLower().Trim() + "_value");
                        }
                        try
                        {
                            dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_datetime", npara.IORealData == null ? "" : npara.IORealData.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));//创建数据采集时间

                            if (npara.IORealData == null)
                            {
                                npara.IORealData = new Scada.IOStructure.IOData();
                                npara.IORealData.ParaName = npara.IO_NAME;
                                npara.IORealData.ParaValue = "-9999";
                                npara.IORealData.QualityStamp = Scada.IOStructure.QualityStamp.BAD;
                                npara.IORealData.Date = device.GetedValueDate;
                            }
                            //字符串量
                            float value = -9999;
                            if (npara.IO_POINTTYPE == "模拟量"
                                || npara.IO_POINTTYPE == "开关量"
                                || npara.IO_POINTTYPE == "计算值"
                                || npara.IO_POINTTYPE == "常量值")
                            {
                                if (float.TryParse(npara.IORealData.ParaValue, out value))
                                {
                                    dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_value", (float)value);
                                }
                                else
                                {
                                    npara.IORealData.QualityStamp = Scada.IOStructure.QualityStamp.BAD;
                                }

                            }
                            else if (npara.IO_POINTTYPE == "字符串量"
                               || npara.IO_POINTTYPE == "关系数据库值")
                            {

                                dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_value", npara.IORealData.ParaValue.ToString().Trim());
                            }


                            dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_qualitystamp", npara.IORealData.QualityStamp.ToString());//创建质量戳

                        }
                        catch (Exception ex)
                        {
                            npara.IORealData.QualityStamp = Scada.IOStructure.QualityStamp.BAD;
                            if (npara.IO_POINTTYPE == "模拟量"
                               || npara.IO_POINTTYPE == "开关量"
                               || npara.IO_POINTTYPE == "计算值"
                               || npara.IO_POINTTYPE == "常量值"
                               || npara.IO_POINTTYPE == "开关量")
                            {
                                dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_value", (float)-9999);
                            }
                            else
                            {
                                dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_value", "");
                            }

                            dict.Add("field_" + npara.IO_NAME.ToLower().Trim() + "_qualitystamp", "BAD");//创建质量戳
                                 }
                    }
                }
                return dict;

            }
            catch (Exception ex)
            {
             
            }

            return null;

        }

        /// <summary>
        /// 创建报警数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateParaAlarmTags(string serverid, string communicationid, IO_PARAALARM alarm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                if (alarm != null)

                {
                    if (string.IsNullOrEmpty(alarm.IO_ALARM_TYPE))
                    {
                        alarm.IO_ALARM_TYPE = "0";
                    }
                    if (string.IsNullOrEmpty(alarm.IO_DEVICE_ID))
                    {
                        alarm.IO_DEVICE_ID = "0";
                    }
                    if (string.IsNullOrEmpty(alarm.IO_ID))
                    {
                        alarm.IO_ID = "0";
                    }
                    if (string.IsNullOrEmpty(alarm.IO_ALARM_LEVEL))
                    {
                        alarm.IO_ALARM_LEVEL = "0";
                    }
                    if (string.IsNullOrEmpty(alarm.DEVICE_NAME))
                    {
                        alarm.DEVICE_NAME = "0";
                    }

                    dict.Add("tag_did", (string)alarm.IO_DEVICE_ID.ToString());
                    dict.Add("tag_cid", (string)communicationid.ToString());
                    dict.Add("tag_sid", (string)serverid.ToString());
                    dict.Add("tag_ioid", (string)alarm.IO_ID.ToString());
                    dict.Add("tag_type", (string)alarm.IO_ALARM_TYPE);
                    dict.Add("tag_level", (string)alarm.IO_ALARM_LEVEL);
                    dict.Add("tag_device_name", (string)alarm.DEVICE_NAME);
                }

            }
            catch (Exception ex)
            {
              
            }

            return dict;
        }
        /// <summary>
        /// 创建报警数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateParaAlarmFields(IO_PARAALARM alarm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (alarm != null)
            {


                dict.Add("field_io_alarm_date", (string)alarm.IO_ALARM_DATE);
                dict.Add("field_io_alarm_disposalidea", (string)alarm.IO_ALARM_DISPOSALIDEA);
                dict.Add("field_io_alarm_disposaluser", (string)alarm.IO_ALARM_DISPOSALUSER);
                dict.Add("field_io_alarm_level", (string)alarm.IO_ALARM_LEVEL);
                dict.Add("field_io_alarm_type", (string)alarm.IO_ALARM_TYPE);
                dict.Add("field_io_alarm_value", (string)alarm.IO_ALARM_VALUE);
                dict.Add("field_io_label", (string)alarm.IO_LABEL);
                dict.Add("field_io_name", (string)alarm.IO_NAME);
            }
            return dict;
        }
        #region 创建事件数据表中的Fields
        /// <summary>
        /// 创建报警数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateEventTags(ScadaEventModel eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {


                try
                {

                    if (string.IsNullOrEmpty(eventObj.COMM_ID))
                    {
                        eventObj.COMM_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.DEVICE_ID))
                    {
                        eventObj.DEVICE_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.IO_ID))
                    {
                        eventObj.IO_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.SERVER_ID))
                    {
                        eventObj.SERVER_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.Event))
                    {
                        eventObj.Event = "0";
                    }
                    dict.Add("tag_sid", (string)eventObj.SERVER_ID);
                    dict.Add("tag_event", (string)eventObj.Event);
                    dict.Add("tag_cid", (string)eventObj.COMM_ID);
                    dict.Add("tag_did", (string)eventObj.DEVICE_ID);
                    dict.Add("tag_ioid", (string)eventObj.IO_ID);


                }
                catch (Exception ex)
                {
                 
                }
            }
            return dict;
        }
        /// <summary>
        /// 创建报警数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateEventFields(ScadaEventModel eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                dict.Add("field_id", (string)eventObj.Id);
                dict.Add("field_comm_name", (string)eventObj.COMM_NAME);
                dict.Add("field_device_name", (string)eventObj.DEVICE_NAME);
                dict.Add("field_io_label", (string)eventObj.IO_LABEL);
                dict.Add("field_io_name", (string)eventObj.IO_NAME);
                dict.Add("field_event", (string)eventObj.Event);
                dict.Add("field_date", (string)eventObj.Date);
                dict.Add("field_content", (string)eventObj.Content);


            }

            return dict;
        }
        #endregion
        #region 创建状态数据表中的Fields
        /// <summary>
        /// 创建报警数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateStatusTags(ScadaStatusCacheObject eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                try
                {

                    if (string.IsNullOrEmpty(eventObj.COMM_ID))
                    {
                        eventObj.COMM_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.DEVICE_ID))
                    {
                        eventObj.DEVICE_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.SERVER_ID))
                    {
                        eventObj.SERVER_ID = "0";
                    }

                    dict.Add("tag_sid", (string)eventObj.SERVER_ID);
                    dict.Add("tag_cid", (string)eventObj.COMM_ID);
                    dict.Add("tag_did", (string)eventObj.DEVICE_ID);



                }
                catch (Exception ex)
                {
                
                }
            }
            return dict;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateStatusFields(ScadaStatusCacheObject eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
                dict.Add("field_status", (string)(eventObj.ScadaStatus == ScadaStatus.True ? "1" : "0"));

            return dict;
        }
        #endregion

        #region 创建机器学习预测数据表中的Fields
        /// <summary>
        /// 创建报警数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateForeastTags(ScadaMachineTrainingForecast eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                try
                {

                    if (string.IsNullOrEmpty(eventObj.COMM_ID))
                    {
                        eventObj.COMM_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.DEVICE_ID))
                    {
                        eventObj.DEVICE_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.SERVER_ID))
                    {
                        eventObj.SERVER_ID = "0";
                    }

                    dict.Add("tag_sid", (string)eventObj.SERVER_ID);
                    dict.Add("tag_cid", (string)eventObj.COMM_ID);
                    dict.Add("tag_did", (string)eventObj.DEVICE_ID);
                    dict.Add("tag_taskid", (string)eventObj.TaskId.ToString());


                }
                catch (Exception ex)
                {
                 
                }
            }
            return dict;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateForeastFields(ScadaMachineTrainingForecast eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                dict.Add("field_predicteddate", (string)(eventObj.PredictedDate.ToString("yyyy-MM-dd HH:mm:ss")));
                dict.Add("field_predictedlabel", (string)(eventObj.PredictedLabel));
                dict.Add("field_score", (string)(eventObj.Score));
                dict.Add("field_featuresname", (string)(eventObj.FeaturesName));
                dict.Add("field_featuresvalue", (string)(eventObj.FeaturesValue));
                dict.Add("field_algorithm", (string)(eventObj.Algorithm));
                dict.Add("field_algorithmtype", (string)(eventObj.AlgorithmType));
                dict.Add("field_server_name", (string)(eventObj.SERVER_NAME));
                dict.Add("field_comm_name", (string)(eventObj.COMM_NAME));
                dict.Add("field_device_name", (string)(eventObj.DEVICE_NAME));
                dict.Add("field_taskname", (string)(eventObj.TaskName));
                dict.Add("field_remark", (string)(eventObj.Remark));
            }


            return dict;
        }
        #endregion
        #region 创建机器学习训练日志表
        /// <summary>
        /// 创建报警数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateMachineTrainTags(MachineTrainInfluxDBModel eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                try
                {

                    if (string.IsNullOrEmpty(eventObj.COMM_ID))
                    {
                        eventObj.COMM_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.DEVICE_ID))
                    {
                        eventObj.DEVICE_ID = "0";
                    }
                    if (string.IsNullOrEmpty(eventObj.SERVER_ID))
                    {
                        eventObj.SERVER_ID = "0";
                    }

                    dict.Add("tag_sid", (string)eventObj.SERVER_ID);
                    dict.Add("tag_cid", (string)eventObj.COMM_ID);
                    dict.Add("tag_did", (string)eventObj.DEVICE_ID);
                    dict.Add("tag_taskid", (string)eventObj.TaskID.ToString());


                }
                catch (Exception ex)
                {
                  
                }
            }
            return dict;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateMachineTrainFields(MachineTrainInfluxDBModel eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                dict.Add("field_createtime", (string)eventObj.CreateTime);
                dict.Add("field_remark", (string)eventObj.Remark);
                dict.Add("field_result", (string)(eventObj.Result));
                dict.Add("field_server_name", (string)(eventObj.SERVER_ID));
                dict.Add("field_comm_name", (string)(eventObj.COMM_NAME));
                dict.Add("field_device_name", (string)(eventObj.DEVICE_NAME));
                dict.Add("field_tasktitle", (string)(eventObj.TaskTitle));

            }


            return dict;
        }
        #endregion
        #region 创建自动控制命令执行日志表
        /// <summary>
        /// 创建报警数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateBatchCommandTags(BatchCommandInfluxDBModel eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                try
                {


                    if (string.IsNullOrEmpty(eventObj.SERVER_ID))
                    {
                        eventObj.SERVER_ID = "0";
                    }
                    dict.Add("tag_sid", (string)eventObj.SERVER_ID);
                    dict.Add("tag_taskid", (string)eventObj.TaskID.ToString());


                }
                catch (Exception ex)
                {
                    
                }
            }
            return dict;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateBatchCommandFields(BatchCommandInfluxDBModel eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (eventObj != null)
            {
                dict.Add("field_createtime", (string)eventObj.CreateTime);
                dict.Add("field_remark", (string)eventObj.Remark);
                dict.Add("field_result", (string)(eventObj.Result));
                dict.Add("field_server_name", (string)(eventObj.SERVER_ID));
                dict.Add("field_tasktitle", (string)(eventObj.TaskTitle));
                dict.Add("field_TaskType", (string)(eventObj.TaskType));

            }


            return dict;
        }
        #endregion
        ///
        /// <summary>
        /// 创建实时数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateParaAlarmConfigTags(string serverid, string communicationid, IO_ALARM_CONFIG alarm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (alarm != null)
            {


                try
                {

                    dict.Add("tag_did", (string)alarm.IO_DEVICE_ID.ToString());
                    dict.Add("tag_cid", (string)communicationid.ToString());
                    dict.Add("tag_sid", (string)serverid.ToString());
                    dict.Add("tag_ioid", (string)alarm.IO_ID.ToString());
                }
                catch (Exception ex)
                {
                   
                }
            }
            return dict;
        }
        /// <summary>
        /// 创建实时数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateParaAlarmConfigFields(IO_ALARM_CONFIG alarm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (alarm != null)
            {
                dict.Add("field_update_date", (string)alarm.UPDATE_DATE.ToString());
                dict.Add("field_update_result", (string)alarm.UPDATE_RESULT.ToString());
                dict.Add("field_update_uid", (string)alarm.UPDATE_UID.ToString());
                dict.Add("field_io_label", (string)alarm.IO_LABEL.ToString());
                dict.Add("field_io_name", (string)alarm.IO_NAME.ToString());

                dict.Add("field_io_condition", (string)alarm.IO_CONDITION);
                dict.Add("field_io_enable_maxmax", (string)alarm.IO_ENABLE_MAXMAX.ToString());
                dict.Add("field_io_maxmax_type", (string)alarm.IO_MAXMAX_TYPE.ToString());
                dict.Add("field_io_maxmax_value", (string)alarm.IO_MAXMAX_VALUE.ToString());
                dict.Add("field_io_enable_max", (string)alarm.IO_ENABLE_MAX.ToString());
                dict.Add("field_io_max_type", (string)alarm.IO_MAX_TYPE.ToString());
                dict.Add("field_io_max_value", (string)alarm.IO_MAX_VALUE.ToString());
                dict.Add("field_io_enable_min", (string)alarm.IO_ENABLE_MIN.ToString());
                dict.Add("field_io_min_type", (string)alarm.IO_MIN_TYPE.ToString());
                dict.Add("field_io_min_value", (string)alarm.IO_MIN_VALUE.ToString());
                dict.Add("field_io_enable_minmin", (string)alarm.IO_ENABLE_MINMIN.ToString());
                dict.Add("field_io_minmin_type", (string)alarm.IO_MINMIN_TYPE.ToString());
                dict.Add("field_io_minmin_value", (string)alarm.IO_MINMIN_VALUE.ToString());
            }

            return dict;
        }


        ///
        /// <summary>
        /// 创建实时数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateCommandTags(string serverid, string communicationid, IO_COMMANDS command)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (command != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(command.COMMAND_ID))
                    {
                        command.COMMAND_ID = GUIDToNormalID.GuidToLongID().ToString();
                    }

                    dict.Add("tag_did", (string)command.IO_DEVICE_ID.ToString());
                    dict.Add("tag_cid", (string)communicationid.ToString());
                    dict.Add("tag_sid", (string)serverid.ToString());
                    dict.Add("tag_ioid", (string)command.IO_ID.ToString());

                }
                catch (Exception ex)
                {
                    
                }
            }
            return dict;
        }
        /// <summary>
        /// 创建实时数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateCommandFields(IO_COMMANDS command)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (command != null)
            {
                dict.Add("field_command_date", (string)command.COMMAND_DATE);
                dict.Add("field_command_id", (string)command.COMMAND_ID.ToString());
                dict.Add("field_command_result", (string)command.COMMAND_RESULT);
                dict.Add("field_command_user", (string)command.COMMAND_USER);
                dict.Add("field_command_value", (string)command.COMMAND_VALUE.ToString());
                dict.Add("field_comm_name", (string)command.IO_COMM_NAME.ToString());
                dict.Add("field_device_name", (string)command.IO_DEVICE_NAME.ToString());
                dict.Add("field_label", (string)command.IO_LABEL.ToString());
                dict.Add("field_name", (string)command.IO_LABEL.ToString());
            }
            return dict;
        }

        ////////////////数据库备份
        ///
        /// <summary>
        /// 创建实时数据表中的Tags
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateBackupTags(InfluxDBBackupLog backup)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                dict.Add("tag_backup_id", backup.BackUpID.ToString());

            }
            catch (Exception ex)
            {
               
            }

            return dict;
        }
        /// <summary>
        /// 创建实时数据表中的Fields
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateBackupFields(InfluxDBBackupLog backup)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("field_backup_date", (string)backup.BackUpDate);
            dict.Add("field_backup_file", (string)backup.BackUpFile);
            dict.Add("field_backup_result", (string)backup.BackUpResult);
            dict.Add("field_backup_path", (string)backup.BackUpPath);

            return dict;
        }

    }
}
