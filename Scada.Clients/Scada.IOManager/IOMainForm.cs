using IOManager.Controls;
using IOManager.Core;
using IOManager.Dialogs;
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
using Scada.DBUtility;
using Scada.Model;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOManager
{
    public partial class IOMainForm : Form
    {
        public Mediator mediator = null;
        public IOMainForm()
        {
            InitializeComponent();

            mediator = new Mediator(this);
            mediator.DockPanel = dockPanel;
            mediator.parent = this;
            this.WindowState = FormWindowState.Maximized;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += IOMainForm_FormClosing;


            IOManagerUIManager.MainForm = this;
        }

      

        private void 加载工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOManagerUIManager.LoadProject();
        }

        private void 新建工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOManagerUIManager.CreateProject();
        }

        private void 保存工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            if (MessageBox.Show(this, "是否要保存采集站工程?", "保存提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IOManagerUIManager.SaveProject();
            }

        }



        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
           

        }

        private void 驱动管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenIODriveManageForm();

        }

        private void 设备驱动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenIODriveManageForm();
        }

        private void 工程视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenIOTreeForm();
        }

        private void iO表视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenIOParaForm();
        }

        private void 日志视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenLogForm();
        }

        private void toolStripMenuItem另存为_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            IOManagerUIManager.SaveAsProject();
        }

        private async void 发布工程toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            await IOManagerUIManager.PublisProject();
        }

        private void 导出CSVToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void IOMainForm_Load(object sender, EventArgs e)
        {
            this.Text = PubConstant.Product;
            this.mediator.OpenLogForm();
            this.mediator.OpenIOTreeForm();
            this.mediator.OpenBatchCommandTaskFormForm();
            this.mediator.OpenIOParaForm();
            IOMonitorConfig config = new IOMonitorConfig();
            if (config.Project != null && config.Project != "")
            {
                IOManagerUIManager.LoadProject(config.Project);
            }
        }

        private void IOMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (IOManagerUIManager.Project != null && IOManagerUIManager.Project != "")
            {
                try
                {

                    if (MessageBox.Show(this, "退出前系统前请先保存当前工程，以防数据丢失！是否要保存", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        var task = IOManagerUIManager.SaveProject();
                        task.Wait();
                    }

                }
                catch
                {

                }

            }

            if (MessageBox.Show(this, "是否要关闭系统", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                IOManagerUIManager.Close();
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.删除参数ToolStripMenuItem_Click(sender, e);
        }

        private void 创建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.添加参数ToolStripMenuItem1_Click(sender, e);
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.编辑参数ToolStripMenuItem_Click(sender, e);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.toolStripMenuItem全选_Click(sender, e);
        }

        private void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.取消全选ToolStripMenuItem_Click(sender, e);
        }



        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.粘贴ToolStripMenuItem_Click(sender, e);
        }

        private void 剪贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.剪贴toolStripMenuItem_Click(sender, e);
        }

        private void 复制toolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mediator.IOParaForm.IOListView.复制参数ToolStripMenuItem_Click(sender, e);
        }

        private void 添加设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IOCommunicationNode)
            {
                IOManagerUIManager.CreateIODeviceNode();
            }

        }
        IO_DEVICE TempDevice = null;
        private void 复制设备ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            TempDevice = null;
            if (this.mediator.IOTreeForm.SelectedNode is IODeviceNode oDeviceNode)
            {
                TempDevice=IOManagerUIManager.CopyIODeviceNode(oDeviceNode.Device);
            }
            else
            {
                TempDevice = null;
            }
        }
        private void 粘贴设备ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
             if(TempDevice!=null)
            {
                if (this.mediator.IOTreeForm.SelectedNode is IODeviceNode oDeviceNode)
                {
                    IO_DEVICE newDevice = IOManagerUIManager.PasteIODeviceNode(TempDevice.Copy(), oDeviceNode.Device.IO_SERVER_ID, oDeviceNode.Device.IO_COMM_ID);
                    //创建节点
                    IODeviceNode newNode = new IODeviceNode();
                    newNode.Device = newDevice;
                    newNode.ChangedNode();
                    oDeviceNode.Parent.Nodes.Insert(oDeviceNode.Index, newNode);
                    
                }
                else if (this.mediator.IOTreeForm.SelectedNode is IOCommunicationNode ocummunicationNode)
                {
                    IO_DEVICE newDevice = IOManagerUIManager.PasteIODeviceNode(TempDevice.Copy(), ocummunicationNode.Communication.IO_SERVER_ID, ocummunicationNode.Communication.IO_COMM_ID);
                    //创建节点
                    IODeviceNode newNode = new IODeviceNode();
                    newNode.Device = newDevice;
                    newNode.ChangedNode();
                    ocummunicationNode.Nodes.Insert(0, newNode);
                }

                
            }
        }

        private void 删除设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IODeviceNode)
            {
                IODeviceNode deviceNode = this.mediator.IOTreeForm.SelectedNode as IODeviceNode;
                if (MessageBox.Show(this, "是否要删除" + deviceNode.Device.IO_DEVICE_LABLE + "设备?", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.mediator.IOTreeForm.SelectedNode.Remove();
                }
            }

        }

        private void 修改设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IODeviceNode)
            {
                IODeviceNode deviceNode = this.mediator.IOTreeForm.SelectedNode as IODeviceNode;
                IOManagerUIManager.EditIODeviceNode(deviceNode);
            }

        }

        private void 删除通道ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IOCommunicationNode)
            {
                IOCommunicationNode commNode = this.mediator.IOTreeForm.SelectedNode as IOCommunicationNode;
                if (MessageBox.Show(this, "是否要删除" + commNode.Communication.IO_COMM_LABEL + "设备?", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.mediator.IOTreeForm.SelectedNode.Remove();
                }
            }
        }

        private void 修改通道ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IOCommunicationNode)
            {
                IOCommunicationNode commNode = this.mediator.IOTreeForm.SelectedNode as IOCommunicationNode;
                IOManagerUIManager.EditIOCommunicationNode(commNode);
            }
        }

        private void 添加通道ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IOServerNode)
            {
                IOManagerUIManager.CreateIOCommunicationNode();
            }

        }

        private void 编辑点表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mediator.IOTreeForm.SelectedNode is IODeviceNode)
            {
                IODeviceNode devNode = this.mediator.IOTreeForm.SelectedNode as IODeviceNode;
                IOCommunicationNode comNode = devNode.Parent as IOCommunicationNode;
                IOServerNode sNode = comNode.Parent as IOServerNode;
                IOManagerUIManager.OpenDeviceParas(sNode.Server, comNode.Communication, devNode.Device);
            }



        }

        private void 添加任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOManagerUIManager.AddMachineTrain();
        }

        private void 删除任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOManagerUIManager.DeleteMachineTrain();
        }

        private void 编辑任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOManagerUIManager.EditMachineTrain();
        }

        private void 控制任务视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenBatchCommandTaskFormForm();
        }

        private void 驱动管理视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenIODriveManageForm();

        }

        private void 控制任务管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            this.mediator.OpenBatchCommandTaskFormForm();
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            IOManagerUIManager.AddBatchCommandTaskForm();
        }

        private void 删除任务ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 编辑任务ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IOManagerUIManager.Project == "")
                return;
            IOManagerUIManager.EditBatchCommandTaskForm();
        }
    }
}
