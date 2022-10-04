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
    public partial class MachineTrainingEditForm : BasicDialogForm
    {
        public ScadaMachineTrainingModel MachineTrainingModel = null;
        IOTree IoTree = null;
        public MachineTrainingEditForm(IOTree ioTree,ScadaMachineTrainingModel scadaMachineTraining=null)
        {
            InitializeComponent();
            MachineTrainingModel = scadaMachineTraining;
            IoTree = ioTree;
           
            this.Load += MachineTrainingEditForm_Load;
        }

        private void MachineTrainingEditForm_Load(object sender, EventArgs e)
        {
            cbTaskAlgorithm.Items.Clear();
            foreach (ScadaMachineTrainingAlgorithm item in Enum.GetValues(typeof(ScadaMachineTrainingAlgorithm)))
            {
            
                cbTaskAlgorithm.Items.Add(item);

            }
           
            InitCommunication_Device(IoTree);

            if(MachineTrainingModel!=null)
            {
                foreach (ScadaMachineTrainingAlgorithm item in cbTaskAlgorithm.Items)
                {
                    if (item.ToString() == MachineTrainingModel.Algorithm)
                    {
                        int index = cbTaskAlgorithm.Items.IndexOf(item); //index 为索引值
                        cbTaskAlgorithm.SelectedIndex = index;
                        break;

                    }

                }
                    tbTaskName.Text = MachineTrainingModel.TaskName;

                this.tbFalse.Text = MachineTrainingModel.FalseText;
                this.tbTrue.Text = MachineTrainingModel.TrueText;
                string[] cols = MachineTrainingModel.Detection.Split(',');
                if (cols.Length == 6)
                {
                    this.tbDetection5.Text = cols[0];
                    this.tbDetection6.Text = cols[1];
                    this.tbDetection7.Text = cols[2];
                    this.tbDetection8.Text = cols[3];
                    this.tbDetection9.Text = cols[4];
                    this.tbDetection10.Text = cols[5];
                }
                for (int i=0;i< cbCommunication.Items.Count;i++)
                {
                    IOCommunicationNode node = cbCommunication.Items[i] as IOCommunicationNode;
                    if(node.Communication.IO_COMM_ID== MachineTrainingModel.COMM_ID)
                    {
                        cbCommunication.SelectedIndex = i;
                        break;
                    }
                }


                for (int i = 0; i < cbDevice.Items.Count; i++)
                {
                    IODeviceNode node = cbDevice.Items[i] as IODeviceNode;
                    if (node.Device.IO_DEVICE_ID == MachineTrainingModel.DEVICE_ID)
                    {
                        cbDevice.SelectedIndex = i;
                        break;
                    }
                }

                cbTaskProperties.SetChecked(MachineTrainingModel.Properties.Split(',').ToList());


            }

        }
        private void InitCommunication_Device(IOTree tree)
        {
            cbCommunication.Items.Clear();
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                if (tree.Nodes[i] is IOServerNode)
                {
                    IOServerNode sNode = tree.Nodes[i] as IOServerNode;
                    
                    foreach(IOCommunicationNode cnode in sNode.Nodes)
                    {
                        cbCommunication.Items.Add(cnode);

                    }

                }
            }
          
        }
        /// <summary>
        /// 获取 enum 的描述信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tField"></param>
        /// <returns></returns>
        public static string GetEnumDesc<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(DescriptionAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (DescriptionAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Description ?? string.Empty;
            }
            return description;
        }

        public static string GetEnumCategory<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(CategoryAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (CategoryAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Category ?? string.Empty;
            }
            return description;
        }
        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (MachineTrainingModel == null)
                MachineTrainingModel = new ScadaMachineTrainingModel() { Id = GUIDToNormalID.GuidToInt() };
            if (string.IsNullOrEmpty(tbTaskName.Text))
            {
                MessageBox.Show("请输入任务名称!");
                return;
            }
            if (cbTaskProperties.CheckedCount <= 0)
            {
                MessageBox.Show("请选择要参与计算的属性!");
                return;
            }

            if (cbTaskAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("请选择算法!");
                return;
            }
            MachineTrainingModel.Algorithm = cbTaskAlgorithm.SelectedItem.ToString();
            MachineTrainingModel.AlgorithmType = GetEnumCategory((ScadaMachineTrainingAlgorithm)cbTaskAlgorithm.SelectedItem);
            MachineTrainingModel.ForecastPriod = Convert.ToInt32(nudTaskForecastPriod.Value);
            MachineTrainingModel.TrainingCycle = Convert.ToInt32(nudTaskTrainingCycle.Value);
            MachineTrainingModel.Properties = this.cbTaskProperties.GetCheckedID();
            MachineTrainingModel.Remark = this.tbTaskRemark.Text;
            MachineTrainingModel.TaskName = tbTaskName.Text.Trim();
            IOCommunicationNode cNode = cbCommunication.SelectedItem as IOCommunicationNode;
            MachineTrainingModel.COMM_ID = cNode.Communication.IO_COMM_ID;
            IODeviceNode dNode = cbDevice.SelectedItem as IODeviceNode;
            MachineTrainingModel.DEVICE_ID = dNode.Device.IO_DEVICE_ID;

            MachineTrainingModel.FalseText = this.tbFalse.Text;
            MachineTrainingModel.TrueText = this.tbTrue.Text;
            this.tbTrue.Text = MachineTrainingModel.TrueText;

            List<string> cols = new List<string>();

            cols.Add(this.tbDetection5.Text);
            cols.Add(this.tbDetection6.Text);
            cols.Add(this.tbDetection7.Text);
            cols.Add(this.tbDetection8.Text);
            cols.Add(this.tbDetection9.Text);
            cols.Add(this.tbDetection10.Text);

            MachineTrainingModel.Detection = String.Join(",", cols);
            MachineTrainingModel.IsTrain = 0;
            this.DialogResult = DialogResult.OK;
        }

        private void cbTaskAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelDesc.Text = GetEnumDesc((ScadaMachineTrainingAlgorithm)cbTaskAlgorithm.SelectedItem);
        }

        private void cbCommunication_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDevice.Items.Clear();
            if (cbCommunication.SelectedItem!=null)
            {
                IOCommunicationNode cNode = cbCommunication.SelectedItem as IOCommunicationNode;
             
                foreach (IODeviceNode dnode in cNode.Nodes)
                {
                    cbDevice.Items.Add(dnode);

                }
                cbTaskProperties.Clear();
            }
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTaskProperties.Clear();
            if (cbDevice.SelectedItem != null)
            {
                IODeviceNode dNode = cbDevice.SelectedItem as IODeviceNode;
            
                foreach (IO_PARA para in dNode.Device.IOParas)
                {
                    cbTaskProperties.Add(new CheckBoxItem() {  ID= para.IO_NAME, Text= para.ToString()});
                }

            }
        }
    }
}
