using Scada.Controls.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Model;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaFlowDesign.Dialog
{
    public partial class FlowUserManagerForm : Form
    {
        public FlowUserManagerForm()
        {
            InitializeComponent();
            this.Load += FlowUserManagerForm_Load;
         
        }

        private void FlowUserManagerForm_Load(object sender, EventArgs e)
        {
            if (_EditUser != null)
            {
                this.tbNikeName.Text = _EditUser.Nickname;
                this.tbUserName.Text = _EditUser.UserName;
                this.tbPassword.Text = _EditUser.Password;
                this.cbRead.Checked = _EditUser.Read == 1 ? true : false;
                this.cbWrite.Checked = _EditUser.Write == 1 ? true : false;
            }
        }

        private ScadaFlowUser _EditUser = null;
        /// <summary>
        /// 返回用户信息
        /// </summary>
        /// <returns></returns>
        public ScadaFlowUser EditUser
        {
            set { _EditUser = value;

              
            }
            get { return _EditUser; }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (this.tbNikeName.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户昵称");
                return;
            }
            if (this.tbUserName.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户名称");
                return;
            }
            if (this.tbPassword.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入用户密码");
                return;
            }
            if (EditUser == null)
                EditUser = new ScadaFlowUser();
            EditUser.Nickname = this.tbNikeName.Text.Trim();
            EditUser.Password = this.tbPassword.Text.Trim();
            EditUser.UserName = this.tbUserName.Text.Trim();
            if (this.cbRead.Checked)
            {
                EditUser.Read = 1;
            }
            else
            {
                EditUser.Read = 0;
            }

            if (this.cbWrite.Checked)
            {
                EditUser.Write = 1;
            }
            else
            {
                EditUser.Write = 0;
            }
            this.DialogResult = DialogResult.OK;

        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult=DialogResult.Cancel;
        }
    }
}
