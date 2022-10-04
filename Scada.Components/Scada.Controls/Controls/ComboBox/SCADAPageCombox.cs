using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    /// 定义一个通用的页面显示数量的控件
    /// </summary>
    public class SCADAPageCombox : UCCombox
    {
        public SCADAPageCombox()
        {
            this.Load += SCADAPageCombox_Load;

        }

        private void SCADAPageCombox_Load(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lstCom = new List<KeyValuePair<string, string>>();

            lstCom.Add(new KeyValuePair<string, string>("100", "显示最近100条"));
            lstCom.Add(new KeyValuePair<string, string>("200", "显示最近200条"));
            lstCom.Add(new KeyValuePair<string, string>("300", "显示最近300条"));
            lstCom.Add(new KeyValuePair<string, string>("400", "显示最近400条"));
            lstCom.Add(new KeyValuePair<string, string>("500", "显示最近500条"));
            lstCom.Add(new KeyValuePair<string, string>("600", "显示最近600条"));
            lstCom.Add(new KeyValuePair<string, string>("700", "显示最近700条"));
            lstCom.Add(new KeyValuePair<string, string>("800", "显示最近800条"));
            lstCom.Add(new KeyValuePair<string, string>("900", "显示最近900条"));
            lstCom.Add(new KeyValuePair<string, string>("1000", "显示最近1000条"));
            this.Source = lstCom;
            this.SelectedIndex = 0;
        }
    }
}
