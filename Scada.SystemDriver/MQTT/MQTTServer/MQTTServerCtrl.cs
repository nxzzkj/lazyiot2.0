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
using System.Net;

namespace MQTTnet
{
    public partial class MQTTServerCtrl : CommunicationKernelControl
    {
        public MQTTServerCtrl()
        {
            InitializeComponent();
          
        }
        public override void SetUIParameter(string para)
        {
            cbReceiveMethod.SelectedIndex = 0;
            cbMessage.SelectedIndex = 0;
            cbWill.SelectedIndex = 0;
          
            cbSSLVersion.SelectedIndex = 0;
            comboMqttPendingMessagesOverflowStrategy.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(para))
            {
                ParaPack paraPack = new ParaPack(para);
                paraPack.SetCtrlValue(txtIp, paraPack.GetValue("服务器IP"));
                paraPack.SetCtrlValue(nudPort, paraPack.GetValue("端口号"));
                paraPack.SetCtrlValue(tbUser, paraPack.GetValue("用户名"));
                paraPack.SetCtrlValue(tbPwd, paraPack.GetValue("密码"));
                paraPack.SetCtrlValue(cbEnableUser, paraPack.GetValue("开启匿名验证"));
            
                paraPack.SetCtrlValue(cbMessage, paraPack.GetValue("消息质量"));
                paraPack.SetCtrlValue(cbWill, paraPack.GetValue("遗愿标志"));
 
          
                paraPack.SetCtrlValue(cbReceiveMethod, paraPack.GetValue("接收方式"));
                paraPack.SetCtrlValue(cbSSLVersion, paraPack.GetValue("SSL版本"));
                paraPack.SetCtrlValue((paraPack.GetValue("通讯模式") == "TCP" ? rbTCP:rbTSL), 1) ;
                paraPack.SetCtrlValue(tbSSLCertificate, paraPack.GetValue("SSL证书"));
                paraPack.SetCtrlValue(nudConnectTimeout, paraPack.GetValue("连接超时"));
                paraPack.SetCtrlValue(nudMaxPendingMessagesPerClient, paraPack.GetValue("最大挂起消息数"));
                paraPack.SetCtrlValue(cbEnableKeepSessions, paraPack.GetValue("启用持久会话"));
                paraPack.SetCtrlValue(comboMqttPendingMessagesOverflowStrategy, paraPack.GetValue("溢出策略") );
            }
            else
            {
                string AddressIP = "127.0.0.1";
                foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                    {
                        AddressIP = _IPAddress.ToString();
                    }
                }
             
                txtIp.Text = AddressIP;
            }
           
            
        }
        private string GetParament()
        {

            ParaPack para = new ParaPack();
            para.AddItem("服务器IP", txtIp);
            para.AddItem("端口号", nudPort);
            para.AddItem("用户名", tbUser);
            para.AddItem("密码", tbPwd);
            para.AddItem("开启匿名验证", cbEnableUser);
          
            para.AddItem("消息质量", cbMessage);
            para.AddItem("遗愿标志", cbWill);
         
          
            para.AddItem("接收方式", cbReceiveMethod);
            para.AddItem("SSL版本", cbSSLVersion);
            para.AddItem("通讯模式", rbTCP.Checked?"TCP":"TSL");
            para.AddItem("SSL证书", tbSSLCertificate);
            para.AddItem("连接超时", nudConnectTimeout);
            para.AddItem("最大挂起消息数", nudMaxPendingMessagesPerClient);
            para.AddItem("启用持久会话", cbEnableKeepSessions);
            para.AddItem("溢出策略", comboMqttPendingMessagesOverflowStrategy);
            
            return para.ToString();


        }
        public override string GetUIParameter()
        {
            return GetParament();
        }
        public override ScadaResult IsValidParameter()
        {
            IPAddress ip;
            if (!IPAddress.TryParse(txtIp.Text, out ip))
            {
                return new ScadaResult(false, "服务器IP地址不正确");
            }
            if (cbMessage.SelectedItem == null)
            {
                return new ScadaResult(false, "请选择消息质量类型");
            }
            if (cbWill.SelectedItem == null)
            {
                return new ScadaResult(false, "请选择遗愿标志");
            }

            if (cbReceiveMethod.SelectedItem == null)
            {
                return new ScadaResult(false, "请选择接收方式");
            }

            if (cbReceiveMethod.SelectedItem == null)
            {
                return new ScadaResult(false, "请选择数据接收方式");
            }



            return new ScadaResult();
        }

        private void cbEnableUser_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = cbEnableUser.Checked;
        }

      
        private void rbTCP_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTCP.Checked)
                cbSSLVersion.Enabled = false;
            nudPort.Value = 1883;
            tbSSLCertificate.ReadOnly = true;
        }

        private void rbTSL_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTSL.Checked)
                cbSSLVersion.Enabled = true;
            nudPort.Value = 8883;
            tbSSLCertificate.ReadOnly = false;
        }
    }
}
