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

namespace Scada.Kernel
{
    public partial class SimulatorEditDeviceDialog : Form
    {
        public SimulatorEditDeviceDialog(IO_DEVICE device,SimulatorDeviceEditControl ctrl)
        {
            InitializeComponent();
            Device = device;
            editCtrl = ctrl;
            this.Load += SimulatorEditDeviceDialog_Load;
        }

        private void SimulatorEditDeviceDialog_Load(object sender, EventArgs e)
        {
            if(editCtrl!=null&& Device!=null)
            {
                editCtrl.SetUIParameter(Device);
                editCtrl.Dock = DockStyle.Fill;
                panel.Controls.Add(editCtrl);
            }
        }

        private IO_DEVICE Device = null;
        private SimulatorDeviceEditControl editCtrl = null;

        private void btStart_Click(object sender, EventArgs e)
        {
            ScadaResult result = editCtrl.IsValidParameter();
            if (!result.Result)
            {
                MessageBox.Show(this.FindForm(), result.Message);

            }
            else
            {
                if (Device != null && editCtrl != null)
                {
                    Device.IO_DEVICE_SIMULATOR_PARASTRING = editCtrl.GetUIParameter();
                }
                this.DialogResult = DialogResult.OK;
            }

           
         
           
        }

        public void BatchDevice(List<IO_DEVICE> devices)
        {
            for(int i=0;i< devices.Count;i++)
            {
                editCtrl.SetUIParameter(devices[i]);
                devices[i].IO_DEVICE_SIMULATOR_PARASTRING = editCtrl.GetUIParameter();
            }
         
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
