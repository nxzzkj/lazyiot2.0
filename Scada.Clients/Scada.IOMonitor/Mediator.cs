using IOMonitor.Forms;
using Scada.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
namespace IOMonitor
{
    public class Mediator
    {

        #region Constructor
        public   MonitorForm MonitorForm = null;
        public Mediator(MonitorForm main)
        {
            MonitorForm = main;
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

        //
        #region 采集监视
        public IOMonitorForm IOMonitorForm
        {
            get
            {
                if (tabFactory.IOMonitorForm == null)
                    return null;
                else
                    return tabFactory.IOMonitorForm;
            }
        }
        public void OpenIOMonitorForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("采集监视", "采集监视", TabTypes.IOMonitoring)) as IOMonitorForm;
            lastAdded.TabText = "采集监视";
            lastAdded.Text = "采集监视";
            OnShowTab(lastAdded);
        }
        #endregion
 

        

        #region 采集日志
        public IOMonitorLogForm IOMonitorLogForm
        {
            get
            {
                if (tabFactory.IOMonitorLogForm == null)
                    return null;
                else
                    return tabFactory.IOMonitorLogForm;
            }
        }
        public void OpenLogForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("采集日志", "采集日志", TabTypes.IOLogbook)) as IOMonitorLogForm;
            lastAdded.TabText = "采集日志";
            lastAdded.Text = "采集日志";
            OnShowTab(lastAdded);
        }
        #endregion
        #region IO属性
        public IOPropertiesForm IOPropertiesForm
        {
            get
            {
                if (tabFactory.IOPropertiesForm == null)
                    return null;
                else
                    return tabFactory.IOPropertiesForm;
            }
        }
        public void OpenIOPropertiesForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("IO属性", "IO属性", TabTypes.Property)) as IOPropertiesForm;
            lastAdded.TabText = "IO属性";
            lastAdded.Text = "IO属性";
            OnShowTab(lastAdded);
        }
        #endregion
        #region IO状态
        public IOStatusForm IOStatusForm
        {
            get
            {
                if (tabFactory.IOStatusForm == null)
                    return null;
                else
                    return tabFactory.IOStatusForm;
            }
        }
        public void OpenIOStatusForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("IO状态", "IO状态", TabTypes.IOPoint)) as IOStatusForm;
            lastAdded.TabText = "IO状态";
            lastAdded.Text = "IO状态";
            OnShowTab(lastAdded);
        }
        #endregion

        
        private void OnTabCall(DockContent tab)
        {
            TabTypes type = (tab as ICobaltTab).TabType;
            switch (type)
            {



                case TabTypes.IOMonitoring:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.MonitorTool:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
                case TabTypes.IOLogbook:
                    tab.Show(dockPanel, DockState.DockBottom);
                    break;
                case TabTypes.LogbookQuery:
                    tab.Show(dockPanel, DockState.Document);
                    break;
                case TabTypes.IOPoint:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
                case TabTypes.Property:
                    tab.Show(dockPanel, DockState.DockRight);
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
            if (tab == null) return;
            TabTypes type = (tab as ICobaltTab).TabType;
            tab.Show(dockPanel, tab.DockState);

        }


    }
}
