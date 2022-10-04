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

namespace ScadaFlowDesign.Dialog
{
    public partial class ViewGroupEdit : Form
    {
        private IOFlowViewGroup _ViewGroup = null;
        public IOFlowViewGroup ViewGroup
        {
            get
            {
               

                return _ViewGroup;
            }
        }
        public ViewGroupEdit(IOFlowViewGroup group)
        {
            InitializeComponent();
            _ViewGroup = group;
            if (group != null)
                this.tbTitle.Text = group.Text;
            this.Load += ViewGroupEdit_Load;
        }

        private void ViewGroupEdit_Load(object sender, EventArgs e)
        {
            if (_ViewGroup != null)
            {
                _ViewGroup.Text = tbTitle.Text;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbTitle.Text))
            {
                MessageBox.Show(this, "请输入分组名称");
                return;
            }
            if (_ViewGroup == null)
                _ViewGroup = new IOFlowViewGroup();
            _ViewGroup.Text = tbTitle.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
