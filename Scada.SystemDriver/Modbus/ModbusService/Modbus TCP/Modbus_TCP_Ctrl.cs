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
using Modbus.Utility;

namespace Modbus.ModbusService
{
    public partial class Modbus_TCP_Ctrl : CommunicationKernelControl
    {
        public Modbus_TCP_Ctrl()
        {
            InitializeComponent();

               comboBoxMode.SelectedIndex = 0;
        }
        public override ScadaResult IsValidParameter()
        {
            if (textIP.Text == null)
            {
                MessageBox.Show(this.FindForm(), "请配置设备IP地址");
                return new ScadaResult(false, "请配置设备IP地址");
            }


            return new ScadaResult();
        }
        public override void SetUIParameter(string para)
        {
            //初始化
            if (string.IsNullOrEmpty(para))
            {
                return;
            }
            base.SetUIParameter(para);
            ParaPack paraPack = new ParaPack(para);
            //波特率
            paraPack.SetCtrlValue(textIP, paraPack.GetValue("IP地址"));
            paraPack.SetCtrlValue(ndLocalPort, paraPack.GetValue("设备端口"));
  
            paraPack.SetCtrlValue(nbWriteTimeout, paraPack.GetValue("写超时时间"));
            paraPack.SetCtrlValue(nbReadTimeout, paraPack.GetValue("读超时时间"));
            paraPack.SetCtrlValue(ndReadBuffSize, paraPack.GetValue("读缓存"));
            paraPack.SetCtrlValue(ndWriteBufferSize, paraPack.GetValue("写缓存"));
            paraPack.SetCtrlValue(comboBoxMode, paraPack.GetValue("通讯协议"));

        }

        public override string GetUIParameter()
        {
            ParaPack paraPack = new ParaPack();

            paraPack.AddItem("设备IP", textIP.Text.ToString());
            paraPack.AddItem("设备端口", ndLocalPort.Text);
  
            paraPack.AddItem("写超时时间", nbWriteTimeout.Text);
            paraPack.AddItem("读超时时间", nbReadTimeout.Text);
            paraPack.AddItem("读缓存", ndReadBuffSize.Text);
            paraPack.AddItem("写缓存", ndWriteBufferSize.Text);
            paraPack.AddItem("通讯协议", comboBoxMode);
            return paraPack.ToString();
        }

    }
}
