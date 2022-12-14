using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOManager.Controls
{
    public partial class TextBoxEx : TextBox
    {
        public TextBoxEx()
        {
            this.KeyPress += (s, e) => {
                //if (e.KeyChar == Convert.ToChar(System.Windows.Forms.Keys.Enter))
                if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z')|| e.KeyChar == Convert.ToChar(System.Windows.Forms.Keys.Back)|| e.KeyChar == Convert.ToChar(System.Windows.Forms.Keys.Delete) || e.KeyChar == '_')
                {

                }
                else
                {
                    e.Handled = true;
               
                }
            };
        }

       
 
    }
    public partial class NumberTextBoxEx : TextBox
    {
        public NumberTextBoxEx()
        {
            this.KeyPress += (s, e) => {
                //if (e.KeyChar == Convert.ToChar(System.Windows.Forms.Keys.Enter))VK_DECIMAL
                if ((e.KeyChar >= '0' && e.KeyChar <= '9') ||  e.KeyChar == Convert.ToChar(System.Windows.Forms.Keys.Back) || e.KeyChar == Convert.ToChar(System.Windows.Forms.Keys.Delete)|| e.KeyChar == '.'|| e.KeyChar == '-')//SUBSTRACT
                {

                }
                else
                {
                    e.Handled = true;

                }
            };
        }
        public override string Text
        {
            get
            {
               
                return base.Text;
            }

            set
            {
                base.Text = value;
                if (base.Text == "")
                    base.Text = "0";
            }
        }


    }
}
