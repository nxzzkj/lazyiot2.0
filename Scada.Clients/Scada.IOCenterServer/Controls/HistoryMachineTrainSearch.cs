using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.Model;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaCenterServer.Controls
{
    public partial class HistoryMachineTrainSearch : UserControl
    {
        public HistoryMachineTrainSearch()
        {
            InitializeComponent();
            hsComboBoxService.IsSelectbaseNode = false;
            comboBoxMachineTrain.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        



    
        public IO_SERVER Server = null;
        public ScadaMachineTrainingModel MachineTrainingModel = null;
        public event EventHandler SearchClick;
        public event EventHandler SelectedIndexChanged = null;
        public void SetTrainSelected(ScadaMachineTrainingModel trainingModel)
        {
            MachineTrainingModel = trainingModel;
            TreeNode[] treeNodes = this.hsComboBoxService.TreeView.Nodes.Find(trainingModel.SERVER_ID, true);
            if(treeNodes.Length>0)
            {
                this.hsComboBoxService.TreeView.SelectedNode = treeNodes[0];
                this.Server = ((IoServerTreeNode)treeNodes[0]).Server;
                //加载模块下的所有学习任务

                List<ScadaMachineTrainingModel> trains = IOCenterManager.IOProject.MachineTrainingModels.FindAll(x => x.SERVER_ID == this.Server.SERVER_ID);
                trains.ForEach(delegate (ScadaMachineTrainingModel training)
                {
                    comboBoxMachineTrain.Items.Add(training);
                });
               for(int i=0;i < comboBoxMachineTrain.Items.Count;i++)
                {
                    if(((ScadaMachineTrainingModel)comboBoxMachineTrain.Items[i]).Id== trainingModel.Id)
                    {
                        comboBoxMachineTrain.SelectedIndex=i;
                        break;
                    }
                }
            }

        }
        /// <summary>
        /// 首先要加载树结构
        /// </summary>
        public async void LoadTreeProject()
        {

            if (this.hsComboBoxService.TreeView.Nodes.Count <= 0)
            {
                comboBoxMachineTrain.Items.Clear();
                await TaskHelper.Factory.StartNew(() =>
                {
                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke(new EventHandler(delegate
                        {

                            try
                            {
                                this.hsComboBoxService.TreeView.Nodes.Clear();

                                int num = IOCenterManager.IOProject.Servers.Count + IOCenterManager.IOProject.Communications.Count + IOCenterManager.IOProject.Devices.Count;
                                

                                ///加载采集站
                                for (int i = 0; i < IOCenterManager.IOProject.Servers.Count; i++)
                                {

                                    IoServerTreeNode serverNode = new IoServerTreeNode();
                                    serverNode.Server = IOCenterManager.IOProject.Servers[i];
                                    serverNode.InitNode();
                                    this.hsComboBoxService.TreeView.Nodes.Add(serverNode);

                                }
                         
                             


                            }
                            catch
                            {

                            }
                        }));
                    }
                });
              
            }
        }

        private void hsComboBoxService_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxMachineTrain.Items.Clear();
            if (hsComboBoxService.SelectedItem != null)
            {
                if (hsComboBoxService.SelectedItem is IoServerTreeNode snode)
                {
                    this.Server = snode.Server;
                    //加载模块下的所有学习任务

                    List<ScadaMachineTrainingModel> trains = IOCenterManager.IOProject.MachineTrainingModels.FindAll(x => x.SERVER_ID == this.Server.SERVER_ID);
                    trains.ForEach(delegate (ScadaMachineTrainingModel trainingModel)
                    {
                        comboBoxMachineTrain.Items.Add(trainingModel);
                    });

                    
                }

            }
        }

        private void ucBtnExt13_BtnClick(object sender, EventArgs e)
        {
            if (hsComboBoxService.SelectedItem == null)
            {
                FrmDialog.ShowDialog(this, "请选择要查询采集站!", "提示");
                return;


            }
            if (comboBoxMachineTrain.SelectedItem == null)
            {
                FrmDialog.ShowDialog(this, "请选择要查询数据任务!", "提示");
                return;


            }
            if (dateStart.Value > dateEnd.Value)
            {

                FrmDialog.ShowDialog(this, "开始时间必须小于结束时间!", "提示");
                return;
            }
            if ((dateEnd.Value - dateStart.Value).Days >= 31)
            {

                FrmDialog.ShowDialog(this, "您选择的时间段太长，时间段不能超过31天!", "提示");
                return;
            }
            
           if (hsComboBoxService.SelectedItem is IoServerTreeNode snode)
            {
                this.Server = snode.Server;
                this.MachineTrainingModel = comboBoxMachineTrain.SelectedItem as ScadaMachineTrainingModel;
                if (SearchClick != null)
                {
                    SearchClick(this.MachineTrainingModel, e);
                }
            }


          
           
        }

        public DateTime StartDate
        {
            get { return dateStart.Value; }
            set
            {
                dateStart.Value = value;
            }
        }
        public DateTime EndDate
        {
            get { return dateEnd.Value; }
            set
            {
                dateEnd.Value = value;
            }
        }
        
        private void comboBoxMachineTrain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxMachineTrain.SelectedItem!=null)
            {
                MachineTrainingModel = comboBoxMachineTrain.SelectedItem as ScadaMachineTrainingModel;
                
                if (SelectedIndexChanged != null)
                {
                    SelectedIndexChanged(sender, e);
                }
            }
          
        }
    }
}
