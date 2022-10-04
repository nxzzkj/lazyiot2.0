

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
    /// Class UCBtnImg.
    /// Implements the <see cref="Scada.Controls.Controls.UCBtnExt" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCBtnExt" />
    public partial class UCBtnImg : UCBtnExt
    {
        /// <summary>
        /// The BTN text
        /// </summary>
        private string _btnText = "自定义按钮";
        /// <summary>
        /// 按钮文字
        /// </summary>
        /// <value>The BTN text.</value>
        [Description("按钮文字"), Category("自定义")]
        public override string BtnText
        {
            get { return _btnText; }
            set
            {
                _btnText = value;
                lbl.Text = value;
                lbl.Refresh();
            }
        }
        /// <summary>
        /// 图片
        /// </summary>
        /// <value>The image.</value>
        [Description("图片"), Category("自定义")]
        public virtual Image Image
        {
            get
            {
                return this.lbl.Image;
            }
            set
            {
                this.lbl.Image = value;
            }
        }

        /// <summary>
        /// The image font icons
        /// </summary>
        private object imageFontIcons;
        /// <summary>
        /// Gets or sets the image font icons.
        /// </summary>
        /// <value>The image font icons.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(ImagePropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public object ImageFontIcons
        {
            get { return imageFontIcons; }
            set
            {
                if (value == null || value is Image)
                {
                    imageFontIcons = value;
                    if (value != null)
                    {
                        Image = (Image)value;
                    }
                }
            }
        }

        /// <summary>
        /// 图片位置
        /// </summary>
        /// <value>The image align.</value>
        [Description("图片位置"), Category("自定义")]
        public virtual ContentAlignment ImageAlign
        {
            get { return this.lbl.ImageAlign; }
            set { lbl.ImageAlign = value; }
        }
        /// <summary>
        /// 文字位置
        /// </summary>
        /// <value>The text align.</value>
        [Description("文字位置"), Category("自定义")]
        public virtual ContentAlignment TextAlign
        {
            get { return this.lbl.TextAlign; }
            set { lbl.TextAlign = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCBtnImg" /> class.
        /// </summary>
        public UCBtnImg()
        {
            InitializeComponent();
            IsShowTips = false;
            base.BtnForeColor = ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            base.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            base.BtnText = "自定义按钮";
        }
    }
}
