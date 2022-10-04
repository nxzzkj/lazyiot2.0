using Scada.Kernel;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus.ModbusService
{
    public partial class Modbus_Serial_Simulator_Device : SimulatorDeviceEditControl
    {
        public Modbus_Serial_Simulator_Device()
        {
            InitializeComponent();
        }
        IO_DEVICE mdevice = null;
        private void cbEnable_CheckedChanged(object sender, EventArgs e)
        {
           
        }
      
        public override void InitUIParameter(IO_DEVICE device)
        {
            if (device != null)
            {
                mdevice = device;
                if(!string.IsNullOrEmpty(mdevice.IO_DEVICE_SIMULATOR_PARASTRING))
                {

                    ParaPack paraPack = new ParaPack(device.IO_DEVICE_SIMULATOR_PARASTRING);
                    paraPack.SetCtrlValue(nudMax, paraPack.GetValue("模拟最大值"));
                    paraPack.SetCtrlValue(nudMin, paraPack.GetValue("模拟最小值"));
             
                }
            }

        }
        public override string GetUIParameter()
        {

            ParaPack para = new ParaPack();
            para.AddItem("模拟最大值", nudMax);
            para.AddItem("模拟最小值", nudMin);
      
            return para.ToString();
        }
     
    }
}
