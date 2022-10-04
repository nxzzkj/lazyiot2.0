

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
    /// Class UCPagerControl2.
    /// Implements the <see cref="Scada.Controls.Controls.UCPagerControlBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCPagerControlBase" />
    [ToolboxItem(true)]
    public partial class UCPagerControl2 : UCPagerControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UCPagerControl2" /> class.
        /// </summary>
        public UCPagerControl2()
        {
            InitializeComponent();
            txtPage.txtInput.KeyDown += txtInput_KeyDown;
            this.Load += UCPagerControl2_Load;
        }

        private void UCPagerControl2_Load(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lstCom = new List<KeyValuePair<string, string>>();
            lstCom.Add(new KeyValuePair<string, string>("10", "每页10条"));//做测试用
            lstCom.Add(new KeyValuePair<string, string>("100", "每页100条"));
            lstCom.Add(new KeyValuePair<string, string>("200", "每页200条"));
            lstCom.Add(new KeyValuePair<string, string>("500", "每页500条"));
            lstCom.Add(new KeyValuePair<string, string>("800", "每页800条"));
            lstCom.Add(new KeyValuePair<string, string>("1000", "每页1000条"));
            lstCom.Add(new KeyValuePair<string, string>("2000", "每页2000条"));

       
            this.ucComboxPageSize.Source = lstCom;
            this.ucComboxPageSize.SelectedIndex = 0;
        }
        public void SetPages(List<KeyValuePair<string, string>> lstCom,int selectIndex)
        {
            this.ucComboxPageSize.Source = lstCom;
          this.ucComboxPageSize.SelectedIndex = selectIndex;
        }

        /// <summary>
        /// Handles the KeyDown event of the txtInput control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnToPage_BtnClick(null, null);
                txtPage.InputText = "";
            }
        }

        /// <summary>
        /// Occurs when [show source changed].
        /// </summary>
        public override event PageControlEventHandler ShowSourceChanged;


        /// <summary>
        /// 关联的数据源
        /// </summary>
        /// <value>The data source.</value>
        public override List<object> DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                if (value == null)
                {
                    base.DataSource = new List<object>();
                }
                else
                {
                    base.DataSource = value;

                }
                PageIndex = 1;
                ResetPageCount();
                var s = GetCurrentSource();
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
        }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        /// <value>The size of the page.</value>
        public override int PageSize
        {
            get
            {
                return base.PageSize;
            }
            set
            {
                base.PageSize = value;
                if (PageModel == Scada.Controls.Controls.PageModel.Soure)
                {
                    ResetPageCount();
                    var s = GetCurrentSource();
                    if (ShowSourceChanged != null)
                    {
                        ShowSourceChanged(s);
                    }
                }
                else
                {
                    if (ShowSourceChanged != null)
                    {
                        ShowSourceChanged(this);
                    }
                }
            }
        }

        public override int PageCount
        {
            get
            {
                return base.PageCount;
            }
            set
            {
                base.PageCount = value;
                ReloadPage();
            }
        }

        public override int PageIndex
        {
            get
            {
                return base.PageIndex;
            }
            set
            {
                base.PageIndex = value;
                ReloadPage();
            }
        }

        /// <summary>
        /// 第一页
        /// </summary>
        public override void FirstPage()
        {
            if (PageIndex == 1)
                return;
            PageIndex = 1;
            ReloadPage();
            if (PageModel == Scada.Controls.Controls.PageModel.Soure)
            {
                StartIndex = (PageIndex - 1) * PageSize;
                var s = GetCurrentSource();
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
            else
            {
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(this);
                }
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public override void PreviousPage()
        {
            if (PageIndex <= 1)
            {
                return;
            }
            PageIndex--;
            ReloadPage();
            if (PageModel == Scada.Controls.Controls.PageModel.Soure)
            {
                StartIndex = (PageIndex - 1) * PageSize;
                var s = GetCurrentSource();
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
            else
            {
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(this);
                }
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public override void NextPage()
        {
            if (PageIndex >= PageCount)
            {
                return;
            }
            PageIndex++;
            ReloadPage();
            if (PageModel == Scada.Controls.Controls.PageModel.Soure)
            {
                StartIndex = (PageIndex - 1) * PageSize;
                var s = GetCurrentSource();
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
            else
            {
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(this);
                }
            }
        }

        /// <summary>
        /// 最后一页
        /// </summary>
        public override void EndPage()
        {
            if (PageIndex == PageCount)
                return;
            PageIndex = PageCount;
            ReloadPage();
            if (PageModel == Scada.Controls.Controls.PageModel.Soure)
            {
                StartIndex = (PageIndex - 1) * PageSize;
                var s = GetCurrentSource();
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
            else
            {
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(this);
                }
            }
        }

        /// <summary>
        /// Resets the page count.
        /// </summary>
        private void ResetPageCount()
        {
            if (PageSize > 0)
            {
                if (base.DataSource != null)
                    PageCount = base.DataSource.Count / base.PageSize + (base.DataSource.Count % base.PageSize > 0 ? 1 : 0);
            }
            txtPage.MaxValue = PageCount;
            txtPage.MinValue = 1;
            ReloadPage();
        }

        /// <summary>
        /// Reloads the page.
        /// </summary>
        private void ReloadPage()
        {
            try
            {
                ControlHelper.FreezeControl(tableLayoutPanel, true);
                List<int> lst = new List<int>();

                if (PageCount <= 9)
                {
                    for (var i = 1; i <= PageCount; i++)
                    {
                        lst.Add(i);
                    }
                }
                else
                {
                    if (this.PageIndex <= 6)
                    {
                        for (var i = 1; i <= 7; i++)
                        {
                            lst.Add(i);
                        }
                        lst.Add(-1);
                        lst.Add(PageCount);
                    }
                    else if (this.PageIndex > PageCount - 6)
                    {
                        lst.Add(1);
                        lst.Add(-1);
                        for (var i = PageCount - 6; i <= PageCount; i++)
                        {
                            lst.Add(i);
                        }
                    }
                    else
                    {
                        lst.Add(1);
                        lst.Add(-1);
                        var begin = PageIndex - 2;
                        var end = PageIndex + 2;
                        if (end > PageCount)
                        {
                            end = PageCount;
                            begin = end - 4;
                            if (PageIndex - begin < 2)
                            {
                                begin = begin - 1;
                            }
                        }
                        else if (end + 1 == PageCount)
                        {
                            end = PageCount;
                        }
                        for (var i = begin; i <= end; i++)
                        {
                            lst.Add(i);
                        }
                        if (end != PageCount)
                        {
                            lst.Add(-1);
                            lst.Add(PageCount);
                        }
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    UCBtnExt c = (UCBtnExt)this.tableLayoutPanel.Controls.Find("p" + (i + 1), false)[0];
                    if (i >= lst.Count)
                    {
                        c.Visible = false;
                    }
                    else
                    {
                        if (lst[i] == -1)
                        {
                            c.BtnText = "...";
                            c.Enabled = false;
                        }
                        else
                        {
                            c.BtnText = lst[i].ToString();
                            c.Enabled = true;
                        }
                        c.Visible = true;
                        if (lst[i] == PageIndex)
                        {
                            c.RectColor = Color.FromArgb(255, 77, 59);
                        }
                        else
                        {
                            c.RectColor = Color.FromArgb(223, 223, 223);
                        }
                    }
                }
                ShowBtn(PageIndex > 1, PageIndex < PageCount);
            }
            finally
            {
                ControlHelper.FreezeControl(tableLayoutPanel, false);
            }
        }

        /// <summary>
        /// Handles the BtnClick event of the page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void page_BtnClick(object sender, EventArgs e)
        {
            PageIndex = (sender as UCBtnExt).BtnText.ToInt();
            StartIndex = (PageIndex - 1) * PageSize;
            ReloadPage();
            if (PageModel == Scada.Controls.Controls.PageModel.Soure)
            {
                var s = GetCurrentSource();

                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
            else
            {
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(this);
                }
            }
        }

        /// <summary>
        /// 控制按钮显示
        /// </summary>
        /// <param name="blnLeftBtn">是否显示上一页，第一页</param>
        /// <param name="blnRightBtn">是否显示下一页，最后一页</param>
        protected override void ShowBtn(bool blnLeftBtn, bool blnRightBtn)
        {
            btnFirst.Enabled = btnPrevious.Enabled = blnLeftBtn;
            btnNext.Enabled = btnEnd.Enabled = blnRightBtn;
        }

        /// <summary>
        /// Handles the BtnClick event of the btnFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnFirst_BtnClick(object sender, EventArgs e)
        {
            FirstPage();
        }

        /// <summary>
        /// Handles the BtnClick event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnPrevious_BtnClick(object sender, EventArgs e)
        {
            PreviousPage();
        }

        /// <summary>
        /// Handles the BtnClick event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnNext_BtnClick(object sender, EventArgs e)
        {
            NextPage();
        }

        /// <summary>
        /// Handles the BtnClick event of the btnEnd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnEnd_BtnClick(object sender, EventArgs e)
        {
            EndPage();
        }

        /// <summary>
        /// Handles the BtnClick event of the btnToPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnToPage_BtnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPage.InputText))
            {
                PageIndex = txtPage.InputText.ToInt();
                StartIndex = (PageIndex - 1) * PageSize;
                ReloadPage();
                if (PageModel == Scada.Controls.Controls.PageModel.Soure)
                {
                    var s = GetCurrentSource();
                    if (ShowSourceChanged != null)
                    {
                        ShowSourceChanged(s);
                    }
                }
                else
                {
                    if (ShowSourceChanged != null)
                    {
                        ShowSourceChanged(this);
                    }
                }
            }
        }

        private void ucComboxPageSize_SelectedChangedEvent(object sender, EventArgs e)
        {
            PageIndex = 1;
            PageSize = int.Parse(ucComboxPageSize.SelectedValue);
            ReloadPage();
            if (PageModel == Scada.Controls.Controls.PageModel.Soure)
            {
                var s = GetCurrentSource();
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(s);
                }
            }
            else
            {
                if (ShowSourceChanged != null)
                {
                    ShowSourceChanged(this);
                }
            }
        }
    }
}
