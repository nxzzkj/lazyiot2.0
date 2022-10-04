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
    public partial class Modbus_Serial_IP_Simulator : SimulatorKernelControl
    {
        public Modbus_Serial_IP_Simulator()
        {
            InitializeComponent();
        }
        private IO_COMMUNICATION Communication = null;
        public override void InitParameter(IO_COMMUNICATION communication, List<IO_DEVICE> devices)
        {
            if (!string.IsNullOrEmpty(communication.IO_COMM_SIMULATOR_PARASTRING))
            {
                ParaPack paraPack = new ParaPack(communication.IO_COMM_SIMULATOR_PARASTRING);
                paraPack.SetCtrlValue(nudUpdateCycle, paraPack.GetValue("数据更新周期"));
                paraPack.SetCtrlValue(nudRegisterNum, paraPack.GetValue("寄存器数量"));
                paraPack.SetCtrlValue(nudStart, paraPack.GetValue("起始地址"));

                





            }
            else
            {
                ParaPack paraPack = new ParaPack(communication.IO_COMM_PARASTRING);
                paraPack.SetCtrlValue(nudUpdateCycle, paraPack.GetValue("数据更新周期"));
         
              



            }
            if (communication != null)
            {

                Communication = communication;
            }

        }
        private string GetParament()
        {

            ParaPack para = new ParaPack();
            para.AddItem("数据更新周期", nudUpdateCycle);
            para.AddItem("寄存器数量", nudRegisterNum);
            para.AddItem("起始地址", nudStart);



            return para.ToString();


        }
        public override string GetUIParameter()
        {
            return GetParament();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Communication != null)
            {
                Communication.IO_COMM_SIMULATOR_PARASTRING = GetParament();
                MessageBox.Show(this.FindForm(), "修改通道模拟参数完成");
            }
         
        }
    }
}
