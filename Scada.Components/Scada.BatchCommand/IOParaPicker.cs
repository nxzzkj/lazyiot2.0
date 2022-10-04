using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Scada.BatchCommand
{

    public partial class IOParaPicker : UserControl
    {

        private System.Windows.Forms.Design.IWindowsFormsEditorService _iws;

        public IOParaPicker(System.Windows.Forms.Design.IWindowsFormsEditorService iws)
        {
            InitializeComponent();
            this._iws = iws;
            
        }
        private BachCommand_IOPara mSelectItem = new BachCommand_IOPara();
        public BachCommand_IOPara SelectItem
        {

            get
            {


                return mSelectItem;

            }
            set
            {

       
                this.IO_TreeView.IsParaNodeSelected = true;
       
                mSelectItem = value;


                cbBoolOp.Items.Clear();
               

                cbBoolOp.Items.Clear();
          
                cbBoolOp.Items.Add("=");
                cbBoolOp.Items.Add(">");
                cbBoolOp.Items.Add("<");
                cbBoolOp.Items.Add(">=");
                cbBoolOp.Items.Add("<=");
                cbBoolOp.SelectedIndex = 0;

              



                 BachCommand_IOPara ditem = (BachCommand_IOPara)mSelectItem;
                if (mSelectItem!=null&&mSelectItem.IOParament != null)
                {

                    TreeNode[] nodes = this.IO_TreeView.Nodes.Find(mSelectItem.IOParament.IO_DEVICE_ID, true);
                    if (nodes.Length > 0 && nodes[0].Nodes.Count <= 0 && nodes[0] is BatchCommandDeviceTreeNode deviceTreeNode)
                    {

                        for (int n = 0; n < deviceTreeNode.Device.IOParas.Count; n++)
                        {
                            BatchCommandParaTreeNode node = new BatchCommandParaTreeNode();

                            node.Parament = deviceTreeNode.Device.IOParas[n];
                            nodes[0].Nodes.Add(node);
                            node.InitNode();
                            if (node.Parament == mSelectItem.IOParament)
                            {
                                this.IO_TreeView.SelectedNode = node;

                                this.tbPara.IOParament = node.Parament;
                                this.tbPara.Text = this.tbPara.FullPath();


                            }

                        }



                    }

                    this.listBoxExpressions.Items.Clear();
                    if(ditem.Expressions!=null)
                    {
                        this.listBoxExpressions.Items.AddRange(ditem.Expressions.ToArray());
                    }
                    //设置条件参数
         
                 
                   
                }

            }

        }



       
        private void IO_TreeView_NodeMouseDoubleClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (e.Clicks == 2)
            {



                if (e.Node is BatchCommandParaTreeNode)
                {
                    BatchCommandServerTreeNode serverNode = IO_TreeView.SelectedNode.Parent.Parent.Parent as BatchCommandServerTreeNode;
                    BatchCommandCommunicationTreeNode commNode = IO_TreeView.SelectedNode.Parent.Parent as BatchCommandCommunicationTreeNode;
                    BatchCommandDeviceTreeNode deviceNode = IO_TreeView.SelectedNode.Parent as BatchCommandDeviceTreeNode;
                    BatchCommandParaTreeNode paraNode = IO_TreeView.SelectedNode as BatchCommandParaTreeNode;
                    BachCommand_IOPara selectPara = new BachCommand_IOPara()
                    {
                       IOParament= paraNode.Parament
                       

                    };
                    tbPara.IOParament = paraNode.Parament;
                    
                    tbPara.Text = tbPara.FullPath();



                }

            }


        }
        public void InitTree()
        {
            if (BatchCommandGraphEngineProject.IOServer != null && BatchCommandGraphEngineProject.IOCommunications != null)
                this.IO_TreeView.InitTree(BatchCommandGraphEngineProject.IOServer, BatchCommandGraphEngineProject.IOCommunications);

        }





        private void btOK_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(this.FindForm(), "是否要保存设置?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                BachCommand_IOPara newObject = null;
                newObject = new BachCommand_IOPara();
                newObject.IOParament = tbPara.IOParament;
                newObject.Expressions = new  List<BachCommandBoolExpression>();
                for(int i=0;i < this.listBoxExpressions.Items.Count;i++)
                {
                    newObject.Expressions.Add((BachCommandBoolExpression)this.listBoxExpressions.Items[i]);

                }
                mSelectItem = newObject;


                _iws.CloseDropDown();

            }

        }

        private void btClear_Click(object sender, EventArgs e)
        {
            tbPara.Tag = null;
            tbPara.Text = "";
            tbPara.IOParament = null;
           

        }



        private void btCancel_Click(object sender, EventArgs e)
        {
            _iws.CloseDropDown();
        }

        private void buttonBoolExpression_Click(object sender, EventArgs e)
        {
            this.listBoxExpressions.Items.Add(new BachCommandBoolExpression() { 
            
             DefaultValue= Convert.ToSingle(nudDefaultValue.Value),
              OpSymbol= cbBoolOp.SelectedItem.ToString(),
               Value= Convert.ToSingle(nbBoolValue.Value)
            });
        }

        private void butBoolExpressDel_Click(object sender, EventArgs e)
        {
            this.listBoxExpressions.Items.Remove(listBoxExpressions.SelectedItem);

        }
    }
}

