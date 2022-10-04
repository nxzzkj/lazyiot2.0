 
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
    public partial class IODeviceForm : BasicDialogForm
    {
        public IO_DEVICE Device = null;
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;
        public IODeviceForm()
        {
            InitializeComponent();
        }
        public void InitForm()
        {
            IOManagerUIManager.IODeviceDriveCombox(cb_Driver, Communication.IO_COMM_DRIVER_ID);
            if (Device == null)
            {
                Device = new IO_DEVICE();
                Device.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID().ToString();
                IOCommunicationNode commNode = IOManagerUIManager.mediator.IOTreeForm.IoTree.GetCommunicationNode(Server, Communication);
                if (commNode != null)
                {
                    int num = commNode.DeviceNumber;
                    Device.IO_DEVICE_NAME = "Device" + num;
                    Device.IO_DEVICE_LABLE = Device.IO_DEVICE_NAME;
                    Device.IO_DEVICE_OVERTIME = 120;
                    Device.IO_DEVICE_UPDATECYCLE = 120;
                    Device.IO_DEVICE_ADDRESS = num.ToString();
                }

                Device.IO_DEVICE_STATUS = 0;

            }
            Device.IO_COMM_ID = Communication.IO_COMM_ID;
            Device.IO_SERVER_ID = Server.SERVER_ID;

            this.txtID.Text = Device.IO_DEVICE_ID;
            this.txtLabel.Text = Device.IO_DEVICE_LABLE;
            this.txtName.Text = Device.IO_DEVICE_NAME;
            this.txtRemark.Text = Device.IO_DEVICE_REMARK;
            this.txtAddress.Text = Device.IO_DEVICE_ADDRESS;
            this.nd_timeout.Value = Device.IO_DEVICE_OVERTIME;
            this.nd_updatecycle.Value = Device.IO_DEVICE_UPDATECYCLE;
            this.textBoxPushUrl.Text = Device.DataPushUrl;
            this.richTextBoxJson.Text = "["+ScadaJsonConvertor.ObjectToJson(new RealWebCacheDataItem()
            {
                CommunicationId = "通道ID",
                DeviceId = "设备ID",
                ServerId = "采集站ID",
                Date = "采集日期",
                ParaReals = new List<WebCacheIOData>()
                {
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"},
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"},
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"},
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"},
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"},
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"},
                    new WebCacheIOData(){  Date="采集日期", ParaID="IO点ID", ParaName="IO名称", ParaValue="采集值", QualityStamp="质量戳(GOOD,BAD)"}


                }

            })+"]";

            //加载dll下的所有设备解析驱动
            for (int i = 0; i < this.cb_Driver.Items.Count; i++)
            {
                Scada.Model.SCADA_DEVICE_DRIVER driver = this.cb_Driver.Items[i] as Scada.Model.SCADA_DEVICE_DRIVER;
                if (driver.Id == Device.DEVICE_DRIVER_ID)
                {
                    this.cb_Driver.SelectedIndex = i;
                    break;
                }
            }
            if (this.cb_Driver.SelectedIndex <= 0 && this.cb_Driver.Items.Count > 0)
            {
                this.cb_Driver.SelectedIndex = 0;
            }

        }

        private   void wizardTabControl_ButtonOK(object sender, EventArgs e)
        {
            if (cb_Driver.SelectedItem == null)
            {
                MessageBox.Show("请选择设备驱动");
                return;
            }
            if (this.txtLabel.Text.Trim() == "")
            {
                MessageBox.Show("请输入中文标识");
                return;
            }
            if (this.txtName.Text.Trim() == "")
            {
                MessageBox.Show("请输入设备名称");
                return;
            }
            IOCommunicationNode commNode = IOManagerUIManager.mediator.IOTreeForm.IoTree.GetCommunicationNode(Server, Communication);

           
            if (DriverKernel != null)
            {
                ScadaResult res = DriverKernel.DeviceCtrl.IsValidParameter();
                if (res.Result)
                {
                    Scada.Model.SCADA_DEVICE_DRIVER driver = cb_Driver.SelectedItem as Scada.Model.SCADA_DEVICE_DRIVER;
                    Device.DEVICE_DRIVER_ID = driver.Id;
                    Device.IO_DEVICE_LABLE = this.txtLabel.Text.Trim();
                    Device.IO_DEVICE_NAME = this.txtName.Text.Trim();
                    Device.IO_DEVICE_REMARK = this.txtRemark.Text;
                    Device.IO_DEVICE_STATUS = 1;
                    Device.IO_DEVICE_ADDRESS = this.txtAddress.Text.Trim();
                    Device.IO_SERVER_ID = Server.SERVER_ID;
                    Device.IO_COMM_ID = Communication.IO_COMM_ID;
                    Device.IO_DEVICE_UPDATECYCLE = Convert.ToInt32(this.nd_updatecycle.Value);
                    Device.IO_DEVICE_OVERTIME = Convert.ToInt32(this.nd_timeout.Value);
                    Device.IO_DEVICE_PARASTRING = DriverKernel.DeviceCtrl.GetUIParameter();
                    Device.DataPushUrl = this.textBoxPushUrl.Text;

         
                    if (commNode != null)
                    {
                        for (int i = 0; i < commNode.Nodes.Count; i++)
                        {
                            IODeviceNode deviceNode = commNode.Nodes[i] as IODeviceNode;
                            if (deviceNode.Device.IO_DEVICE_ADDRESS.Trim() == Device.IO_DEVICE_ADDRESS.Trim() && deviceNode.Device != Device)
                            {
                                MessageBox.Show("设备地址与" + deviceNode.Device.IO_DEVICE_LABLE + " 的设备地址重复!");
                                return;
                            }
                        }
                    }
                    if (commNode != null)
                    {
                        for (int i = 0; i < commNode.Nodes.Count; i++)
                        {
                            IODeviceNode deviceNode = commNode.Nodes[i] as IODeviceNode;
                            if (deviceNode.Device.IO_DEVICE_NAME.Trim() == Device.IO_DEVICE_NAME.Trim() && deviceNode.Device != Device)
                            {
                                MessageBox.Show("设备标识与" + deviceNode.Device.IO_DEVICE_LABLE + " 的设备标识重复!");
                                return;
                            }
                        }
                    }
                    IOManagerUIManager.InsertIODeviceNode(this.Server, this.Communication,this.Device);
                   
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(res.Message);
                }

            }



            this.txtID.Text = Device.IO_DEVICE_ID;
            this.txtLabel.Text = Device.IO_DEVICE_LABLE;
            this.txtName.Text = Device.IO_DEVICE_NAME;
            this.txtRemark.Text = Device.IO_DEVICE_REMARK;
            this.nd_timeout.Value = Device.IO_DEVICE_OVERTIME;
            this.nd_updatecycle.Value = Device.IO_DEVICE_UPDATECYCLE;
        }

        private void wizardTabControl_ButtonCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        public ScadaDeviceKernel DriverKernel = null;
        private   void IODeviceForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.wizardTabControl.InitWizard();
            }
            catch
            {

            }
            
          
        }

        private void cb_Driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Driver.SelectedItem != null)
            {
                try
                {


                    Scada.Model.SCADA_DEVICE_DRIVER driver = cb_Driver.SelectedItem as Scada.Model.SCADA_DEVICE_DRIVER;
                    DriverKernel = IOManagerUIManager.CreateDeviceDrive(driver);
                    if (DriverKernel != null)
                    {
                        DriverKernel.IsCreateControl = true;
                        try
                        {
                            DriverKernel.InitKernel(Server, Communication, Device, null, driver);

                            if (DriverKernel.DeviceCtrl != null)
                                DriverKernel.DeviceCtrl.SetUIParameter(Server, Device);
                        }
                        catch (Exception emx)
                        {
                            IOManagerUIManager.DisplayException(new Exception("解析设备驱动参数失败(SetUIParameter)" + emx.Message));

                        }
                        this.tabPage2.Controls.Clear();
                        if (DriverKernel.DeviceCtrl != null)
                            this.tabPage2.Controls.Add(DriverKernel.DeviceCtrl);
                    }



                 
                }
                catch (Exception emx)
                {
                    IOManagerUIManager.DisplayException(new Exception("加载设备驱动失败" + emx.Message));
                }
            }
        }
    }
}
