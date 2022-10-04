 
using Scada.Model;
using IOManager.Controls;
using IOManager.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;
using Scada.Kernel;


 
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
    public partial class IOCommunicationForm : BasicDialogForm
    {
        public IO_COMMUNICATION Comunication = null;
        public IO_SERVER Server = null;
        public IOCommunicationForm()
        {
            InitializeComponent();


        }
        public   void InitForm()
        {
              IOManagerUIManager.IOCommunicationDriveCombox(comboDrive);

            if (Comunication == null)
            {
                Comunication = new IO_COMMUNICATION();
                Comunication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();
                IOServerNode serverNode = IOManagerUIManager.mediator.IOTreeForm.IoTree.GetServerNode(Server);
                if(serverNode!=null)
                {
                  
                    Comunication.IO_COMM_LABEL= "Communication" + serverNode.CommunicationNumber;
                    Comunication.IO_COMM_NAME = "";
                }
             
                Comunication.IO_COMM_STATUS = 1;
            }
            Comunication.IO_SERVER_ID = Server.SERVER_ID;

            this.txtID.Text = Comunication.IO_COMM_ID;
            this.txtLabel.Text = Comunication.IO_COMM_LABEL;
            this.txtName.Text = Comunication.IO_COMM_NAME;
            this.txtRemark.Text = Comunication.IO_COMM_REMARK;
            for (int i = 0; i < this.comboDrive.Items.Count; i++)
            {
                Scada.Model.SCADA_DRIVER driver = this.comboDrive.Items[i] as Scada.Model.SCADA_DRIVER;
                if (driver.Id == Comunication.IO_COMM_DRIVER_ID)
                {
                    this.comboDrive.SelectedIndex = i;
                    break;
                }
            }
            if (this.comboDrive.SelectedIndex <= 0 && this.comboDrive.Items.Count > 0)
            {
                this.comboDrive.SelectedIndex = 0;
            }

        }

        private void wizardTabControl_ButtonCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private   void wizardTabControl_ButtonOK(object sender, EventArgs e)
        {
            if (comboDrive.SelectedItem == null)
            {
                MessageBox.Show("请选择通讯驱动");
                return;
            }
            if (this.txtLabel.Text.Trim() == "")
            {
                MessageBox.Show("请输入中文名称");
                return;
            }
            if (this.txtName.Text.Trim() == "")
            {
                MessageBox.Show("请输入通道标识");
                return;
            }
            IOServerNode serverNode = IOManagerUIManager.mediator.IOTreeForm.IoTree.GetServerNode(Server);

            if (serverNode != null)
            {
                for (int i = 0; i < serverNode.Nodes.Count; i++)
                {
                    IOCommunicationNode commNode = serverNode.Nodes[i] as IOCommunicationNode;
                    if (commNode.Communication.IO_COMM_NAME.Trim() == Comunication.IO_COMM_NAME.Trim()&& commNode.Communication!= Comunication)
                    {
                        MessageBox.Show("通讯通道" + commNode.Communication.IO_COMM_NAME.Trim() + " 的标识重复!");
                        return;
                    }
                }
            }
            if (DriverCom != null)
            {
                ScadaResult res = DriverCom.CommunicationControl.IsValidParameter();
               
                if (res.Result)
                {
                    Scada.Model.SCADA_DRIVER driver = comboDrive.SelectedItem as Scada.Model.SCADA_DRIVER;
                    Comunication.IO_COMM_DRIVER_ID = driver.Id;
                    Comunication.IO_COMM_LABEL = this.txtLabel.Text.Trim();
                    Comunication.IO_COMM_NAME = this.txtName.Text.Trim();
                    Comunication.IO_COMM_REMARK = this.txtRemark.Text;
                    Comunication.IO_COMM_STATUS = 1;
                    Comunication.IO_SERVER_ID = Server.SERVER_ID;
                    Comunication.IO_COMM_PARASTRING = DriverCom.GetUIParameter();
                      IOManagerUIManager.InsertIOCommunicationNode(this.Server, Comunication);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(res.Message);
                }

            }


        }

        private void wizardTabControl_ButtonNext(object sender, EventArgs e)
        {

        }

        private void wizardTabControl_ButtonPrevious(object sender, EventArgs e)
        {

        }

        private   void IOCommunicationForm_Load(object sender, EventArgs e)
        {
            this.wizardTabControl.InitWizard();
       

        }
        public ScadaCommunicateKernel DriverCom = null;
        private   void comboDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDrive.SelectedItem != null)
            {
                try
                {


                    Scada.Model.SCADA_DRIVER driver = comboDrive.SelectedItem as Scada.Model.SCADA_DRIVER;

                    DriverCom= IOManagerUIManager.CreateCommunicateDriver(driver);
                    DriverCom.IsCreateControl = true;
                    if (DriverCom == null)
                    {
                        IOManagerUIManager.DisplayException(new Exception("通讯驱动加载失败" ));
                        return;
                    }

                    try
                    {
                       
                        DriverCom.InitKernel(this.Server, this.Comunication, this.Comunication.Devices, driver);
                    }
                    catch(Exception emx)
                    {
                         IOManagerUIManager.DisplayException(new Exception("通讯驱动初始化InitDriver失败" + emx.Message));

                    }
           
                    
                    try
                    {
                     

                        if (DriverCom.CommunicationControl!=null)
                            DriverCom.CommunicationControl.SetUIParameter(Comunication.IO_COMM_PARASTRING);
                    }
                    catch (Exception emx)
                    {
                         IOManagerUIManager.DisplayException(new Exception("解析通讯驱动参数失败(SetUIParameter)" + emx.Message));

                    }
                    this.tabPage2.Controls.Clear();
                    this.tabPage2.Controls.Add(DriverCom.CommunicationControl);
                }
                catch(Exception emx)
                {
                     IOManagerUIManager.DisplayException(new Exception("加载通讯驱动失败" + emx.Message));
                }
            }
        }
    }
}
