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


#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
namespace Scada.DBUtility
{
    public partial class IPAddressSelector : Form
    {
        public IPAddressSelector()
        {
            InitializeComponent();
            this.Load += IPAddressSelector_Load;
        }
        private static IPAddressSelector addressSelector = null;
        public static IPAddressSelector Instance()
        {
            if (addressSelector == null)
            {
                addressSelector = new IPAddressSelector();

            }
        
            return addressSelector;
        }
        public  string AddressIP = "127.0.0.1";
        public string AddressIPNoPoint
        {
            get { return AddressIP.Replace(".",""); }
        }
        private void IPAddressSelector_Load(object sender, EventArgs e)
        {
            cbIPAddress.Items.Clear();
            ///获取本地的IP地址
       
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
             
                    cbIPAddress.Items.Add(_IPAddress.ToString());
             
               
            }
            if (cbIPAddress.Items.Count>0 && cbIPAddress.Items.Count==1)
            {
                //如果只有一个连接的时候则,自动选择
                AddressIP = cbIPAddress.Items[0].ToString();
                this.DialogResult = DialogResult.OK;
            }
         
  
        }
        private void btOK_Click(object sender, EventArgs e)
        {
            if(cbIPAddress.SelectedItem==null)
            {
                MessageBox.Show("请选择合适的网络");
                return;
            }
            AddressIP = cbIPAddress.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
