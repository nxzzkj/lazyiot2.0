using Scada.Controls;
using ScadaFlowDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScadaFlowDesign.Control;
using ScadaFlowDesign.Dialog;
using ScadaFlowDesign.Core;
using Scada.FlowGraphEngine.GraphicsMap;
using Scada.Controls.Forms;
using Scada.FlowGraphEngine.GraphicsCusControl;
using Scada.DBUtility;
using Scada.Model;
using Scada.FlowGraphEngine.GraphicsEngine;
using System.IO;
using Scada.FlowGraphEngine;
using Scada.FlowGraphEngine.GraphicsShape;
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
    public partial class ToolForm : DockContent, ICobaltTab
    {
        private Mediator mediator;
        private string identifier;


        public ToolForm(Mediator m)
        {
            this.InitializeComponent();
            this.mediator = m;
            base.HideOnClose = true;
            base.Load += new EventHandler(this.ToolForm_Load);
        }



        private void ToolForm_Load(object sender, EventArgs e)
        {
            this.LoadTreeViewTemplate();
        }

        private void toolStripSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveProject();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void toolStripSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveAsProject();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void toolStripSaveTemplateView_Click(object sender, EventArgs e)
        {
            ViewToTemplate();
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OpenView();
        }

        private void treeViewTemplate_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        private void treeViewUser_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            EditFlowUser();
        }

        private void 编辑名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditViewName();
        }

        private void 编辑权限ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditRole();
        }

        private void 编辑数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDataSource();
        }

        private void 编辑用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditFlowUser();
        }

        private void 发布工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Publish();
        }

        private void 拷贝视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyView();
        }

        private void 全部打开视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAllView();
        }

        private void 全部关闭视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClosedAllView();
        }

        private void 删除工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                this.DeleteProject();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void 删除视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.DeleteView();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void 删除数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteDataSource();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }

        }

        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeleteFlowUser();
        }

        private void 设为主视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetProjectIndex();
        }

        private void 添加MySql数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMySqlSource();
        }

        private void 添加Oracle数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOracleSource();
        }

        private void 添加SQLit数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSqllitSource();
        }

        private void 添加SqlServer数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSqlServerSource();
        }

        private void 添加SyBase数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSyBaseSource();
        }

        private void 添加用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddFlowUser();
        }

        private void 新增视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateView();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditProjectUserAndPassword();
        }

        private void 应用背景到其它视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewPropeitiesToOther();
        }

        private void 预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Debug();
        }

        private void 粘贴视图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PasteView();
        }

        // Properties
        public TabTypes TabType =>
            TabTypes.Project;

        public string TabIdentifier
        {
            get
            {
                return this.identifier;
            }
            set
            {
                this.identifier = value;
            }
        }

        public TreeNode SelectNode =>
            this.treeView.SelectedNode;

        private void 预览视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Debug();

        }

        private void 创建分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateViewGroup();
        }
      

        private void toolStripMenuItemGroupEdit_Click(object sender, EventArgs e)
        {
            EditViewGroup();
        }

        private void toolStripMenuItemGroupDelete_Click(object sender, EventArgs e)
        {
            DeleteViewGroup();
        }
        private void 粘贴视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteView();
        }

        private void 打开视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenView();
        }
        #region 视图节点的拖动和移动
        private void tvProject_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (true)
            {
                if (e.Item.GetType() == typeof(SCADAViewNode))//只允许拖动视图节点
                {
                    //  开始进行拖放操作，并将拖放的效果设置成移动。
                    this.DoDragDrop(e.Item, DragDropEffects.Move);
                }

            }
        }


        private void tvProject_DragEnter(object sender, DragEventArgs e)
        {
            //  拖动效果设成移动
            e.Effect = DragDropEffects.Move;
        }


        private void tvProject_DragDrop(object sender, DragEventArgs e)
        {
            //  定义一个中间变量
            TreeNode treeNode;
            // 判断拖动的是否为TreeNode类型，不是的话不予处理
            if (e.Data.GetDataPresent("ScadaFlowDesign.Control.SCADAViewNode", false))
            {
                //  拖放的目标节点
                TreeNode targetTreeNode;
                //  获取当前光标所处的坐标
                //  定义一个位置点的变量，保存当前光标所处的坐标点
                Point point = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                //  根据坐标点取得处于坐标点位置的节点
                targetTreeNode = ((TreeView)sender).GetNodeAt(point);
                //  获取被拖动的节点
                treeNode = (TreeNode)e.Data.GetData("ScadaFlowDesign.Control.SCADAViewNode");

                //  判断拖动的节点与目标节点是否是同一个,同一个不予处理
                if (!treeNode.Equals(targetTreeNode) && treeNode.GetType() == typeof(SCADAViewNode) && (targetTreeNode.GetType() == typeof(FlowProjectNode) || targetTreeNode.GetType() == typeof(FlowProjectGroupNode)))
                {
                    SCADAViewNode viewNode = (SCADAViewNode)treeNode;
                    //  往目标节点中加入被拖动节点的一份克隆
                    targetTreeNode.Nodes.Add((TreeNode)treeNode.Clone());
                    //  将被拖动的节点移除
                    treeNode.Remove();
                    if (targetTreeNode.GetType() == typeof(FlowProjectGroupNode))
                    {
                        FlowProjectGroupNode groupNode = (FlowProjectGroupNode)targetTreeNode;
                        for (int i = 0; i < groupNode.Project.ViewGroups.Count; i++)
                        {
                            groupNode.Project.ViewGroups[i].Views.Remove(viewNode.View.GraphControl.Abstract.GID);
                        }
                        groupNode.Group.Views.Add(viewNode.View.GraphControl.Abstract.GID);
                    }
                }
            }
        }
        #endregion
        #region 操作方法
        private void LoadViewTemplate()
        {
            if ((this.treeViewTemplate.SelectedNode.Level == 2) && (this.treeViewTemplate.SelectedNode.Tag != null))
            {
                try
                {
                    this.LoadTempViewToProject(this.treeViewTemplate.SelectedNode.Tag.ToString());
                }
                catch (Exception exception1)
                {
                    MessageBox.Show(exception1.Message);
                }
            }
        }
        private void RefreshNodeText()
        {
            for (int i = 0; i < this.treeView.Nodes.Count; i++)
            {
                TreeNode firstNode = this.treeView.Nodes[i];
                SetText(firstNode);
                for (int j = 0; j < firstNode.Nodes.Count; j++)
                {
                    TreeNode secNode = firstNode.Nodes[j];
                    SetText(secNode);
                    for (int k = 0; k < secNode.Nodes.Count; k++)
                    {
                        TreeNode thirdNode = secNode.Nodes[j];
                        SetText(thirdNode);

                    }
                }
            }
        }
        private void SetText(TreeNode tn)
        {
            if (tn is IProjectNode node)
            {
                node.RefreshText();
            }
        }
        public void PasteView()
        {
            try
            {
                if (this.treeView.SelectedNode is FlowProjectNode)
                {
                    FlowGraphAbstract data = null;
                    IDataObject dataObject = Clipboard.GetDataObject();
                    if (dataObject.GetDataPresent("Scada.FlowGraphEngine.GraphicsMap.GraphSite.Copy"))
                    {
                        data = dataObject.GetData("Scada.FlowGraphEngine.GraphicsMap.GraphSite.Copy") as FlowGraphAbstract;
                    }
                    if (data != null)
                    {
                        FlowProjectNode selectedNode = this.treeView.SelectedNode as FlowProjectNode;
                        CreateViewDialog dialog = new CreateViewDialog();
                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            data.GID = "V_" + GUIDToNormalID.GuidToLongID();
                            SCADAViewNode node = new SCADAViewNode
                            {
                                Text = dialog.ViewName,
                                ContextMenuStrip = this.contextMenuView,
                                View = (WorkForm)this.mediator.CreateWorkForm(dialog.ViewName, (float)dialog.PageWidth, (float)dialog.PageHeight)
                            };
                            node.View.GraphControl.Abstract = data;
                            node.View.GraphControl.Abstract.Site = node.View.GraphControl;
                            node.View.GraphControl.BasicLayer = data.Layers[0];
                            node.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                            {
                                if (res)
                                {
                                    this.LoadTreeViewTemplate();
                                }
                                else
                                {
                                    MessageBox.Show(this, msg);
                                }
                            };
                            selectedNode.Nodes.Add(node);
                            selectedNode.Project.GraphList.Add(node.View.GraphControl.Abstract);
                           
                        }
                        Clipboard.Clear();
                    }
                }
                else if (this.treeView.SelectedNode is FlowProjectGroupNode)
                {
                    FlowGraphAbstract data = null;
                    IDataObject dataObject = Clipboard.GetDataObject();
                    if (dataObject.GetDataPresent("Scada.FlowGraphEngine.GraphicsMap.GraphSite.Copy"))
                    {
                        data = dataObject.GetData("Scada.FlowGraphEngine.GraphicsMap.GraphSite.Copy") as FlowGraphAbstract;
                    }
                    if (data != null)
                    {
                        FlowProjectGroupNode selectedNode = this.treeView.SelectedNode as FlowProjectGroupNode;
                        CreateViewDialog dialog = new CreateViewDialog();
                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            data.GID = "V_" + GUIDToNormalID.GuidToLongID();
                            SCADAViewNode node = new SCADAViewNode
                            {
                                Text = dialog.ViewName,
                                ContextMenuStrip = this.contextMenuView,
                                View = (WorkForm)this.mediator.CreateWorkForm(dialog.ViewName, (float)dialog.PageWidth, (float)dialog.PageHeight)
                            };
                            node.View.GraphControl.Abstract = data;
                            node.View.GraphControl.Abstract.Site = node.View.GraphControl;
                            node.View.GraphControl.BasicLayer = data.Layers[0];
                            node.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                            {
                                if (res)
                                {
                                    this.LoadTreeViewTemplate();
                                }
                                else
                                {
                                    MessageBox.Show(this, msg);
                                }
                            };
                            selectedNode.Nodes.Add(node);
                            selectedNode.Project.GraphList.Add(node.View.GraphControl.Abstract);
                            selectedNode.Project.AddViewGroup(selectedNode.Group);
                            
                        }
                        Clipboard.Clear();
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void CopyView()
        {
            try
            {
                if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    Clipboard.Clear();
                    SCADAViewNode selectedNode = this.treeView.SelectedNode as SCADAViewNode;
                    Clipboard.SetDataObject(new DataObject("Scada.FlowGraphEngine.GraphicsMap.GraphSite.Copy", selectedNode.GraphSite));
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void OpenAllView()
        {
            try
            {
                if (this.treeView.SelectedNode is FlowProjectNode)
                {
                    foreach (FlowGraphAbstract @abstract in (this.treeView.SelectedNode as FlowProjectNode).Project.GraphList)
                    {
                        for (int i = 0; i < this.mediator.DockPanel.Documents.Count<DockContent>(); i++)
                        {
                            if ((this.mediator.DockPanel.Documents[i] is WorkForm) && ((this.mediator.DockPanel.Documents[i] as WorkForm).GraphControl.Abstract == @abstract))
                            {
                                this.mediator.DockPanel.Documents[i].Show();
                            }
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void ClosedAllView()
        {
            try
            {
                if (this.treeView.SelectedNode is FlowProjectNode)
                {
                    foreach (FlowGraphAbstract @abstract in (this.treeView.SelectedNode as FlowProjectNode).Project.GraphList)
                    {
                        for (int i = 0; i < this.mediator.DockPanel.Documents.Count<DockContent>(); i++)
                        {
                            if ((this.mediator.DockPanel.Documents[i] is WorkForm) && ((this.mediator.DockPanel.Documents[i] as WorkForm).GraphControl.Abstract == @abstract))
                            {
                                this.mediator.DockPanel.Documents[i].Hide();
                            }
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void DeleteDataSource()
        {
            try
            {
                if ((this.treeViewConnections.SelectedNode is ScadaConnectionNode) && (MessageBox.Show(this, "是否要删除选中的数据源?", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.Yes))
                {
                    ScadaConnectionNode selectedNode = this.treeViewConnections.SelectedNode as ScadaConnectionNode;
                    FlowProjectNode parent = this.treeViewConnections.SelectedNode.Parent as FlowProjectNode;
                    parent.Nodes.Remove(selectedNode);
                    parent.Project.ScadaConnections.Remove(selectedNode.ScadaConnection);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void EditDataSource()
        {
            try
            {
                if (this.treeViewConnections.SelectedNode is ScadaConnectionNode)
                {
                    FlowProjectNode parent = this.treeViewConnections.SelectedNode.Parent as FlowProjectNode;
                    ScadaConnectionNode selectedNode = this.treeViewConnections.SelectedNode as ScadaConnectionNode;
                    if (selectedNode.ScadaConnection.DataBaseType == DataBaseType.SQLServer)
                    {
                        SqlServerConnectionFrm frm = new SqlServerConnectionFrm
                        {
                            Connection = (SqlServer_Connection)selectedNode.ScadaConnection
                        };
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {

                            selectedNode.ScadaConnection = frm.Connection;
                            selectedNode.Text = frm.Connection.ToString();
                        }
                    }
                    else if (selectedNode.ScadaConnection.DataBaseType == DataBaseType.Oracle)
                    {
                        OracleConnectionFrm frm2 = new OracleConnectionFrm
                        {
                            Connection = (Oracle_Connection)selectedNode.ScadaConnection
                        };
                        if (frm2.ShowDialog(this) == DialogResult.OK)
                        {
                            selectedNode.ScadaConnection = frm2.Connection;
                            selectedNode.Text = frm2.Connection.ToString();
                        }
                    }
                    else if (selectedNode.ScadaConnection.DataBaseType == DataBaseType.MySQL)
                    {
                        MySqlConnectionFrm frm3 = new MySqlConnectionFrm
                        {
                            Connection = (MySQL_Connection)selectedNode.ScadaConnection
                        };
                        if (frm3.ShowDialog(this) == DialogResult.OK)
                        {
                            selectedNode.ScadaConnection = frm3.Connection;
                            selectedNode.Text = frm3.Connection.ToString();
                        }
                    }
                    else if (selectedNode.ScadaConnection.DataBaseType == DataBaseType.SQLite)
                    {
                        SQLiteConnectionFrm frm4 = new SQLiteConnectionFrm
                        {
                            Connection = (SQLite_Connection)selectedNode.ScadaConnection
                        };
                        if (frm4.ShowDialog(this) == DialogResult.OK)
                        {
                            selectedNode.ScadaConnection = frm4.Connection;
                            selectedNode.Text = frm4.Connection.ToString();
                        }
                    }
                    else if (selectedNode.ScadaConnection.DataBaseType == DataBaseType.SyBase)
                    {
                        SyBaseConnectionFrm frm5 = new SyBaseConnectionFrm
                        {
                            Connection = (SyBase_Connection)selectedNode.ScadaConnection
                        };
                        if (frm5.ShowDialog(this) == DialogResult.OK)
                        {
                            selectedNode.ScadaConnection = frm5.Connection;
                            selectedNode.Text = frm5.Connection.ToString();
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void EditRole()
        {
            try
            {
                if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    SCADAViewNode selectedNode = this.treeView.SelectedNode as SCADAViewNode;
                    FlowUserPickerEditor editor = new FlowUserPickerEditor();
                    editor.InitUsers(FlowServiceGlobel.Users, selectedNode.View.GraphControl.Abstract.RoleUsers);
                    if (editor.ShowDialog(this) == DialogResult.OK)
                    {
                        selectedNode.View.GraphControl.Abstract.RoleUsers = editor.GetCheckUsers();
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void ViewPropeitiesToOther()
        {
            try
            {
                if ((this.treeView.SelectedNode is SCADAViewNode) && (FrmDialog.ShowDialog(this, "是否应用到其它视图?", "提示", true, false, false, true, null) == DialogResult.OK))
                {
                    SCADAViewNode selectedNode = this.treeView.SelectedNode as SCADAViewNode;
                    if (this.treeView.SelectedNode.Parent is FlowProjectNode parent)
                    {


                        for (int i = 0; i < parent.Project.GraphList.Count; i++)
                        {
                            if (selectedNode.GraphSite.backImage != null)
                            {
                                parent.Project.GraphList[i].backImage = (Image)selectedNode.GraphSite.backImage.Clone();
                            }
                            parent.Project.GraphList[i].mBackgroundColor = selectedNode.GraphSite.mBackgroundColor;

                            parent.Project.GraphList[i].mBackgroundType = selectedNode.GraphSite.mBackgroundType;
                            parent.Project.GraphList[i].mGradientBottom = selectedNode.GraphSite.mGradientBottom;
                            parent.Project.GraphList[i].mGradientMode = selectedNode.GraphSite.mGradientMode;
                            parent.Project.GraphList[i].mGradientTop = selectedNode.GraphSite.mGradientTop;
                            parent.Project.GraphList[i].MapHeight = selectedNode.GraphSite.MapHeight;
                            parent.Project.GraphList[i].MapWidth = selectedNode.GraphSite.MapWidth;
                            if (selectedNode.GraphSite.backImage!=null)
                            parent.Project.GraphList[i].backImage = selectedNode.GraphSite.backImage.Clone() as Image;
                        }
                    }
                    else if (this.treeView.SelectedNode.Parent is FlowProjectGroupNode groupnode)
                    {


                        for (int i = 0; i < groupnode.Project.GraphList.Count; i++)
                        {
                            if (selectedNode.GraphSite.backImage != null)
                            {
                                groupnode.Project.GraphList[i].backImage = (Image)selectedNode.GraphSite.backImage.Clone();
                            }
                            groupnode.Project.GraphList[i].mBackgroundColor = selectedNode.GraphSite.mBackgroundColor;

                            groupnode.Project.GraphList[i].mBackgroundType = selectedNode.GraphSite.mBackgroundType;
                            groupnode.Project.GraphList[i].mGradientBottom = selectedNode.GraphSite.mGradientBottom;
                            groupnode.Project.GraphList[i].mGradientMode = selectedNode.GraphSite.mGradientMode;
                            groupnode.Project.GraphList[i].mGradientTop = selectedNode.GraphSite.mGradientTop;
                            groupnode.Project.GraphList[i].MapHeight = selectedNode.GraphSite.MapHeight;
                            groupnode.Project.GraphList[i].MapWidth = selectedNode.GraphSite.MapWidth;
                            if (selectedNode.GraphSite.backImage != null)
                                groupnode.Project.GraphList[i].backImage = selectedNode.GraphSite.backImage.Clone() as Image;
                        }
                    }


                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void ViewToTemplate()
        {
            try
            {
                if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    SCADAViewNode selectedNode = this.treeView.SelectedNode as SCADAViewNode;
                    SaveViewTemplateFrm frm = new SaveViewTemplateFrm
                    {
                        TemplateName = selectedNode.GraphSite.ViewTitle
                    };
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!Directory.Exists(Application.StartupPath + "/ScadaTemplate/TemplateViews/" + frm.TemplateClassic))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "/ScadaTemplate/TemplateViews/" + frm.TemplateClassic);
                        }
                        string[] files = Directory.GetFiles(Application.StartupPath + "/ScadaTemplate/TemplateViews/" + frm.TemplateClassic, "*.vtpl");
                        bool flag = false;
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (Path.GetFileNameWithoutExtension(files[i]).Trim().ToLower() == frm.TemplateName.Trim().ToLower())
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            MessageBox.Show(this, "已经存在此名称的模板,请重新命名");
                        }
                        else
                        {
                            string[] textArray1 = new string[] { Application.StartupPath, "/ScadaTemplate/TemplateViews/", frm.TemplateClassic, "/", frm.TemplateName, ".vtpl" };
                            ((FlowGraphControl)selectedNode.GraphSite.Site).SaveView(string.Concat(textArray1), frm.TemplateName);
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void SetProjectIndex()
        {
            try
            {
                if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    FlowProjectNode parent = this.treeView.SelectedNode.Parent as FlowProjectNode;
                    for (int i = 0; i < parent.Project.GraphList.Count; i++)
                    {
                        parent.Project.GraphList[i].Index = false;
                    }
                    SCADAViewNode selectedNode = this.treeView.SelectedNode as SCADAViewNode;
                    selectedNode.GraphSite.Index = true;
                    selectedNode.ForeColor = Color.Red;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        public void CreateViewGroup()
        {
            if (this.treeView.SelectedNode is FlowProjectNode projectNode)
            {

                ViewGroupEdit viewGroup = new ViewGroupEdit(null);
                if (viewGroup.ShowDialog(this) == DialogResult.OK)
                {
                    if (projectNode.Project.AddViewGroup(viewGroup.ViewGroup))
                    {
                        FlowProjectGroupNode groupNode = new FlowProjectGroupNode(viewGroup.ViewGroup);
                        groupNode.Project = projectNode.Project;
                        groupNode.ContextMenuStrip = this.contextMenuStripViewGroup;
                        projectNode.Nodes.Add(groupNode);
                    }
                    RefreshNodeText();
                }

            }
        }
        public void EditProjectUserAndPassword()
        {
            try
            {
                if (this.treeView.SelectedNode is FlowProjectNode)
                {
                    FlowProjectNode selectedNode = this.treeView.SelectedNode as FlowProjectNode;
                    ProjectUpdatePasswordDialog dialog = new ProjectUpdatePasswordDialog(selectedNode.Project);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        selectedNode.Project.Password = dialog.Password;
                        selectedNode.Project.Title = dialog.ProjectTitle;
                        selectedNode.Text = dialog.ProjectTitle;
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void DeleteViewGroup()
        {
            if (this.treeView.SelectedNode is FlowProjectGroupNode projectNode)
            {
                if (MessageBox.Show(this, "是否要删除分组", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {


                    for (int i = 0; i < projectNode.Nodes.Count; i++)
                    {
                        projectNode.Parent.Nodes.Add(projectNode.Nodes[i]);
                    }
                    projectNode.Parent.Nodes.Remove(projectNode);
                    projectNode.Project.DeleteViewGroup(projectNode.Project, projectNode.Group);
                }



            }
        }
        public void EditViewGroup()
        {
            if (this.treeView.SelectedNode is FlowProjectGroupNode projectNode)
            {

                ViewGroupEdit viewGroup = new ViewGroupEdit(projectNode.Group);

                if (viewGroup.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshNodeText();
                }

            }
        }

        public void AddSqlServerSource()
        {
            try
            {
                if (this.treeViewConnections.SelectedNode is FlowProjectNode)
                {
                    FlowProjectNode selectedNode = this.treeViewConnections.SelectedNode as FlowProjectNode;
                    SqlServerConnectionFrm frm = new SqlServerConnectionFrm();
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        ScadaConnectionNode node = new ScadaConnectionNode(frm.Connection)
                        {
                            ContextMenuStrip = this.contextMenuConnectionDelete
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Project.ScadaConnections.Add(frm.Connection);
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void AddOracleSource()
        {
            try
            {
                if (this.treeViewConnections.SelectedNode is FlowProjectNode)
                {
                    FlowProjectNode selectedNode = this.treeViewConnections.SelectedNode as FlowProjectNode;
                    OracleConnectionFrm frm = new OracleConnectionFrm();
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        ScadaConnectionNode node = new ScadaConnectionNode(frm.Connection)
                        {
                            ContextMenuStrip = this.contextMenuConnectionDelete
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Project.ScadaConnections.Add(frm.Connection);
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void AddMySqlSource()
        {
            try
            {
                if (this.treeViewConnections.SelectedNode is FlowProjectNode)
                {
                    FlowProjectNode selectedNode = this.treeViewConnections.SelectedNode as FlowProjectNode;
                    MySqlConnectionFrm frm = new MySqlConnectionFrm();
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        ScadaConnectionNode node = new ScadaConnectionNode(frm.Connection)
                        {
                            ContextMenuStrip = this.contextMenuConnectionDelete
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Project.ScadaConnections.Add(frm.Connection);
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void AddSyBaseSource()
        {
            try
            {
                if (this.treeViewConnections.SelectedNode is FlowProjectNode)
                {
                    FlowProjectNode selectedNode = this.treeViewConnections.SelectedNode as FlowProjectNode;
                    SyBaseConnectionFrm frm = new SyBaseConnectionFrm();
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        ScadaConnectionNode node = new ScadaConnectionNode(frm.Connection)
                        {
                            ContextMenuStrip = this.contextMenuConnectionDelete
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Project.ScadaConnections.Add(frm.Connection);
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        public void AddSqllitSource()
        {
            try
            {
                if (this.treeViewConnections.SelectedNode is FlowProjectNode)
                {
                    FlowProjectNode selectedNode = this.treeViewConnections.SelectedNode as FlowProjectNode;
                    SQLiteConnectionFrm frm = new SQLiteConnectionFrm();
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        ScadaConnectionNode node = new ScadaConnectionNode(frm.Connection)
                        {
                            ContextMenuStrip = this.contextMenuConnectionDelete
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Project.ScadaConnections.Add(frm.Connection);
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        public void LoadTreeViewTemplate()
        {
            try
            {
                this.treeViewTemplate.Nodes[0].Nodes.Clear();
                this.treeViewTemplate.Nodes[0].Tag = null;
                this.treeViewTemplate.Nodes[0].ExpandAll();
                string[] directories = Directory.GetDirectories(Application.StartupPath + "/ScadaTemplate/TemplateViews/");
                for (int i = 0; i < directories.Length; i++)
                {
                    TreeNode node = new TreeNode
                    {
                        Text = Path.GetFileName(directories[i]),
                        Tag = null,
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        StateImageIndex = 1
                    };
                    string[] files = Directory.GetFiles(directories[i], "*.vtpl");
                    for (int j = 0; j < files.Length; j++)
                    {
                        TreeNode node2 = new TreeNode
                        {
                            Text = Path.GetFileNameWithoutExtension(files[j]),
                            Tag = files[j],
                            ImageIndex = 2,
                            SelectedImageIndex = 2,
                            StateImageIndex = 2
                        };
                        node.Nodes.Add(node2);
                    }
                    this.treeViewTemplate.Nodes[0].Nodes.Add(node);
                    node.ExpandAll();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message);
            }
        }

        public void Publish()
        {
            try
            {
                if (this.treeView.SelectedNode is FlowProjectNode)
                {
                    IOFlowManager.PublishFlowStart((this.treeView.SelectedNode as FlowProjectNode).Project);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        public void SaveAsProject()
        {
            if (this.treeView.SelectedNode is FlowProjectNode)
            {
                IOFlowManager.SaveAsProject((this.treeView.SelectedNode as FlowProjectNode).Project);
            }
        }

        public void SaveProject()
        {
            if (this.treeView.SelectedNode is FlowProjectNode)
            {
                IOFlowManager.SaveProject((this.treeView.SelectedNode as FlowProjectNode).Project);
            }
        }
        public void AddFlowUser()
        {
            if (this.treeViewUser.SelectedNode == null)
                return;
            IOFlowProject project = null;
            if ((this.treeViewUser.SelectedNode.Tag != null) && (this.treeViewUser.SelectedNode.Tag.GetType() == typeof(IOFlowProject)))
            {
                project = (IOFlowProject)this.treeViewUser.SelectedNode.Tag;
            }
            else if (this.treeViewUser.SelectedNode.Tag.GetType() == typeof(ScadaFlowUser))
            {
                project =  (IOFlowProject)this.treeViewUser.SelectedNode.Parent.Tag;
            }
                if ((this.treeViewUser.SelectedNode.Tag != null) && (this.treeViewUser.SelectedNode.Tag.GetType() == typeof(IOFlowProject)))
            {
                FlowUserManagerForm form = new FlowUserManagerForm();
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    IOFlowProject tag = (IOFlowProject)this.treeViewUser.SelectedNode.Tag;
                    for (int i = 0; i < tag.FlowUsers.Count; i++)
                    {
                        if (tag.FlowUsers[i].UserName.Trim() == form.EditUser.UserName.Trim())
                        {
                            MessageBox.Show(this, "已经存在此用户名", "提示");
                            return;
                        }
                    }
                    tag.FlowUsers.Add(form.EditUser);
                    TreeNode node = new TreeNode
                    {
                        Text = form.EditUser.ToString(),
                        Tag = form.EditUser,
                        ImageIndex = 4,
                        StateImageIndex = 4,
                        SelectedImageIndex=4
                    };
                    this.treeViewUser.SelectedNode.Nodes.Add(node);
                }
            }
        }

        public void CreateView()
        {
            if ((this.treeView.SelectedNode != null) && (this.treeView.SelectedNode.GetType() == typeof(FlowProjectNode) || this.treeView.SelectedNode.GetType() == typeof(FlowProjectGroupNode)))
            {
                if (this.treeView.SelectedNode.GetType() == typeof(FlowProjectNode))
                {
                    FlowProjectNode selectedNode = this.treeView.SelectedNode as FlowProjectNode;
                    CreateViewDialog dialog = new CreateViewDialog();
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        SCADAViewNode node = new SCADAViewNode
                        {
                            Text = dialog.ViewName,
                            ContextMenuStrip = this.contextMenuView,
                            View = (WorkForm)this.mediator.CreateWorkForm(dialog.ViewName, (float)dialog.PageWidth, (float)dialog.PageHeight)
                        };
                        node.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                        {
                            if (res)
                            {
                                this.LoadTreeViewTemplate();
                            }
                            else
                            {
                                MessageBox.Show(this, msg);
                            }
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Project.GraphList.Add(node.View.GraphControl.Abstract);
                    
                    }
                }
                else
                {
                    FlowProjectGroupNode selectedNode = this.treeView.SelectedNode as FlowProjectGroupNode;
                    CreateViewDialog dialog = new CreateViewDialog();
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        SCADAViewNode node = new SCADAViewNode
                        {
                            Text = dialog.ViewName,
                            ContextMenuStrip = this.contextMenuView,
                            View = (WorkForm)this.mediator.CreateWorkForm(dialog.ViewName, (float)dialog.PageWidth, (float)dialog.PageHeight)
                        };

                        node.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                        {
                            if (res)
                            {
                                this.LoadTreeViewTemplate();
                            }
                            else
                            {
                                MessageBox.Show(this, msg);
                            }
                        };
                        selectedNode.Nodes.Add(node);
                        selectedNode.Group.Views.Add(node.View.GraphControl.Abstract.GID);
                        selectedNode.Project.GraphList.Add(node.View.GraphControl.Abstract);
                       
                    }
                }

            }
        }
      

        public void Debug()
        {
            try
            {
                if (this.treeView.SelectedNode is FlowProjectNode projectNode)
                {
                    new DebugForm(projectNode.Project, null).ShowDialog(this.mediator.Parent);
                }
                else if (this.treeView.SelectedNode is SCADAViewNode viewNode)
                {
                    if (this.treeView.SelectedNode.Parent is FlowProjectNode projNode)
                    {
                        new DebugForm(projNode.Project, viewNode.View.GraphControl.Abstract).ShowDialog(this.mediator.Parent);
                    }
                    else if (this.treeView.SelectedNode.Parent is FlowProjectGroupNode groupNode)
                    {
                        new DebugForm(groupNode.Project, viewNode.View.GraphControl.Abstract).ShowDialog(this.mediator.Parent);
                    }

                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        public void DeleteFlowUser()
        {
            if (this.treeViewUser.SelectedNode == null)
                return;
            if (((this.treeViewUser.SelectedNode.Tag != null) && (this.treeViewUser.SelectedNode.Tag.GetType() == typeof(ScadaFlowUser))) && (MessageBox.Show(this, "是否要删除用户", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK))
            {
                ((IOFlowProject)this.treeViewUser.SelectedNode.Parent.Tag).FlowUsers.Remove((ScadaFlowUser)this.treeViewUser.SelectedNode.Tag);
                this.treeViewUser.Nodes.Remove(this.treeViewUser.SelectedNode);
            }
        }

        public void DeleteProject()
        {
            if (this.treeView.SelectedNode is FlowProjectNode)
            {
                FlowProjectNode selectedNode = this.treeView.SelectedNode as FlowProjectNode;


                if (MessageBox.Show(this, "是否要删除工程吗？", "删除工程提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.treeView.Nodes.Remove(this.treeView.SelectedNode);
                    foreach (FlowGraphAbstract @abstract in selectedNode.Project.GraphList)
                    {
                        for (int k = 0; k < this.mediator.DockPanel.Documents.Count<DockContent>(); k++)
                        {
                            if ((this.mediator.DockPanel.Documents[k] is WorkForm) && ((this.mediator.DockPanel.Documents[k] as WorkForm).GraphControl.Abstract == @abstract))
                            {
                                this.mediator.DockPanel.Documents[k].Hide();
                                this.mediator.DockPanel.Documents[k].Dispose();
                            }
                        }
                    }
                    for (int i = 0; i < this.treeViewUser.Nodes.Count; i++)
                    {
                        if ((this.treeViewUser.Nodes[i].Tag != null) && (((IOFlowProject)this.treeViewUser.Nodes[i].Tag) == selectedNode.Project))
                        {
                            this.treeViewUser.Nodes.Remove(this.treeViewUser.Nodes[i]);
                        }
                    }
                    for (int j = 0; j < this.treeViewConnections.Nodes.Count; j++)
                    {
                        if ((this.treeViewConnections.Nodes[j].Tag != null) && (((IOFlowProject)this.treeViewConnections.Nodes[j].Tag) == selectedNode.Project))
                        {
                            this.treeViewConnections.Nodes.Remove(this.treeViewConnections.Nodes[j]);
                        }
                    }
                    IOFlowManager.Projects.Remove(selectedNode.Project);
                }
            }
        }

        public void DeleteView()
        {
            if ((this.treeView.SelectedNode is SCADAViewNode) && (MessageBox.Show("是否要删除" + this.treeView.SelectedNode.Text + "视图?", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                try
                {
                    SCADAViewNode selectedNode = (SCADAViewNode)this.treeView.SelectedNode;
                    if (this.treeView.SelectedNode.Parent is FlowProjectNode)
                    {
                        FlowProjectNode parent = (FlowProjectNode)this.treeView.SelectedNode.Parent;
                        parent.Project.GraphList.Remove(selectedNode.GraphSite);
                        parent.Nodes.Remove(selectedNode);
                        for (int i = 0; i < this.mediator.DockPanel.Documents.Count<DockContent>(); i++)
                        {
                            if ((this.mediator.DockPanel.Documents[i] is WorkForm) && ((this.mediator.DockPanel.Documents[i] as WorkForm) == selectedNode.View))
                            {
                                this.mediator.DockPanel.Documents[i].Hide();
                                this.mediator.DockPanel.Documents[i].Dispose();

                            }
                        }
                        IOFlowManager.AddLogToMainLog("删除视图成功!" + parent.Text + "//" + selectedNode.Text);
                    }
                    else if (this.treeView.SelectedNode.Parent is FlowProjectGroupNode)
                    {
                        FlowProjectGroupNode parent = (FlowProjectGroupNode)this.treeView.SelectedNode.Parent;
                        parent.Project.GraphList.Remove(selectedNode.GraphSite);
                        parent.Nodes.Remove(selectedNode);
                        for (int i = 0; i < this.mediator.DockPanel.Documents.Count<DockContent>(); i++)
                        {
                            if ((this.mediator.DockPanel.Documents[i] is WorkForm) && ((this.mediator.DockPanel.Documents[i] as WorkForm) == selectedNode.View))
                            {
                                this.mediator.DockPanel.Documents[i].Hide();
                                this.mediator.DockPanel.Documents[i].Dispose();

                            }
                        }
                        IOFlowManager.AddLogToMainLog("删除视图成功!" + parent.Text + "//" + selectedNode.Text);

                    }

                }
                catch (Exception exception)
                {
                    IOFlowManager.AddLogToMainLog("删除视图失败!" + exception.InnerException);
                }
            }
        }



        public void EditFlowUser()
        {
            if (this.treeViewUser.SelectedNode == null)
                return;
            if ((this.treeViewUser.SelectedNode.Tag != null) && (this.treeViewUser.SelectedNode.Tag.GetType() == typeof(ScadaFlowUser)))
            {
                FlowUserManagerForm form = new FlowUserManagerForm
                {
                    EditUser = (ScadaFlowUser)this.treeViewUser.SelectedNode.Tag
                };
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    IOFlowProject tag = (IOFlowProject)this.treeViewUser.SelectedNode.Parent.Tag;
                    for (int i = 0; i < tag.FlowUsers.Count; i++)
                    {
                        if ((tag.FlowUsers[i].UserName.Trim() == form.EditUser.UserName.Trim()) && (tag.FlowUsers[i] != form.EditUser))
                        {
                            MessageBox.Show(this, "已经存在此用户名", "提示");
                            return;
                        }
                    }
                    this.treeViewUser.SelectedNode.Text = form.EditUser.ToString();
                }
            }
        }
        public void EditViewName()
        {
            try
            {
                if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    SCADAViewNode selectedNode = this.treeView.SelectedNode as SCADAViewNode;
                    EditViewDialog dialog = new EditViewDialog(selectedNode.View);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        selectedNode.Text = dialog.ViewName;
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
        public void OpenView()
        {
            try
            {

                if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    IOFlowProject project = null;
                    if (this.treeView.SelectedNode.Parent is FlowProjectNode projectNode)
                    {
                        project = projectNode.Project;
                    }
                    else if (this.treeView.SelectedNode.Parent is FlowProjectGroupNode projectGroupNode)
                    {
                        project = projectGroupNode.Project;
                    }
                    SCADAViewNode node = this.treeView.SelectedNode as SCADAViewNode;
                    if (node.View != null)
                    {
                        node.View.Activate();
                    }
                    else
                    {
                        node.View = (WorkForm)this.mediator.CreateWorkForm(node.GraphSite.GraphInformation.Title, node.GraphSite.MapWidth, node.GraphSite.MapHeight, node.GraphSite);
                    }

                    for (int j = 0; j < node.View.GraphControl.Shapes.Count; j++)
                    {
                        node.View.GraphControl.Shapes[j].Layer = node.View.GraphControl.BasicLayer;
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }



        public void InitTreeConnections(IOFlowProject project)
        {
            FlowProjectNode node = new FlowProjectNode(project)
            {
                Text = project.Title,
                Tag = project,
                ImageIndex = 0,
                StateImageIndex = 0,
                ContextMenuStrip = this.contextMenuConnection
            };
            for (int i = 0; i < project.ScadaConnections.Count; i++)
            {
                ScadaConnectionNode node2 = new ScadaConnectionNode(project.ScadaConnections[i])
                {
                    ContextMenuStrip = this.contextMenuConnectionDelete
                };
                node.Nodes.Add(node2);
            }
            this.treeViewConnections.Nodes.Add(node);
        }

        public void InitTreeUser(IOFlowProject project)
        {
            TreeNode node = new TreeNode
            {
                Text = project.Title,
                Tag = project,
                ImageIndex = 3,
                StateImageIndex = 3
            };
            for (int i = 0; i < project.FlowUsers.Count; i++)
            {
                TreeNode node2 = new TreeNode
                {
                    Text = project.FlowUsers[i].ToString(),
                    Tag = project.FlowUsers[i],
                    ImageIndex = 4,
                    StateImageIndex = 4
                };
                node.Nodes.Add(node2);
            }
            this.treeViewUser.Nodes.Add(node);
        }

        public void InitTreeView(IOFlowProject project)
        {
            TreeNodeEx ex = new TreeNodeEx(TreeNodeType.View);
            if (this.treeView.Nodes.Count <= 0)
            {
                ex.Text = "LazyOS流程图";
                this.treeView.Nodes.Add(ex);
            }
            else
            {
                ex = (TreeNodeEx)this.treeView.Nodes[0];
            }
            FlowProjectNode node = new FlowProjectNode(project)
            {
                ToolTipText = project.FileFullName,
                ContextMenuStrip = this.contextMenuProject
            };
            //首先创建分组
            var gourps = project.GetViewGroups();
            for (int i = 0; i < gourps.Count; i++)
            {
                FlowProjectGroupNode groupNode = new FlowProjectGroupNode(gourps[i]);
                groupNode.ContextMenuStrip = this.contextMenuStripViewGroup;
                groupNode.Project = project;
                groupNode.Group = gourps[i];
                groupNode.RefreshText();
                node.Nodes.Add(groupNode);
                for (int j = 0; j < project.GraphList.Count; j++)
                {
                    if (gourps[i].Views.Exists(x => x == project.GraphList[j].GID))
                    {
                        SCADAViewNode node2 = new SCADAViewNode
                        {
                            ContextMenuStrip = this.contextMenuView,
                            View = (WorkForm)this.mediator.CreateWorkForm(project.GraphList[j].ViewTitle, project.GraphList[j].MapWidth, project.GraphList[j].MapHeight, project.GraphList[j])
                        };
                        node2.View.GraphControl.Abstract = project.GraphList[j];
                        node2.View.GraphControl.Abstract.Site = node2.View.GraphControl;
                        node2.View.GraphControl.BasicLayer = project.GraphList[j].Layers[0];
                        for (int p = 0; p < project.GraphList[i].Shapes.Count; p++)
                        {
                            project.GraphList[i].Shapes[p].Layer = node2.View.GraphControl.BasicLayer;
                        }
                        node2.Text = node2.GraphSite.ViewTitle;
                        node2.View.GraphControl.LoadPropertiesEvent();
                        node2.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                        {
                            if (res)
                            {
                                this.LoadTreeViewTemplate();
                            }
                            else
                            {
                                MessageBox.Show(this, msg);
                            }
                        };
                        
                        groupNode.Nodes.Add(node2);

                    }
                }
            }
            //
            for (int i = 0; i < project.GraphList.Count; i++)
            {
                if (project.ExistViewInGroup(project.GraphList[i].GID))
                    continue;
                SCADAViewNode node2 = new SCADAViewNode
                {
                    ContextMenuStrip = this.contextMenuView,
                    View = (WorkForm)this.mediator.CreateWorkForm(project.GraphList[i].ViewTitle, project.GraphList[i].MapWidth, project.GraphList[i].MapHeight, project.GraphList[i])
                };
                node2.View.GraphControl.Abstract = project.GraphList[i];
                node2.View.GraphControl.Abstract.Site = node2.View.GraphControl;
                node2.View.GraphControl.BasicLayer = project.GraphList[i].Layers[0];
                for (int j = 0; j < project.GraphList[i].Shapes.Count; j++)
                {
                    project.GraphList[i].Shapes[j].Layer = node2.View.GraphControl.BasicLayer;
                }
                node2.Text = node2.GraphSite.ViewTitle;
                node2.View.GraphControl.LoadPropertiesEvent();
                node2.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                {
                    if (res)
                    {
                        this.LoadTreeViewTemplate();
                    }
                    else
                    {
                        MessageBox.Show(this, msg);
                    }
                };
                
                node.Nodes.Add(node2);

            }
            node.ExpandAll();
            ex.Nodes.Add(node);
        }

        public void LoadTempViewToProject(string tempFile)
        {
            FlowProjectNode selectedNode = null;
            if (this.treeView.SelectedNode != null)
            {
                if (this.treeView.SelectedNode is FlowProjectNode)
                {
                    selectedNode = this.treeView.SelectedNode as FlowProjectNode;
                }
                else if (this.treeView.SelectedNode is SCADAViewNode)
                {
                    selectedNode = this.treeView.SelectedNode.Parent as FlowProjectNode;
                }
                CreateViewDialog dialog = new CreateViewDialog();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SCADAViewNode node2 = new SCADAViewNode
                    {
                        Text = dialog.ViewName,
                        ContextMenuStrip = this.contextMenuView,
                        View = (WorkForm)this.mediator.CreateWorkForm(dialog.ViewName, (float)dialog.PageWidth, (float)dialog.PageHeight)
                    };
                    FlowGraphAbstract @abstract = node2.View.GraphControl.LoadView(tempFile);
                    node2.View.GraphControl.Abstract.MapHeight = @abstract.MapHeight;
                    node2.View.GraphControl.Abstract.MapWidth = @abstract.MapWidth;
                    node2.View.GraphControl.Abstract.ViewTitle = dialog.ViewName;
                    node2.View.GraphControl.Layers.Clear();
                    node2.View.GraphControl.AddLayer(@abstract.Layers[0]);
                    node2.View.GraphControl.BasicLayer = @abstract.Layers[0];
                    for (int i = 0; i < @abstract.Shapes.Count; i++)
                    {
                        node2.View.GraphControl.AddShape(@abstract.Shapes[i], AddShapeType.Create, null, -1);
                    }
                    node2.View.GraphControl.SaveViewResult = delegate (bool res, string msg)
                    {
                        if (res)
                        {
                            this.LoadTreeViewTemplate();
                        }
                        else
                        {
                            MessageBox.Show(this, msg);
                        }
                    };
                    node2.View.GraphControl.LoadViewResult = (res, msg) => MessageBox.Show(this, msg);
                    selectedNode.Nodes.Add(node2);
                    selectedNode.Project.GraphList.Add(node2.View.GraphControl.Abstract);
                }
            }
        }
        #endregion

    }
}
