using Scada.Controls;
using IOManager.Page;
using System;
using System.Collections.Generic;
using System.Text;



 
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
    /// <summary>
    /// Tab factory, handles the creation and uniqueness of tabs (docking forms)
    /// </summary>
    public class TabFactory
    {
        public event TabInfo OnTabCreation;
       private Mediator mediator;
       //
    
        #region 工程区
        /// <summary>
        ///
        /// </summary>
        private IOTreeForm _IOTreeForm;
        public IOTreeForm IOTreeForm
        {
            get
            {
                CreateIOTreeForm(new TabCodon("工程", "工程", TabTypes.Project));
                return _IOTreeForm;
            }
        }
        private void CreateIOTreeForm(TabCodon tabCodon)
        {
            if(_IOTreeForm == null|| _IOTreeForm.IsDisposed)
            {
                _IOTreeForm = new IOTreeForm(this.mediator);
                _IOTreeForm.Text = "工程";
                _IOTreeForm.TabIdentifier = tabCodon.CodonName;

            }
        
            RaiseNewTab(_IOTreeForm);
        }

        #endregion
        #region 工作区
        /// <summary>
        ///WorkForm
        /// </summary>
        private IOParaForm _IOParaForm;
        public IOParaForm IOParaForm
        {
            get
            {
                CreateIOParaForm(new TabCodon("点表区", "点表区", TabTypes.PointArea));
                return _IOParaForm;
            }
        }
        private void CreateIOParaForm(TabCodon tabCodon)
        {
            if (_IOParaForm == null || _IOParaForm.IsDisposed)
            {

                _IOParaForm = new IOParaForm(this.mediator);
                _IOParaForm.Text = "点表区";
                _IOParaForm.TabIdentifier = tabCodon.CodonName;
            }
            RaiseNewTab(_IOParaForm);
        }

        #endregion
        #region 日志
        /// <summary>
        ///WorkForm
        /// </summary>
        private IOLogForm _IOLogForm;
        public IOLogForm IOLogForm
        {
            get
            {
                CreateIOLogForm(new TabCodon("日志", "日志", TabTypes.Logbook));
                return _IOLogForm;
            }
        }
        private void CreateIOLogForm(TabCodon tabCodon)
        {
            if (_IOLogForm == null || _IOLogForm.IsDisposed)
            {
                _IOLogForm = new IOLogForm(this.mediator);
                _IOLogForm.Text = "日志";
                _IOLogForm.TabIdentifier = tabCodon.CodonName;
             
            }
            RaiseNewTab(_IOLogForm);
        }

        #endregion
        #region 驱动管理
        /// <summary>
        ///WorkForm
        /// </summary>
        private IODriveManageForm _IODriveManageForm;
        public IODriveManageForm IODriveManageForm
        {
            get
            {
                CreateIODriveManageForm(new TabCodon("驱动管理", "驱动管理", TabTypes.DriverManagement));
                return _IODriveManageForm;
            }
        }
        private void CreateIODriveManageForm(TabCodon tabCodon)
        {
            if (_IODriveManageForm == null || _IODriveManageForm.IsDisposed)
            {
                _IODriveManageForm = new IODriveManageForm(this.mediator);
                _IODriveManageForm.Text = "驱动管理";
                _IODriveManageForm.TabIdentifier = tabCodon.CodonName;

            }
            RaiseNewTab(_IODriveManageForm);
        }

        #endregion
        #region  BatchCommandTaskForm 批任务
        #region 驱动管理
        /// <summary>
        ///WorkForm
        /// </summary>
        private BatchCommandTaskForm _BatchCommandTaskForm;
        public BatchCommandTaskForm BatchCommandTaskForm
        {
            get
            {
                CreateBatchCommandTaskFormForm(new TabCodon("自动控制任务管理", "自动控制任务管理", TabTypes.BatchCommandTask));
                return _BatchCommandTaskForm;
            }
        }
        private void CreateBatchCommandTaskFormForm(TabCodon tabCodon)
        {
            if (_BatchCommandTaskForm == null || _BatchCommandTaskForm.IsDisposed)
            {
                _BatchCommandTaskForm = new BatchCommandTaskForm(this.mediator);
                _BatchCommandTaskForm.Text = "自动控制任务管理";
                _BatchCommandTaskForm.TabIdentifier = tabCodon.CodonName;

            }
            RaiseNewTab(_BatchCommandTaskForm);
        }

        #endregion
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
                case TabTypes.Project:
                    CreateIOTreeForm(codon);
                    return _IOTreeForm;
                case TabTypes.Logbook:
                    CreateIOLogForm(codon);
                    return _IOLogForm;
                case TabTypes.PointArea:
                    CreateIOParaForm(codon);
                    return _IOParaForm;
                case TabTypes.DriverManagement:
                    CreateIODriveManageForm(codon);
                    return _IODriveManageForm;
                case TabTypes.BatchCommandTask:
                    CreateBatchCommandTaskFormForm(codon);
                    return _BatchCommandTaskForm;
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
