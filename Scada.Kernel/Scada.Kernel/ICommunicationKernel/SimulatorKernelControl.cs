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
    public partial class SimulatorKernelControl : UserControl
    {
        public SimulatorKernelControl()
        {
            InitializeComponent();
            ParameterString = "";
        }

        /// <summary>
        /// 保存用户的界面参数
        /// </summary>
        public string ParameterString = "";

        /// <summary>
        /// 设置界面参数
        /// </summary>
        /// <param name="para"></param>
        public void SetUIParameter(IO_COMMUNICATION communication, List<IO_DEVICE> devices)
        {
            this.dataGridViewDevice.AutoGenerateColumns = false;
            this.dataGridViewPara.AutoGenerateColumns = false;

            if (communication != null)
            {
                ParameterString = communication.IO_COMM_SIMULATOR_PARASTRING;

                Communication = communication;
                Devices = devices;
                dataGridViewDevice.DataSource = null;
                dataGridViewDevice.DataSource = Devices;
            }

            InitParameter(communication, devices);

        }
        public virtual void InitParameter(IO_COMMUNICATION communication, List<IO_DEVICE> devices)
        {

        }
        //从界面返回用户设置的参数
        public virtual string GetUIParameter()
        {
            return ParameterString;
        }
        public virtual ScadaResult IsValidParameter()
        {
            return new ScadaResult(true, "参数有效");
        }
        private IO_COMMUNICATION Communication = null;
        private List<IO_DEVICE> Devices = null;
        private void dataGridViewDevice_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewDevice.SelectedRows.Count > 0 && Communication != null)
            {
                this.dataGridViewPara.DataSource = null;
                this.dataGridViewPara.Tag = Devices[dataGridViewDevice.SelectedRows[0].Index];
                this.dataGridViewPara.DataSource = Devices[dataGridViewDevice.SelectedRows[0].Index].IOParas;
            }
        }
        //编辑设备
        private void EditDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewDevice.SelectedRows.Count > 0 && Communication != null)
            {

                IO_DEVICE device = Devices[dataGridViewDevice.SelectedRows[0].Index];

                if (Communication.CommunicateDriver != null)
                {
                    ScadaCommunicateKernel driver = Communication.CommunicateDriver as ScadaCommunicateKernel;
                    SimulatorEditDeviceDialog deviceEdit = new SimulatorEditDeviceDialog(device, driver.CreateSimulatorDeviceEdit());
                    if (deviceEdit.ShowDialog(this) == DialogResult.OK)
                    {
                        if (Communication != null)
                        {

                            dataGridViewDevice.DataSource = null;
                            dataGridViewDevice.DataSource = Devices;

                        }
                    }
                }




            }
        }
        //编辑参数
        private void EditParatoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewPara.SelectedRows.Count > 0 && Communication != null)
            {

                IO_DEVICE device = this.dataGridViewPara.Tag as IO_DEVICE;
                IO_PARA para = device.IOParas[dataGridViewPara.SelectedRows[0].Index];

                if (para.IO_POINTTYPE == "开关量")
                {
                    device.IOParas[dataGridViewPara.SelectedRows[0].Index].IO_SIMULATOR_MAX = 1;
                    device.IOParas[dataGridViewPara.SelectedRows[0].Index].IO_SIMULATOR_MIN = 0;
                }
                else
                {
                    SimulatorEditDeviceParaDialog devicePara = new SimulatorEditDeviceParaDialog(device.IOParas[dataGridViewPara.SelectedRows[0].Index]);
                    if (devicePara.ShowDialog(this) == DialogResult.OK)
                    {
                        this.dataGridViewPara.DataSource = null;
                        this.dataGridViewPara.DataSource = device.IOParas;
                    }

                }


            }

        }

        private void 批量编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Communication != null)
            {
                ScadaCommunicateKernel driver = Communication.CommunicateDriver as ScadaCommunicateKernel;
                SimulatorDeviceEditControl editeCtrl = driver.CreateSimulatorDeviceEdit();
                SimulatorEditDeviceDialog deviceEdit = new SimulatorEditDeviceDialog(null, editeCtrl);
                deviceEdit.BatchDevice(Devices);
                if (Communication != null)
                {

                    dataGridViewDevice.DataSource = null;
                    dataGridViewDevice.DataSource = Devices;

                }

            }
        }
    }
}
