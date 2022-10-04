using Scada.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using IOMonitor.Forms;


 
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
    /// <summary>
    /// Tab factory, handles the creation and uniqueness of tabs (docking forms)
    /// </summary>
    public class TabFactory
    {
        public event TabInfo OnTabCreation;
       private Mediator mediator;
        //

        #region 采集监视
        /// <summary>
        ///
        /// </summary>
        private IOMonitorForm _IOMonitorForm;
        public IOMonitorForm IOMonitorForm
        {
            get
            {
                CreateIOMonitorForm(new TabCodon("采集监视", "采集监视", TabTypes.IOMonitoring));
                return _IOMonitorForm;
            }
        }
        private void CreateIOMonitorForm(TabCodon tabCodon)
        {
            if(_IOMonitorForm == null|| _IOMonitorForm.IsDisposed)
            {
                _IOMonitorForm = new IOMonitorForm(this.mediator);
                _IOMonitorForm.Text = "采集监视";
                _IOMonitorForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_IOMonitorForm);
            }
        
         
        }

        #endregion
  
        #region 采集日志
        /// <summary>
        ///WorkForm
        /// </summary>
        private IOMonitorLogForm _IOMonitorLogForm;
        public IOMonitorLogForm IOMonitorLogForm
        {
            get
            {
                CreateIOMonitorLogForm(new TabCodon("采集日志", "采集日志", TabTypes.IOLogbook));
                return _IOMonitorLogForm;
            }
        }
        private void CreateIOMonitorLogForm(TabCodon tabCodon)
        {
            if (_IOMonitorLogForm == null || _IOMonitorLogForm.IsDisposed)
            {
                _IOMonitorLogForm = new IOMonitorLogForm(this.mediator);
                _IOMonitorLogForm.Text = "采集日志";
                _IOMonitorLogForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_IOMonitorLogForm);
            }
          
        }

        #endregion
        #region IO属性
        /// <summary>
        ///WorkForm
        /// </summary>
        private IOPropertiesForm _IOPropertiesForm;
        public IOPropertiesForm IOPropertiesForm
        {
            get
            {
                CreateIOPropertiesForm(new TabCodon("IO属性", "IO属性", TabTypes.Property));
                return _IOPropertiesForm;
            }
        }
        private void CreateIOPropertiesForm(TabCodon tabCodon)
        {
            if (_IOPropertiesForm == null || _IOPropertiesForm.IsDisposed)
            {
                _IOPropertiesForm = new IOPropertiesForm(this.mediator);
                _IOPropertiesForm.Text = "IO属性";
                _IOPropertiesForm.TabIdentifier = tabCodon.CodonName;
                RaiseNewTab(_IOPropertiesForm);
            }
       
        }

        #endregion
        #region IO状态
        /// <summary>
        ///WorkForm
        /// </summary>
        private IOStatusForm _IOStatusForm;
        public IOStatusForm IOStatusForm
        {
            get
            {
                CreateIOStatusForm(new TabCodon("IO状态", "IO状态", TabTypes.IOPoint));
                return _IOStatusForm;
            }
        }
        private void CreateIOStatusForm(TabCodon tabCodon)
        {
            if (_IOStatusForm == null || _IOStatusForm.IsDisposed)
            {
                _IOStatusForm = new IOStatusForm(this.mediator);
                _IOStatusForm.Text = "IO状态";
                _IOStatusForm.TabIdentifier = tabCodon.CodonName;

            }
            RaiseNewTab(_IOStatusForm);
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
                case TabTypes.IOMonitoring:
                    CreateIOMonitorForm(codon);
                    return _IOMonitorForm;
             
                case TabTypes.IOLogbook:
                    CreateIOMonitorLogForm(codon);
                    return _IOMonitorLogForm;
                case TabTypes.Property:
                    CreateIOPropertiesForm(codon);
                    return _IOPropertiesForm;
                case TabTypes.IOPoint:
                    CreateIOStatusForm(codon);
                    return _IOStatusForm;
                  
                
                    
            }
            
            return null;
        }
        //
        private void RaiseNewTab(DockContent tab)
        {
            if (OnTabCreation != null)
                OnTabCreation(tab);
        }
        #endregion
    }
    /// <summary>
    /// Passes the tab created, to be added on a higher level in a TabControl
    /// </summary>
    public delegate void TabInfo(DockContent tab);
}
