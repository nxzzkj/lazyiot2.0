 
using Scada.Model;
using IOManager.Controls;
using System;

using IOManager.Core;
using Scada.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
    public partial class IOParaForm : DockContent, ICobaltTab
    { 
        public IOParaForm(Mediator m)
        {
            InitializeComponent();
            mediator = m;
        }
        public IOListView IOListView
        {
            get { return this.ioListView; }
        }
        public  void AddListViewItem(IOListViewItem lvi)
        {
            ioListView.AddListViewItem(lvi);
        }
        public IOListViewItem GetListViewItem(IO_PARA para)
        {
            for(int i=0;i< ioListView.ListView.Items.Count;i++)
            {
                IOListViewItem item = ioListView.ListView.Items[i] as IOListViewItem;
                if(item.Para== para)
                {
                    return item;
                }
            }
            return null;
        }
        private Mediator mediator = null;
        private string identifier;
        /// <summary>
        /// 异步初始化ListView控件
        /// </summary>
        /// <param name="device"></param>
        public  void InitListView(IO_SERVER Server, IO_COMMUNICATION Communication, IO_DEVICE Device)
        {
            this.ioListView.ListView.Items.Clear();
            this.ioListView.Server = Server;
            this.ioListView.Communication = Communication;
            this.ioListView.Device = Device;
            this.ioListView.IOPath= Server.SERVER_NAME + "\\" + Communication.IO_COMM_NAME + "\\" + Device.IO_DEVICE_NAME;
            try
            {
             

                this.ioListView.ListView.BeginUpdate();
                for (int i = 0; i < Device.IOParas.Count; i++)
                {
                    IOListViewItem lvi = new IOListViewItem(Device.IOParas[i]);
                    this.ioListView.AddListViewItem(lvi);
                }
                this.ioListView.ListView.EndUpdate();
            }
            catch(Exception ex)
            {
                  IOManagerUIManager.DisplayException(ex);
            }
           
        }
        public TabTypes TabType
        {
            get
            {
                return TabTypes.PointArea;
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
