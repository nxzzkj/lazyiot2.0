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
    public partial class ScadaEventQueryWorkForm : DockForm
    {

        private IO_DEVICE Device = null;
        private IO_SERVER Server = null;
        private IO_COMMUNICATION Communication = null;

        public ScadaEventQueryWorkForm()
        {
            InitializeComponent();
            this.Load += RealAlarmQueryWorkForm_Load;

            this.CloseButton = false;

        }
        public ScadaEventQueryWorkForm(Mediator m):base(m)
        {
            InitializeComponent();
            this.Load += RealAlarmQueryWorkForm_Load;

            this.CloseButton = false;

        }

        private void Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.search.Server != null)
            {
                this.Text = this.search.Server.SERVER_NAME;
                    if(this.search.Communication != null)
                {
                    this.Text += "//" + this.search.Communication.IO_COMM_NAME;
                }

                if (this.search.Device != null)
                {
                    this.Text += "//" + this.search.Device.IO_DEVICE_NAME;
                }

         
                this.Device = search.Device;
                this.Communication = this.search.Communication;
                this.Server = this.search.Server;
                ReadMachineTrainHistory();
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

                //初始化多选框

                this.Device = search.Device;
                this.Communication = this.search.Communication;
                this.Server = this.search.Server;
                ReadMachineTrainHistory();
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
            ReadMachineTrainHistory();
        }

  


        private async void ReadMachineTrainHistory()
        {
            try
            {
             
                this.listView.Items.Clear();
              
                //获取读取历史数据
                InfluxDBHistoryData resultData = await IOCenterManager.QueryFormManager.ReadHistoryEvent(this.Server, this.Communication, this.Device, this.search.StartDate, this.search.EndDate, this.search.EventType, this.ucPagerControl.PageSize, this.ucPagerControl.PageIndex);
                if (resultData == null || !resultData.ReturnResult)
                {


                 
                    return;
                }
                //设置页眉控件参数
                this.ucPagerControl.PageCount = resultData.PageCount;
                this.ucPagerControl.RecordCount = resultData.RecordCount;
         
                if (resultData.Seres.Count() > 0)
                {
                 
                    Serie s = resultData.Seres.ElementAt(0);
                    //获取首个时间

                    for (int i = 0; i < s.Values.Count; i++)
                    {   
                      
                        ListViewItem lvi = null;
                        int field_id = s.Columns.IndexOf("field_id");
                        if (field_id >= 0)
                        {
                            lvi = new ListViewItem(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_id]));
                            
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }
                        int tag_sid = s.Columns.IndexOf("tag_sid");
                        if (tag_sid >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][tag_sid]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }
                        ///
                        int field_comm_name = s.Columns.IndexOf("field_comm_name");
                        if (field_comm_name >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_comm_name]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        ///
                        int field_device_name = s.Columns.IndexOf("field_device_name");
                        if (field_device_name >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_device_name]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int field_io_label = s.Columns.IndexOf("field_io_label");
                        if (field_io_label >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_io_label]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int field_io_name = s.Columns.IndexOf("field_io_name");
                        if (field_io_name >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_io_name]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int field_event = s.Columns.IndexOf("field_event");
                        if (field_event >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_event]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int field_date = s.Columns.IndexOf("field_date");
                        if (field_date >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_date]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int field_content = s.Columns.IndexOf("field_content");
                        if (field_content >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_content]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }
                        listView.Items.Add(lvi);




                    }
                }
                this.ucPagerControl.SetSearchInfo(resultData.Msg);
            }
            catch (Exception ex)
            {
                IOCenterManager.QueryFormManager.DisplyException(ex);
            }
        }

  

        public override TabTypes TabType
        {
            get
            {
                return TabTypes.HistoricalEvent;
            }
        }
        

        
    }
}
