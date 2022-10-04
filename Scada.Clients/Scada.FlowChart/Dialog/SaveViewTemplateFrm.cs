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
    public partial class SaveViewTemplateFrm : FrmWithOKCancel1
    {
        public SaveViewTemplateFrm()
        {
            InitializeComponent();
        }

    
        public string TemplateName
        {
            get { return tbName.Text.Trim(); }
            set { tbName.Text = value; }
        }
        public string TemplateClassic
        {
            get { return cbClassic.Text.Trim(); }
            set { cbClassic.Text = value; }
        }
 
        public override void btnOK_BtnClick(object sender, EventArgs e)
        {
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入名称");
                return;
            }
            if (cbClassic.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入或者选择换一个分类");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
        public override void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SaveViewTemplateFrm_Load(object sender, EventArgs e)
        {
            this.cbClassic.Items.Clear();
            string[] folds = System.IO.Directory.GetDirectories(Application.StartupPath + "/ScadaTemplate/TemplateViews/");


            for (int i = 0; i < folds.Length; i++)
            {

                this.cbClassic.Items.Add(Path.GetFileName(folds[i]));
            }
        }
    }
}
