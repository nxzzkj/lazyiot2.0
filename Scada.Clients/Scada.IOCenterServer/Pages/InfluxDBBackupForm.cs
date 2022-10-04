using Scada.Controls;
using Scada.Controls.Controls;
using Scada.Controls.Forms;
using Scada.DBUtility;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Temporal.DbAPI;
using Temporal.Net.InfluxDb.Models.Responses;

namespace ScadaCenterServer.Pages
{

     
    /*----------------------------------------------------------------
    // Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
    // 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
    // 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
    // 请大家尊重作者的劳动成果，共同促进行业健康发展。
    // 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
    // 创建者：马勇
    //----------------------------------------------------------------*/
    public partial class InfluxDBBackupForm : DockForm
    {
        public InfluxDBBackupForm()
        {
            InitializeComponent();
        }
        public InfluxDBBackupForm(Mediator m) : base(m)
        {

            InitializeComponent();
        }
        public override TabTypes TabType
        {
            get
            {
                return TabTypes.DatabaseConfiguration;
            }
        }

        private void InfluxDBBackupForm_Load(object sender, EventArgs e)
        {
            this.txtPath.Text = IOCenterManager.IOProject.ServerConfig.Backups.BackupFullPath;
            this.ucCBackupEnable.Checked = IOCenterManager.IOProject.ServerConfig.Backups.Enable;
            dateTimePicker.Value = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd") + " " + IOCenterManager.IOProject.ServerConfig.Backups.BackupTime);
            //设置备份信息
            List<KeyValuePair<string, string>> lstAlarLevelCom = new List<KeyValuePair<string, string>>();
            lstAlarLevelCom.Add(new KeyValuePair<string, string>("每天", "每天备份"));
            lstAlarLevelCom.Add(new KeyValuePair<string, string>("每月", "每月备份（1号开始备份）"));
            lstAlarLevelCom.Add(new KeyValuePair<string, string>("每周", "每周备份(周日开始备份)"));
            this.ucBackupCryle.Source = lstAlarLevelCom;
            ucBackupCryle.SelectedValue = IOCenterManager.IOProject.ServerConfig.Backups.BackupCycle;
            InitPage();

            if (IOCenterManager.InfluxdbBackupManager.IsRun)
            {
                this.ucBtnStart.Enabled = false;
                this.ucBtnStop.Enabled = true;
            }
            else
            {
                this.ucBtnStart.Enabled = true;
                this.ucBtnStop.Enabled = false;
            }


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
        //读取备份日志信息
        private void UcPagerControl_OnPageIndexed(int pageindex)
        {
            ReadBackupHistory();
        }

        private async void ReadBackupHistory()
        {
            this.listView.Items.Clear();
            try
            {
                //获取读取历史数据
                InfluxDBHistoryData resultData = await IOCenterManager.QueryFormManager.ReadBackupHistory(this.ucPagerControl.PageSize, this.ucPagerControl.PageIndex);
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

                if (resultData.Seres.Count() > 0)
                {

                    Serie s = resultData.Seres.ElementAt(0);
                    //获取首个时间

                    for (int i = 0; i < s.Values.Count; i++)
                    {
                        int timeindex = s.Columns.IndexOf("time");
                        string time = UnixDateTimeConvert.ConvertDateTimeInt(Convert.ToDateTime(InfluxDbManager.GetInfluxdbValue(s.Values[i][timeindex]))).ToString();
                        int dateindex = s.Columns.IndexOf("field_backup_date");

                        string date = InfluxDbManager.GetInfluxdbValue(s.Values[i][dateindex]);
                        ListViewItem lvi = new ListViewItem(time);
                        int field_backup_date = s.Columns.IndexOf("field_backup_date");
                        if (field_backup_date >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_backup_date]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        int field_backup_path = s.Columns.IndexOf("field_backup_path");
                        if (field_backup_path >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_backup_path]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }
                        ///
                        int field_backup_file = s.Columns.IndexOf("field_backup_file");
                        if (field_backup_file >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_backup_file]));
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                        }

                        ///
                        int field_backup_result = s.Columns.IndexOf("field_backup_result");
                        if (field_backup_result >= 0)
                        {
                            lvi.SubItems.Add(InfluxDbManager.GetInfluxdbValue(s.Values[i][field_backup_result]));
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

        private void ucBtnPath_BtnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dig = new FolderBrowserDialog();
            dig.ShowNewFolderButton = true;
            if (dig.ShowDialog(this) == DialogResult.OK)
            {
                txtPath.Text = dig.SelectedPath;
            }

        }

        private void ucbtSave_BtnClick(object sender, EventArgs e)
        {
            if (txtPath.Text == "")
            {
                FrmDialog.ShowDialog(this, "请选择备份路径");
                return;
            }
            IOCenterManager.IOProject.ServerConfig.Backups.BackupFullPath = this.txtPath.Text;
            IOCenterManager.IOProject.ServerConfig.Backups.Enable = this.ucCBackupEnable.Checked;
            IOCenterManager.IOProject.ServerConfig.Backups.BackupCycle = ucBackupCryle.SelectedValue;
            IOCenterManager.IOProject.ServerConfig.Backups.BackupTime = dateTimePicker.Value.ToString("HH:mm:ss");

            IOCenterManager.IOProject.ServerConfig.WriteConfig();
            FrmDialog.ShowDialog(this, "保存备份信息成功");


        }

        private void ucBtnRefresh_BtnClick(object sender, EventArgs e)
        {
            ReadBackupHistory();
        }

        private void ucBtnStart_BtnClick(object sender, EventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "是否要启动数据库备份服务?", "备份提醒", true, true, true, true) == DialogResult.OK)
            {
                if (IOCenterManager.InfluxdbBackupManager != null)
                {
                    IOCenterManager.InfluxdbBackupManager.Start();
                }

                this.ucBtnStart.Enabled = false;
                this.ucBtnStop.Enabled = true;
            }


        }

        private void ucBtnStop_BtnClick(object sender, EventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "是否要停止数据库备份服务?", "备份提醒", true, true, true, true) == DialogResult.OK)
            {
                if (IOCenterManager.InfluxdbBackupManager != null)
                {
                    IOCenterManager.InfluxdbBackupManager.Stop();
                }

                this.ucBtnStart.Enabled = true;
                this.ucBtnStop.Enabled = false;
            }

        }
    }
}
