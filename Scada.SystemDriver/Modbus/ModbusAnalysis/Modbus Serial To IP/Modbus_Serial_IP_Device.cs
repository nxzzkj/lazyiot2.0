using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Kernel;
using System.IO.Ports;
using Modbus.Globel;
using Scada.Model;

namespace Modbus.ModbusService
{
    public partial class Modbus_RTU_Device : IODeviceKernelControl
    {
        public Modbus_RTU_Device()
        {
            InitializeComponent();
            
        }


        public override void SetUIParameter(IO_SERVER server, IO_DEVICE device)
        {
            base.SetUIParameter(server, device);
            ParaPack paraPack = new ParaPack(device.IO_DEVICE_PARASTRING);
            paraPack.SetCtrlValue(cbRetries, paraPack.GetValue("重试"));
            paraPack.SetCtrlValue(ndRetiresNum, paraPack.GetValue("重试次数"));
            paraPack.SetCtrlValue(nbRetiresInternal, paraPack.GetValue("重试间隔"));
            paraPack.SetCtrlValue(cbModels, paraPack.GetValue("通讯协议"));

        }
        public override string GetUIParameter()
        {
            ParaPack paraPack = new ParaPack();
            paraPack.AddItem("重试", cbRetries.Checked ? "1" : "0");
            paraPack.AddItem("重试次数", ndRetiresNum.Text);
            paraPack.AddItem("重试间隔", nbRetiresInternal.Text.ToString());
     
            paraPack.AddItem("通讯协议", cbModels.SelectedItem.ToString());
            return paraPack.ToString();
        }
    }
}
