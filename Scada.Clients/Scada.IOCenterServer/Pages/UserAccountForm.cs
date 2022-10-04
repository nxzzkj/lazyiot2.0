using Scada.Controls.Forms;
using ScadaCenterServer.Core;
using ScadaCenterServer.Dialogs;
using System;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaCenterServer.Pages
{
    public partial class UserAccountForm : PopBaseForm
    {
        public UserAccountForm()
        {
            InitializeComponent();
            this.Load += UserAccountForm_Load;
        }

        private void UserAccountForm_Load(object sender, EventArgs e)
        {
            this.tbUser.Text = IOCenterManager.IOProject.ServerConfig.User;
            this.tbPassword.Text = IOCenterManager.IOProject.ServerConfig.Password;
        }

        private void btnOK_BtnClick(object sender, EventArgs e)
        {
            if (this.tbPassword.Text.Trim() == "")
            {
                FrmDialog.ShowDialog(this, "请输入密码!");
                return;
            }
            if (this.tbPassword.Text.Trim() != this.tbConfirm.Text.Trim())
            {
                FrmDialog.ShowDialog(this, "密码确认不正确!");
                return;
            }
            if (FrmDialog.ShowDialog(this, "请输入密码!", "修改密码提醒", true, true, true, true) == DialogResult.OK)
            {


                IOCenterManager.IOProject.ServerConfig.User = this.tbUser.Text;
                IOCenterManager.IOProject.ServerConfig.Password = this.tbPassword.Text;
                IOCenterManager.IOProject.ServerConfig.WriteConfig();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_BtnClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
