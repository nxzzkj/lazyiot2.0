using Scada.Business;
using IOMonitor.Core;

using Scada.Controls;
using Scada.Controls.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;
using System.Diagnostics;
using IOMonitor.Forms;


 
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
    public partial class MonitorForm : Form
    {
 
        public MonitorForm()
        {
            InitializeComponent();
            this.Load += MonitorForm_Load;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.VisibleChanged += MonitorForm_VisibleChanged;

        }

        private void MonitorForm_VisibleChanged(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public Mediator mediator = null;
        private   void MonitorForm_Load(object sender, EventArgs e)
        {
            
            timer.Start();
            
            DbHelperSQLite.connectionString = "Data Source=" + Application.StartupPath + "\\IOProject\\Station.station";
          
          
             
            try
            {
                 runBakcMenu.Checked= IOMonitorManager.IsBackRun;
                runBackToolMenu.Checked= IOMonitorManager.IsBackRun;
                IOMonitorUIManager.InitMonitorMainForm(this);

            }
            catch (Exception ex)
            {
               IOMonitorUIManager.AppendLogItem(ex.Message);
            }
        }

     
 
 
      

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private   void 退出采集站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否要退出IO采集站，退出后设备将无法获取传输数据!", "退出提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
           
                IOMonitorUIManager.ApplicationExit();
            
            }


        }

        private void 打卡监视器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel5.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            if(IOMonitorManager.MDSClientConnectedStatus)
            {
                this.toolStripConnectedStatus.Text = "数据中心网络链接正常";
            }
            else
            {
                this.toolStripConnectedStatus.Text = "数据中心网络链接断开";
            }
        }

        private void 采集工程管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process ioMonitor = Process.Start(Application.StartupPath + "//IOManager.exe");
        }

        private void btMax_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
       
        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void 采集站模拟器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                SimulatorForm simulatorForm = new SimulatorForm();
                simulatorForm.Show(this);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           


        }

        private void 采集站编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "//IOMonitor.exe");
        }

        private void 网络配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
        }

        private void 正常窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private   void 停止服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(this,"是否要停止数据采集?","操作提示",MessageBoxButtons.OKCancel)==DialogResult.OK)
            {
                  IOMonitorManager.Stop();
            }
        }

        private   void 启动服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否要启动数据采集服务?", "操作提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                 IOMonitorManager.Start();
            }
        }

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;

        }

        private void runBakcMenu_Click(object sender, EventArgs e)
        {
            runBakcMenu.Checked = !runBakcMenu.Checked;
            IOMonitorManager.IsBackRun = runBakcMenu.Checked;
            runBackToolMenu.Checked = runBakcMenu.Checked;
        }

        private void runBackToolMenu_Click(object sender, EventArgs e)
        {
            runBackToolMenu.Checked = !runBackToolMenu.Checked;
            IOMonitorManager.IsBackRun = runBackToolMenu.Checked;
            runBakcMenu.Checked = runBackToolMenu.Checked;
        }

        private void 系统配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetClientForm netClientForm = new NetClientForm();
            netClientForm.ShowDialog(this);
        }

        private void 最小化ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
      
            
        }

        private void 最大化ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
          
        }
    }
}
