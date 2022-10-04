using Scada.Controls.Forms;
using ScadaFlowDesign.Core;
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
namespace ScadaFlowDesign.Dialog
{
    public partial class ProjectPasswordDialog : FrmWithOKCancel1
    {
        private IOFlowProject Project = null;
        public ProjectPasswordDialog(IOFlowProject project)
        {
            InitializeComponent();
            Project = project;
            tbProjectName.Text = Project.Title;

        }
        public string ProjectTitle
        {
            get { return tbProjectName.Text.Trim(); }
        }
        public string Password
        {
            get { return this.tbPassword.Text.Trim(); }
        }
       


        public override void btnOK_BtnClick(object sender, EventArgs e)
        {
            
            if (Password.Trim() == "")
            {
                FrmDialog.ShowDialog(this, "请输入工程加密密码!");
                return;
            }
            if (Password.Trim() != this.tbConfirm.Text.Trim())
            {
                FrmDialog.ShowDialog(this, "密码和确认密码不一致!");
                return;
            }
           if(Project.Password== Password.Trim())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                FrmDialog.ShowDialog(this, "您输入的密码不正确!");
                return;
            }
         
        }

       
    }
}
