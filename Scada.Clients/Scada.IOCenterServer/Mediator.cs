using Scada.Controls;
using ScadaCenterServer.Pages;
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
namespace ScadaCenterServer
{
    public class Mediator
    {

        #region Constructor
        public Mediator(Form main)
        {
            this.parent = main;
            tabFactory = new TabFactory(this);
            tabFactory.OnTabCreation += new TabInfo(OnTabCall);
            rnd = new Random();
        }

        #endregion
        protected Random rnd;
        private TabFactory tabFactory;
        internal Form parent;
        private DockContent lastAdded;
        private DockPanel dockPanel;
        public DockPanel DockPanel
        {
            get { return dockPanel; }
            set { dockPanel = value; }
        }

        #region  IO目录树
        public IOTreeForm IOTreeForm
        {
            get
            {
                if (tabFactory.IOTreeForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.IOTreeForm;
                }
            }
        }
        public void OpenIOTreeForm()
        {

            lastAdded = tabFactory.GetTab(new TabCodon("IO目录", "IO目录", TabTypes.IOCatalog)) as IOTreeForm;
            lastAdded.TabText = "IO目录";
            lastAdded.Text = "IO目录";

            OnShowTab(lastAdded);
        }
        #endregion
        #region   机器训练任务，自动控制任务目录
        public IOTaskForm IOTaskForm
        {
            get
            {
                if (tabFactory.IOTreeForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.IOTaskForm;
                }
            }
        }
        public void OpenIOTaskForm()
        {

            lastAdded = tabFactory.GetTab(new TabCodon("任务列表", "任务列表", TabTypes.IOTask)) as IOTaskForm;
            lastAdded.TabText = "任务列表";
            lastAdded.Text = "任务列表";

            OnShowTab(lastAdded);
        }
        #endregion
        
        #region  
        public IOPropeitesForm IOPropeitesForm
        {
            get
            {
                if (tabFactory.IOPropeitesForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.IOPropeitesForm;
                }
            }
        }
        public void OpenIOPropeitesForm()
        {

            lastAdded = tabFactory.GetTab(new TabCodon("属性", "属性", TabTypes.Property)) as IOPropeitesForm;
            lastAdded.TabText = "属性";
            lastAdded.Text = "属性";

            OnShowTab(lastAdded);
        }
        #endregion
        #region   打开日志
        public OperatorLogForm OperatorLogForm
        {
            get
            {
                if (tabFactory.OperatorLogForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.OperatorLogForm;
                }
            }
        }
        public void OpenOperatorLogForm()
        {

            lastAdded = tabFactory.GetTab(new TabCodon("日志", "日志", TabTypes.Logbook)) as OperatorLogForm;
            lastAdded.TabText = "日志";
            lastAdded.Text = "日志";


            OnShowTab(lastAdded);
        }
        #endregion
        #region  实时数据
        public RealQueryWorkForm RealQueryWorkForm
        {
            get
            {
                if (tabFactory.RealQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.RealQueryWorkForm;
                }
            }
        }
        public void OpenRealQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("实时数据", "实时数据", TabTypes.RealTimeData)) as RealQueryWorkForm;
            lastAdded.TabText = "实时数据";
            lastAdded.Text = "实时数据";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史数据查询
        public HistoryQueryWorkForm HistoryQueryWorkForm
        {
            get
            {
                if (tabFactory.HistoryQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.HistoryQueryWorkForm;
                }
            }
        }
        public void OpenHistoryQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("历史数据", "历史数据", TabTypes.HistoricalData)) as HistoryQueryWorkForm;
            lastAdded.TabText = "历史数据";
            lastAdded.Text = "历史数据";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史报警查询
        public HistoryAlarmQueryWorkForm HistoryAlarmQueryWorkForm
        {
            get
            {
                if (tabFactory.HistoryAlarmQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.HistoryAlarmQueryWorkForm;
                }
            }
        }
        public void OpenHistoryAlarmQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("历史报警", "历史报警", TabTypes.HistoricalAlarm)) as HistoryAlarmQueryWorkForm;
            lastAdded.TabText = "历史报警";
            lastAdded.Text = "历史报警";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史事件查询
        public ScadaEventQueryWorkForm ScadaEventQueryWorkForm
        {
            get
            {
                if (tabFactory.ScadaEventQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.ScadaEventQueryWorkForm;
                }
            }
        }
        public void OpenScadaEventQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("历史事件", "历史事件", TabTypes.HistoricalEvent)) as ScadaEventQueryWorkForm;
            lastAdded.TabText = "历史事件";
            lastAdded.Text = "历史事件";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史机器训练日志
        public ScadaMachineTrainQueryWorkForm ScadaMachineTrainQueryWorkForm
        {
            get
            {
                if (tabFactory.ScadaMachineTrainQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.ScadaMachineTrainQueryWorkForm;
                }
            }
        }
        public void OpenScadaMachineTrainQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("历史机器训练日志", "历史机器训练日志", TabTypes.HistoricalMachineTrain)) as ScadaMachineTrainQueryWorkForm;
            lastAdded.TabText = "历史机器训练日志";
            lastAdded.Text = "历史机器训练日志";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史机器训练预测查询
        public ScadaMachineTrainForeastQueryWorkForm ScadaMachineTrainForeastQueryWorkForm
        {
            get
            {
                if (tabFactory.ScadaMachineTrainForeastQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.ScadaMachineTrainForeastQueryWorkForm;
                }
            }
        }
        public void OpenScadaMachineTrainForeastQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("机器预测查询", "机器预测查询", TabTypes.HistoricalMachineTrainForeast)) as ScadaMachineTrainForeastQueryWorkForm;
            lastAdded.TabText = "机器预测查询";
            lastAdded.Text = "机器预测查询";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史自动控制命令执行日志
        public ScadaBatchCommandTaskForm ScadaBatchCommandTaskForm
        {
            get
            {
                if (tabFactory.ScadaBatchCommandTaskForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.ScadaBatchCommandTaskForm;
                }
            }
        }
        public void OpenScadaBatchCommandTaskForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("自动控制命令执行记录", "自动控制命令执行记录", TabTypes.HistoricalBatchCommandTask)) as ScadaBatchCommandTaskForm;
            lastAdded.TabText = "自动控制命令执行记录";
            lastAdded.Text = "自动控制命令执行记录";
            OnShowTab(lastAdded);

        }
        #endregion

        


        #region  历史下置查询
        public SendCommandQueryWorkForm SendCommandQueryWorkForm
        {
            get
            {
                if (tabFactory.SendCommandQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.SendCommandQueryWorkForm;
                }
            }
        }
        public void OpenSendCommandQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("下置查询", "下置查询", TabTypes.LowerQuery)) as SendCommandQueryWorkForm;
            lastAdded.TabText = "下置查询";
            lastAdded.Text = "下置查询";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史报警配置日志
        public AlarmConfigQueryWorkForm AlarmConfigQueryWorkForm
        {
            get
            {
                if (tabFactory.AlarmConfigQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.AlarmConfigQueryWorkForm;
                }
            }
        }
        public void OpenAlarmConfigQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("报警配置日志", "报警配置日志", TabTypes.AlarmConfigurationLog)) as AlarmConfigQueryWorkForm;
            lastAdded.TabText = "报警配置日志";
            lastAdded.Text = "报警配置日志";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  历史统计
        public HistoryStaticsQueryWorkForm HistoryStaticsQueryWorkForm
        {
            get
            {
                if (tabFactory.HistoryStaticsQueryWorkForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.HistoryStaticsQueryWorkForm;
                }
            }
        }
        public void OpenHistoryStaticsQueryWorkForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("历史统计", "历史统计", TabTypes.HistoricalStatistics)) as HistoryStaticsQueryWorkForm;
            lastAdded.TabText = "历史统计";
            lastAdded.Text = "历史统计";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  数据库备份
        public InfluxDBBackupForm InfluxDBBackupForm
        {
            get
            {
                if (tabFactory.InfluxDBBackupForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.InfluxDBBackupForm;
                }
            }
        }
        public void OpenInfluxDBBackupForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("数据库备份", "数据库备份", TabTypes.DatabaseBackup)) as InfluxDBBackupForm;
            lastAdded.TabText = "数据库备份";
            lastAdded.Text = "数据库备份";
            OnShowTab(lastAdded);

        }
        #endregion
        #region  采集站授权
        public ScadaMDSManagerForm ScadaMDSManagerForm
        {
            get
            {
                if (tabFactory.ScadaMDSManagerForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.ScadaMDSManagerForm;
                }
            }
        }
        public void OpenScadaMDSManagerForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("采集站授权", "采集站授权", TabTypes.MonitoAuthorization)) as ScadaMDSManagerForm;
            lastAdded.TabText = "采集站授权";
            lastAdded.Text = "采集站授权";
            OnShowTab(lastAdded);

        }
        #endregion


        #region  
        public InfluxConfigForm InfluxConfigForm
        {
            get
            {
                if (tabFactory.InfluxConfigForm == null)
                {
                    return null;
                }
                else
                {
                    return tabFactory.InfluxConfigForm;
                }
            }
        }
        public void OpenInfluxConfigForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("时序数据库配置", "时序数据库配置", TabTypes.DatabaseConfiguration)) as InfluxConfigForm;
            lastAdded.TabText = "时序数据库配置";
            lastAdded.Text = "时序数据库配置";
            OnShowTab(lastAdded);
        }
        #endregion

        private void OnTabCall(DockContent tab)
        {
            TabTypes type = (tab as ICobaltTab).TabType;
            switch (type)
            {

                case TabTypes.Logbook:
                    tab.Show(dockPanel, DockState.DockBottom);
                    break;
                case TabTypes.Property:
                    tab.Show(dockPanel, DockState.DockRight);
                    break;
                case TabTypes.IOCatalog:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
                case TabTypes.IOTask:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
                case TabTypes.PointArea:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.DatabaseConfiguration:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.RealTimeData:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalData:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalAlarm:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.LowerQuery:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.AlarmConfigurationLog:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalStatistics:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.DatabaseBackup:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.MessageServiceConfiguration:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.MonitoAuthorization:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalEvent:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalMachineTrain:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalMachineTrainForeast:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.HistoricalBatchCommandTask:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                default:
                    {
                        tab.Show(dockPanel, DockState.Document);
                        break;
                    }

            }
        }

        private void OnShowTab(DockContent tab)
        {
            if (tab == null)
            {
                return;
            }

            TabTypes type = (tab as ICobaltTab).TabType;
            tab.Show(dockPanel, tab.DockState);

        }


    }
}
