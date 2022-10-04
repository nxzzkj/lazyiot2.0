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
    public class Modbus_TCP_DeviceDriver : Modbus_DeviceDriver
    {
    
        private string mGuid = "7D22423C-BC96-4771-AE32-7C00FD35B70E";
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
        private string mTitle = " Modbus TCP/UDP 协议";
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
