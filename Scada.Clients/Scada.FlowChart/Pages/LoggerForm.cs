using Scada.Controls;
using ScadaFlowDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
namespace ScadaFlowDesign
{
    public partial class LoggerForm : DockContent, ICobaltTab
    {
      
        public LoggerForm(Mediator m )
        {
            InitializeComponent();
            mediator = m;
            this.HideOnClose = true;
        }
        private Mediator mediator = null;
        private string identifier;
        public async void  AppendLogItem(string log)
        {
         await   Task.Run(()=>{
            if (uccbLog.Checked)
            {
                if (this.IsHandleCreated&&  listViewLog.IsHandleCreated)
                {
                    listViewLog.BeginInvoke(new Action(delegate
                {

                    ListViewItem lvi = new ListViewItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    lvi.SubItems.Add(log);
                    listViewLog.Items.Insert(0, lvi);
                    if (listViewLog.Items.Count > int.Parse(ucLateLogSIze.SelectedValue))
                    {
                        listViewLog.Items.RemoveAt(listViewLog.Items.Count - 1);
                    }


                }));
                }
            }
            });


        }
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
    }
}
