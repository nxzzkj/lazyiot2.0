

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
    /// Class UCTransfer.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [DefaultEvent("Transfered")]
    public partial class UCTransfer : UserControl
    {
        /// <summary>
        /// 移动数据事件
        /// </summary>
        [Description("移动数据事件"), Category("自定义")]
        public event TransferEventHandler Transfered;

        /// <summary>
        /// The left columns
        /// </summary>
        private DataGridViewColumnEntity[] leftColumns;

        /// <summary>
        /// Gets or sets the left columns.
        /// </summary>
        /// <value>The left columns.</value>
        [Description("左侧列表列"), Category("自定义")]
        public DataGridViewColumnEntity[] LeftColumns
        {
            get { return leftColumns; }
            set
            {
                leftColumns = value;
                this.dgvLeft.Columns = leftColumns.ToList();
            }
        }

        /// <summary>
        /// The right columns
        /// </summary>
        private DataGridViewColumnEntity[] rightColumns;

        /// <summary>
        /// Gets or sets the right columns.
        /// </summary>
        /// <value>The right columns.</value>
        [Description("右侧列表列"), Category("自定义")]
        public DataGridViewColumnEntity[] RightColumns
        {
            get { return rightColumns; }
            set
            {
                rightColumns = value;
                this.dgvRight.Columns = leftColumns.ToList();
            }
        }

        /// <summary>
        /// The left data source
        /// </summary>
        private object[] leftDataSource;
        /// <summary>
        /// 左右列表必须设置相同类型的数据源列表，如果为空必须为长度为0的数组
        /// </summary>
        /// <value>The left data source.</value>
        [Description("左侧列表数据源"), Category("自定义"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object[] LeftDataSource
        {
            get { return leftDataSource; }
            set
            {
                leftDataSource = value;
                dgvLeft.DataSource = value;
            }
        }

        /// <summary>
        /// The right data source
        /// </summary>
        private object[] rightDataSource;
        /// <summary>
        /// 左右列表必须设置相同类型的数据源列表，如果为空必须为长度为0的数组
        /// </summary>
        /// <value>The left data source.</value>
        [Description("右侧列表数据源"), Category("自定义"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object[] RightDataSource
        {
            get { return rightDataSource; }
            set
            {
                rightDataSource = value;
                dgvRight.DataSource = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCTransfer"/> class.
        /// </summary>
        public UCTransfer()
        {
            InitializeComponent();
            LeftColumns = new DataGridViewColumnEntity[0];
            RightColumns = new DataGridViewColumnEntity[0];
            LeftDataSource = new object[0];
            RightDataSource = new object[0];
        }

        /// <summary>
        /// Handles the BtnClick event of the btnRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.Exception">
        /// 左右数据源列表不能为空
        /// or
        /// 左右数据源列表类型不一致，无法进行操作
        /// </exception>
        private void btnRight_BtnClick(object sender, EventArgs e)
        {
            if (LeftDataSource == null || RightDataSource == null)
            {
                throw new Exception("左右数据源列表不能为空");
            }
            if (LeftDataSource.GetType() != RightDataSource.GetType())
            {
                throw new Exception("左右数据源列表类型不一致，无法进行操作");
            }
            if (dgvLeft.SelectRows == null || dgvLeft.SelectRows.Count <= 0)
                return;
            List<object> lst = new List<object>();
            dgvLeft.SelectRows.ForEach(p =>
            {
                lst.Add(p.DataSource);
                p.IsChecked = false;
            });
            var lstRight = RightDataSource.ToList();
            lstRight.AddRange(lst);
            var lstLeft = LeftDataSource.ToList();
            lstLeft.RemoveAll(p => lst.Contains(p));
            RightDataSource = lstRight.ToArray();
            LeftDataSource = lstLeft.ToArray();
            if (Transfered != null)
            {
                Transfered(this, new TransferEventArgs() { TransferDataSource = lst.ToArray(), ToRightOrLeft = true });
            }
        }

        /// <summary>
        /// Handles the BtnClick event of the btnLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.Exception">
        /// 左右数据源列表不能为空
        /// or
        /// 左右数据源列表类型不一致，无法进行操作
        /// </exception>
        private void btnLeft_BtnClick(object sender, EventArgs e)
        {
            if (LeftDataSource == null || RightDataSource == null)
            {
                throw new Exception("左右数据源列表不能为空");
            }
            if (LeftDataSource.GetType() != RightDataSource.GetType())
            {
                throw new Exception("左右数据源列表类型不一致，无法进行操作");
            }
            if (dgvRight.SelectRows == null || dgvRight.SelectRows.Count <= 0)
                return;
            List<object> lst = new List<object>();
            dgvRight.SelectRows.ForEach(p =>
            {
                lst.Add(p.DataSource);
                p.IsChecked = false;
            });
            var lstLeft = LeftDataSource.ToList();
            lstLeft.AddRange(lst);
            var lstRight = RightDataSource.ToList();
            lstRight.RemoveAll(p => lst.Contains(p));
            RightDataSource = lstRight.ToArray();
            LeftDataSource = lstLeft.ToArray();
            if (Transfered != null)
            {
                Transfered(this, new TransferEventArgs() { TransferDataSource = lst.ToArray(), ToRightOrLeft = false });
            }
        }
    }
}
