using IOManager.Core;
using Scada.BatchCommand;
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


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOManager.Dialogs
{
    public partial class BatchCommandTaskEditForm : Form
    {
        public string SERVER_ID = "";
        public BatchCommandTaskEditForm()
        {
            InitializeComponent();
            this.comboBoxDay.SelectedIndex = 0;
            this.comboBoxHour.SelectedIndex = 0;
            this.comboBoxMinute.SelectedIndex = 0;
            this.comboBoxSecond.SelectedIndex = 0;
            this.comboBoxIOTirgBoolExpression.SelectedIndex = 0;
  
            SERVER_ID = IOManagerUIManager.ServerID;
            this.Load += BatchCommandTaskForm_Load;
          


        }
        public void InitForm()
        {
            panelDSTask.Visible = false;
            panelMachineTrain.Visible = false;
            panelIOTrig.Visible = false;
            this.comboBoxCommunication.Items.Clear();
            this.comboBoxDevice.Items.Clear();
            this.comboBoxIOPara.Items.Clear();

         
            List<IO_COMMUNICATION> communications = IOManagerUIManager.GetIOProject();
            communications.ForEach(delegate (IO_COMMUNICATION comm) {

                this.comboBoxCommunication.Items.Add(comm);

               
            });
            enumDropListTaskStartRunType.SetItems<BatchCommandStartRunType>();
            enumDropListBatchCommandTimingTime.SetItems<BatchCommandTimingTimeType>();

            comboBoxMachineTrain.Items.Clear();
            List<Scada.Model.ScadaMachineTrainingModel> MachineTrainingModels = IOManagerUIManager.MachineTrainingModels;
            MachineTrainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel trainingModel) {

                comboBoxMachineTrain.Items.Add(trainingModel);
            });


        }
        private BatchCommandTaskModel _taskModel = null;
        
        public BatchCommandTaskModel TaskModel
        {
            set
            {

                panelDSTask.Visible = false;
                panelMachineTrain.Visible = false;
                panelIOTrig.Visible = false;
                panelManualTask.Visible = false;
                comboBoxIOTirgBoolExpression.SelectedIndex = 0;
                enumDropListBatchCommandTimingTime.SelectedIndex = 0;
                 _taskModel = value;
                if (_taskModel == null)
                {
                    _taskModel = new BatchCommandTaskModel();
                    _taskModel.Id = "";
                }

                
                  
                if(_taskModel.MachineTrainingTaskId!=null)
                {
                    for(int i=0;i< comboBoxMachineTrain.Items.Count;i++)
                    {
                        if(!string.IsNullOrEmpty(_taskModel.MachineTrainingTaskId)&&comboBoxMachineTrain.Items[i] is Scada.Model.ScadaMachineTrainingModel trainingModel)
                        {
                            string taskId = _taskModel.MachineTrainingTaskId.Split(',')[0].Split(':')[1];
                            if (trainingModel.Id.ToString()== taskId.Trim())
                            {
                                comboBoxMachineTrain.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
              BatchCommandStartRunType startRunType = BatchCommandStartRunType.ManualTask;
                Enum.TryParse(_taskModel.TaskStartRunType, out startRunType);
                
                enumDropListTaskStartRunType.SelectedItem = startRunType;
                if (startRunType != null && BatchCommandStartRunType.TimedTask == startRunType)
                {
                    panelDSTask.Visible = true;
                    panelMachineTrain.Visible = false;
                    panelIOTrig.Visible = false;
                    panelManualTask.Visible = false;
                    string str = _taskModel.ExecuteTaskTimingTime;
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] arrays = str.Split(',');
                        //    return "TimingTimeType:"+TimingTimeType.ToString()+ ",Day:" + Day+ ",Hour:" + Hour+ ",Minute:" + Minute+ ",Second:" + Second+ ",ExecuteCycleTimes:" + ExecuteCycleTimes;
                        BatchCommandTimingTimeType timeType = BatchCommandTimingTimeType.Month;
                        Enum.TryParse(arrays[0].Split(':')[1], out timeType);
                        enumDropListBatchCommandTimingTime.SelectedItem = startRunType;
                        this.comboBoxDay.SelectedItem = arrays[1].Split(':')[1];
                        this.comboBoxHour.SelectedItem = arrays[2].Split(':')[1];
                        this.comboBoxMinute.SelectedItem = arrays[3].Split(':')[1];
                        this.comboBoxSecond.SelectedItem = arrays[4].Split(':')[1];
                        this.numericUpDownExecuteCycleTimes.Value = Convert.ToDecimal(arrays[5].Split(':')[1]);

                    }
                }
                else if (startRunType != null && BatchCommandStartRunType.ManualTask == startRunType)
                {
                    panelDSTask.Visible = false;
                    panelMachineTrain.Visible = false;
                    panelIOTrig.Visible = false;
                    panelManualTask.Visible = true;

                    string startManualStr = _taskModel.ManualTask;
                    if (!string.IsNullOrEmpty(startManualStr))
                    {
                        string[] arrays = startManualStr.Split(',');
                        this.textBoxManualTaskTitle.Text = arrays[0].Split(',')[1];
                        if (arrays[1].Split(',')[1] == "1")
                            this.checkBoxManualTask.Checked = true;
                        else
                            this.checkBoxManualTask.Checked = false;
                    }
                }
                else if (startRunType != null && BatchCommandStartRunType.IOTriggerTask == startRunType)
                {
                    panelDSTask.Visible = false;
                    panelMachineTrain.Visible = false;
                    panelIOTrig.Visible = true;
                    panelManualTask.Visible = false;
                    string startTrgStr = _taskModel.IOStartConditionValue;


                    if (!string.IsNullOrEmpty(startTrgStr))
                    {
                          string[] arrays = startTrgStr.Split(',');
                        for (int i = 0; i < this.comboBoxCommunication.Items.Count; i++)
                        {
                            if (this.comboBoxCommunication.Items[i] is IO_COMMUNICATION comm)
                            {
                                if (comm.IO_COMM_ID == arrays[1].Split(':')[1].Trim())
                                {
                                    this.comboBoxCommunication.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        InitDevice();

                        for (int i = 0; i < this.comboBoxDevice.Items.Count; i++)
                        {
                            if (this.comboBoxDevice.Items[i] is IO_DEVICE device)
                            {
                                if (device.IO_DEVICE_ID == arrays[2].Split(':')[1].Trim())
                                {
                                    this.comboBoxDevice.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        InitPara();

                        for (int i = 0; i < this.comboBoxIOPara.Items.Count; i++)
                        {
                            if (this.comboBoxIOPara.Items[i] is IO_PARA para)
                            {
                                if (para.IO_ID == arrays[3].Split(':')[1].Trim())
                                {
                                    this.comboBoxIOPara.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        if (arrays.Length > 4)
                        {


                            for (int i = 4; i < arrays.Length; i++)
                            {
                                string[] strs = arrays[i].Split(';');
                                this.listBoxTrigExpression.Items.Add(new BachCommandBoolExpression()
                                {
                                    DefaultValue = Convert.ToSingle(strs[2].Split(':')[1]),
                                    Label = strs[3].Split(':')[1],
                                    OpSymbol = strs[0].Split(':')[1],
                                    Value = Convert.ToSingle(strs[1].Split(':')[1])



                                });
                            }
                        }


                    }
                }
                else if (startRunType != null && BatchCommandStartRunType.MachineTrainTask == startRunType)
                {
                    panelDSTask.Visible = false;
                    panelMachineTrain.Visible = true;
                    panelIOTrig.Visible = false;
                    panelManualTask.Visible = false;
                    string machineTrgstr = _taskModel.MachineTrainingTaskId;
                    if (!string.IsNullOrEmpty(machineTrgstr))
                    {
                        string[] arrays = machineTrgstr.Split(',');
                        string taskid = arrays[0].Split(':')[1].Trim();
                        string title = arrays[1].Split(':')[1].Trim();


                        this.listBoxExpression.Items.Clear();
                        if (arrays.Length > 2)
                        {
                            string[] strs = arrays[2].Split(';');
                            if (strs.Length > 0)
                                this.listBoxExpression.Items.AddRange(strs);

                        }

                    }

                }
              
               

                this.textBoxRemark.Text = _taskModel.CommandTaskRemark;
                this.tbTaskName.Text = _taskModel.CommandTaskTitle;


            }
            get {

                if (_taskModel == null)
                {
                    _taskModel = new BatchCommandTaskModel();
                    _taskModel.Id = "";
                }
              
                _taskModel.SERVER_ID = SERVER_ID;
                _taskModel.TaskStartRunType = enumDropListTaskStartRunType.SelectedItem.ToString();
                _taskModel.CommandTaskCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _taskModel.CommandTaskRemark = this.textBoxRemark.Text;
                _taskModel.CommandTaskTitle = this.tbTaskName.Text;
                _taskModel.ExecuteTaskTimingTime = "";
                _taskModel.IOStartConditionValue = "";
                _taskModel.MachineTrainingTaskId = "";
                _taskModel.ManualTask = "";
                if (enumDropListTaskStartRunType.SelectedItem.ToString() == BatchCommandStartRunType.IOTriggerTask.ToString())
                {
                   
                    string str = "";

                    for (int i = 0; i < listBoxTrigExpression.Items.Count; i++)
                    {
                        BachCommandBoolExpression boolExpression = listBoxTrigExpression.Items[i] as BachCommandBoolExpression;
                        str += boolExpression.GetDataString() + ",";
                    }
                    str = str.Substring(0, str.Length - 1);
                    _taskModel.IOStartConditionValue = "SERVER_ID:" + ((IO_COMMUNICATION)this.comboBoxCommunication.SelectedItem).IO_SERVER_ID + ",COMM_ID:" + ((IO_COMMUNICATION)this.comboBoxCommunication.SelectedItem).IO_COMM_ID + ",DEVICE_ID:" + ((IO_DEVICE)this.comboBoxDevice.SelectedItem).IO_DEVICE_ID + ",IO_ID:" + ((IO_PARA)this.comboBoxIOPara.SelectedItem).IO_ID + "," + str;



                }
                else if (enumDropListTaskStartRunType.SelectedItem.ToString() == BatchCommandStartRunType.TimedTask.ToString())
                {
                    
                    _taskModel.ExecuteTaskTimingTime = "TimingTimeType:" + enumDropListBatchCommandTimingTime.SelectedItem.ToString() + ",Day:" + this.comboBoxDay.SelectedItem.ToString() + ",Hour:" + this.comboBoxHour.SelectedItem.ToString() + ",Minute:" + this.comboBoxMinute.SelectedItem.ToString() + ",Second:" + this.comboBoxSecond.SelectedItem.ToString() + ",ExecuteCycleTimes:" + int.Parse(numericUpDownExecuteCycleTimes.Value.ToString());


                }
                else if (enumDropListTaskStartRunType.SelectedItem.ToString() == BatchCommandStartRunType.ManualTask.ToString())
                {
                    _taskModel.ManualTask = "Title:" + this.textBoxManualTaskTitle.Text.Replace(",", "，") + " ,InQuiry:" + (this.checkBoxManualTask.Checked ? "1" : "0");


                }


              
                else if (enumDropListTaskStartRunType.SelectedItem.ToString() == BatchCommandStartRunType.MachineTrainTask.ToString())
                {
                    ScadaMachineTrainingModel commandTaskModel = comboBoxMachineTrain.SelectedItem as ScadaMachineTrainingModel;

                    string str = "";

                    for (int i = 0; i < listBoxExpression.Items.Count; i++)
                    {
                        string boolExpression = listBoxExpression.Items[i].ToString();
                        str += boolExpression + ";";
                    }
                    str = str.Substring(0, str.Length - 1);
                    _taskModel.MachineTrainingTaskId = "TaskId:" + commandTaskModel.Id + ",TaskTitle:" + commandTaskModel.TaskName + "," + str;

                }

                


                return _taskModel;
            
            
            }


        }
        private void BatchCommandTaskForm_Load(object sender, EventArgs e)
        {


           

            enumDropListTaskStartRunType.SelectedChanged = (Enum selector) =>
            {

            
                if (selector != null && BatchCommandStartRunType.TimedTask == (BatchCommandStartRunType)selector)
                {
                    panelDSTask.Visible = true;
                    panelMachineTrain.Visible = false;
                    panelIOTrig.Visible = false;
                    panelManualTask.Visible = false;
                }
                else if (selector != null && BatchCommandStartRunType.ManualTask == (BatchCommandStartRunType)selector)
                {
                    panelDSTask.Visible = false;
                    panelMachineTrain.Visible = false;
                    panelIOTrig.Visible = false;
                    panelManualTask.Visible = true;
                }
                else if (selector != null && BatchCommandStartRunType.IOTriggerTask == (BatchCommandStartRunType)selector)
                {
                    panelDSTask.Visible = false;
                    panelMachineTrain.Visible = false;
                    panelIOTrig.Visible = true;
                    panelManualTask.Visible = false;
                }
                else if (selector != null && BatchCommandStartRunType.MachineTrainTask == (BatchCommandStartRunType)selector)
                {
                    panelDSTask.Visible = false;
                    panelMachineTrain.Visible = true;
                    panelIOTrig.Visible = false;
                    panelManualTask.Visible = false;
                }
            };
            enumDropListBatchCommandTimingTime.SelectedChanged = (Enum selector) =>
            {
                this.comboBoxDay.Enabled = false;
                this.comboBoxHour.Enabled = false;
                this.comboBoxMinute.Enabled = false;
                this.comboBoxSecond.Enabled = false;
                if (selector != null && BatchCommandTimingTimeType.Month == (BatchCommandTimingTimeType)selector)
                {
                    this.comboBoxDay.Enabled = true;
                    this.comboBoxHour.Enabled = true;
                    this.comboBoxMinute.Enabled = true;
                    this.comboBoxSecond.Enabled = true;
                }
                else if (selector != null && BatchCommandTimingTimeType.Day == (BatchCommandTimingTimeType)selector)
                {
                    this.comboBoxDay.Enabled = false;
                    this.comboBoxHour.Enabled = true;
                    this.comboBoxMinute.Enabled = true;
                    this.comboBoxSecond.Enabled = true;
                }
                else if (selector != null && BatchCommandTimingTimeType.Hour == (BatchCommandTimingTimeType)selector)
                {
                    this.comboBoxDay.Enabled = false;
                    this.comboBoxHour.Enabled = false;
                    this.comboBoxMinute.Enabled = true;
                    this.comboBoxSecond.Enabled = true;
                }
                else if (selector != null && BatchCommandTimingTimeType.Minute == (BatchCommandTimingTimeType)selector)
                {
                    this.comboBoxDay.Enabled = false;
                    this.comboBoxHour.Enabled = false;
                    this.comboBoxMinute.Enabled = false;
                    this.comboBoxSecond.Enabled = true;
                }
            };
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbTaskName.Text))
            {
                MessageBox.Show(this,"请输入任务名称");
                return;
            }
            if (enumDropListTaskStartRunType.SelectedItem != null && BatchCommandStartRunType.IOTriggerTask == (BatchCommandStartRunType)enumDropListTaskStartRunType.SelectedItem)
            {
                if (this.comboBoxCommunication.SelectedItem == null)
                {
                    MessageBox.Show(this, "请选择通道");
                    return;
                }
                if (this.comboBoxDevice.SelectedItem == null)
                {
                    MessageBox.Show(this, "请选择设备");
                    return;
                }
                if (this.comboBoxIOPara.SelectedItem == null)
                {
                    MessageBox.Show(this, "请选择IO参数");
                    return;
                }

                if(listBoxTrigExpression.Items.Count<=0)
                {
                    if (this.comboBoxIOPara.SelectedItem == null)
                    {
                        MessageBox.Show(this, "请皮配置IO触发条件");
                        return;
                    }
                }
                
            }
            else if (enumDropListTaskStartRunType.SelectedItem != null && BatchCommandStartRunType.MachineTrainTask == (BatchCommandStartRunType)enumDropListTaskStartRunType.SelectedItem)
            {
                if (this.comboBoxMachineTrain.SelectedItem == null)
                {
                    MessageBox.Show(this, "请选择训练任务");
                    return;
                }
                if (listBoxExpression.Items.Count <= 0)
                {
                    if (this.comboBoxIOPara.SelectedItem == null)
                    {
                        MessageBox.Show(this, "请配置触发标签");
                        return;
                    }
                }
            }
                this.DialogResult = DialogResult.OK;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboBoxCommunication_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDevice();
        }
        private void InitDevice()
        {
            this.comboBoxDevice.Items.Clear();
            if (comboBoxCommunication.SelectedItem != null && comboBoxCommunication.SelectedItem is IO_COMMUNICATION comm)
            {
                comm.Devices.ForEach(delegate (IO_DEVICE device) {

                    this.comboBoxDevice.Items.Add(device);
                });
            }

        }
        
        private void InitPara()
        {
            this.comboBoxIOPara.Items.Clear();
            if (comboBoxDevice.SelectedItem != null && comboBoxDevice.SelectedItem is IO_DEVICE device)
            {
                device.IOParas.ForEach(delegate (IO_PARA para) {

                    this.comboBoxIOPara.Items.Add(para);
                });
            }
        }
        
        private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitPara();

        }

        private void btAddBoolExpression_Click(object sender, EventArgs e)
        {

            if (textBoxLabel.Text.Trim() == null)
                return;
            for (int i=0;i< this.listBoxExpression.Items.Count;i++)
            {
                if(this.listBoxExpression.Items[i].ToString().Trim()== textBoxLabel.Text.Trim())
                {
                    MessageBox.Show(this, "已经存在此标签");
                    return;
                }

            }
            this.listBoxExpression.Items.Add(textBoxLabel.Text.Trim());


             


        }

        private void btBoolExpressionDel_Click(object sender, EventArgs e)
        {
            this.listBoxExpression.Items.Remove(this.listBoxExpression.SelectedItem);

        }

        private void btTirgExpressionAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxIOTirgBoolExpression.SelectedItem == null)
                return;
            this.listBoxTrigExpression.Items.Add(new BachCommandBoolExpression()
            {
                OpSymbol = comboBoxIOTirgBoolExpression.SelectedItem.ToString(),
                Value = Convert.ToSingle(nudTirgValue.Value)
               
            });
        }

        private void btTirgExpressionDel_Click(object sender, EventArgs e)
        {
            this.listBoxTrigExpression.Items.Remove(this.listBoxTrigExpression.SelectedItem);
        }

        
    }
}
