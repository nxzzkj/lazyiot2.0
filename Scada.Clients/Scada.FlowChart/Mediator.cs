using System;
using Scada.Controls;
using Scada.DBUtility;
using Scada.FlowGraphEngine.GraphicsMap;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaFlowDesign
{
    public class Mediator
    {
        
        #region Constructor
        public Mediator(FlowDesign main)
        {
            this.Parent = main;
            tabFactory = new TabFactory(this);
            tabFactory.OnTabCreation += new TabInfo(OnTabCall);
            rnd = new Random();
        }
      
        #endregion
        protected Random rnd;
        private TabFactory tabFactory;
        internal FlowDesign Parent;
        private DockContent lastAdded;
        private DockPanel dockPanel;
        public DockPanel DockPanel
        {
            get { return dockPanel; }
            set { dockPanel = value; }
        }
        public DockContent ActiveWork
        {
            get
            {
                return DockPanel.ActiveDocument;

            }
        }
        //
        #region 属性
        public PropertiesForm PropertiesForm
        {
            get
            {
             
                    return tabFactory.PropertiesForm;
            }
        }
        public void OpenPropertiesForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("属性", "属性", TabTypes.Property)) as PropertiesForm;
            lastAdded.TabText = "属性";
            lastAdded.Text = "属性";
            OnShowTab(lastAdded);
        }
        #endregion
        #region 目录
        public ToolForm ToolForm
        {
            get
            {
             
                    return tabFactory.ToolForm;
            }
        }
        public void OpenToolForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("工程", "工程", TabTypes.Project)) as ToolForm;
            lastAdded.TabText = "工程";
            lastAdded.Text = "工程";
            OnShowTab(lastAdded);
        }
        #endregion
        #region 工作区
        
        public DockContent CreateWorkForm(string Title,float width, float height)
        {
            lastAdded = tabFactory.GetTab(new TabCodon("W" + GUIDToNormalID.GuidToLongID(), Title, TabTypes.WorkArea) {  MapHeight= height, MapWidth=  width }) as WorkForm;
            lastAdded.TabText = Title;
            lastAdded.Text = Title;
            OnShowTab(lastAdded);
            return lastAdded;
        }
        public DockContent CreateWorkForm(string Title, float width, float height, FlowGraphAbstract site)
        {
            lastAdded = tabFactory.GetTab(new TabCodon("W" + GUIDToNormalID.GuidToLongID(), Title, TabTypes.WorkArea) { MapHeight = height, MapWidth = width }) as WorkForm;
            WorkForm work = lastAdded as WorkForm;
            work.GraphControl.Abstract = site;
            lastAdded.TabText = Title;
            lastAdded.Text = Title;
            OnShowTab(lastAdded);
            return lastAdded;
        }
        #endregion
        #region 日志
        public LoggerForm LogForm
        {
            get
            {
           
                    return tabFactory.LogForm;
            }
        }
        public void OpenLogForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("日志", "日志", TabTypes.Logbook)) as LoggerForm;
            lastAdded.TabText = "日志";
            lastAdded.Text = "日志";
            OnShowTab(lastAdded);
        }
        #endregion
        #region 图元
        public ShapeForm ShapeForm
        {
            get
            {
      
                    return tabFactory.ShapeForm;
            }
        }
        public void OpenShapeForm()
        {
            lastAdded = tabFactory.GetTab(new TabCodon("图元", "图元", TabTypes.Shape)) as ShapeForm;
            lastAdded.TabText = "图元";
            lastAdded.Text = "图元";
            OnShowTab(lastAdded);
        }
        #endregion
        private void OnTabCall(DockContent tab)
        {
            TabTypes type = (tab as ICobaltTab).TabType;
            switch (type)
            {
               
                case TabTypes.Property:
                    tab.Show(dockPanel, DockState.DockRight);
                    break;
                case TabTypes.Shape:
                    tab.Show(dockPanel, DockState.DockLeft);
                    break;
                    
                case TabTypes.WorkArea:
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
