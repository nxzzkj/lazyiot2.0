using Scada.DBUtility;
using Scada.MDSCore.Settings;
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
namespace ScadaCenterServer.Dialogs
{
    public partial class AddIOMonitorClient : Form
    {
        public AddIOMonitorClient()
        {
            InitializeComponent();
            this.Load += AddIOMonitorClient_Load;
        }

        private void AddIOMonitorClient_Load(object sender, EventArgs e)
        {
            try
            {
                mDSServerConfig = new MDSConfig();
            }
            catch (Exception emx)
            {
                MessageBox.Show(emx.Message);

            }
        }

        private MDSConfig mDSServerConfig = null;
        public IOStationInfoItem Station
        {
            set;
            get;
        }
        public void InitFormItem(IOStationInfoItem station)
        {
            mDSServerConfig = new MDSConfig();
            Station = station;
            this.tbName.Text = station.StationName;
            this.tbTitle.Text = station.StationTitle;
            this.tbIP.Text = station.PhysicalAddress;
            this.tbMAC.Text = station.PhysicalMAC;
        }
        private void ucBtnSave_BtnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbName.Text))
            {
                MessageBox.Show(this, "请输入采集站标识");
                return;
            }
            if (string.IsNullOrEmpty(this.tbTitle.Text))
            {
                MessageBox.Show(this, "请输入采集站名称");
                return;
            }
            if (string.IsNullOrEmpty(this.tbIP.Text))
            {
                MessageBox.Show(this, "请输入采集站名称");
                return;
            }
            if (string.IsNullOrEmpty(this.tbMAC.Text))
            {
                MessageBox.Show(this, "请输入采集站物理标识");
                return;
            }
            if (Station == null)
            {
                Station = new IOStationInfoItem()
                {
                    StationName = this.tbName.Text.Trim(),
                    StationTitle = this.tbTitle.Text.Trim(),
                    PhysicalAddress = this.tbIP.Text.Trim(),
                    PhysicalMAC = this.tbMAC.Text.Trim()
                };

            }
            else
            {
                Station.StationName = this.tbName.Text.Trim();
                Station.StationTitle = this.tbTitle.Text.Trim();
                Station.PhysicalAddress = this.tbIP.Text.Trim();

            }


            
            this.DialogResult = DialogResult.OK;
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void tbIP_TextChanged(object sender, EventArgs e)
        {
            this.tbName.Text = mDSServerConfig.IOStationPrefix + "_" + tbIP.Text.Trim().Replace(".", "").Replace("。", "");
        }
    }
}
