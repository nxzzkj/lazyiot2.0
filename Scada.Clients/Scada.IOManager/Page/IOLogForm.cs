using IOManager.Core;
using Scada.Controls;
 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOManager.Page
{

     
    /*----------------------------------------------------------------
    // Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
    // 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
    // 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
    // 请大家尊重作者的劳动成果，共同促进行业健康发展。
    // 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
    // 创建者：马勇
    //----------------------------------------------------------------*/
    public partial class IOLogForm : DockContent, ICobaltTab
    {
        public IOLogForm(Mediator m)
        {
            InitializeComponent();
            mediator = m;
        
        }
        //异步调用
        public   void AppendText(string text)
        {
            if (this.IsHandleCreated)
            {
                listBoxEx.BeginInvoke(new EventHandler(delegate
                {

                    listBoxEx.Items.Insert(0, text + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (listBoxEx.Items.Count > 100)
                    {
                        listBoxEx.Items.RemoveAt(listBoxEx.Items.Count - 1);
                    }

                }));
            }
           
        }
        private Mediator mediator = null;
        private string identifier;
        public TabTypes TabType
        {
            get
            {
                return TabTypes.Logbook;
            }
        }
        public string TabIdentifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
            }
        }
        public List<string> GetLogContent()
        {
            List<string> sb = new List<string>();
            for(int i=0;i< this.listBoxEx.Items.Count;i++)
            {
                sb.Add(this.listBoxEx.Items[i].ToString());
            }
            return sb;
        }

        private  void 导出TXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
             IOManagerUIManager.ExportLog();
        }
    }
}
