using Scada.Controls;
using Scada.Controls.Forms;
using ScadaCenterServer.Controls;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
    public partial class InfluxConfigForm : DockForm
    {
        public CenterServerConfig Config = null;
        public InfluxConfigForm(Mediator m) : base(m)
        {

            InitializeComponent();

            this.Load += InfluxConfigForm_Load;
        }
        public InfluxConfigForm()
        {
            InitializeComponent();
            this.Load += InfluxConfigForm_Load;
        }
        public override TabTypes TabType
        {
            get
            {
                return TabTypes.DatabaseConfiguration;
            }
        }
        List<InfluxConfigBox> boxs = new List<InfluxConfigBox>();
        private void InfluxConfigForm_Load(object sender, EventArgs e)
        {
            boxs.Clear();
            Config = new CenterServerConfig();
            labelHead.Text = Config.influxdConfig.HeadItem.Description;
            for (int i = 0; i < Config.influxdConfig.HeadItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.HeadItem.Items[i];
                flowLayoutPanelHead.Controls.Add(box);
                boxs.Add(box);
            }
            labelMeta.Text = Config.influxdConfig.MetaItem.Description;
            for (int i = 0; i < Config.influxdConfig.MetaItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.MetaItem.Items[i];
                flowLayoutPanelMeta.Controls.Add(box);
                boxs.Add(box);
            }

            labelData.Text = Config.influxdConfig.DataItem.Description;
            for (int i = 0; i < Config.influxdConfig.DataItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.DataItem.Items[i];
                flowLayoutPanelData.Controls.Add(box);
                boxs.Add(box);
            }

            labelCoordinator.Text = Config.influxdConfig.CoordinatorItem.Description;
            for (int i = 0; i < Config.influxdConfig.CoordinatorItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.CoordinatorItem.Items[i];
                flowLayoutPanelCoordinator.Controls.Add(box);
                boxs.Add(box);
            }


            labelRetention.Text = Config.influxdConfig.RetentionItem.Description;
            for (int i = 0; i < Config.influxdConfig.RetentionItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.RetentionItem.Items[i];
                flowLayoutPanelRetention.Controls.Add(box);
                boxs.Add(box);
            }


            labelShard_Precreation.Text = Config.influxdConfig.Shard_PrecreationItem.Description;
            for (int i = 0; i < Config.influxdConfig.Shard_PrecreationItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.Shard_PrecreationItem.Items[i];
                flowLayoutPanelShard_Precreation.Controls.Add(box);
                boxs.Add(box);
            }

            labelMonitor.Text = Config.influxdConfig.MonitorItem.Description;
            for (int i = 0; i < Config.influxdConfig.MonitorItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.MonitorItem.Items[i];
                flowLayoutPanelMonitor.Controls.Add(box);
                boxs.Add(box);
            }


            labelHttp.Text = Config.influxdConfig.HttpItem.Description;
            for (int i = 0; i < Config.influxdConfig.HttpItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.HttpItem.Items[i];
                flowLayoutPanelHttp.Controls.Add(box);
                boxs.Add(box);
            }


            labelLogging.Text = Config.influxdConfig.LoggingItem.Description;
            for (int i = 0; i < Config.influxdConfig.LoggingItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.LoggingItem.Items[i];
                flowLayoutPanelLogging.Controls.Add(box);
                boxs.Add(box);
            }


            labelSubscriber.Text = Config.influxdConfig.SubscriberItem.Description;
            for (int i = 0; i < Config.influxdConfig.SubscriberItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.SubscriberItem.Items[i];
                flowLayoutPanelSubscriber.Controls.Add(box);
                boxs.Add(box);
            }



            labelContinuous_queries.Text = Config.influxdConfig.Continuous_queriesItem.Description;
            for (int i = 0; i < Config.influxdConfig.Continuous_queriesItem.Items.Count; i++)
            {
                InfluxConfigBox box = new InfluxConfigBox();
                box.Dock = DockStyle.Top;
                box.ConfigItem = Config.influxdConfig.Continuous_queriesItem.Items[i];
                flowLayoutPanelContinuous_queries.Controls.Add(box);
                boxs.Add(box);
            }



        }

        private void ucBtnSave_BtnClick(object sender, EventArgs e)
        {

            if (FrmDialog.ShowDialog(this, "修改配置后需要重新启动服务器，是否要修改配置？", "配置保存", true, false, true, true) == DialogResult.OK)
            {
                for (int i = 0; i < boxs.Count; i++)
                {
                    boxs[i].SaveConfig();
                }
                try
                {
                    Config.WriteConfig();
                    IOCenterManager.QueryFormManager.AddLog("修改数据库配置文件成功");
                    FrmDialog.ShowDialog(this, "修改数据库配置文件成功");
                }
                catch (Exception emx)
                {
                    FrmDialog.ShowDialog(this, emx.Message);

                    IOCenterManager.QueryFormManager.DisplyException(emx);
                }


            }
        }

        private void ucBtnRedo_BtnClick(object sender, EventArgs e)
        {
            if (FrmDialog.ShowDialog(this, "是否要将配置信息恢复为默认值？", "配置保存", true, false, true, true) == DialogResult.OK)
            {
                Config.RecoveryConfig();
                for (int i = 0; i < boxs.Count; i++)
                {
                    boxs[i].ResetConfig();
                }
                try
                {

                    IOCenterManager.QueryFormManager.AddLog("将配置信息恢复为默认值成功");
                    FrmDialog.ShowDialog(this, "将配置信息恢复为默认值成功");
                }
                catch (Exception emx)
                {

                    FrmDialog.ShowDialog(this, emx.Message);
                    IOCenterManager.QueryFormManager.DisplyException(emx);
                }


            }
        }
    }
}
