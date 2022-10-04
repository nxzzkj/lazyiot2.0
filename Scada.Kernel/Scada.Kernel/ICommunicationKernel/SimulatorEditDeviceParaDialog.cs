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
    public partial class SimulatorEditDeviceParaDialog : Form
    {
        public SimulatorEditDeviceParaDialog(IO_PARA para)
        {
            InitializeComponent();
            this.Load += DevicePara_Load;
            Pata = para;
        }

        private void DevicePara_Load(object sender, EventArgs e)
        {
            if (Pata != null)
            {
                tbName.Text = Pata.IO_NAME;
                nbMax.Value = Pata.IO_SIMULATOR_MAX;
                nbMin.Value = Pata.IO_SIMULATOR_MIN;
            }

        }

        private IO_PARA Pata = null;
        private void btStart_Click(object sender, EventArgs e)
        {
            if (Pata != null)
            {

                Pata.IO_SIMULATOR_MAX = Convert.ToInt32(nbMax.Value);
                Pata.IO_SIMULATOR_MIN = Convert.ToInt32(nbMin.Value);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
