
using Scada.Controls;
using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.Model;
using ScadaCenterServer.Controls;
using ScadaCenterServer.Core;
using ScadaCenterServer.Dialogs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
namespace ScadaCenterServer
{


    public partial class MonitorForm : Form
    {

 
        protected List<ListViewItem> EventCacheItemsSource
        {
            get;
            private set;
        }
        protected ConcurrentStack<ListViewItem> EventItemsSource
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
        protected List<ListViewItem> AiTrainCacheItemsSource
        {
            get;
            private set;
        }
        protected List<ListViewItem> BatchCommandCacheItemsSource
        {
            get;
            private set;
        }
        protected ConcurrentStack<ListViewItem> RealItemsSource
        {
            get;
            private set;
        }
        protected ConcurrentStack<ListViewItem> AiTrainItemsSource
        {
            get;
            private set;
        }
        protected ConcurrentStack<ListViewItem> BatchCommandItemsSource
        {
            get;
            private set;
        }
        protected ConcurrentStack<ListViewItem> AlarmItemsSource
        {
            get;
            private set;
        }
     
        public MonitorForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
           
        }

        

        

        
        System.Threading.Timer listTimer = null;
        private void ListViewBatchCommand_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try
            {

                if (this.BatchCommandCacheItemsSource == null || this.BatchCommandCacheItemsSource.Count == 0)
                {
                    return;
                }

                e.Item = this.BatchCommandCacheItemsSource[e.ItemIndex];
                if (e.ItemIndex == this.BatchCommandCacheItemsSource.Count)
                {
                    this.BatchCommandCacheItemsSource = null;

                }

            }
            catch
            {

            }
        }
        private void ListViewAlarm_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try { 
           
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
            catch
            {

            }
        }

        private void ListViewReport_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try { 
            
                if (this.EventCacheItemsSource == null || this.EventCacheItemsSource.Count == 0)
                {
                    return;
                }

                e.Item = this.EventCacheItemsSource[e.ItemIndex];
                if (e.ItemIndex == this.EventCacheItemsSource.Count)
                {
                    this.EventCacheItemsSource = null;

                }
            }
            catch
            {

            }

        }

        private void ListViewReceive_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try
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
            catch
            {

            }
           
        }

        private void MachineTrainListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try
            {
                if (this.AiTrainCacheItemsSource == null || this.AiTrainCacheItemsSource.Count == 0)
                {
                    return;
                }

                e.Item = this.AiTrainCacheItemsSource[e.ItemIndex];
                if (e.ItemIndex == this.AiTrainCacheItemsSource.Count)
                {
                    this.AiTrainCacheItemsSource = null;
                }
            }
            catch
            {

            }
        }
        private void ClientForm_Load(object sender, EventArgs e)
        {


            labelIP.Text = LocalIp.GetLocalIp();

            ControlHelper.FreezeControl(this, true);
            this.listViewReport.VirtualMode = true;
            listViewReport.VirtualListSize = 0;
            this.machineTrainListView.VirtualMode = true;
            this.machineTrainListView.VirtualListSize = 0;

            this.listViewBatchCommand.VirtualMode = true;
            this.listViewBatchCommand.VirtualListSize = 0;
            
            listViewReceive.VirtualMode = true;
            listViewReceive.VirtualListSize = 0;
            this.listViewAlarm.VirtualListSize = 0;
            listViewAlarm.VirtualMode = true;
            listViewReceive.RetrieveVirtualItem += ListViewReceive_RetrieveVirtualItem;
            listViewAlarm.RetrieveVirtualItem += ListViewAlarm_RetrieveVirtualItem;
            listViewReport.RetrieveVirtualItem += ListViewReport_RetrieveVirtualItem;
            machineTrainListView.RetrieveVirtualItem += MachineTrainListView_RetrieveVirtualItem;
            listViewBatchCommand.RetrieveVirtualItem += ListViewBatchCommand_RetrieveVirtualItem;

            
            RealCacheItemsSource = new List<ListViewItem>();
            AlarmCacheItemsSource = new List<ListViewItem>();
            RealItemsSource = new ConcurrentStack<ListViewItem>();
            AlarmItemsSource = new ConcurrentStack<ListViewItem>();
            EventCacheItemsSource = new List<ListViewItem>();
            EventItemsSource = new ConcurrentStack<ListViewItem>();
            this.AiTrainCacheItemsSource = new List<ListViewItem>();
            this.AiTrainItemsSource = new ConcurrentStack<ListViewItem>();

            this.BatchCommandCacheItemsSource = new List<ListViewItem>();
            this.BatchCommandItemsSource = new ConcurrentStack<ListViewItem>();
            listTimer = new System.Threading.Timer(delegate
            {
                try
                {

                    //定时加载日志




                    TaskHelper.Factory.StartNew(() =>
                    {
                        try
                        {
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
                        try
                        {
                            EventCacheItemsSource = EventItemsSource.ToList();
                            if (EventItemsSource.Count > 2000)
                            {
                                EventItemsSource.Clear();
                                this.listViewReport.VirtualListSize = 0;
                            }
                            else
                            {
                                this.listViewReport.VirtualListSize = EventCacheItemsSource.Count;
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
                            AiTrainCacheItemsSource = AiTrainItemsSource.ToList();
                            if (AiTrainItemsSource.Count > 2000)
                            {
                                AiTrainItemsSource.Clear();
                                this.machineTrainListView.VirtualListSize = 0;
                            }
                            else
                            {
                                this.machineTrainListView.VirtualListSize = AiTrainCacheItemsSource.Count;
                            }
                        }
                        catch
                        {

                        }
                    });

                }
                catch
                {

                }

            }, null, 1000, 2000);

        }
        public void AddAiTrain(string appname, string datetime, string server, string communication, string device,  string taskName,string Algorithm,string AlgorithmType,string PredictedLabel,string Score,string ColumnValue,string Columns)
        {
            if (IOCenterManager.IsBackRun)
            {
                return;
            }

            if (!this.ucCheckBoxAi.Checked)
            {
                return;
            }

            ListViewItem lvi = new ListViewItem(appname);
            lvi.SubItems.Add(datetime);
            lvi.SubItems.Add(PredictedLabel);
            lvi.SubItems.Add(ColumnValue);
            lvi.SubItems.Add(Columns);
            lvi.SubItems.Add(Score);
            lvi.SubItems.Add(communication);
            lvi.SubItems.Add(device);
            lvi.SubItems.Add(taskName);
            lvi.SubItems.Add(Algorithm);
            lvi.SubItems.Add(AlgorithmType);

            this.AiTrainItemsSource.Push(lvi);
        }

        public void AddBatchCommand(string appname, string datetime, string server, string taskName, string taskType, string result, string remark)
        {
            if (IOCenterManager.IsBackRun)
            {
                return;
            }

            if (!this.ucCheckBoxBatchCommand.Checked)
            {
                return;
            }

            ListViewItem lvi = new ListViewItem(appname);
            lvi.SubItems.Add(taskName);
            lvi.SubItems.Add(datetime);
            lvi.SubItems.Add(result);
            lvi.SubItems.Add(taskType);
            lvi.SubItems.Add(remark);
            

            this.BatchCommandItemsSource.Push(lvi);
        }

        public void AddReeiveDevice(string appname, string datetime, string server, string communication, string device, string msg, bool result)
        {
            if (IOCenterManager.IsBackRun)
            {
                return;
            }

            if (!ucReceive.Checked)
            {
                return;
            }

            ListViewItem lvi = new ListViewItem(appname);
            lvi.SubItems.Add(datetime);
            lvi.SubItems.Add(device);
            lvi.SubItems.Add(communication);
            lvi.SubItems.Add(server);
            if (msg.Length > 900)
            {
                lvi.SubItems.Add(msg.Substring(0, 899) + "......");
            }
            else
            {
                lvi.SubItems.Add(msg);
            }

            if (result)
            {
                lvi.SubItems.Add("入库成功");

            }
            else
            {
                lvi.SubItems.Add("入库失败");
            }

            RealItemsSource.Push(lvi);
        }
        public void AddReeiveAlarm(string appname, string server, string communication, string device, IO_PARAALARM alarm, bool result)
        {
            if (IOCenterManager.IsBackRun)
            {
                return;
            }
  

            if (!ucEnableAlarm.Checked)
            {
                return;
            }

            ListViewItem lvi = new ListViewItem(appname);
            lvi.SubItems.Add(alarm.IO_ALARM_DATE);
            lvi.SubItems.Add(alarm.IO_LABEL + "[" + alarm.IO_NAME + "]");
            lvi.SubItems.Add(alarm.IO_ALARM_VALUE);
            lvi.SubItems.Add(alarm.IO_ALARM_TYPE);
            lvi.SubItems.Add(alarm.IO_ALARM_LEVEL);
            lvi.SubItems.Add(server);

            lvi.SubItems.Add(communication);
            lvi.SubItems.Add(device);
            if (result)
            {
                lvi.SubItems.Add("入库成功");
            }
            else
            {
                lvi.SubItems.Add("入库失败");
            }

            AlarmItemsSource.Push(lvi);
        }
        /// <summary>
        /// 增加一个事件报告
        /// </summary>
        /// <param name="application"></param>
        /// <param name="msg"></param>
        public void AddEvent(string application, string eventdate, string eventname, string server, string communication, string device, string msg)
        {
            if (IOCenterManager.IsBackRun)
            {
                return;
            }

            if (!this.ucLog.Checked)
            {
                return;
            }

            ListViewItem lvi = new ListViewItem(eventdate);
            lvi.SubItems.Add(eventname);
            lvi.SubItems.Add(application + "  " + server);
            lvi.SubItems.Add(server);
            lvi.SubItems.Add(communication);
            lvi.SubItems.Add(device);
            lvi.SubItems.Add(msg);
            EventItemsSource.Push(lvi);




        }
        public void AddCommand(string appname, string server, string communication, string device, string para, IO_COMMANDS command)
        {
            if (IOCenterManager.IsBackRun)
            {
                return;
            }

            if (ucbSendCommand.Checked && listViewSendCommand.InvokeRequired && listViewSendCommand.IsHandleCreated)
            {

                listViewSendCommand.BeginInvoke(new EventHandler(delegate
            {
                try
                {


                    ListViewItem lvi = new ListViewItem(command.COMMAND_DATE);
                    lvi.SubItems.Add(server);
                    lvi.SubItems.Add(communication);
                    lvi.SubItems.Add(device);
                    lvi.SubItems.Add(para);
                    lvi.SubItems.Add(command.COMMAND_VALUE);
                    lvi.SubItems.Add(command.COMMAND_RESULT);

                    lvi.SubItems.Add(command.COMMAND_USER);
                    this.listViewSendCommand.Items.Insert(0, lvi);
                    if (this.listViewSendCommand.Items.Count > int.Parse(this.cbSendCommandSize.SelectedValue))
                    {
                        this.listViewSendCommand.Items.RemoveAt(this.listViewSendCommand.Items.Count - 1);
                    }
                    Scada.Logger.Logger.GetInstance().Info(command.COMMAND_DATE + "   " + command.GetCommandString());
                }
                catch
                {

                }
            }));

            }

        }
       
        //主动下置命令
        
 

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }


    }
    public delegate void SendMessageHandle(string ip, string msg);
}
