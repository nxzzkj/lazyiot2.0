

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
using Scada.Controls;
using Scada.Controls.Forms;
using Scada.DBUtility;
using ScadaCenterServer.Core;
using ScadaCenterServer.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaCenterServer
{

    public partial class IOCenterMainForm : Form
    {

        public IOCenterMainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += MainForm_Load;
            Control.CheckForIllegalCrossThreadCalls = false;
        }
  
        
        
        private void MainForm_Load(object sender, EventArgs e)
        {
          
            IOCenterManager.QueryFormManager.InitQueryForm(this);
            IOCenterManager.LoadSystem(this);
            IOCenterManager.ServerForm.Show(this);
            timerDate.Start();
        }
        

         




        private void 系统管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "是否要退出数据中心服务", "退出系统", true, true, true, true) == DialogResult.OK)
            {

            
                IOCenterManager.Close();

                Application.ExitThread();
                Application.Exit();
                Process[] pross = Process.GetProcessesByName(Application.ProductName);
                if (pross.Length > 0)
                {
                    pross[0].Kill();
                }

            }

        }

        private void 模拟器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOCenterManager.SimulatorManager != null)
            {
                IOCenterManager.SimulatorManager.ShowSimulator();
            }
        }

        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
        }

        private void IOCenterMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        //用户启动服务


        private void 启动服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.StartServer();
        }

        private void 停止服务toolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.CloseServer();
        }
        public void ServerStoped()
        {
            ToolStripStatus.Text = "服务已关闭";
            ToolStripStatus.ForeColor = Color.Red;
            notifyIcon.BalloonTipTitle = "服务状态";
            notifyIcon.BalloonTipText = "服务已关闭,请尽快启动数据监视服务!";
            启动服务ToolStripMenuItem.Enabled = true;
            停止服务toolStripMenuItem.Enabled = false;
            启动服务ToolStripMenuItem1.Enabled = true;
            停止服务ToolStripMenuItem1.Enabled = false;
        }
        public void ServerStarted()
        {
            ToolStripStatus.Text = "服务运行中......";
            ToolStripStatus.ForeColor = Color.DarkGreen;
            notifyIcon.BalloonTipTitle = "服务状态";
            notifyIcon.BalloonTipText = "服务运行中......";
            启动服务ToolStripMenuItem.Enabled = false;
            停止服务toolStripMenuItem.Enabled = true;
            启动服务ToolStripMenuItem1.Enabled = false;
            停止服务ToolStripMenuItem1.Enabled = true;
        }
        
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOCenterMainForm));
        private void timerDate_Tick(object sender, EventArgs e)
        {
            if (IOCenterManager.IOCenterServer == null)
                return;
            labelDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (IOCenterManager.IOCenterServer.MDSServerStatus == MDSServerStatus.运行)
            {
                if (this.notifyIcon.Tag == null)
                {
                    this.notifyIcon.Tag = 0;
                }

                if (this.notifyIcon.Tag.ToString() == "1")
                {
                    this.notifyIcon.Tag = 0;
                    this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
                }

                else
                {
                    this.notifyIcon.Tag = 1;
                    this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                }


            }
            else
            {
                this.notifyIcon.Tag = 0;
                this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            }

        }

        private void 实时库配置toolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenInfluxConfigForm();
        }

        private void 实时数据查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenRealQueryWorkForm();
        }

        private void 历史数据查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenHistoryQueryWorkForm();
        }

        private void 历史报警查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenHistoryAlarmQueryWorkForm();
        }

        private void 下置命令查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenSendCommandQueryWorkForm();
        }

        private void 报警配置日志toolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenAlarmConfigQueryWorkForm();
        }

        private void iO树ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOCenterManager.QueryFormManager.Mediator.IOTreeForm.DockState == DockState.Hidden)
            {
                IOCenterManager.QueryFormManager.Mediator.IOTreeForm.DockState = Scada.Controls.DockState.DockLeft;
            }
            else
            {
                IOCenterManager.QueryFormManager.Mediator.IOTreeForm.DockState = Scada.Controls.DockState.Hidden;
            }

        }

        private void 属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOCenterManager.QueryFormManager.Mediator.IOPropeitesForm.DockState == Scada.Controls.DockState.Hidden)
            {
                IOCenterManager.QueryFormManager.Mediator.IOPropeitesForm.DockState = Scada.Controls.DockState.DockLeft;
            }
            else
            {
                IOCenterManager.QueryFormManager.Mediator.IOPropeitesForm.DockState = Scada.Controls.DockState.Hidden;
            }
        }

        private void 日志窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (IOCenterManager.QueryFormManager.Mediator.OperatorLogForm.DockState == Scada.Controls.DockState.Hidden)
            {
                IOCenterManager.QueryFormManager.Mediator.OperatorLogForm.DockState = Scada.Controls.DockState.DockLeft;
            }
            else
            {
                IOCenterManager.QueryFormManager.Mediator.OperatorLogForm.DockState = Scada.Controls.DockState.Hidden;
            }
        }

        private void 关闭ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void 统计汇总查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenHistoryStaticsQueryWorkForm();
        }

        private void 备份管理toolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenInfluxDBBackupForm();
        }

        private void 网络设置toolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserAccountForm form = new UserAccountForm();
            form.ShowDialog(this);
        }

        private void 关于我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> sb = new List<string>();
            OpenFileDialog dig = new OpenFileDialog();
            if (dig.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(dig.FileName, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    sb.Add(sr.ReadLine());
                }
                sr.Close();
                foreach (string sql in sb)
                {
                    string sql2 = sql.Replace(";", "");
                    if (sql2 != "")
                    {
                        DbHelperSQLite.ExecuteSql(sql2);
                    }
                }
                MessageBox.Show(this, "保存成功");

            }
        }



        private void 退出系统ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "是否要退出数据中心服务", "退出系统", true, true, true, true) == DialogResult.OK)
            {
                IOCenterManager.Close();

                Application.ExitThread();
                Application.Exit();
                Process[] pross = Process.GetProcessesByName(Application.ProductName);
                if (pross.Length > 0)
                {
                    pross[0].Kill();
                };
               

            }

        }
        public void SetRunBackMenuStatus(bool isback)
        {
            runBackMenu.Checked = isback;

        }
        private void runBackMenu_Click(object sender, EventArgs e)
        {
            runBackMenu.Checked = !runBackMenu.Checked;
            IOCenterManager.IsBackRun = runBackMenu.Checked;
            runBackToolMenu.Checked = runBackMenu.Checked;
        }

        private void runBackToolMenu_Click(object sender, EventArgs e)
        {
            runBackToolMenu.Checked = !runBackToolMenu.Checked;
            IOCenterManager.IsBackRun = runBackToolMenu.Checked;
            runBackMenu.Checked = runBackToolMenu.Checked;
        }

        private void 采集站授权管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenScadaMDSManagerForm();
        }

        private void 历史事件查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenScadaEventQueryWorkForm();
        }

        private void 机器训练日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenScadaMachineTrainQueryWorkForm();
            IOCenterManager.QueryFormManager.Mediator.ScadaMachineTrainQueryWorkForm.InitTreeProject();
        }

        private void 机器训练预测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenScadaMachineTrainForeastQueryWorkForm();
            IOCenterManager.QueryFormManager.Mediator.ScadaMachineTrainForeastQueryWorkForm.InitTreeProject();
        }

        private void 自动控制日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOCenterManager.QueryFormManager.Mediator.OpenScadaBatchCommandTaskForm();
            IOCenterManager.QueryFormManager.Mediator.ScadaBatchCommandTaskForm.InitTreeProject();
        }

        private void lazyOSAPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebAppForm webAppForm = new WebAppForm();
            webAppForm.Show();
        }

        private void 监控运行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebAppForm webAppForm = new WebAppForm();
            webAppForm.Show();
        }
    }
}
