﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
    /// <summary>
    /// 报警数据模型
    /// </summary>
 
 
    public class ScadaGreneralAlarmModel : ScadaAlarmModel
    {
        public ScadaGreneralAlarmModel()
        {
          
   
            StartDate = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
            EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public IEnumerable<DeviceGroupModel>  AllDeviceList { set; get; }


        public string SERVER_ID { set; get; }
        public string COMM_ID { set; get; }
        public string DEVICE_ID { set; get; }

        public string StartDate
        {
            set;
            get;
        }

        public string EndDate
        {
            set;
            get;
        }
    }
    public class ScadaAlarmModel
    {
        public string time
        {
            set; get;
        }
        public string DEVICE_NAME
        { set; get; }
        public string IO_ID
        { set; get; }
        public string IO_DEVICE_ID
        { set; get; }
        public string IO_SERVER_ID
        { set; get; }
        public string IO_COMMUNICATE_ID
        { set; get; }
        /// <summary>
        /// 报警类型
        /// </summary>
        public string IO_ALARM_TYPE
        { set; get; }
        /// <summary>
        /// 报警等级
        /// </summary>
        public string IO_ALARM_LEVEL
        { set; get; }
        /// <summary>
        /// 报警时间
        /// </summary>
        public string IO_ALARM_DATE
        { set; get; }
        public string IO_ALARM_DISPOSALIDEA
        { set; get; }
        public string IO_ALARM_DISPOSALUSER
        { set; get; }
        public string IO_ALARM_VALUE
        { set; get; }
        public string IO_LABEL
        { set; get; }
        public string IO_NAME
        { set; get; }
      

    }
}
