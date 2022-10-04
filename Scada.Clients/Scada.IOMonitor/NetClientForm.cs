using IOMonitor.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor
{
 
    public partial class NetClientForm : Form
    {
     
        public NetClientForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConnect_Click(object sender, EventArgs e)
        {
            
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            
           
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(this,"是否要保存网络配置，保存后需要重新启动采集站!\r\n是否要保存配置?","保存提示",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                try
                {
                    IOMonitorManager.MonitorConfig.CacheMaxNumber = Convert.ToInt32(this.nudMaxNumber.Value);
                    IOMonitorManager.MonitorConfig.CacheInterval = Convert.ToInt32(this.nudTimeInternal.Value);
                    IOMonitorManager.MonitorConfig.SendBlockTime = Convert.ToInt32(this.nudBlocking.Value);
                    IOMonitorManager.MonitorConfig.DataMessageTimeout = Convert.ToInt32(this.nudTransTimeout.Value);
                    
                    IOMonitorManager.MonitorConfig.User = this.tbUser.Text.Trim();
                    IOMonitorManager.MonitorConfig.Password = this.tbPwd.Text.Trim();
                    IOMonitorManager.MonitorConfig.AutoLogin = this.cbAuto.Checked ? 1 : 0;
                    IOMonitorManager.EnableWriterLog = this.cbLog.Checked;
                    IOMonitorManager.MonitorConfig.WriteConfig();
                }
                catch(Exception emx)
                {
                    MessageBox.Show(this,emx.Message);
                }
              

            }
        }

        private void NetClientForm_Load(object sender, EventArgs e)
        {
            this.nudMaxNumber.Value = IOMonitorManager.MonitorConfig.CacheMaxNumber;
            this.nudTimeInternal.Value = IOMonitorManager.MonitorConfig.CacheInterval;
            this.tbUser.Text = IOMonitorManager.MonitorConfig.User;
            this.tbPwd.Text = IOMonitorManager.MonitorConfig.Password;
            this.nudBlocking.Value = IOMonitorManager.MonitorConfig.SendBlockTime;
            this.nudTransTimeout.Value = IOMonitorManager.MonitorConfig.DataMessageTimeout;
        

            if (IOMonitorManager.MonitorConfig.AutoLogin==1)
            {
                this.cbAuto.Checked = true;
            }
            else
            {
                this.cbAuto.Checked = false;
            }

            this.cbLog.Checked = IOMonitorManager.EnableWriterLog;






        }
    }
}
