using Scada.Kernel;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTTnet
{
    public partial class MQTTSimulatorCtrl : SimulatorKernelControl
    {
        public MQTTSimulatorCtrl()
        {
            InitializeComponent();
        }
        private IO_COMMUNICATION Communication = null;

        public override void InitParameter( IO_COMMUNICATION communication,List<IO_DEVICE> devices)
        {
            if (!string.IsNullOrEmpty(communication.IO_COMM_SIMULATOR_PARASTRING))
            {
                ParaPack paraPack = new ParaPack(communication.IO_COMM_SIMULATOR_PARASTRING);
                paraPack.SetCtrlValue(tbServerIP, paraPack.GetValue("MQTT服务器"));
                paraPack.SetCtrlValue(tbPeried, paraPack.GetValue("保持有效期"));

                paraPack.SetCtrlValue(tbPort, paraPack.GetValue("端口号"));
                paraPack.SetCtrlValue(tbUser, paraPack.GetValue("用户名"));
                paraPack.SetCtrlValue(tbPassword, paraPack.GetValue("密码"));
                paraPack.SetCtrlValue(cbAuto, paraPack.GetValue("被动上传"));
             
               
                
            }
            else
            {
                ParaPack paraPack = new ParaPack(communication.IO_COMM_PARASTRING);
                paraPack.SetCtrlValue(tbServerIP, paraPack.GetValue("服务器IP"));
                paraPack.SetCtrlValue(tbPeried, paraPack.GetValue("心跳时间"));
                paraPack.SetCtrlValue(tbPort, paraPack.GetValue("端口号"));
                paraPack.SetCtrlValue(tbUser, paraPack.GetValue("用户名"));
                paraPack.SetCtrlValue(tbPassword, paraPack.GetValue("密码"));

 
                
            }
            if (communication != null)
            {

                Communication = communication;
            }

        }
        private string GetParament()
        {

            ParaPack para = new ParaPack();
            para.AddItem("MQTT服务器", tbServerIP);
            para.AddItem("保持有效期", tbPeried);
            para.AddItem("端口号", tbPort);
            para.AddItem("用户名", tbUser);
            para.AddItem("密码", tbPassword);
            para.AddItem("被动上传", cbAuto);
      
          
            return para.ToString();


        }
        public override string GetUIParameter()
        {
            return GetParament();
        }
        public override ScadaResult IsValidParameter()
        {
            IPAddress ip;
            if (!IPAddress.TryParse(tbServerIP.Text, out ip))
            {
                return new ScadaResult(false, "MQTT服务器IP地址不正确");
            }
            int port = 1883;
            if (int.TryParse(tbPort.Text,out port))
            {
                return new ScadaResult(false, "请输入正确的端口号");
            }
         
            return new ScadaResult();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if(Communication != null)
            {
                Communication.IO_COMM_SIMULATOR_PARASTRING = GetParament();
                MessageBox.Show(this.FindForm(), "修改通道模拟参数完成");
            }
        }
    }
}
