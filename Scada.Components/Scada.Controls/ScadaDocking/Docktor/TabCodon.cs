namespace Scada.Controls
{
    /// <summary>
    /// 定义的Tab页面
    /// </summary>
    public class TabCodon : CodonBase
    {
        public float MapWidth = 1024;
        public float MapHeight = 800;

        private TabTypes tabType;
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public TabTypes TabType
        {
            get { return tabType; }
            set { tabType = value; }
        }


        public TabCodon(string name, TabTypes type)
            : base(name)
        {
            tabType = type;
        }

        public TabCodon(string name, string text, TabTypes type)
            : base(name)
        {
            tabType = type;
            this.text = text;
        }
    }

    /// <summary>
    /// 每个Tab页面的模块枚举
    /// </summary>
    public enum TabTypes
    {
        Project,//工程
        Shape,// 图元
        Property,//属性
        WorkArea,//工作区
        PointArea,// 点表区
        Logbook,//日志
        DriverManagement,// 驱动管理
        IOMonitoring,//采集监视
        IOLogbook,//采集日志
        IOLogbookQuery,//采集日志查询
        MonitorTool,//监视器工具
        IOPoint,// IO点表
        //服务器中心
        IOCatalog,//IO目录
        IOTask,//机器训练任务，自动控制任务
        DatabaseConfiguration,// 数据库配置,
        DatabaseBackup,// 数据库备份,
        RealTimeData,// 实时数据,
        HistoricalData,//历史数据,
        HistoricalStatistics,//历史统计
        HistoricalAlarm,//历史报警
        HistoricalEvent,//历史事件
        HistoricalMachineTrain,//历史训练日志
        HistoricalMachineTrainForeast,//历史训练预测数据
        HistoricalBatchCommandTask,//历史自动控制命令执行记录
        LowerQuery,//下置查询
        AlarmConfigurationLog,//报警配置日志
        LogbookQuery,//日志查询,
        MessageServiceConfiguration,//消息服务配置,
        MonitoAuthorization,//采集站授权
        BatchCommandTask,
        ThreeWork,
        ThreeProject,
        ThreeProperites,
        ThreeLog,
        ThreeShapes,
        ThreeViewStructure,
        ThreeSceneView,
        ThreeModelWork,
        ThreeModelProject,
        ThreeModelProperites,
        ThreeModelLog,
        ThreeModelShapes,
        ThreeModelViewStructure,
        Unknown
    }
 
}
