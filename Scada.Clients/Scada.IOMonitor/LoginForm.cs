using IOMonitor.Core;
using Scada.Controls.Forms;
using Scada.DBUtility;
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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Load += LoginForm_Load;
        }
       
        public LoginForm(string user,string password)
        {
            InitializeComponent();
            this.Load += LoginForm_Load;
            this.tbUser.Text = user;
            this.tbPassword.Text = password;
          
    }
 
        public string  IP
        {
            get { return this.txtIP.Text; }
        }
        public string User
        {
            get {
                return this.tbUser.Text.Trim();
            }
        }
        public string Password
        {
            get
            {
                return this.tbPassword.Text.Trim();
            }
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {  
            
             
            IOMonitorManager.MonitorConfig = new IOMonitorConfig();

            this.tbPhysicalAddress.Text = ComputerInfo.GetInstall().ServerID;

            this.txtIP.Text = Core.IOMonitorManager.MonitorConfig.RemoteIP;
            tbUser.Text = Core.IOMonitorManager.MonitorConfig.User;
            tbPassword.Text = Core.IOMonitorManager.MonitorConfig.Password;
            if (Core.IOMonitorManager.MonitorConfig.AutoLogin == 1)
            {
                IOMonitorManager.EnableWriterLog = true;
            }
            else
            {
                IOMonitorManager.EnableWriterLog = false;
            }
     

            if (Core.IOMonitorManager.MonitorConfig.AutoLogin == 1 
                &&!string.IsNullOrEmpty(tbUser.Text)
                && !string.IsNullOrEmpty(tbPassword.Text)
                && !string.IsNullOrEmpty(txtIP.Text))
            {
                btnOK_BtnClick(sender, e);//自动执行登录
            }
        }
        public void SetShowInfo(string msg)
        {
            try
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new EventHandler(delegate
                {
                    labelInfo.Text = msg;
                }));
                }
            }
            catch
            {

            }
        }

        private   void btnOK_BtnClick(object sender, EventArgs e)
        {
           
            if (this.tbUser.Text.Trim() == "")
            {
                FrmDialog.ShowDialog(this, "请输入账户", "账户输入");
                return;
            }
            if (this.tbPassword.Text.Trim() == "")
            {
                FrmDialog.ShowDialog(this, "请输入密码", "密码输入");
                return;
            }
            if (this.txtIP.Text.Trim() == "")
            {
                FrmDialog.ShowDialog(this, "请输入服务器的IP地址", "IP提示");
                return;
            }


            IPAddress ip = IPAddress.Any;
            if (IPAddress.TryParse(this.txtIP.Text.Trim(), out ip))
            {
                 IOMonitorManager.CloseMDSClient();
                //判断是否连接到服务器
                IOMonitorManager.OnConnectedServer += IOMonitorManager_OnConnectedServer;
                IOMonitorManager.OnUserLogined += IOMonitorManager_OnUserLogined;
             
                IOMonitorManager.InitMDSClient(this.txtIP.Text);
                IOMonitorManager.LoginManager(this.tbUser.Text.Trim(), this.tbPassword.Text.Trim());
            }
            else
            {
                MessageBox.Show(this, "请输入正确服务器的IP地址", "IP提示");
            }

        }

        private void IOMonitorManager_OnUserLogined(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(sender))
            {
                try
                {

                    Core.IOMonitorManager.MonitorConfig.User = this.tbUser.Text.Trim();
                    Core.IOMonitorManager.MonitorConfig.Password = this.tbPassword.Text.Trim();
                    Core.IOMonitorManager.MonitorConfig.RemoteIP = this.txtIP.Text.Trim();
                }
                catch
                {

                }


                this.DialogResult = DialogResult.OK;
            }
            else
            {
                SetShowInfo("登录服务器失败!" );


            }
        }

        private void IOMonitorManager_OnConnectedServer(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(sender) & Core.IOMonitorManager.MDSClient != null)
            {
                IOMonitorManager.LoginManager(User, Password);
                             
            }
        }

        private   void btnCancel_BtnClick(object sender, EventArgs e)
        {
            IOMonitorUIManager.ApplicationExit();
        

            this.Close();

        }
    }
}
