using Scada.Controls;
using ScadaCenterServer.Pages;


 
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
    /// <summary>
    /// 
    /// </summary>
    public class TabFactory
    {
        public event TabInfo OnTabCreation;
        private Mediator mediator;
        #region IO目录
        /// <summary>
        ///
        /// </summary>
        private IOTreeForm _IOTreeForm;
        public IOTreeForm IOTreeForm
        {
            get
            {
                CreateIOTreeForm(new TabCodon("IO目录", "IO目录", TabTypes.IOCatalog));
                return _IOTreeForm;
            }
        }
        private void CreateIOTreeForm(TabCodon tabCodon)
        {
            if (_IOTreeForm == null || _IOTreeForm.IsDisposed)
            {
                _IOTreeForm = new IOTreeForm(this.mediator);
                _IOTreeForm.Text = "采集监视";
                _IOTreeForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_IOTreeForm);
            }


        }

        #endregion
        #region 机器训练任务，自动控制任务目录树
        /// <summary>
        ///
        /// </summary>
        private IOTaskForm _IOTaskForm;
        public IOTaskForm IOTaskForm
        {
            get
            {
                CreateIOTaskForm(new TabCodon("IO目录", "IO目录", TabTypes.IOCatalog));
                return _IOTaskForm;
            }
        }
        private void CreateIOTaskForm(TabCodon tabCodon)
        {
            if (_IOTaskForm == null || _IOTaskForm.IsDisposed)
            {
                _IOTaskForm = new IOTaskForm(this.mediator);
                _IOTaskForm.Text = "任务列表";
                _IOTaskForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_IOTaskForm);
            }


        }

        #endregion
        #region 属性
        /// <summary>
        ///
        /// </summary>
        private IOPropeitesForm _IOPropeitesForm;
        public IOPropeitesForm IOPropeitesForm
        {
            get
            {
                CreateIOPropeitesForm(new TabCodon("属性", "属性", TabTypes.Property));
                return _IOPropeitesForm;
            }
        }
        private void CreateIOPropeitesForm(TabCodon tabCodon)
        {
            if (_IOPropeitesForm == null || _IOPropeitesForm.IsDisposed)
            {
                _IOPropeitesForm = new IOPropeitesForm(this.mediator);
                _IOPropeitesForm.Text = "属性";
                _IOPropeitesForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_IOPropeitesForm);
            }


        }

        #endregion
        #region 日志
        /// <summary>
        ///
        /// </summary>
        private OperatorLogForm _OperatorLogForm;
        public OperatorLogForm OperatorLogForm
        {
            get
            {
                CreateOperatorLogForm(new TabCodon("日志", "日志", TabTypes.Logbook));
                return _OperatorLogForm;
            }
        }
        private void CreateOperatorLogForm(TabCodon tabCodon)
        {
            if (_OperatorLogForm == null || _OperatorLogForm.IsDisposed)
            {
                _OperatorLogForm = new OperatorLogForm(this.mediator);
                _OperatorLogForm.Text = "日志";
                _OperatorLogForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_OperatorLogForm);
            }


        }

        #endregion
        #region 实时数据
        /// <summary>
        ///
        /// </summary>
        private RealQueryWorkForm _RealQueryWorkForm;
        public RealQueryWorkForm RealQueryWorkForm
        {
            get
            {
                CreateRealQueryWorkForm(new TabCodon("实时数据", "实时数据", TabTypes.RealTimeData));
                return _RealQueryWorkForm;
            }
        }
        private void CreateRealQueryWorkForm(TabCodon tabCodon)
        {
            if (_RealQueryWorkForm == null || _RealQueryWorkForm.IsDisposed)
            {
                _RealQueryWorkForm = new RealQueryWorkForm(this.mediator);
                _RealQueryWorkForm.Text = "实时数据";
                _RealQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_RealQueryWorkForm);

            }


        }

        #endregion
        #region 历史数据
        /// <summary>
        ///
        /// </summary>
        private HistoryQueryWorkForm _HistoryQueryWorkForm;
        public HistoryQueryWorkForm HistoryQueryWorkForm
        {
            get
            {
                CreateHistoryQueryWorkForm(new TabCodon("历史数据", "历史数据", TabTypes.HistoricalData));
                return _HistoryQueryWorkForm;
            }
        }
        private void CreateHistoryQueryWorkForm(TabCodon tabCodon)
        {
            if (_HistoryQueryWorkForm == null || _HistoryQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _HistoryQueryWorkForm = new HistoryQueryWorkForm(this.mediator, null);
                _HistoryQueryWorkForm.Text = "历史数据";
                _HistoryQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_HistoryQueryWorkForm);

            }


        }

        #endregion
        #region 历史报警
        /// <summary>
        ///
        /// </summary>
        private HistoryAlarmQueryWorkForm _HistoryAlarmQueryWorkForm;
        public HistoryAlarmQueryWorkForm HistoryAlarmQueryWorkForm
        {
            get
            {
                CreateHistoryAlarmQueryWorkForm(new TabCodon("历史报警", "历史报警", TabTypes.HistoricalAlarm));
                return _HistoryAlarmQueryWorkForm;
            }
        }
        private void CreateHistoryAlarmQueryWorkForm(TabCodon tabCodon)
        {
            if (_HistoryAlarmQueryWorkForm == null || _HistoryAlarmQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _HistoryAlarmQueryWorkForm = new HistoryAlarmQueryWorkForm(this.mediator, null);
                _HistoryAlarmQueryWorkForm.Text = "历史报警";
                _HistoryAlarmQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_HistoryAlarmQueryWorkForm);

            }


        }

        #endregion
        #region 历史事件
        /// <summary>
        ///
        /// </summary>
        private ScadaEventQueryWorkForm _ScadaEventQueryWorkForm;
        public ScadaEventQueryWorkForm ScadaEventQueryWorkForm
        {
            get
            {
                CreateScadaEventQueryWorkForm(new TabCodon("历史事件", "历史事件", TabTypes.HistoricalEvent));
                return _ScadaEventQueryWorkForm;
            }
        }
        private void CreateScadaEventQueryWorkForm(TabCodon tabCodon)
        {
            if (_ScadaEventQueryWorkForm == null || _ScadaEventQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _ScadaEventQueryWorkForm = new ScadaEventQueryWorkForm(this.mediator);
                _ScadaEventQueryWorkForm.Text = "历史事件";
                _ScadaEventQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_ScadaEventQueryWorkForm);

            }


        }

        #endregion
        #region 机器训练执行日志
        /// <summary>
        ///
        /// </summary>
        private ScadaMachineTrainQueryWorkForm _ScadaMachineTrainQueryWorkForm;
        public ScadaMachineTrainQueryWorkForm ScadaMachineTrainQueryWorkForm
        {
            get
            {
                CreateScadaMachineTrainQueryWorkForm(new TabCodon("历史机器训练日志", "历史机器训练日志", TabTypes.HistoricalMachineTrain));
                return _ScadaMachineTrainQueryWorkForm;
            }
        }
        private void CreateScadaMachineTrainQueryWorkForm(TabCodon tabCodon)
        {
            if (_ScadaMachineTrainQueryWorkForm == null || _ScadaMachineTrainQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _ScadaMachineTrainQueryWorkForm = new ScadaMachineTrainQueryWorkForm(this.mediator);
                _ScadaMachineTrainQueryWorkForm.Text = "历史事件";
                _ScadaMachineTrainQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_ScadaMachineTrainQueryWorkForm);

            }


        }

        #endregion
        #region 机器训练预测数据查询模块
        /// <summary>
        ///
        /// </summary>
        private ScadaMachineTrainForeastQueryWorkForm _ScadaMachineTrainForeastQueryWorkForm;
        public ScadaMachineTrainForeastQueryWorkForm ScadaMachineTrainForeastQueryWorkForm
        {
            get
            {
                CreateScadaMachineTrainForeastQueryWorkForm(new TabCodon("机器预测查询", "机器预测查询", TabTypes.HistoricalMachineTrainForeast));
                return _ScadaMachineTrainForeastQueryWorkForm;
            }
        }
        private void CreateScadaMachineTrainForeastQueryWorkForm(TabCodon tabCodon)
        {
            if (_ScadaMachineTrainForeastQueryWorkForm == null || _ScadaMachineTrainForeastQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _ScadaMachineTrainForeastQueryWorkForm = new ScadaMachineTrainForeastQueryWorkForm(this.mediator);
                _ScadaMachineTrainForeastQueryWorkForm.Text = "机器预测查询";
                _ScadaMachineTrainForeastQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_ScadaMachineTrainForeastQueryWorkForm);

            }


        }

        #endregion
        #region 自动控制命令执行记录
        /// <summary>
        ///
        /// </summary>
        private ScadaBatchCommandTaskForm _ScadaBatchCommandTaskForm;
        public ScadaBatchCommandTaskForm ScadaBatchCommandTaskForm
        {
            get
            {
                CreateScadaBatchCommandTaskForm(new TabCodon("自动控制命令执行记录", "自动控制命令执行记录", TabTypes.HistoricalBatchCommandTask));
                return _ScadaBatchCommandTaskForm;
            }
        }
        private void CreateScadaBatchCommandTaskForm(TabCodon tabCodon)
        {
            if (_ScadaBatchCommandTaskForm == null || _ScadaBatchCommandTaskForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _ScadaBatchCommandTaskForm = new ScadaBatchCommandTaskForm(this.mediator);
                _ScadaBatchCommandTaskForm.Text = "自动控制命令执行记录";
                _ScadaBatchCommandTaskForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_ScadaBatchCommandTaskForm);

            }


        }

        #endregion

        
        #region 历史下置
        /// <summary>
        ///
        /// </summary>
        private SendCommandQueryWorkForm _SendCommandQueryWorkForm;
        public SendCommandQueryWorkForm SendCommandQueryWorkForm
        {
            get
            {
                CreateSendCommandQueryWorkForm(new TabCodon("下置查询", "下置查询", TabTypes.LowerQuery));
                return _SendCommandQueryWorkForm;
            }
        }
        private void CreateSendCommandQueryWorkForm(TabCodon tabCodon)
        {
            if (_SendCommandQueryWorkForm == null || _SendCommandQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _SendCommandQueryWorkForm = new SendCommandQueryWorkForm(this.mediator, null);
                _SendCommandQueryWorkForm.Text = "下置查询";
                _SendCommandQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_SendCommandQueryWorkForm);

            }


        }

        #endregion
        #region 报警规则配置日志
        /// <summary>
        ///
        /// </summary>
        private AlarmConfigQueryWorkForm _AlarmConfigQueryWorkForm;
        public AlarmConfigQueryWorkForm AlarmConfigQueryWorkForm
        {
            get
            {
                CreateAlarmConfigQueryWorkForm(new TabCodon("报警配置日志", "报警配置日志", TabTypes.AlarmConfigurationLog));
                return _AlarmConfigQueryWorkForm;
            }
        }
        private void CreateAlarmConfigQueryWorkForm(TabCodon tabCodon)
        {
            if (_AlarmConfigQueryWorkForm == null || _AlarmConfigQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _AlarmConfigQueryWorkForm = new AlarmConfigQueryWorkForm(this.mediator, null);
                _AlarmConfigQueryWorkForm.Text = "报警配置日志";
                _AlarmConfigQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_AlarmConfigQueryWorkForm);

            }


        }

        #endregion
        #region 报警规则配置日志
        /// <summary>
        ///
        /// </summary>
        private HistoryStaticsQueryWorkForm _HistoryStaticsQueryWorkForm;
        public HistoryStaticsQueryWorkForm HistoryStaticsQueryWorkForm
        {
            get
            {
                CreateHistoryStaticsQueryWorkForm(new TabCodon("历史统计", "历史统计", TabTypes.HistoricalStatistics));
                return _HistoryStaticsQueryWorkForm;
            }
        }
        private void CreateHistoryStaticsQueryWorkForm(TabCodon tabCodon)
        {
            if (_HistoryStaticsQueryWorkForm == null || _HistoryStaticsQueryWorkForm.IsDisposed)
            {
                //初始的时候没有加载任何设备
                _HistoryStaticsQueryWorkForm = new HistoryStaticsQueryWorkForm(this.mediator, null);
                _HistoryStaticsQueryWorkForm.Text = "历史统计";
                _HistoryStaticsQueryWorkForm.TabIdentifier = tabCodon.CodonName;

                RaiseNewTab(_HistoryStaticsQueryWorkForm);

            }


        }

        #endregion
        #region 数据库配置
        /// <summary>
        ///
        /// </summary>
        private InfluxConfigForm _InfluxConfigForm;
        public InfluxConfigForm InfluxConfigForm
        {
            get
            {
                CreateInfluxConfigForm(new TabCodon("时序数据库配置", "时序数据库配置", TabTypes.DatabaseConfiguration));
                return _InfluxConfigForm;
            }
        }
        private void CreateInfluxConfigForm(TabCodon tabCodon)
        {
            if (_InfluxConfigForm == null || _InfluxConfigForm.IsDisposed)
            {
                _InfluxConfigForm = new InfluxConfigForm(this.mediator);
                _InfluxConfigForm.Text = "时序数据库配置";
                _InfluxConfigForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_InfluxConfigForm);
            }


        }

        #endregion
        #region 数据备份配置
        /// <summary>
        ///
        /// </summary>
        private InfluxDBBackupForm _InfluxDBBackupForm;
        public InfluxDBBackupForm InfluxDBBackupForm
        {
            get
            {
                CreateInfluxDBBackupForm(new TabCodon("数据库备份", "数据库备份", TabTypes.DatabaseBackup));
                return _InfluxDBBackupForm;
            }
        }
        private void CreateInfluxDBBackupForm(TabCodon tabCodon)
        {
            if (_InfluxDBBackupForm == null || _InfluxDBBackupForm.IsDisposed)
            {
                _InfluxDBBackupForm = new InfluxDBBackupForm(this.mediator);
                _InfluxDBBackupForm.Text = "数据库备份";
                _InfluxDBBackupForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_InfluxDBBackupForm);
            }


        }

        #endregion
        #region 采集站授权 ScadaMDSManagerForm
        /// <summary>
        ///
        /// </summary>
        private ScadaMDSManagerForm _ScadaMDSManagerForm;
        public ScadaMDSManagerForm ScadaMDSManagerForm
        {
            get
            {
                CreateScadaMDSManagerForm(new TabCodon("采集站授权", "采集站授权", TabTypes.MonitoAuthorization));
                return _ScadaMDSManagerForm;
            }
        }
        private void CreateScadaMDSManagerForm(TabCodon tabCodon)
        {
            if (_ScadaMDSManagerForm == null || _ScadaMDSManagerForm.IsDisposed)
            {
                _ScadaMDSManagerForm = new ScadaMDSManagerForm(this.mediator);
                _ScadaMDSManagerForm.Text = "采集站授权";
                _ScadaMDSManagerForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_ScadaMDSManagerForm);
            }


        }


        #endregion
        #region Constructor
        public TabFactory(Mediator mediator)
        {
            this.mediator = mediator;
        }
        #endregion
        #region Methods
        public object GetTab(TabCodon codon)
        {
            switch (codon.TabType)
            {

                case TabTypes.IOCatalog:
                    CreateIOTreeForm(codon);
                    return _IOTreeForm;
                case TabTypes.IOTask:
                    CreateIOTaskForm(codon);
                    return _IOTaskForm;
                case TabTypes.Property:
                    CreateIOPropeitesForm(codon);
                    return _IOPropeitesForm;
                case TabTypes.Logbook:
                    CreateOperatorLogForm(codon);
                    return _OperatorLogForm;
                case TabTypes.PointArea:
                    CreateRealQueryWorkForm(codon);
                    return _RealQueryWorkForm;
                case TabTypes.DatabaseConfiguration:
                    CreateInfluxConfigForm(codon);
                    return _InfluxConfigForm;
                case TabTypes.RealTimeData:
                    CreateRealQueryWorkForm(codon);
                    return _RealQueryWorkForm;
                case TabTypes.HistoricalData:
                    CreateHistoryQueryWorkForm(codon);
                    return _HistoryQueryWorkForm;
                case TabTypes.HistoricalAlarm:
                    CreateHistoryAlarmQueryWorkForm(codon);
                    return _HistoryAlarmQueryWorkForm;
                case TabTypes.LowerQuery:
                    CreateSendCommandQueryWorkForm(codon);
                    return _SendCommandQueryWorkForm;
                case TabTypes.AlarmConfigurationLog:
                    this.CreateAlarmConfigQueryWorkForm(codon);
                    return _AlarmConfigQueryWorkForm;
                case TabTypes.HistoricalStatistics:
                    this.CreateHistoryStaticsQueryWorkForm(codon);
                    return _HistoryStaticsQueryWorkForm;
                case TabTypes.DatabaseBackup:
                    this.CreateInfluxDBBackupForm(codon);
                    return _InfluxDBBackupForm;
                case TabTypes.MonitoAuthorization:
                    this.CreateScadaMDSManagerForm(codon);
                    return _ScadaMDSManagerForm;
                case TabTypes.HistoricalEvent:
                    this.CreateScadaEventQueryWorkForm(codon);
                    return _ScadaEventQueryWorkForm;
                case TabTypes.HistoricalMachineTrainForeast:
                    this.CreateScadaMachineTrainForeastQueryWorkForm(codon);
                    return _ScadaMachineTrainForeastQueryWorkForm;
                    
                case TabTypes.HistoricalMachineTrain:
                    this.CreateScadaMachineTrainQueryWorkForm(codon);
                    return _ScadaMachineTrainQueryWorkForm;

                case TabTypes.HistoricalBatchCommandTask:
                    this.CreateScadaBatchCommandTaskForm(codon);
                    return _ScadaBatchCommandTaskForm;
            }

            return null;
        }
        //
        private void RaiseNewTab(DockContent tab)
        {
            if (OnTabCreation != null)
            {
                OnTabCreation(tab);
            }
        }
        #endregion
    }
    /// <summary>
    /// Passes the tab created, to be added on a higher level in a TabControl
    /// </summary>
    public delegate void TabInfo(DockContent tab);
}
