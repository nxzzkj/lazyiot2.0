using Scada.FlowGraphEngine.GraphicsMap;
using System;
using System.Collections.Generic;
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
namespace ScadaFlowDesign.Control
{
    public class SCADAViewNode : TreeNode, IProjectNode
    {
        public override object Clone()
        {
            SCADAViewNode viewNode= (SCADAViewNode)base.Clone();
            viewNode.Text = this.Text;
            viewNode.View = View;
         
            return viewNode;
        }
        public void RefreshText()
        {
            base.Text = this.mView.GraphControl.Abstract.ViewTitle;
        }
        // Fields
        private WorkForm mView;

        // Methods
        public SCADAViewNode()
        {
            base.ImageIndex = 2;
            base.SelectedImageIndex = 2;
         
        }

        private void SCADAViewNode_Click(object sender, EventArgs e)
        {
        }

        // Properties
        public WorkForm View
        {
            get
            {
                return this.mView;
            }
            set
            {
                this.mView = value;
                if (this.mView.GraphControl.Abstract.Index)
                {
                    base.ForeColor = Color.Red;
                }
                else
                {
                    base.ForeColor = Color.Black;
                }
            }
        }

        public FlowGraphAbstract GraphSite
        {
            get
            {
                if (this.View.GraphControl.Abstract != null)
                {
                    return this.View.GraphControl.Abstract;
                }
                return null;
            }
        }
    }

}
