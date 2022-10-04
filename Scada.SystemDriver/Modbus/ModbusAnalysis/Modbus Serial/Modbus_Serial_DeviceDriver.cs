using Scada.Kernel;
using System;
using System.Collections.Generic;
using Scada.Model;
using Scada.IOStructure;
using Modbus.Globel;

namespace Modbus.ModbusAnalysis
{

    public class Modbus_Serial_DeviceDriver : Modbus_DeviceDriver
    {
        private const string mGuid = "945F7BEA-ED7E-4668-B145-C809FC5DA299";
        /// <summary>
        /// 驱动唯一标识，采用系统GUID分配
        /// </summary>
        public override string GUID
        {
            get
            {
                return mGuid;
            }


        }
        private string mTitle = " Modbus 串行协议";
        public override string Title
        {
            get
            {
                return mTitle;
            }

            set
            {
                mTitle = value;
            }
        }
     
    }
}
