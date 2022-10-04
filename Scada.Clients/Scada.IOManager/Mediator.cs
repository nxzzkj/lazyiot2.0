using System;
using System.Collections.Generic;
using System.Text;


using System.Windows.Forms;



using System.Reflection;
using Scada.Controls;
using IOManager.Page;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/

namespace IOManager
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
      
        //
        #region 工程区
        public IOTreeForm IOTreeForm
        {
            get
            {
                if (tabFactory.IOTreeForm == null)
                    return null;
                else
                    return tabFactory.IOTreeForm;
            }
        }
        public void OpenIOTreeForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("工程", "工程", TabTypes.Project)) as IOTreeForm;
            lastAdded.TabText = "工程";
            lastAdded.Text = "工程";
            OnShowTab(lastAdded);
        }
        #endregion
 
        #region 工作区
        public IOParaForm IOParaForm
        {
            get
            {
                if (tabFactory.IOParaForm == null)
                    return null;
                else
                    return tabFactory.IOParaForm;
            }
        }
        public void OpenIOParaForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("点表区", "点表区", TabTypes.PointArea)) as IOParaForm;
            lastAdded.TabText = "点表区";
            lastAdded.Text = "点表区";
            OnShowTab(lastAdded);
        }
        #endregion
        #region 日志
        public IOLogForm IOLogForm
        {
            get
            {
                if (tabFactory.IOLogForm == null)
                    return null;
                else
                    return tabFactory.IOLogForm;
            }
        }
        public void OpenLogForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("日志", "日志", TabTypes.Logbook)) as IOLogForm;
            lastAdded.TabText = "日志";
            lastAdded.Text = "日志";
            OnShowTab(lastAdded);
        }
        #endregion
        #region 驱动管理
        public IODriveManageForm IODriveManageForm
        {
            get
            {
                if (tabFactory.IODriveManageForm == null)
                    return null;
                else
                    return tabFactory.IODriveManageForm;
            }
        }
        public void OpenIODriveManageForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("驱动管理", "驱动管理", TabTypes.DriverManagement)) as IODriveManageForm;
            lastAdded.TabText = "驱动管理";
            lastAdded.Text = "驱动管理";
            OnShowTab(lastAdded);
        }
        #endregion

        #region 驱动管理
        public BatchCommandTaskForm BatchCommandTaskForm
        {
            get
            {
                if (tabFactory.BatchCommandTaskForm == null)
                    return null;
                else
                    return tabFactory.BatchCommandTaskForm;
            }
        }
        public void OpenBatchCommandTaskFormForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("自动控制任务管理", "自动控制任务管理", TabTypes.BatchCommandTask)) as BatchCommandTaskForm;
            lastAdded.TabText = "自动控制任务管理";
            lastAdded.Text = "自动控制任务管理";
            OnShowTab(lastAdded);
        }
        #endregion
        

        private void OnTabCall(DockContent tab)
        {
            TabTypes type = (tab as ICobaltTab).TabType;
            switch (type)
            {
               
                 
                    
                case TabTypes.PointArea:
                    tab.Show(dockPanel, DockState.Document);
                    break;

                case TabTypes.Project:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
                case TabTypes.Logbook:
                    tab.Show(dockPanel, DockState.DockBottom);
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
