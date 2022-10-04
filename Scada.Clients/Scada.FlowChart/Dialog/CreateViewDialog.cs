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
    public partial class CreateViewDialog : FrmWithOKCancel1
    {
        public CreateViewDialog()
        {
            InitializeComponent();
            comboBoxShowModel.SelectedIndex = 0;
        }
        public int PageWidth
        {
            get { return Convert.ToInt32(this.nubWidth.Value); }
        }
        public int PageHeight
        {
            get { return Convert.ToInt32(this.nubHeight.Value); }
        }
        public string ViewName
        {
            get { return this.tbViewName.Text.Trim(); }
        }
        public override void btnOK_BtnClick(object sender, EventArgs e)
        {
            if(ViewName.Trim()=="")
            {
                FrmDialog.ShowDialog(this, "请输入视图名称!");
                return;
            }
            if (PageWidth <= 300)
            {
                FrmDialog.ShowDialog(this, "页面宽度不能小于300!");
                return;
            }
            if (PageHeight <= 300)
            {
                FrmDialog.ShowDialog(this, "页面高度不能小于300!");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void comboBoxShowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBL.Items.Clear();
            if (comboBoxShowModel.SelectedIndex==0)
            {
                comboBoxBL.Items.Clear();
                comboBoxBL.Items.Add("16:9");
                comboBoxBL.Items.Add("4:3");
                comboBoxBL.Items.Add("3:2");
                
            }
            else
            {
                comboBoxBL.Items.Add("16:9");
                comboBoxBL.Items.Add("18:9");
                comboBoxBL.Items.Add("3:2");
                comboBoxBL.Items.Add("16:10");
                comboBoxBL.Items.Add("15:9");
            }
        }

        private void comboBoxBL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxShowModel.SelectedIndex>=0)
            {
                string str = comboBoxBL.SelectedItem.ToString();
                int w =int.Parse( str.Split(':')[0]);
                int h = int.Parse(str.Split(':')[1]);
                switch (comboBoxShowModel.SelectedIndex)
                {
                    case 0:
                        {
                            this.nubWidth.Value = 2400;
                            this.nubHeight.Value = 2400 * h / (w + h);
                        }
                        break;
                    case 1:
                        this.nubHeight.Value = 1920;
                        this.nubWidth.Value = 1920 * h / (w + h);
                        break;
                }
            }

        }
    }
}
