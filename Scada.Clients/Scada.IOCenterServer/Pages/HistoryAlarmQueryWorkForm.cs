using Scada.Controls;
using Scada.Controls.Controls;
using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.Model;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Temporal.DbAPI;
using Temporal.Net.InfluxDb.Models.Responses;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaCenterServer.Pages
{
    public partial class HistoryAlarmQueryWorkForm : DockForm
    {

        private IO_DEVICE Device = null;
        private IO_SERVER Server = null;
        private IO_COMMUNICATION Communication = null;

        public HistoryAlarmQueryWorkForm()
        {
            InitializeComponent();
            this.Load += RealAlarmQueryWorkForm_Load;

            this.CloseButton = false;

        }

        private void Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.search.Server != null && this.search.Communication != null && this.search.Device != null)
            {
                this.Text = this.search.Server.SERVER_NAME + "//" + this.search.Communication.IO_COMM_NAME + "//" + this.search.Device.IO_DEVICE_NAME;
                IO_DEVICE selectDevice = sender as IO_DEVICE;
                //初始化多选框

                Device = selectDevice;
                ReadAlarmHistory();
            }

        }

        /// <summary>
        /// 用户点击查询按钮进行数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_SearchClick(object sender, EventArgs e)
        {
            if (sender != null)
            {
                IO_DEVICE selectDevice = sender as IO_DEVICE;
                //初始化多选框

                Device = selectDevice;
                ReadAlarmHistory();
            }

        }





        /// <summary>
        /// 加载目录树结构
        /// </summary>
        public async void InitTreeProject()
        {
            await TaskHelper.Factory.StartNew(() =>
            {

                search.LoadTreeProject();

            });

        }

        private void RealAlarmQueryWorkForm_Load(object sender, EventArgs e)
        {
            InitTreeProject();

            //初始化分页控件
            InitPage();
        }
        private void InitPage()
        {
            List<PageSizeItem> lstCom = new List<PageSizeItem>();


            lstCom.Add(new PageSizeItem(200, "每页200条"));
            lstCom.Add(new PageSizeItem(500, "每页500条"));
            lstCom.Add(new PageSizeItem(1000, "每页1000条"));
            lstCom.Add(new PageSizeItem(2000, "每页2000条"));
            lstCom.Add(new PageSizeItem(5000, "每页5000条"));
            lstCom.Add(new PageSizeItem(8000, "每页8000条"));
            lstCom.Add(new PageSizeItem(10000, "每页10000条"));
            this.ucPagerControl.SetPageItems(lstCom, 3);
            this.ucPagerControl.OnPageIndexed += UcPagerControl_OnPageIndexed;


        }

        private void UcPagerControl_OnPageIndexed(int pageindex)
        {
            ReadAlarmHistory();
        }

        public void InitDevice(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device)
        {

            Device = device;
            Server = server;
            Communication = communication;
            this.search.StartDate = DateTime.Now.AddDays(-10);
            this.search.EndDate = DateTime.Now;


            if (Device != null)
            {
                ///设置下拉菜单选中项
                this.search.SetSelectItem(this.Server, this.Communication, this.Device);

                ReadAlarmHistory();

            }





        }


        private async void ReadAlarmHistory()
        {
            try
            {
                if (Device == null)
                {

                    return;
                }
                this.listView.Items.Clear();
                IO_DEVICE copyDevice = this.Device.Copy();
                //获取读取历史数据
                InfluxDBHistoryData resultData = await IOCenterManager.QueryFormManager.ReadHistoryAlarmDevice(this.Server, this.Communication, copyDevice, this.search.StartDate, this.search.EndDate, this.search.AlarmType, this.search.AlarmLevel, this.ucPagerControl.PageSize, this.ucPagerControl.PageIndex);
                if (resultData == null || !resultData.ReturnResult)
                {
                    if (resultData != null)
                    {
                        FrmDialog.ShowDialog(this, resultData.Msg);
                    }
                    else
                    {
                        FrmDialog.ShowDialog(this, "查询失败");
                    }

                    return;
                }
                //设置页眉控件参数
                this.ucPagerControl.PageCount = resultData.PageCount;
                this.ucPagerControl.RecordCount = resultData.RecordCount;
                //删除曲线


                //循环读取每个历史数据
                //首先构造ListColumn



                if (resultData.Seres.Count() > 0)
                {
                    //field_io_alarm_date
                    //field_io_alarm_disposalidea
                    //field_io_alarm_disposaluser
                    //field_io_alarm_level
                    //field_io_alarm_type
                    //field_io_alarm_value
                    //field_io_label
                    //field_io_name
                    Serie s = resultData.Seres.ElementAt(0);
                    //获取首个时间

                    for (int i = 0; i < s.Values.Count; i++)
                    {
                        int timeindex = s.Columns.IndexOf("time");
                        string time = UnixDateTimeConvert.ConvertDateTimeInt(Convert.ToDateTime(InfluxDbManager.GetInfluxdbValue(s.Values[i][timeindex]))).ToString();
                        int dateindex = s.Columns.IndexOf("field_io_alarm_date");

                        string date = InfluxDbManager.GetInfluxdbValue(s.Values[i][dateindex]);
                        ListViewItem lvi = new ListViewItem(time);
                        int io_name_index = s.Columns.IndexOf("field_io_name");
                        if (io_name_index >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_name_index]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }
                        ///
                        int io_name_label = s.Columns.IndexOf("field_io_label");
                        if (io_name_label >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_name_label]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        ///
                        int io_alarm_value = s.Columns.IndexOf("field_io_alarm_value");
                        if (io_alarm_value >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_alarm_value]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int io_alarm_date = s.Columns.IndexOf("field_io_alarm_date");
                        if (io_alarm_date >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_alarm_date]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int io_alarm_level = s.Columns.IndexOf("field_io_alarm_level");
                        if (io_alarm_level >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_alarm_level]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int io_alarm_type = s.Columns.IndexOf("field_io_alarm_type");
                        if (io_alarm_type >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_alarm_type]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int io_alarm_disposalidea = s.Columns.IndexOf("field_io_alarm_disposalidea");
                        if (io_alarm_disposalidea >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_alarm_disposalidea]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int io_alarm_disposaluser = s.Columns.IndexOf("field_io_alarm_disposaluser");
                        if (io_alarm_disposaluser >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][io_alarm_disposaluser]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }
                        listView.Items.Add(lvi);




                    }
                }
            }
            catch (Exception ex)
            {
                IOCenterManager.QueryFormManager.DisplyException(ex);
            }
        }

        public HistoryAlarmQueryWorkForm(Mediator m, IO_DEVICE mDevice) : base(m)
        {

            InitializeComponent();
            Device = mDevice;
            this.Load += RealAlarmQueryWorkForm_Load;

        }

        public override TabTypes TabType
        {
            get
            {
                return TabTypes.HistoricalAlarm;
            }
        }
        //分页事件
        private void ucPagerControl_ShowSourceChanged(object currentSource)
        {

        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            if (e.Column >= 0 && this.listView.Columns[e.Column].Tag != null)
            {
                this.mediator.IOPropeitesForm.SetPara(this.Server, this.Communication, this.Device, this.listView.Columns[e.Column].Tag as IO_PARA);
            }

        }
    }
}
