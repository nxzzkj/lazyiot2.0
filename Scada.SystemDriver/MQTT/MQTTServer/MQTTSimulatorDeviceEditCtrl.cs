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

namespace MQTTnet
{
    public partial class MQTTSimulatorDeviceEditCtrl : SimulatorDeviceEditControl
    {
        public MQTTSimulatorDeviceEditCtrl()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tbClientID.Text = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }
        public override void InitUIParameter(IO_DEVICE device)
        {
            if(device!=null)
            {
                this.tbDeviceID.Text = device.IO_DEVICE_ADDRESS;
                this.tbName.Text = device.IO_DEVICE_NAME;
                if (!string.IsNullOrEmpty(device.IO_DEVICE_SIMULATOR_PARASTRING))
                {
                    ParaPack paraPack = new ParaPack(device.IO_DEVICE_SIMULATOR_PARASTRING);
                    paraPack.SetCtrlValue(tbClientID, paraPack.GetValue("MQTT客户端ID"));
                    paraPack.SetCtrlValue(tb_subTopic, paraPack.GetValue("数据订阅主题"));
                    paraPack.SetCtrlValue(tb_cmdSubTopic, paraPack.GetValue("下置命令主题"));
                    paraPack.SetCtrlValue(tbTimes, paraPack.GetValue("循环周期主题"));
                }
                else
                {
                    ParaPack paraPack = new ParaPack(device.IO_DEVICE_PARASTRING);
                    paraPack.SetCtrlValue(tbClientID, paraPack.GetValue("MQTT连接ID号"));
                    paraPack.SetCtrlValue(tb_subTopic, paraPack.GetValue("数据订阅主题"));
                    paraPack.SetCtrlValue(tb_cmdSubTopic, paraPack.GetValue("下置命令主题"));
                    paraPack.SetCtrlValue(tbTimes, paraPack.GetValue("循环周期主题"));

                }
            }
           
        }
        public override string GetUIParameter()
        {
            ParaPack para = new ParaPack();
            para.AddItem("MQTT客户端ID", tbClientID);
            para.AddItem("数据订阅主题", tb_subTopic);
            para.AddItem("下置命令主题", tb_cmdSubTopic);
            para.AddItem("循环周期主题", tbTimes);
            return para.ToString();
        }
        public override ScadaResult IsValidParameter()
        {
            if(string.IsNullOrEmpty(tbClientID.Text))
            {
                return new ScadaResult(false,"请输入MQTT客户端ID");
            }
            if (string.IsNullOrEmpty(tb_subTopic.Text))
            {
                return new ScadaResult(false, "请输入数据订阅主题");
            }
            if (string.IsNullOrEmpty(tb_cmdSubTopic.Text))
            {
                return new ScadaResult(false, "请输入下置命令主题");
            }
            if (string.IsNullOrEmpty(tbTimes.Text))
            {
                return new ScadaResult(false, "请输入循环周期主题");
            }
            return base.IsValidParameter();
        }
    }
}
