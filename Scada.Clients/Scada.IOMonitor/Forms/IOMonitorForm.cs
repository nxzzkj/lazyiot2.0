using Scada.Model;
using Scada.Controls.Controls;
using Scada.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IOMonitor.Core;
using Scada.DBUtility;
using System.Threading;
using System.Collections.Concurrent;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Forms
{

    public partial class IOMonitorForm : DockContent, ICobaltTab
    {
        System.Threading.Timer listTimer = null;
        public IOMonitorForm(Mediator m)
        {
            mediator = m;
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.HideOnClose = true;
            this.CloseButton = false;
            
           
        }
        protected List<ListViewItem> RealViewCacheItemsSource
        {
            get;
            private set;
        }
        
        protected List<ListViewItem> RealCacheItemsSource
        {
            get;
            private set;
        }
        protected List<ListViewItem> AlarmCacheItemsSource
        {
            get;
            private set;
        }

        protected ConcurrentStack<ListViewItem> RealItemsSource
        {
            get;
            private set;
        }
        protected ConcurrentStack<ListViewItem> AlarmItemsSource
        {
            get;
            private set;
        }
        private void ListViewReal_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
           
                if (this.RealViewCacheItemsSource == null || this.RealViewCacheItemsSource.Count == 0)
                {
                    return;
                }

                e.Item = this.RealViewCacheItemsSource[e.ItemIndex];
                if (e.ItemIndex == this.RealViewCacheItemsSource.Count)
                {
                    this.RealViewCacheItemsSource = null;

                }
           

        }
        private void ListViewAlarm_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
             
                if (this.AlarmCacheItemsSource == null || this.AlarmCacheItemsSource.Count == 0)
                {
                    return;
                }

                e.Item = this.AlarmCacheItemsSource[e.ItemIndex];
                if (e.ItemIndex == this.AlarmCacheItemsSource.Count)
                {
                    this.AlarmCacheItemsSource = null;

                }
           

        }

        private void ListViewReceive_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            
                if (this.RealCacheItemsSource == null || this.RealCacheItemsSource.Count == 0)
                {
                    return;
                }

                e.Item = this.RealCacheItemsSource[e.ItemIndex];
                if (e.ItemIndex == this.RealCacheItemsSource.Count)
                {
                    this.RealCacheItemsSource = null;
                }
            
        }

        private Mediator mediator = null;
        private string identifier;


        public IO_DEVICE Device = null;
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;
        #region 显示用户选择的设备的值

        public void ChangedBinds(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device)
        {
            if (IOMonitorManager.IsBackRun)
                return;

            if (device != null)
            {

                ucRollText.Text = "IO路径:  /" + server.SERVER_NAME + "/" + comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]/" + device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]";
              
                List<ListViewItem> clist = new List<ListViewItem>();
                foreach (IO_PARA para in device.IOParas)
                {

                    ListViewItem lvItem = new ListViewItem(para.IO_ID);
                    lvItem.Text = para.IO_ID;
                    lvItem.Tag = para;
                    lvItem.SubItems.Add(para.IO_NAME);
                    lvItem.SubItems.Add(para.IO_LABEL);
                    lvItem.SubItems.Add(para.RealValue);
                    lvItem.SubItems.Add(para.IO_UNIT);
                    lvItem.SubItems.Add(para.RealDate);
                    lvItem.SubItems.Add(para.RealQualityStamp.ToString());
                    lvItem.SubItems.Add(para.IO_POINTTYPE.ToString());
                    clist.Add(lvItem);
                }
                RealViewCacheItemsSource = clist;



            }

        }
        /// <summary>
        /// 设置采集点的采集值
        /// </summary>
        /// <returns></returns>
        public void SetIOValue(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION comm, Scada.Model.IO_DEVICE device)
        {
            if (server == null)
                return;
            try
            {


                if (device.IO_SERVER_ID == server.SERVER_ID && Communication.IO_COMM_ID == comm.IO_COMM_ID && Device.IO_DEVICE_ID == device.IO_DEVICE_ID)
                {
                    ChangedBinds(server, comm, device.Copy());
                }
            }
            catch (Exception ex)
            {
                IOMonitorUIManager.DisplyException(ex);
            }
        }
        
        #endregion
        public TabTypes TabType
        {
            get
            {
                return TabTypes.IOMonitoring;
            }
        }
        public string TabIdentifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
            }
        }

        private void IOMonitorForm_Load(object sender, EventArgs e)
        {
            ControlHelper.FreezeControl(this, true);
            listViewReceive.VirtualMode = true;
            listViewReceive.VirtualListSize = 0;
            this.listViewAlarm.VirtualListSize = 0;
            listView.VirtualMode = true;
            listView.VirtualListSize = 0;
            listViewAlarm.VirtualMode = true;
            listViewReceive.RetrieveVirtualItem += ListViewReceive_RetrieveVirtualItem;
            listViewAlarm.RetrieveVirtualItem += ListViewAlarm_RetrieveVirtualItem;
            this.listView.RetrieveVirtualItem += ListViewReal_RetrieveVirtualItem;
            RealCacheItemsSource = new List<ListViewItem>();
            AlarmCacheItemsSource = new List<ListViewItem>();
            RealItemsSource = new ConcurrentStack<ListViewItem>();
            AlarmItemsSource = new ConcurrentStack<ListViewItem>();
            RealViewCacheItemsSource = new List<ListViewItem>();

            ucLateCommandSize.SelectedIndex = 0;
            listTimer = new System.Threading.Timer(delegate
            {
                if (IOMonitorManager.IsBackRun)
                    return;
                TaskHelper.Factory.StartNew(() =>
                {
                    try
                    {
                        //定时加载日志
                        RealCacheItemsSource = RealItemsSource.ToList();
                        if (RealItemsSource.Count > 2000)
                        {

                            RealItemsSource.Clear();

                            listViewReceive.VirtualListSize = 0;

                        }
                        else
                        {

                            listViewReceive.VirtualListSize = RealCacheItemsSource.Count;

                        }
                    }
                    catch
                    {

                    }
                });
                TaskHelper.Factory.StartNew(() =>
                {
                    try
                    {
                        AlarmCacheItemsSource = AlarmItemsSource.ToList();
                        if (AlarmItemsSource.Count > 2000)
                        {

                            AlarmItemsSource.Clear();
                            this.listViewAlarm.VirtualListSize = 0;

                        }
                        else
                        {

                            this.listViewAlarm.VirtualListSize = AlarmCacheItemsSource.Count;
                        }


                     

                    }
                    catch
                    {

                    }
                });
                TaskHelper.Factory.StartNew(() =>
                {
                    if (RealViewCacheItemsSource != null)
                        this.listView.VirtualListSize = RealViewCacheItemsSource.Count;
                });
            }, null, 10000, 1000);
        }





        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection col = listView.SelectedIndices;
            if (col.Count < 0)
                return;
            try
            {


                IO_PARA para = RealViewCacheItemsSource[col[0]].Tag as IO_PARA;
                IOMonitorUIManager.SetIOPara(this.Server, this.Communication, this.Device, para);
            }
            catch (Exception ex)
            {
                IOMonitorUIManager.DisplyException(ex);
            }
        }
        /// <summary>
        /// 将读取的数据上传值服务器端后并未在上传日志中显示
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        public void ShowMonitorUploadListView(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, string uploadresult)
        {
            if (IOMonitorManager.IsBackRun)
                return;

            if (ucbReceive.Checked&& device!=null)
            {
                ListViewItem lvi = new ListViewItem(device.GetedValueDate.ToString());
                lvi.SubItems.Add(server.SERVER_NAME);
                lvi.SubItems.Add(communication.IO_COMM_NAME + "[" + communication.IO_COMM_LABEL + "]");
                lvi.SubItems.Add(device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]");
                lvi.SubItems.Add(uploadresult);
                if (device.ReceiveBytes.Length > 0)
                {
                    //将接收的数据转为16字节进行显示
                   if (device.ReceiveBytes.Length<100)
                    lvi.SubItems.Add(CVT.ByteToHexStr(device.ReceiveBytes)+" ");
                   else
                    {
                        byte[] datas = new byte[100];
                        Array.Copy(device.ReceiveBytes, datas,100);
                  
                        lvi.SubItems.Add(CVT.ByteToHexStr(datas)+" ......");
                    }
                }
                else
                {
                    lvi.SubItems.Add("......");
                }
              
                
                RealItemsSource.Push(lvi);
            }

        }
        /// <summary>
        /// 在ListView中显示下置命令
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="command"></param>
        public void InsertMonitorCommandListView(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, IO_PARA para, IO_COMMANDS command)
        {
            if (IOMonitorManager.IsBackRun)
                return;
            if (ucbSendCommand.Checked)
            {
                if (this.IsHandleCreated && listViewSendCommand.InvokeRequired)
                {
                    listViewSendCommand.BeginInvoke(new EventHandler(delegate
                    {

                        ListViewItem lvi = new ListViewItem(command.COMMAND_DATE);
                        lvi.SubItems.Add(command.COMMAND_RESULT);
                        lvi.SubItems.Add(command.COMMAND_VALUE);
                        lvi.SubItems.Add(server.SERVER_NAME);
                        lvi.SubItems.Add(communication.IO_COMM_NAME + "[" + communication.IO_COMM_LABEL + "]");
                        lvi.SubItems.Add(device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]");
                        if (para != null)
                            lvi.SubItems.Add(para.IO_NAME + "[" + para.IO_LABEL + "]");
                        else
                        {
                            lvi.SubItems.Add("未知IO参数");
                        }




                        listViewSendCommand.Items.Insert(0, lvi);
                        if (this.ucLateCommandSize.SelectedValue == "")
                            this.ucLateCommandSize.SelectedValue = "100";
                        if (listViewSendCommand.Items.Count > int.Parse(this.ucLateCommandSize.SelectedValue))
                        {
                            listViewSendCommand.Items.RemoveAt(listViewSendCommand.Items.Count - 1);
                        }

                    }));
                }
            }

        }
        /// <summary>
        /// 报警生产的显示
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="alarm"></param>
        /// <param name="uploadresult"></param>
        public void InsertMonitorAlarmListView(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, IO_PARAALARM alarm, string uploadresult)
        {
            if (IOMonitorManager.IsBackRun)
                return;
            if (uccbRealAlarm.Checked)
            {

                ListViewItem lvi = new ListViewItem(alarm.IO_ALARM_ID);
                lvi.SubItems.Add(alarm.IO_ALARM_DATE);
                lvi.SubItems.Add(alarm.IO_NAME);
                lvi.SubItems.Add(alarm.IO_ALARM_VALUE);
                lvi.SubItems.Add(alarm.IO_ALARM_TYPE);
                lvi.SubItems.Add(alarm.IO_ALARM_LEVEL);
                lvi.SubItems.Add(server.SERVER_ID);
                lvi.SubItems.Add(communication.IO_COMM_NAME + "[" + communication.IO_COMM_LABEL + "]");
                lvi.SubItems.Add(device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]");
                lvi.SubItems.Add(uploadresult);
    
                AlarmItemsSource.Push(lvi);
            }

        }

        
    }
}
