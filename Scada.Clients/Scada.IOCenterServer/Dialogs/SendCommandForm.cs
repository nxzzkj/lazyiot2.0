using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.MDSCore;
using Scada.MDSCore.Settings;
using Scada.Model;
using ScadaCenterServer.Core;
using System;
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
namespace ScadaCenterServer.Dialogs
{
    public partial class SendCommandForm : PopBaseForm
    {

        public SendCommandForm()
        {
            InitializeComponent();
            //用户发送命令后客户都返回时间

        }



        private IO_SERVER Server = null;
        IO_COMMUNICATION Communication = null;
        IO_DEVICE Device = null;
        public IO_COMMANDS Command = null;
        public void InitCommand(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device)
        {



            Server = server;
            Communication = communication;
            Device = device;
            textBoxServer.Text = server.SERVER_IP + "[" + server.SERVER_NAME + "]";
            textBoxCommunication.Text = communication.IO_COMM_LABEL + "[" + communication.IO_COMM_NAME + "]";
            textBoxDevice.Text = device.IO_DEVICE_LABLE + "[" + device.IO_DEVICE_NAME + "]";
            comboIOPara.Items.Clear();


            for (int i = 0; i < device.IOParas.Count; i++)
            {
                if (device.IOParas[i].IO_POINTTYPE == "模拟量" || device.IOParas[i].IO_POINTTYPE == "开关量")
                {
                    comboIOPara.Items.Add(device.IOParas[i]);

                }

            }

        }
        private void SendCommandForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_BtnClick(object sender, EventArgs e)
        {

            Server = null;
            Communication = null;
            Device = null;

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_BtnClick(object sender, EventArgs e)
        {

            Server = null;
            Communication = null;
            Device = null;

            this.DialogResult = DialogResult.Cancel;
        }

        public void CloseForm()
        {

            Server = null;
            Communication = null;
            Device = null;

            this.DialogResult = DialogResult.Cancel;
        }
        private void ucBtnSend_BtnClick(object sender, EventArgs e)
        {
            this.labelTime.Text = "";
            if (comboIOPara.SelectedItem == null)
            {
                FrmDialog.ShowDialog(this, "请选择要设置的参数");
                return;

            }
            double dvalue = 0;

            if (tbValue.Text.Trim() == "")
            {
                FrmDialog.ShowDialog(this, "请输入下置的值");
                return;
            }
            if (!double.TryParse(tbValue.Text.Trim(), out dvalue))
            {

                FrmDialog.ShowDialog(this, "请输入下置的数值，不能是文本");
                return;
            }
            if (FrmDialog.ShowDialog(this, "您确定要下置命令吗?", "提醒", true, true, true, true) == DialogResult.OK)
            {

                IO_COMMANDS cmmd = new IO_COMMANDS();
                cmmd.COMMAND_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cmmd.COMMAND_ID = GUIDToNormalID.GuidToLongID().ToString();
                cmmd.COMMAND_RESULT = "FALSE";
                cmmd.COMMAND_USER = IOCenterManager.IOProject.ServerConfig.User;
                cmmd.COMMAND_VALUE = tbValue.Text.Trim();
                cmmd.IO_COMM_ID = Communication.IO_COMM_ID;
                cmmd.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                cmmd.IO_SERVER_ID = Server.SERVER_ID;
                cmmd.IO_LABEL = ((IO_PARA)comboIOPara.SelectedItem).IO_LABEL;
                cmmd.IO_NAME = ((IO_PARA)comboIOPara.SelectedItem).IO_NAME;
                cmmd.IO_ID = ((IO_PARA)comboIOPara.SelectedItem).IO_ID;

                Command = cmmd;

                try
                {
                    MDSConfig MDSServerConfig = new MDSConfig();
                    this.TimerNumber = 0;
                    timerNumber.Start();
                    //下置命令
                    byte[] datas = Encoding.UTF8.GetBytes(cmmd.GetCommandString());
                    TaskHelper.Factory.StartNew(() =>
                    {
                        IOStationInfoItem station = MDSSettings.Instance.Stations.Find(x => x.PhysicalMAC.Trim().ToLower() == cmmd.IO_SERVER_ID.Trim().ToLower());
                        if (station != null)
                        {


                            var responseMessage = IOCenterManager.IOCenterClient.SendAndGetResponse(datas, cmmd.IO_SERVER_ID, MDSServerConfig.MonitorAppPrefix + "_" + station.PhysicalAddress.Replace(".", "").Replace("。", ""), MDSCommandType.下置命令, ScadaClientType.IoServer, Scada.MDSCore.Communication.Messages.MessageTransmitRules.StoreAndForward, 10000);
                            if (responseMessage != null)//解析用户数据并获取返回结果
                            {
                                timerNumber.Stop();
                                TimerNumber = 0;
                                TcpData tcpData = new TcpData();
                                tcpData.BytesToTcpItem(SplitPackage.RemoveIdentificationBytes(responseMessage.MessageData).Datas);
                                string IO_COMM_ID = tcpData.GetItemValue("IO_COMM_ID");
                                string IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                                string COMMAND_RESULT = tcpData.GetItemValue("COMMAND_RESULT");
                                if (IO_COMM_ID == cmmd.IO_COMM_ID && IO_SERVER_ID == cmmd.IO_SERVER_ID)
                                {
                                    if (COMMAND_RESULT.ToUpper() == "TRUE")
                                    {
                                        this.labelTime.Text = "下置命令成功";
                                    }
                                    else
                                    {
                                        this.labelTime.Text = "下置命令失败";
                                    }
                                }
                                cmmd.IO_COMM_NAME= Communication.IO_COMM_NAME+"["+ Communication .IO_COMM_LABEL+ "]";
                                cmmd.IO_DEVICE_NAME = Device.IO_DEVICE_NAME + "[" + Device.IO_DEVICE_LABLE + "]";
                                //写入下置命令道实时数据库
                                IOCenterManager.InfluxDbManager.DbWrite_CommandPoint(cmmd.IO_SERVER_ID, cmmd.COMMAND_ID, cmmd, DateTime.Now);
                                //在主界面窗体中显示下置数据的日志
                                if (IOCenterManager.IOCenterServer.ServerForm != null && !IOCenterManager.IOCenterServer.ServerForm.IsDisposed)
                                {
                                    IOCenterManager.IOCenterServer.ServerForm.AddCommand(responseMessage.SourceApplicationName, cmmd.IO_SERVER_ID, cmmd.IO_COMM_ID, cmmd.IO_DEVICE_ID, cmmd.IO_NAME, cmmd);
                                }
                            }
                            else
                            {
                                TimerNumber = 0;
                                this.labelTime.Text = "下置命令失败";
                                timerNumber.Stop();
                            }
                        }
                        else
                        {
                            TimerNumber = 0;
                            this.labelTime.Text = "下置命令失败,采集站不存在";
                            timerNumber.Stop();
                        }
                    });

                }
                catch (Exception ex)
                {
                    TimerNumber = 0;
                    this.labelTime.Text = "下置命令失败,出现错误";
                    timerNumber.Stop();
                    FrmDialog.ShowDialog(this, ex.Message);
                }

            }

        }
        private int TimerNumber = 0;
        private void timerNumber_Tick(object sender, EventArgs e)
        {
            TimerNumber++;
            this.labelTime.Text = TimerNumber.ToString() + "秒";
        }
    }
}
