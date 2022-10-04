

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
using System.Windows.Forms;
using Scada.Controls.Controls;

namespace Scada.Controls.Forms
{
    /// <summary>
    /// Class FrmInputs.
    /// Implements the <see cref="Scada.Controls.Forms.FrmWithOKCancel1" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Forms.FrmWithOKCancel1" />
    public partial class FrmInputs : FrmWithOKCancel1
    {
        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public string[] Values { get; private set; }
        /// <summary>
        /// The m mast inputs
        /// </summary>
        private Dictionary<int, string> m_mastInputs = new Dictionary<int, string>();
        #region 构造函数
        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:HZH
        /// 创建日期:2019-08-05 10:57:26
        /// 任务编号:POS
        /// </summary>
        /// <param name="strTitle">窗体标题</param>
        /// <param name="inPutLabels">The in put labels.</param>
        /// <param name="inTypes">输入项对应输入类型，key:输入项名称，如不设置默认不控制输入</param>
        /// <param name="regexs">输入项对应正则规则，当imTypes=Regex时有效，key:输入项名称，如不设置默认不控制输入</param>
        /// <param name="keyBoards">文本框键盘，key:输入项名称，如不设置默认英文键盘</param>
        /// <param name="mastInputs">必填输入项名称</param>
        /// <param name="defaultValues">输入项默认值，key:输入项名称</param>
        /// <exception cref="System.Exception">输入数量不能为空</exception>
        /// <exception cref="Exception">输入数量不能为空</exception>
        public FrmInputs(
            string strTitle,
            string[] inPutLabels,
            Dictionary<string, TextInputType> inTypes = null,
            Dictionary<string, string> regexs = null,
            Dictionary<string, Scada.Controls.Controls.KeyBoardType> keyBoards = null,
            List<string> mastInputs = null,
            Dictionary<string, string> defaultValues = null)
        {
            InitializeComponent();
            this.Title = strTitle;
            if (inPutLabels.Length <= 0)
            {
                throw new Exception("输入数量不能为空");
            }
            try
            {
                Values = new string[inPutLabels.Length];
                Scada.Controls.ControlHelper.FreezeControl(this, true);

                for (int i = inPutLabels.Length - 1; i >= 0; i--)
                {
                    Panel p = new Panel();
                    p.Dock = DockStyle.Top;
                    p.Height = 62;
                    p.Padding = new Padding(10);

                    Scada.Controls.Controls.UCTextBoxEx txt = new Controls.UCTextBoxEx();
                    txt.Dock = DockStyle.Fill;
                    txt.IsShowKeyboard = true;
                    txt.IsShowClearBtn = true;
                    txt.Name = "txt_" + i;
                    txt.TabIndex = i;
                    if (inTypes != null && inTypes.ContainsKey(inPutLabels[i]))
                    {
                        txt.InputType = inTypes[inPutLabels[i]];
                        if (txt.InputType == TextInputType.Regex && regexs != null && regexs.ContainsKey(inPutLabels[i]))
                            txt.RegexPattern = regexs[inPutLabels[i]];
                    }
                    if (keyBoards != null && keyBoards.ContainsKey(inPutLabels[i]))
                        txt.KeyBoardType = keyBoards[inPutLabels[i]];
                    if (mastInputs != null && mastInputs.Contains(inPutLabels[i]))
                    {
                        m_mastInputs[i] = inPutLabels[i];
                    }
                    if (defaultValues != null && defaultValues.ContainsKey(inPutLabels[i]))
                        txt.InputText = defaultValues[inPutLabels[i]];
                    p.Controls.Add(txt);

                    Label lbl = new Label();
                    lbl.Text = inPutLabels[i];
                    lbl.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
                    lbl.TextAlign = ContentAlignment.MiddleRight;
                    lbl.AutoSize = false;
                    lbl.Width = 120;
                    lbl.Dock = DockStyle.Left;
                    lbl.Font = new System.Drawing.Font("微软雅黑", 12);
                    p.Controls.Add(lbl);

                    Label lblT = new Label();
                    if (mastInputs != null && mastInputs.Contains(inPutLabels[i]))
                    {
                        lblT.Text = "*";
                    }
                    else
                    {
                        lblT.Text = "";
                    }
                    lblT.AutoSize = false;
                    lblT.TextAlign = ContentAlignment.MiddleLeft;
                    lblT.Width = 25;
                    lblT.Dock = DockStyle.Right;
                    lblT.Font = new System.Drawing.Font("微软雅黑", 12);
                    lblT.ForeColor = Color.Red;
                    p.Controls.Add(lblT);
                    this.panel3.Controls.Add(p);
                    this.ActiveControl = txt;
                }

                this.Height = 124 + inPutLabels.Length * 62;
            }
            finally
            {
                Scada.Controls.ControlHelper.FreezeControl(this, false);
            }
        }
        #endregion

        /// <summary>
        /// Does the enter.
        /// </summary>
        protected override void DoEnter()
        {
            for (int i = 0; i < Values.Length; i++)
            {
                var cs = this.panel3.Controls.Find("txt_" + i, true);
                if (cs.Length > 0)
                {
                    var txt = cs[0] as Scada.Controls.Controls.UCTextBoxEx;
                    Values[i] = txt.InputText;
                    if (m_mastInputs.ContainsKey(i) && string.IsNullOrWhiteSpace(txt.InputText))
                    {
                        Scada.Controls.Forms.FrmTips.ShowTipsInfo(this, "[" + m_mastInputs[i] + "]必须输入。");                        
                        this.ActiveControl = txt.txtInput;
                        return;
                    }
                }
            }
            base.DoEnter();
        }
    }
}
