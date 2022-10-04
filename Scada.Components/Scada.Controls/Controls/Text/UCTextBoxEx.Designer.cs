

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
namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCTextBoxEx.
    /// Implements the <see cref="Scada.Controls.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCControlBase" />
    partial class UCTextBoxEx
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCTextBoxEx));
            this.txtInput = new Scada.Controls.Controls.TextBoxEx();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.btnClear = new System.Windows.Forms.Panel();
            this.btnKeybord = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInput.DecLength = 2;
            this.txtInput.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtInput.InputType = TextInputType.NotControl;
            this.txtInput.Location = new System.Drawing.Point(8, 9);
            this.txtInput.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtInput.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txtInput.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.txtInput.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.txtInput.Name = "txtInput";
            this.txtInput.OldText = null;
            this.txtInput.PromptColor = System.Drawing.Color.Gray;
            this.txtInput.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtInput.PromptText = "";
            this.txtInput.RegexPattern = "";
            this.txtInput.Size = new System.Drawing.Size(309, 24);
            this.txtInput.TabIndex = 0;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ic_cancel_black_24dp.png");
            this.imageList1.Images.SetKeyName(1, "ic_search_black_24dp.png");
            this.imageList1.Images.SetKeyName(2, "keyboard.png");
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::Scada.Controls.Properties.Resources.input_clear;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(227, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(30, 32);
            this.btnClear.TabIndex = 4;
            this.btnClear.Visible = false;
            this.btnClear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnClear_MouseDown);
            // 
            // btnKeybord
            // 
            this.btnKeybord.BackgroundImage = global::Scada.Controls.Properties.Resources.keyboard;
            this.btnKeybord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKeybord.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnKeybord.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnKeybord.Location = new System.Drawing.Point(257, 5);
            this.btnKeybord.Name = "btnKeybord";
            this.btnKeybord.Size = new System.Drawing.Size(30, 32);
            this.btnKeybord.TabIndex = 6;
            this.btnKeybord.Visible = false;
            this.btnKeybord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnKeybord_MouseDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Scada.Controls.Properties.Resources.ic_search_black_24dp;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(287, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(30, 32);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Visible = false;
            this.btnSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSearch_MouseDown);
            // 
            // UCTextBoxEx
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ConerRadius = 5;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnKeybord);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtInput);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.IsShowRect = true;
            this.IsRadius = true;
            this.Name = "UCTextBoxEx";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(322, 42);
            this.Load += new System.EventHandler(this.UCTextBoxEx_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UCTextBoxEx_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// The image list1
        /// </summary>
        private System.Windows.Forms.ImageList imageList1;
        /// <summary>
        /// The text input
        /// </summary>
        public TextBoxEx txtInput;
        /// <summary>
        /// The BTN clear
        /// </summary>
        private System.Windows.Forms.Panel btnClear;
        /// <summary>
        /// The BTN search
        /// </summary>
        private System.Windows.Forms.Panel btnSearch;
        /// <summary>
        /// The BTN keybord
        /// </summary>
        private System.Windows.Forms.Panel btnKeybord;
    }
}
