

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scada.Controls.Forms
{
    /// <summary>
    /// Class FrmLoading.
    /// Implements the <see cref="Scada.Controls.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Forms.FrmBase" />
    public partial class FrmLoading : FrmBase
    {
        /// <summary>
        /// The update database worker
        /// </summary>
        BackgroundWorker updateDBWorker = new BackgroundWorker();
        /// <summary>
        /// 获取或设置加载任务
        /// </summary>
        /// <value>The background work action.</value>
        public Action BackgroundWorkAction
        {
            get;
            set;
        }
        private int mProgressNum = 0;
        public int ProgressNum
        {
            get { return mProgressNum++; }
            set { mProgressNum = value; }
        }
        /// <summary>
        /// 设置当前执行进度及任务名称，key:任务进度，取值0-100  value：当前任务名称
        /// </summary>
        /// <value>The current MSG.</value>
        public KeyValuePair<int, string> CurrentMsg
        {
            set
            {
                this.updateDBWorker.ReportProgress(value.Key, value.Value);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FrmLoading"/> class.
        /// </summary>
        public FrmLoading()
        {
            InitializeComponent();
            this.updateDBWorker.WorkerReportsProgress = true;
            this.updateDBWorker.WorkerSupportsCancellation = true;
            this.updateDBWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.updateDBWorker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
        }
        /// <summary>
        /// 设置进度信息，重写此函数可以处理界面信息绑定
        /// </summary>
        /// <param name="strText">进度任务名称</param>
        /// <param name="intValue">进度值</param>
        protected virtual void BindingProcessMsg(string strText, int intValue)
        {

        }

        /// <summary>
        /// Sets the message.
        /// </summary>
        /// <param name="strText">The string text.</param>
        /// <param name="intValue">The int value.</param>
        private void SetMessage(string strText, int intValue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate() { SetMessage(strText, intValue); }));
            }
            else
            {
                BindingProcessMsg(strText, intValue);
            }
        }

        /// <summary>
        /// Handles the Load event of the FrmLoading control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FrmLoading_Load(object sender, EventArgs e)
        {
            if (ControlHelper.IsDesignMode())
                return;
            this.updateDBWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.BackgroundWorkAction != null)
            {
                this.BackgroundWorkAction();
            }
            Thread.Sleep(100);
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new MethodInvoker(delegate
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }));
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the backgroundWorker1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetMessage((e.UserState == null) ? "" : e.UserState.ToString(), e.ProgressPercentage);
        }
    }
}
