using Scada.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada.Model;
using Scada.IOStructure;
using System.ComponentModel;
using Modbus.Globel;

namespace Modbus.ModbusAnalysis
{
    //ScadaDeviceKernel
    public class Modbus_Serial_IP_DeviceDriver : Modbus_DeviceDriver
    {
    
        private string mGuid = "3F057B04-F411-45ED-98B3-75BE265B633C";
        /// <summary>
        /// 驱动唯一标识，采用系统GUID分配
        /// </summary>
        public override string GUID
        {
            get
            {
                return mGuid;
            }
            set
            {
                mGuid = value;
            }
        }
        private string mTitle = " Modbus 串口转 TCP/UDP 协议";
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
