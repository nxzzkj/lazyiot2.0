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

namespace Modbus.ModbusService
{
    public partial class Modbus_Serial_Ctrl : CommunicationKernelControl
    {
        public Modbus_Serial_Ctrl()
        {
            InitializeComponent();
            cbHandshake.SelectedIndex = 0;


        }

     

        public override ScadaResult IsValidParameter()
        {
            if(comboSeriePort.SelectedItem==null)
            {
                MessageBox.Show(this.FindForm(),"请选择串口");
                return new ScadaResult(false, "请选择串口");
            }
            if (cbCheck.SelectedItem == null)
            {
                MessageBox.Show(this.FindForm(), "请选择校验方式");
                return new ScadaResult(false, "请选择校验方式");
            }
            if (cbStopbits.SelectedItem == null)
            {
                MessageBox.Show(this.FindForm(), "请选择停止位");
                return new ScadaResult(false, "请选择停止位");
            }

            return new ScadaResult();
        }
        public override void SetUIParameter(string para)
        {
            if (string.IsNullOrEmpty(para))
            {
                return;
            }
            //初始化
            comboSeriePort.Items.Clear();
            foreach (string vPortName in SerialPort.GetPortNames())
            {
                comboSeriePort.Items.Add(vPortName);
            }
            if (comboSeriePort.Items.Count > 0)
                comboSeriePort.SelectedIndex = 0;
            cbCheck.SelectedIndex = 0;
        
            cbStopbits.SelectedIndex = 0;
            base.SetUIParameter(para);
            ParaPack paraPack = new ParaPack(para);
            //串口
            for (int i=0;i< comboSeriePort.Items.Count;i++)
            {
                if (comboSeriePort.Items[i].ToString() == paraPack.GetValue("串口"))
                {
                    comboSeriePort.SelectedIndex = i;
                    break;
                }
            }
           

            //波特率
            cbBaudRate.Text = paraPack.GetValue("波特率");
            //校验
            for (int i = 0; i < cbCheck.Items.Count; i++)
            {
                if (cbCheck.Items[i].ToString() == paraPack.GetValue("校验"))
                {
                    cbCheck.SelectedIndex = i;
                    break;
                }
            }
            //数据位
            cbDataBits.Text = paraPack.GetValue("数据位");
            //停止位
            for (int i = 0; i < cbStopbits.Items.Count; i++)
            {
                if (cbStopbits.Items[i].ToString() == paraPack.GetValue("停止位"))
                {
                    cbStopbits.SelectedIndex = i;
                    break;
                }
            }

 
            nbPackOffset.Value = Convert.ToDecimal(paraPack.GetValue("偏移间隔"));
            nbWriteTimeout.Value = Convert.ToDecimal(paraPack.GetValue("写超时时间"));
            nbReadTimeout.Value = Convert.ToDecimal(paraPack.GetValue("读超时时间"));
            cbRTSEnable.Checked = paraPack.GetValue("RTS") == "1" ? true : false;
            cbDTREnable.Checked = paraPack.GetValue("DTR") == "1" ? true : false;
          
 
            paraPack.SetCtrlValue(ndReadBuffSize, paraPack.GetValue("读缓存"));
            paraPack.SetCtrlValue(ndWriteBufferSize, paraPack.GetValue("写缓存"));
            paraPack.SetCtrlValue(cbHandshake, paraPack.GetValue("握手协议"));

        }
        public override string GetUIParameter()
        {
            ParaPack paraPack = new ParaPack();
            paraPack.AddItem("串口", comboSeriePort.SelectedItem.ToString());
    
            
            paraPack.AddItem("波特率", cbBaudRate.Text);
            paraPack.AddItem("数据位", cbDataBits.Text);
            paraPack.AddItem("校验", cbCheck.SelectedItem.ToString());
            paraPack.AddItem("停止位", cbStopbits.SelectedItem.ToString());
     
            paraPack.AddItem("握手协议", cbHandshake.SelectedItem.ToString());
            paraPack.AddItem("偏移间隔", nbPackOffset.Value.ToString("0"));
            paraPack.AddItem("写超时时间", nbWriteTimeout.Value.ToString("0"));
            paraPack.AddItem("读超时时间", nbReadTimeout.Value.ToString("0"));
            paraPack.AddItem("RTS", cbRTSEnable.Checked ? "1" : "0");
            paraPack.AddItem("DTR", cbDTREnable.Checked ? "1" : "0");
 
            paraPack.AddItem("读缓存", ndReadBuffSize.Text);
            paraPack.AddItem("写缓存", ndWriteBufferSize.Text);
            return paraPack.ToString();
        }

       
    }
}
