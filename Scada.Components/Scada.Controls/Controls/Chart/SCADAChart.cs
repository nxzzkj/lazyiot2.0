using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
namespace Scada.Controls.Controls.SCADAChart
{
   public  class SCADAChart:Chart
    {
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuItem1;
        private System.ComponentModel.IContainer components;

        public SCADAChart()
        {
            InitializeComponent();
         
            this.MouseDoubleClick += SCADAChart_MouseDoubleClick;
            
        }
        //设置颜色
        private void SCADAChart_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           
            HitTestResult result = this.HitTest(e.X, e.Y);
            if(result.Series!=null)
            {
                ColorDialog dig = new ColorDialog();
                dig.Color = result.Series.Color;
                if(dig.ShowDialog()==DialogResult.OK)
                {
                    result.Series.Color = dig.Color;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "导出图片";
            toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            // 
            // SCADAChart
            // 
            this.ContextMenuStrip = this.contextMenuStrip;
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dig = new SaveFileDialog();
            dig.Filter = "Jpg(*.jpg)|*.jpg";
            if(dig.ShowDialog()==DialogResult.OK)
            {
                this.SaveImage(dig.FileName, ChartImageFormat.Jpeg);
                MessageBox.Show("导出图片成功!");
            }
        }
    }
}
