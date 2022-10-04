

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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCHorizontalList.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCHorizontalList : UserControl
    {
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public UCHorizontalListItem SelectedItem { get; set; }
        /// <summary>
        /// Occurs when [selected item event].
        /// </summary>
        public event EventHandler SelectedItemEvent;
        /// <summary>
        /// The m start item index
        /// </summary>
        private int m_startItemIndex = 0;
        /// <summary>
        /// The is automatic select first
        /// </summary>
        private bool isAutoSelectFirst = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic select first.
        /// </summary>
        /// <value><c>true</c> if this instance is automatic select first; otherwise, <c>false</c>.</value>
        public bool IsAutoSelectFirst
        {
            get { return isAutoSelectFirst; }
            set { isAutoSelectFirst = value; }
        }

        /// <summary>
        /// The data source
        /// </summary>
        private List<KeyValuePair<string, string>> dataSource = null;

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public List<KeyValuePair<string, string>> DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;
                ReloadSource();
            }
        }
        private Color selectedColor = Color.FromArgb(255, 77, 59);

        [Description("选中颜色"), Category("自定义")]
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCHorizontalList" /> class.
        /// </summary>
        public UCHorizontalList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reloads the source.
        /// </summary>
        public void ReloadSource()
        {
            try
            {
                ControlHelper.FreezeControl(this, true);
                this.panList.SuspendLayout();
                this.panList.Controls.Clear();
                this.panList.Width = this.panMain.Width;
                if (DataSource != null)
                {
                    foreach (var item in DataSource)
                    {
                        UCHorizontalListItem uc = new UCHorizontalListItem();
                        uc.SelectedColor = selectedColor;
                        uc.DataSource = item;
                        uc.SelectedItem += uc_SelectItem;
                        this.panList.Controls.Add(uc);
                    }
                }
                this.panList.ResumeLayout(true);
                if (this.panList.Controls.Count > 0)
                    this.panList.Width = panMain.Width + this.panList.Controls[0].Location.X * -1;
                this.panList.Location = new Point(0, 0);
                m_startItemIndex = 0;
                if (this.panList.Width > panMain.Width)
                    panRight.Visible = true;
                else
                    panRight.Visible = false;
                panLeft.Visible = false;
                panList.SendToBack();
                panRight.SendToBack();
                if (isAutoSelectFirst && DataSource != null && DataSource.Count > 0)
                {
                    SelectItem((UCHorizontalListItem)this.panList.Controls[0]);
                }
            }
            finally
            {
                ControlHelper.FreezeControl(this, false);
            }
        }

        /// <summary>
        /// Handles the SelectItem event of the uc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void uc_SelectItem(object sender, EventArgs e)
        {
            SelectItem(sender as UCHorizontalListItem);
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void SelectItem(UCHorizontalListItem item)
        {
            if (SelectedItem != null && !SelectedItem.IsDisposed)
                SelectedItem.SetSelect(false);
            SelectedItem = item;
            SelectedItem.SetSelect(true);
            if (SelectedItemEvent != null)
                SelectedItemEvent(item, null);
        }

        /// <summary>
        /// Handles the MouseDown event of the panLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.panList.Location.X >= 0)
            {
                this.panList.Location = new Point(0, 0);
                return;
            }

            for (int i = m_startItemIndex; i >= 0; i--)
            {
                if (this.panList.Controls[i].Location.X < this.panList.Controls[m_startItemIndex].Location.X - panMain.Width)
                {
                    m_startItemIndex = i + 1;
                    break; ;
                }
                if (i == 0)
                {
                    m_startItemIndex = 0;
                }
            }

            ResetListLocation();
            panRight.Visible = true;
            if (this.panList.Location.X >= 0)
            {
                panLeft.Visible = false;
            }
            else
            {
                panLeft.Visible = true;
            }
            panList.SendToBack();
            panRight.SendToBack();
        }

        /// <summary>
        /// Handles the MouseDown event of the panRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.panList.Location.X + this.panList.Width <= this.panMain.Width)
                return;
            if (this.panList.Controls.Count <= 0)
                return;
            for (int i = m_startItemIndex; i < this.panList.Controls.Count; i++)
            {
                if (this.panList.Location.X + this.panList.Controls[i].Location.X + this.panList.Controls[i].Width > panMain.Width)
                {
                    m_startItemIndex = i;
                    break;
                }
            }
            ResetListLocation();
            panLeft.Visible = true;
            if (panList.Width + panList.Location.X <= panMain.Width)
                panRight.Visible = false;
            else
                panRight.Visible = true;
            panList.SendToBack();
            panRight.SendToBack();
        }

        /// <summary>
        /// Resets the list location.
        /// </summary>
        private void ResetListLocation()
        {
            if (this.panList.Controls.Count > 0)
            {
                this.panList.Location = new Point(this.panList.Controls[m_startItemIndex].Location.X * -1, 0);
            }
        }

        /// <summary>
        /// Sets the select.
        /// </summary>
        /// <param name="strKey">The string key.</param>
        public void SetSelect(string strKey)
        {
            foreach (UCHorizontalListItem item in this.panList.Controls)
            {
                if (item.DataSource.Key == strKey)
                {
                    SelectItem(item);
                    return;
                }
            }
        }
    }
}
