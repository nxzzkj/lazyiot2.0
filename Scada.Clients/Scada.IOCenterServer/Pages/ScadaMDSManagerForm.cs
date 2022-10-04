using Scada.DBUtility;
using Scada.MDSCore.Settings;
using ScadaCenterServer.Dialogs;
using System;
using System.Windows.Forms;


 
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
    public partial class ScadaMDSManagerForm : DockForm
    {
        public ScadaMDSManagerForm()
        {
            InitializeComponent();
            this.Load += ScadaMDSManagerForm_Load;
        }
        public ScadaMDSManagerForm(Mediator m) : base(m)
        {
            InitializeComponent();
            this.Load += ScadaMDSManagerForm_Load;
        }
        private MDSConfig mDSServerConfig = null;
        private void ScadaMDSManagerForm_Load(object sender, EventArgs e)
        {
            mDSServerConfig = new MDSConfig();

            LoadMDSConfigs();
        }

        /// <summary>
        /// 此处增加采集站授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucBtnAdd_BtnClick(object sender, EventArgs e)
        {
            AddIOMonitorClient addIOMonitor = new AddIOMonitorClient();
            if (addIOMonitor.ShowDialog(this) == DialogResult.OK && addIOMonitor.Station != null)
            {

                if (MDSSettings.Instance.Stations.Exists(x => x.StationName.Trim().ToLower() == addIOMonitor.Station.StationName.Trim().ToLower()
                || x.PhysicalAddress.Trim().ToLower() == addIOMonitor.Station.PhysicalAddress.Trim().ToLower()))
                {
                    MessageBox.Show(this, "采集站名称和IP地址不能重复!");
                    return;
                }
                MDSSettings.Instance.ThisServer.Port = int.Parse(mDSServerConfig.MDSServerPort);
                MDSSettings.Instance.Stations.Add(addIOMonitor.Station);
                MDSSettings.Instance.SaveToXml();
                LoadMDSConfigs();
                MessageBox.Show(this, "保存成功！ 重启数据中心服务有效");
            }
        }

        private void LoadMDSConfigs()
        {
            listView.Items.Clear();
            foreach (IOStationInfoItem infoItem in MDSSettings.Instance.Stations)
            {

                ListViewItem lvi = new ListViewItem(infoItem.StationTitle);
                lvi.Tag = infoItem;
                lvi.SubItems.Add(infoItem.StationName);
                lvi.SubItems.Add(infoItem.PhysicalAddress);
                lvi.SubItems.Add(infoItem.PhysicalMAC);
                listView.Items.Add(lvi);

            }
        }

        /// <summary>
        /// 删除采集站授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                if (MessageBox.Show(this, "是否要删除选中的采集站" + listView.SelectedItems[0].Text, "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IOStationInfoItem serverInfo = (IOStationInfoItem)listView.SelectedItems[0].Tag;
                    listView.Items.Remove(listView.SelectedItems[0]);
                    MDSSettings.Instance.Stations.Remove(serverInfo);
                    ///删除对应的应用程序
                    for (int i = MDSSettings.Instance.Applications.Count - 1; i >= 0; i--)
                    {
                        if (MDSSettings.Instance.Applications[i].StationName.Trim().ToLower() == serverInfo.StationName.Trim().ToLower())
                        {
                            MDSSettings.Instance.Applications.RemoveAt(i);
                        }
                    }
                    MDSSettings.Instance.SaveToXml();
                    MessageBox.Show(this, "保存成功！ 重启数据中心服务有效");
                }
            }
        }
        /// <summary>
        /// 编辑采集站授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucBtnExt2_BtnClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                AddIOMonitorClient addIOMonitor = new AddIOMonitorClient();

                addIOMonitor.Station = (IOStationInfoItem)listView.SelectedItems[0].Tag;
                addIOMonitor.InitFormItem((IOStationInfoItem)listView.SelectedItems[0].Tag);
                string oldIp = addIOMonitor.Station.PhysicalAddress.Replace(".", "").Replace("。", "");
                if (addIOMonitor.ShowDialog(this) == DialogResult.OK && addIOMonitor.Station != null)
                {
                    IOStationInfoItem myStation = (IOStationInfoItem)listView.SelectedItems[0].Tag;
                    myStation= addIOMonitor.Station;
                    MDSSettings.Instance.ThisServer.Port = int.Parse(mDSServerConfig.MDSServerPort);

                    MDSSettings.Instance.SaveToXml();
                    LoadMDSConfigs();
                    MessageBox.Show(this, "保存成功！ 重启数据中心服务有效");
                }
            }
        }
    }
}
