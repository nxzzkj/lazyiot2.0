using Scada.Business;
using ScadaCenterServer.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaCenterServer.Pages
{
    public partial class IOServerAdminForm : PopBaseForm
    {
        public IOServerAdminForm()
        {
            InitializeComponent();
            this.Load += IOServerAdminForm_Load;
        }

        private void IOServerAdminForm_Load(object sender, EventArgs e)
        {
            IO_SERVER serverBll = new IO_SERVER();
            List<Scada.Model.IO_SERVER> servers = serverBll.GetModelList("");
            this.listView.Items.Clear();
            for (int i = 0; i < servers.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(servers[i].SERVER_ID);
                lvi.Tag = servers[i].SERVER_ID;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = servers[i].SERVER_NAME });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = servers[i].SERVER_STATUS == 1 ? "在线" : "离线" });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = servers[i].SERVER_IP });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = servers[i].SERVER_CREATEDATE });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = servers[i].SERVER_REMARK });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "删除" });
                this.listView.Items.Add(lvi);
            }
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView.HitTest(e.X, e.Y);
            if (info.Item != null)
            {
                ListViewItem.ListViewSubItem sbi = info.Item.GetSubItemAt(e.X, e.Y);
                if (sbi.Text == "删除")
                {
                    if (MessageBox.Show("是否要删除" + info.Item.SubItems[1].Text + "采集站", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        IO_SERVER serverBll = new IO_SERVER();
                        serverBll.Delete(info.Item.Tag.ToString());
                        this.listView.Items.Remove(info.Item);
                        MessageBox.Show("删除某个采集站后需要重新启动数据中心服务！");
                    }

                }
            }
        }
    }
}
