
using CefSharp.WinForms;
using Scada.DBUtility;
using ScadaFlowDesign.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
namespace ScadaFlowDesign
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.FormClosed += LoginForm_FormClosed;
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            browser.Dispose();
            browser = null;
        }

        private void ucBtLogin_BtnClick(object sender, EventArgs e)
        {
            if (this.tbUser.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入账户", "账户输入");
                return;
            }
            if (this.tbPassword.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入密码", "密码输入");
                return;
            }
            if (this.txtIP.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入服务器的IP地址", "IP提示");
                return;
            }
            SetInfo("正在尝试连接服务器....");
            IPAddress ip = IPAddress.Any;
            if (IPAddress.TryParse(this.txtIP.Text.Trim(), out ip))
            {

                //判断是否连接到服务器
                IOFlowManager.OnConnectedServer += FormManager_OnConnectedServer;
                IOFlowManager.OnUserLogined += FormManager_OnUserLogined;
                //初始化服务
                IOFlowManager.InitMDSClient(this.txtIP.Text);
                IOFlowManager.LoginManager(this.tbUser.Text.Trim(), this.tbPassword.Text.Trim());
            }
            else
            {
                MessageBox.Show(this, "请输入正确服务器的IP地址", "IP提示");
            }






        }

        

        private void ucBtnClose_BtnClick(object sender, EventArgs e)
        {
            IOFlowManager.Close();
            Application.ExitThread();
            Application.Exit();

            this.Close();
      
        }
        public string User
        {
            get {return  this.tbUser.Text.Trim(); }
        }
        public string Password
        {
            get { return this.tbPassword.Text.Trim(); }
        }

        private void SetInfo(string msg)
        {
            try
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new EventHandler(delegate
                {
                    this.labelInfo.Text = msg;
                }));
                }
            }
            catch
            {

            }

        }
        ChromiumWebBrowser browser = null;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            IOFlowManager.Config = new  IOMonitorConfig();
            this.tbPassword.Text = IOFlowManager.Config.Password;
            this.tbUser.Text = IOFlowManager.Config.User;
            this.txtIP.Text = IOFlowManager.Config.RemoteIP;
            this.tbPhysicalAddress.Text = ComputerInfo.GetInstall().ServerID;
              browser = new ChromiumWebBrowser();
            browser.Visible = false;
         
            browser.Width = 20;
            browser.Height = 20;
            this.Controls.Add(browser);
        }
      

        /// <summary>
        /// 用户登录成功的标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormManager_OnUserLogined(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(sender))
            {
                IOFlowManager.Config.User = this.tbUser.Text.Trim();
                IOFlowManager.Config.Password = this.tbPassword.Text.Trim();
                IOFlowManager.Config.RemoteIP = this.txtIP.Text.Trim();

                IOFlowManager.Config.WriteConfig();//保存用户配置信息
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                SetInfo("无法登录服务器，账号密码错误或未在数据中心注册此采集站IP，!");


            }
        }
        /// <summary>
        /// 连接服务器正常的标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormManager_OnConnectedServer(object sender, EventArgs e)
        {
            SetInfo("与服务器连接成功!");
            Thread.Sleep(1000);
            SetInfo("正在验证登录.....");
            if (Convert.ToBoolean(sender))
            {

                IOFlowManager.LoginManager(User, Password);
            }

        }
    }
}
