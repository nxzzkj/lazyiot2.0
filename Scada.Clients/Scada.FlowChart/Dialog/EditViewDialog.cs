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
    public partial class EditViewDialog : FrmWithOKCancel1
    {
        WorkForm mWork = null;
        public EditViewDialog(WorkForm work)
        {
            InitializeComponent();
            mWork = work;
            this.nubWidth.Value = Convert.ToDecimal(mWork.GraphControl.Abstract.MapWidth);
            this.nubHeight.Value = Convert.ToDecimal(mWork.GraphControl.Abstract.MapHeight);
            this.tbViewName.Text = mWork.GraphControl.Abstract.ViewTitle;
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
            if(PageWidth<=300)
            {
                FrmDialog.ShowDialog(this, "页面宽度不能小于300!");
                return;
            }
            if (PageHeight <= 300)
            {
                FrmDialog.ShowDialog(this, "页面高度不能小于300!");
                return;
            }
            mWork.GraphControl.Abstract.MapHeight = PageHeight;
            mWork.GraphControl.Abstract.MapWidth = PageWidth;
            mWork.GraphControl.Abstract.ViewTitle = ViewName;
            mWork.Text = ViewName;
            this.DialogResult = DialogResult.OK;
        }

    }
}
