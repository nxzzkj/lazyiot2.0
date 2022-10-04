﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

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

    /// <summary>
    /// IO_DEVICE:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class IO_DEVICE : ISerializable, IDisposable
    {
        public IO_DEVICE()
        {
            mIOParas = new List<IO_PARA>();
            Last_Online_Time = DateTime.Now;

        }
        public string GetCommandString()
        {
            try
            {
                string str = "TABLE:IO_DEVICE#DEVICE_DRIVER_ID:" + DEVICE_DRIVER_ID;
                str += "#IO_COMM_ID:" + IO_COMM_ID;
                str += "#IO_DEVICE_ADDRESS:" + IO_DEVICE_ADDRESS;
                str += "#IO_DEVICE_ID:" + IO_DEVICE_ID;
                str += "#IO_DEVICE_LABLE:" + IO_DEVICE_LABLE;
                str += "#IO_DEVICE_NAME:" + IO_DEVICE_NAME;
                str += "#IO_DEVICE_OVERTIME:" + IO_DEVICE_OVERTIME;
                str += "#IO_DEVICE_PARASTRING:" + IO_DEVICE_PARASTRING.Replace("#", "//").Replace(":", "\\");
                str += "#IO_DEVICE_REMARK:" + IO_DEVICE_REMARK.Replace("#", "//").Replace(":", "\\"); ;
                str += "#IO_DEVICE_STATUS:" + IO_DEVICE_STATUS;
                str += "#IO_DEVICE_UPDATECYCLE:" + IO_DEVICE_UPDATECYCLE;
                str += "#IO_SERVER_ID:" + IO_SERVER_ID;
                str += "#IO_DEVICE_SIMULATOR_PARASTRING:" + IO_DEVICE_SIMULATOR_PARASTRING.Replace("#", "//").Replace(":", "\\");
                return str;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取设备及其下面的所有iO参数点的信息
        /// </summary>
        /// <returns></returns>
        public string GetCommandAllString()
        {
            try
            {
                string str = "TABLE:IO_DEVICE#DEVICE_DRIVER_ID:" + DEVICE_DRIVER_ID;
                str += "#IO_COMM_ID:" + IO_COMM_ID;
                str += "#IO_DEVICE_ADDRESS:" + IO_DEVICE_ADDRESS;
                str += "#IO_DEVICE_ID:" + IO_DEVICE_ID;
                str += "#IO_DEVICE_LABLE:" + IO_DEVICE_LABLE;
                str += "#IO_DEVICE_NAME:" + IO_DEVICE_NAME;
                str += "#IO_DEVICE_OVERTIME:" + IO_DEVICE_OVERTIME;
                str += "#IO_DEVICE_PARASTRING:" + IO_DEVICE_PARASTRING.Replace("#", "//").Replace(":", "\\");
                str += "#IO_DEVICE_REMARK:" + IO_DEVICE_REMARK.Replace("#", "//").Replace(":", "\\"); ;
                str += "#IO_DEVICE_STATUS:" + IO_DEVICE_STATUS;
                str += "#IO_DEVICE_UPDATECYCLE:" + IO_DEVICE_UPDATECYCLE;
                str += "#IO_SERVER_ID:" + IO_SERVER_ID;
                str += "#IO_DEVICE_SIMULATOR_PARASTRING:" + IO_DEVICE_SIMULATOR_PARASTRING.Replace("#", "//").Replace(":", "\\");
                string parastr = "";
                for (int i = 0; i < this.IOParas.Count; i++)
                {
                    parastr += "**" + this.IOParas[i].GetCommandString();

                }
                if (parastr != "")
                    parastr = parastr.Remove(0, 2);
                str += "#PARAS:" + parastr;

                return str;
            }
            catch
            {
                return "";
            }
        }
        public IO_DEVICE Copy()
        {
            IO_DEVICE copyDevice = new IO_DEVICE()
            {
                DEVICE_DRIVER_ID = this.DEVICE_DRIVER_ID,
                IO_COMM_ID = this.IO_COMM_ID,
                IO_DEVICE_ADDRESS = this.IO_DEVICE_ADDRESS,
                IO_DEVICE_ID = this.IO_DEVICE_ID,
                IO_DEVICE_LABLE = this.IO_DEVICE_LABLE,
                IO_DEVICE_NAME = this.IO_DEVICE_NAME,
                IO_DEVICE_OVERTIME = this.IO_DEVICE_OVERTIME,
                IO_DEVICE_PARASTRING = this.IO_DEVICE_PARASTRING,
                IO_DEVICE_REMARK = this.IO_DEVICE_REMARK,
                IO_DEVICE_STATUS = this.IO_DEVICE_STATUS,
                GetedValueDate = this.GetedValueDate,
                IO_DEVICE_UPDATECYCLE = this.IO_DEVICE_UPDATECYCLE,
                IO_DEVICE_SIMULATOR_PARASTRING = this.IO_DEVICE_SIMULATOR_PARASTRING,
                IO_SERVER_ID = this.IO_SERVER_ID,
                Tag = this.Tag,
                ReceiveBytes = this.ReceiveBytes,
                DriverInfo = this.DriverInfo == null ? null : this.DriverInfo.Copy(),
                Last_Online_Time = this.Last_Online_Time,
                DataPushUrl=this.DataPushUrl

            };

            copyDevice.DeviceDrive = this.DeviceDrive;
            copyDevice.IOParas = new List<IO_PARA>();
            for (int i = 0; i < this.IOParas.Count; i++)
            {
                copyDevice.IOParas.Add(this.IOParas[i].Copy());
            }
            return copyDevice;
        }
        [NonSerialized]
        public DateTime? GetedValueDate = null;
        #region Model
        private string _io_device_id = "";
        private string _io_comm_id = "";
        private string _IO_SERVER_id = "";
        private string _io_device_name = "";
        private string _io_device_lable = "";
        private string _io_device_remark = "";
        private int _io_device_updatecycle = 120;
        private int _io_device_status = 1;
        private int _io_device_overtime = 120;
        private string _io_device_address = "";
        private string _io_device_parastring = "";
        private string _device_driver_id = "";

        private List<IO_PARA> mIOParas = new List<IO_PARA>();
        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IO_DEVICE(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性

            this._io_device_id = (string)info.GetValue("_io_device_id", typeof(string));
            this._IO_SERVER_id = (string)info.GetValue("_IO_SERVER_id", typeof(string));
            this._io_comm_id = (string)info.GetValue("_io_comm_id", typeof(string));
            this._io_device_name = (string)info.GetValue("_io_device_name", typeof(string));
            this._io_device_lable = (string)info.GetValue("_io_device_lable", typeof(string));
            this._io_device_updatecycle = (int)info.GetValue("_io_device_updatecycle", typeof(int));
            this._io_device_status = (int)info.GetValue("_io_device_status", typeof(int));
            this._io_device_overtime = (int)info.GetValue("_io_device_overtime", typeof(int));
            this._io_device_remark = (string)info.GetValue("_io_device_remark", typeof(string));
            this._io_device_address = (string)info.GetValue("_io_device_address", typeof(string));
            this._io_device_parastring = (string)info.GetValue("_io_device_parastring", typeof(string));
            this._device_driver_id = (string)info.GetValue("_device_driver_id", typeof(string));
            this._IO_DEVICE_SIMULATOR_PARASTRING = (string)info.GetValue("_IO_DEVICE_SIMULATOR_PARASTRING", typeof(string));
            try
            {
                this.DataPushUrl = (string)info.GetValue("DataPushUrl", typeof(string));
            }
            catch
            {
                
            }
            this.mIOParas = (List<IO_PARA>)info.GetValue("mIOParas", typeof(List<IO_PARA>));


            
            #endregion





        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_io_device_id", this._io_device_id);
            info.AddValue("_IO_SERVER_id", this._IO_SERVER_id);
            info.AddValue("_io_comm_id", this._io_comm_id);
            info.AddValue("_io_device_name", this._io_device_name);
            info.AddValue("_io_device_lable", this._io_device_lable);
            info.AddValue("_io_device_updatecycle", this._io_device_updatecycle);
            info.AddValue("_io_device_status", this._io_device_status);
            info.AddValue("_io_device_overtime", this._io_device_overtime);
            info.AddValue("_io_device_address", this._io_device_address);
            info.AddValue("_io_device_parastring", this._io_device_parastring);
            info.AddValue("_device_driver_id", this._device_driver_id);
            info.AddValue("_io_device_remark", this._io_device_remark);
            info.AddValue("_IO_DEVICE_SIMULATOR_PARASTRING", this._IO_DEVICE_SIMULATOR_PARASTRING);
            info.AddValue("DataPushUrl", this.DataPushUrl);
            info.AddValue("mIOParas", this.mIOParas);
        }

        #endregion
        public List<IO_PARA> IOParas
        {
            set { mIOParas = value; }
            get { return mIOParas; }
        }

        [NonSerialized]
        public SCADA_DEVICE_DRIVER DriverInfo = null;
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_ID
        {
            set { _io_device_id = value; }
            get { return _io_device_id; }
        }
        private string _DataPushUrl = "";
        public string DataPushUrl
        { set { _DataPushUrl = value; } get { return _DataPushUrl; } }
        /// <summary>
        /// 
        /// </summary>
        public string IO_COMM_ID
        {
            set { _io_comm_id = value; }
            get { return _io_comm_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_SERVER_ID
        {
            set { _IO_SERVER_id = value; }
            get { return _IO_SERVER_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_NAME
        {
            set { _io_device_name = value; }
            get { return _io_device_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_LABLE
        {
            set { _io_device_lable = value; }
            get { return _io_device_lable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_REMARK
        {
            set { _io_device_remark = value; }
            get { return _io_device_remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IO_DEVICE_UPDATECYCLE
        {
            set { _io_device_updatecycle = value; }
            get { return _io_device_updatecycle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IO_DEVICE_STATUS
        {
            set { _io_device_status = value; }
            get { return _io_device_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IO_DEVICE_OVERTIME
        {
            set { _io_device_overtime = value; }
            get { return _io_device_overtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_ADDRESS
        {
            set { _io_device_address = value; }
            get { return _io_device_address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_PARASTRING
        {
            set { _io_device_parastring = value; }
            get { return _io_device_parastring; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEVICE_DRIVER_ID
        {
            set { _device_driver_id = value; }
            get { return _device_driver_id; }
        }
        [NonSerialized]
        /// <summary>
        /// 存储一些临时定义的数据，非数据库字段
        /// </summary>
        private string mTag = "";

        public string Tag
        {
            set { mTag = value; }
            get { return mTag; }
        }
        #endregion Model
        #region 模拟器模拟配置参数

        private string _IO_DEVICE_SIMULATOR_PARASTRING = "";
        public string IO_DEVICE_SIMULATOR_PARASTRING { set { _IO_DEVICE_SIMULATOR_PARASTRING = value; } get { return _IO_DEVICE_SIMULATOR_PARASTRING; } }

        #endregion
        #region 当前设备的驱动

        [NonSerialized]
        public object DeviceDrive = null;
        /// <summary>
        /// 存储的从设备传递上来的原始数据字节
        /// </summary>
        [NonSerialized]
        public byte[] ReceiveBytes = new byte[0];
        [NonSerialized]
        public bool SimulatorCheckedStatus = false;
        /// <summary>
        /// 判断当前的数据采集是否完成,根据系统的循环时间来判断，如果这些数据超过更新周期，则判断此次数据已经完成，如果没有超过更新周期则判断这些数据需要继续等待
        /// </summary>
        public bool IsCollectSuccess
        {
            get
            {
                DateTime mindt = DateTime.MaxValue;

                bool res = true;
                for (int i = 0; i < this.IOParas.Count; i++)
                {
                    if (this.IOParas[i].IORealData != null)
                    {
                        DateTime dataDt = DateTime.MinValue;
                        if (DateTime.TryParse(this.IOParas[i].RealDate, out dataDt))
                        {
                            if (dataDt <= mindt)
                            {
                                mindt = dataDt;
                            }

                        }




                    }
                }
                if (mindt == DateTime.MaxValue)
                {
                    res = false;
                }

                //如果设备数据接收时间超过10秒，则默认本次数据接收已经完成
                int second = (DateTime.Now - mindt).Seconds;
                if (second >= 5)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                return res;

            }
        }
        public string TableName
        {
            get { return this.IO_SERVER_ID + "_" + IO_DEVICE_ID; }
        }

        #endregion
        public override string ToString()
        {
            return this.IO_DEVICE_NAME + "[" + this.IO_DEVICE_LABLE + "]";
        }

        public DateTime Last_Online_Time { set; get; }

        public void Dispose()
        {

            this.DeviceDrive = null;
            this.DriverInfo = null;

            if (IOParas != null)
            {
                for (int i = 0; i < IOParas.Count; i++)
                {
                    IOParas[i].Dispose();
                    IOParas[i] = null;
                }
                this.IOParas.Clear();
            }

            this.IOParas = null;
            this.ReceiveBytes = null;

        }
    }
}

