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

namespace Modbus.ModbusAnalysis
{
    public partial class Modbus_TCP_Device : IODeviceKernelControl
    {
        public Modbus_TCP_Device()
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
     
        }
        public override string GetUIParameter()
        {
            ParaPack paraPack = new ParaPack();
            paraPack.AddItem("重试", cbRetries.Checked ? "1" : "0");
            paraPack.AddItem("重试次数", ndRetiresNum.Text);
            paraPack.AddItem("重试间隔", nbRetiresInternal.Text.ToString());
  
            return paraPack.ToString();
        }

    }
}
