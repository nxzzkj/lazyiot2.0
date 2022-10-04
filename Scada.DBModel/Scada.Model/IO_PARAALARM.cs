﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Model
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
    public class IO_PARAALARM: IDisposable
    {
        private string _io_alarm_id = "";
        private string _io_id = "";
        private string _IO_SERVER_id = "";
        private string _io_comm_id = "";
        private string _io_device_id = "";
        private string _io_name = "";
        private string _io_label = "";
        private string _device_name = "";
        public string DEVICE_NAME
        {
            set { _device_name = value; }
            get { return _device_name; }
        }
        /// <summary>
        /// 报警ID
        /// </summary>
        public string IO_ALARM_ID
        {
            set
            {
                _io_alarm_id = value;


            }
            get { return _io_alarm_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_ID
        {
            set
            {
                _io_id = value;


            }
            get { return _io_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_SERVER_ID
        {
            set
            {
                _IO_SERVER_id = value;

            }
            get { return _IO_SERVER_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_COMM_ID
        {
            set
            {
                _io_comm_id = value;

            }
            get { return _io_comm_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_ID
        {
            set
            {
                _io_device_id = value;


            }
            get { return _io_device_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_NAME
        {
            set { _io_name = value; }
            get { return _io_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_LABEL
        {
            set { _io_label = value; }
            get { return _io_label; }
        }

        private string _io_alarm_date = "";
        public string IO_ALARM_DATE
        {
            set { _io_alarm_date = value; }
            get
            {
                return _io_alarm_date;
            }
        }
        private string _io_alarm_value = "";
        public string IO_ALARM_VALUE
        {
            set { _io_alarm_value = value; }
            get
            {
                return _io_alarm_value;
            }
        }
        private string _io_alarm_type = "";
        /// <summary>
        /// 报警类型
        /// </summary>
        public string IO_ALARM_TYPE
        {
            set { _io_alarm_type = value; }
            get
            {
                return _io_alarm_type;
            }
        }
        private string _io_alarm_level = "";
        /// <summary>
        /// 报警等级
        /// </summary>
        public string IO_ALARM_LEVEL
        {
            set { _io_alarm_level = value; }
            get
            {
                return _io_alarm_level;
            }
        }
        private string io_alarm_disposalidea = "";
        /// <summary>
        /// 处理意见
        /// </summary>
        public string IO_ALARM_DISPOSALIDEA
        {
            set { io_alarm_disposalidea = value; }
            get
            {
                return io_alarm_disposalidea;
            }
        }
        private string io_alarm_disposaluser = "";
        /// <summary>
        /// 处理人
        /// </summary>
        public string IO_ALARM_DISPOSALUSER
        {
            set { io_alarm_disposaluser = value; }
            get
            {
                return io_alarm_disposaluser;
            }
        }
        public string GetCommandString()
        {

            string str = "IO_ALARM_ID:" + IO_ALARM_ID + "#IO_ID:" + IO_ID + "#IO_SERVER_ID:" + IO_SERVER_ID + "#IO_COMM_ID:" + IO_COMM_ID + "#IO_DEVICE_ID:" + IO_DEVICE_ID + "#IO_NAME:" + IO_NAME + "#IO_LABEL:" + IO_LABEL + "#IO_ALARM_DATE:" + IO_ALARM_DATE.Replace("#", "//").Replace(":", "\\") + "#IO_ALARM_VALUE:" + IO_ALARM_VALUE + "#IO_ALARM_TYPE:" + IO_ALARM_TYPE + "#IO_ALARM_LEVEL:" + IO_ALARM_LEVEL + "#IO_ALARM_DISPOSALIDEA:" + IO_ALARM_DISPOSALIDEA + "#IO_ALARM_DISPOSALUSER:" + IO_ALARM_DISPOSALUSER+ "#DEVICE_NAME:"+ DEVICE_NAME;
            return str;
        }

        public void Dispose()
        {
           
        }
    }
}
