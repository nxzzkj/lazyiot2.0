using Scada.Controls.Forms;
using ScadaFlowDesign.Control;
using ScadaFlowDesign.Core;
using Scada.FlowGraphEngine;
using Scada.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.IndustryDesigner;



/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaFlowDesign
{
    public partial class FlowDesign : Form
    {
        public Mediator mediator;
        public DockPanel DockPanel
        {
            get { return this.dockPanel; }
        }
        public FlowDesign()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            this.Load += FlowDesign_Load;


        }

        private void FlowDesign_Load(object sender, EventArgs e)
        {
            toolStripZoom.SelectedIndex = 6;
            List<string> fiels = new List<string>();
            StreamReader sr = new StreamReader(Application.StartupPath + "//Lately.log", Encoding.Default);
            while (!sr.EndOfStream)
            {
                fiels.Add(sr.ReadLine());
                if (fiels.Count > 10)
                {
                    fiels.RemoveAt(0);
                }
            }
            sr.Close();
            foreach (string str in fiels)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = str;
                item.Click += Item_Click;
                最近打开ToolStripMenuItem.DropDownItems.Add(item);
            }
            //加载第三方库
            SVGAnalysisIndustrySymbol.LoadSymbol();
            //加载本地库
            CustumShapeManage.Loader();
        }

        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            IOFlowManager.LoadProject(item.Text);
        }



        private void 属性区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            属性区ToolStripMenuItem.Checked = !属性区ToolStripMenuItem.Checked;
            if (属性区ToolStripMenuItem.Checked)
            {
                this.mediator.PropertiesForm.DockState = DockState.DockRight;
            }
            else
            {
                this.mediator.PropertiesForm.DockState = DockState.Hidden;
            }

        }

        private void 目录区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            目录区ToolStripMenuItem.Checked = !目录区ToolStripMenuItem.Checked;
            if (目录区ToolStripMenuItem.Checked)
            {
                this.mediator.ToolForm.DockState = DockState.DockLeft;
            }
            else
            {
                this.mediator.ToolForm.DockState = DockState.Hidden;
            }


        }

        private void 图元视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            图元视图ToolStripMenuItem.Checked = !图元视图ToolStripMenuItem.Checked;
            if (图元视图ToolStripMenuItem.Checked)
            {
                this.mediator.ShapeForm.DockState = DockState.DockLeft;
            }
            else
            {
                this.mediator.ShapeForm.DockState = DockState.Hidden;
            }

        }
        private void 日志视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日志视图ToolStripMenuItem.Checked = !日志视图ToolStripMenuItem.Checked;
            if (日志视图ToolStripMenuItem.Checked)
            {
                this.mediator.LogForm.DockState = DockState.DockBottom;
            }
            else
            {
                this.mediator.LogForm.DockState = DockState.Hidden;
            }
        }
        private void 新建页面ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            mediator.ToolForm.CreateView();

        }
        public ToolStripStatusLabel ToolStatusInfo
        {
            get { return toolStripInfo; }
        }
        public void SetStatusText(string msg)
        {
            toolStripInfo.Text = msg;
        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,this.mediator.DockPanel.Documents.Length.ToString());
        }

        private void 新建工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.CreateNewProject();
        }

        private void 保存工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < IOFlowManager.Projects.Count; i++)
            {

                IOFlowManager.SaveProject(IOFlowManager.Projects[i]);

            }

        }

        private void 发布工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(FrmDialog.ShowDialog(this,"是否要发布选中的流程","发布流程提示",true)==DialogResult.OK)
            {
                IOFlowManager.PublishProject();
            }
   
        }

        private void 预览工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.ViewProject();
    
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.SaveAsProject();
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
                this.Close();

        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "是否要删除当前活动视图中选择的图元", "删除提示", true, true, true, true) == DialogResult.OK)
            {
                if (IOFlowManager.Graph != null)
                {
                    IOFlowManager.Graph.Delete();
                }
            }
        }



        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            IOFlowManager.Graph.Paste();
        }





        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            IOFlowManager.Graph.Copy();

        }

        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            if (IOFlowManager.Graph.Hover != null)
            {
                IOFlowManager.Graph.SendForwards(IOFlowManager.Graph.Hover);
            }

        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            if (IOFlowManager.Graph.Hover != null)
            {
                IOFlowManager.Graph.SendBackwards(IOFlowManager.Graph.Hover);
            }
        }

        private void 置顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            if (IOFlowManager.Graph.Hover != null)
            {
                IOFlowManager.Graph.SendToFront(IOFlowManager.Graph.Hover);
            }
        }

        private void 置底ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            if (IOFlowManager.Graph.Hover != null)
            {
                IOFlowManager.Graph.SendToBack(IOFlowManager.Graph.Hover);
            }
        }

        private void 打开工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.OpenProject();
        }


        private void 剪贴toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOFlowManager.Graph == null)
                return;
            IOFlowManager.Graph.Cut();
        }

        private void 删除工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.DeleteProject();
 
        }

        private void 删除视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.DeleteView();
        }
        public void SetHoverInformation(string msg)
        {
            this.toolStripHitHover.Text = msg;

        }
        public void SetGraphMouseInformation(string msg)
        {
            this.toolStripMapXY.Text = msg;
        }
        public void SetCombination()
        {
            toolCombination.Visible = false;
            toolUnCombination.Visible = true;
        }
        public void SetUnCombination()
        {
            toolCombination.Visible = true;
            toolUnCombination.Visible = false;

        }
        public void SetUnLock(bool res)
        {
            this.toolLocked.Checked = res;
        }

        private void toolCombination_Click(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork != null)
            {
                ((WorkForm)this.mediator.ActiveWork).GraphControl.Combination();

            }
        }

        private void toolUnCombination_Click(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork != null)
            {
                ((WorkForm)this.mediator.ActiveWork).GraphControl.UnCombination();

            }
        }

        private void toolLocked_Click(object sender, EventArgs e)
        {
            toolLocked.Checked = !toolLocked.Checked;
            ((WorkForm)this.mediator.ActiveWork).GraphControl.LockedShape(toolLocked.Checked);
        }

        private void FlowDesign_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "退出系统前请先保存工程,是否要退出当前系统?", "退出提示", true, true, true, true) == DialogResult.OK)
            {
                IOFlowManager.Close();
               

            }
            else
            {
                e.Cancel=true;
            }
            
        }

        private void 添加用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.ToolForm.AddFlowUser();
        }

        private void 编辑用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.ToolForm.EditFlowUser();
        }

        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.ToolForm.DeleteFlowUser();
        }

        private void 图标资源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://icon.52112.com");
        }

        private void toolStripZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork!=null)
            {
                WorkForm workForm =(WorkForm) this.mediator.ActiveWork;
                workForm.GraphControl.Zoom = float.Parse(toolStripZoom.SelectedItem.ToString()) / 100;
            }

        }

        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork != null)
            {
                WorkForm workForm = (WorkForm)this.mediator.ActiveWork;
                for(int i=0;i< toolStripZoom.Items.Count;i++)
                {
                    if (toolStripZoom.Items[i].ToString()==Convert.ToInt32(workForm.GraphControl.Zoom*100).ToString())
                    {
                        toolStripZoom.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void 创建视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.CreateView();
        }

        private void 编辑视图名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.EditViewName();
        }

        private void 删除视图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IOFlowManager.DeleteView();
        }

        private void 打开视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.OpenView();
        }

        private void 复制视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.CopyView();
        }

        private void 粘贴视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.PasteView();
        }

        private void 应用背景到其它视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.ViewPropeitiesToOther();
        }

        private void 全部打开视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.OpenAllView();
        }

        private void 全部关闭视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.ClosedAllView();
        }

        private void 创建分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.CreateViewGroup();
        }

        private void 编辑分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.EditViewGroup();
        }

        private void 删除分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.DeleteViewGroup();
        }

        private void 设为主视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.SetProjectIndex();
        }

        private void 添加数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
     
        }

        private void sqllitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.AddSqllitSource();
        }

        private void sqlServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.AddSqlServerSource();
        }

        private void oracleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.AddOracleSource();
        }

        private void syBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.AddSyBaseSource();
        }

        private void mySqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.AddMySqlSource();
        }

        private void 编辑工程用户和密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.EditProjectUserAndPassword();
        }

        private void 关于我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(" http://124.223.27.3/Scada/ScadaFlow?id=17");
        }

        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
       
            Process.Start(" http://124.223.27.3/Scada/ScadaFlow?id=16");
        }

        private void 技术支持ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(" http://124.223.27.3/Scada/ScadaFlow?id=18");
        }

        private void 预览视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOFlowManager.Debug();
        }

        private void 自定义图元ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scada.IndustryDesigner.DesignerForm designerForm = new Scada.IndustryDesigner.DesignerForm();
            designerForm.ShowDialog(this);
      
        }

        private void toolStripButtonScreenshot_Click(object sender, EventArgs e)
        {
            IOFlowManager.ScreenShot();
            
        }
    }
}