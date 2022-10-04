using IOManager.Controls;
using Scada.DBUtility;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOManager.Dialogs
{

     
    /*----------------------------------------------------------------
    // Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
    // 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
    // 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
    // 请大家尊重作者的劳动成果，共同促进行业健康发展。
    // 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
    // 创建者：马勇
    //----------------------------------------------------------------*/
    public partial class MachineTrainingConditionEditForm : BasicDialogForm
    {
        public ScadaMachineTrainingCondition ConditionModel = null;
        private IOTree IOTree = null;
        public MachineTrainingConditionEditForm(IOTree ioTree,ScadaMachineTrainingCondition  condition =null)
        {
            InitializeComponent();
            ConditionModel = condition;
            IOTree = ioTree;
            this.Load += MachineTrainingConditionEditForm_Load;
        }

        private void MachineTrainingConditionEditForm_Load(object sender, EventArgs e)
        {
            if(ConditionModel!=null)
            {
                tbConditionName.Text = ConditionModel.ConditionName;
                tbConditionTitle.Text = ConditionModel.ConditionTitle;
                tbConditionRemark.Text = ConditionModel.Remark;
              


            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (ConditionModel == null)
                ConditionModel = new ScadaMachineTrainingCondition() {  Id= GUIDToNormalID.GuidToInt()};
            if(string.IsNullOrEmpty(tbConditionName.Text))
            {
                MessageBox.Show("请输入工况标识!");
                return;
            }
            if (string.IsNullOrEmpty(tbConditionName.Text))
            {
                MessageBox.Show("请输入工况名称!");
                return;
            }
            ConditionModel.ConditionName = tbConditionName.Text.Trim();
            ConditionModel.ConditionTitle = tbConditionTitle.Text.Trim();
            ConditionModel.Remark = this.tbConditionRemark.Text;
         
            this.DialogResult = DialogResult.OK;
        }
    }
}
