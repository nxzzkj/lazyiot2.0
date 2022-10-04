using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;

using System.Windows.Forms.Design;
using System.Threading.Tasks;
using Scada.Model;
using IOManager.Core;
using System.Threading;


 
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
    [Designer(typeof(ListViewControlDesigner))]
    public partial class IOListView : UserControl
    {
        public string IOPath
        {
            set
            {
                this.txtPath.Text = value;

            }
            get
            {
                return this.txtPath.Text;
            }
        }
        public IOListView()
        {
            InitializeComponent();
            this.listView.MouseDoubleClick += ListView_MouseDoubleClick;
        }

 

        private   void ListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           if(e.Clicks==2&&e.Button== MouseButtons.Left)
            {
                if (Device != null && this.listView.SelectedItems.Count > 0)
                {
                    IOListViewItem lvi = this.listView.SelectedItems[0] as IOListViewItem;
                      IOManagerUIManager.EditDevicePara(this.Server, this.Communication, this.Device, lvi.Para);

                }
                else
                {
                    MessageBox.Show("请选择要编辑的IO测点");
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ListView ListView
        {
            get { return this.listView; }


        }
        public IO_DEVICE Device
        {
            set;
            get;
        }
        public IO_SERVER Server
        {
            set;
            get;
        }
        public IO_COMMUNICATION Communication
        {
            set;
            get;
        }
        #region ListView操作

        public  void AddListViewItem(IOListViewItem lvi)
        {
            this.listView.Items.Add(lvi);
        }
        public  void RemoveListViewItem(IOListViewItem lvi)
        {
            this.listView.Items.Remove(lvi);
        }
        public  void RemoveListViewItem(IO_PARA para)
        {
            for(int i = this.listView.Items.Count-1;i>=0;i--)
            {
                IOListViewItem item = this.listView.Items[i] as IOListViewItem;
                if(item.Para== para)
                {
                    this.listView.Items.Remove(item);
                    break;
                }
                
            }
       
        }
        public  void RemoveAtListViewItem(int index)
        {
            this.listView.Items.RemoveAt(index);
        }
        /// <summary>
        /// 判断是否已存在此名称的点表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            for (int i = 0; i < this.listView.Items.Count; i++)
            {
                IOListViewItem lvi = this.listView.Items[i] as IOListViewItem;
                if (lvi.Para.IO_NAME.Trim() == name.Trim())
                {
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// 判断是否地址重复
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool ExistAddress(string address)
        {
            for (int i = 0; i < this.listView.Items.Count; i++)
            {
                IOListViewItem lvi = this.listView.Items[i] as IOListViewItem;
                if (lvi.Para.IO_ADDRESS.Trim() == address.Trim())
                {
                    return true;
                }

            }
            return false;
        }
        #endregion
        #region 鼠标右键操作

        public   void 添加参数ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (Device != null)
            {
                  IOManagerUIManager.EditDevicePara(this.Server, this.Communication, this.Device, null);

            }
        }

        public  void 删除参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Device != null)
            {
                if (MessageBox.Show(this.FindForm(), "是否要删除选中的IO测点?", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {


                 
                    for (int i = this.listView.SelectedItems.Count-1; i >= 0; i--)
                    {
                        IOListViewItem lvi = this.listView.SelectedItems[i] as IOListViewItem;
                        
                        lvi.Remove();
                        this.Device.IOParas.Remove(lvi.Para);
                        string name = lvi.SubItems[1].Text;
                        IOManagerUIManager.mediator.IOLogForm.AppendText("删除" + Device.IO_DEVICE_NAME + "设备下" + name + "IO点");
                    }
                }
            }
        }

        public   void 编辑参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Device != null && this.listView.SelectedItems.Count > 0)
            {
                IOListViewItem lvi = this.listView.SelectedItems[0] as IOListViewItem;
                  IOManagerUIManager.EditDevicePara(this.Server, this.Communication, this.Device, lvi.Para);

            }
            else
            {
                MessageBox.Show("请选择要编辑的IO测点");
            }
        }
        //复制的信息
        List<IO_PARA> copyIds = new List<IO_PARA>();
        //剪切的信息
        List<IO_PARA> cutIds = new List<IO_PARA>();
        IO_DEVICE copyDevice = null;
        public  void 复制参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Device != null)
            {
                copyDevice = null;
                cutIds.Clear();
                copyIds.Clear();
                for (int i = this.listView.SelectedItems.Count - 1; i >= 0; i--)
                {
                    IOListViewItem lvi = this.listView.SelectedItems[i] as IOListViewItem;
                    copyIds.Add(lvi.Para);
                }
                copyDevice = Device;

            }
        }

        public  void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Device != null)
            {
                try
                {


                    if (this.IsHandleCreated)
                    {

                        List<IOListViewItem> lvis = new List<IOListViewItem>();
                        if (copyIds.Count > 0)
                        {
                            if (MessageBox.Show(this.FindForm(), "是否要粘贴复制的IO点?", "粘贴提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {


                                for (int i = copyIds.Count - 1; i >= 0; i--)
                                {

                                    IO_PARA newPara = copyIds[i].Copy();
                                    string oldName = newPara.IO_NAME;
                                    newPara.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                    newPara.IO_COMM_ID = Device.IO_COMM_ID;
                                    newPara.IO_SERVER_ID = Device.IO_SERVER_ID;
                                    newPara.IO_ID = Scada.DBUtility.GUIDToNormalID.GuidToLongID().ToString();//分配新的ID
                                    if (newPara != null)
                                    {
                                        newPara.AlarmConfig.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                        newPara.AlarmConfig.IO_COMM_ID = Device.IO_COMM_ID;
                                        newPara.AlarmConfig.IO_SERVER_ID = Device.IO_SERVER_ID;
                                        newPara.AlarmConfig.IO_ID = newPara.IO_ID;
                                    }
                                    bool existName = false;
                                    for (int p = 0; p < Device.IOParas.Count; p++)
                                    {
                                        if (newPara.IO_NAME.Trim() == Device.IOParas[p].IO_NAME.Trim())
                                        {
                                            existName = true;
                                            break;
                                        }
                                    }
                                    //出现相同名称的时候要重新命名
                                    if (existName)
                                    {
                                        newPara.IO_NAME = newPara.IO_NAME + "_C" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                    }

                                    //不是同一个设备的时候要考虑驱动参数
                                    lvis.Add(new IOListViewItem(newPara));
                                 

                                    Device.IOParas.Add(newPara);


                                }
                            }
                        }
                        if (cutIds.Count > 0)
                        {
                            if (copyDevice == Device)
                            {
                                MessageBox.Show("不能剪贴到同一设备下");
                                return;
                            }
                            if (MessageBox.Show(this.FindForm(), "是否要粘贴剪贴的IO点?", "粘贴提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                for (int i = cutIds.Count - 1; i >= 0; i--)
                                {

                                    IO_PARA newPara = cutIds[i].Copy();
                                    string oldName = newPara.IO_NAME;
                                    newPara.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                    newPara.IO_COMM_ID = Device.IO_COMM_ID;
                                    newPara.IO_SERVER_ID = Device.IO_SERVER_ID;
                                    newPara.IO_ID = Scada.DBUtility.GUIDToNormalID.GuidToLongID().ToString();//分配新的ID

                                    if (newPara != null)
                                    {
                                        newPara.AlarmConfig.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                        newPara.AlarmConfig.IO_COMM_ID = Device.IO_COMM_ID;
                                        newPara.AlarmConfig.IO_SERVER_ID = Device.IO_SERVER_ID;
                                        newPara.AlarmConfig.IO_ID = newPara.IO_ID;
                                    }
                                    bool existName = false;
                                    for (int p = 0; p < Device.IOParas.Count; p++)
                                    {
                                        if (newPara.IO_NAME.Trim() == Device.IOParas[p].IO_NAME.Trim())
                                        {
                                            existName = true;
                                            break;
                                        }
                                    }
                                    //出现相同名称的时候要重新命名
                                    if (existName)
                                    {
                                        newPara.IO_NAME = newPara.IO_NAME + "_C" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                    }
                                    lvis.Add(new IOListViewItem(newPara));
                                    Device.IOParas.Add(newPara);
                                    //删除原来的点表信息
                                    copyDevice.IOParas.Remove(cutIds[i]);
                                    if (copyDevice == Device)
                                    {
                                        this.RemoveListViewItem(cutIds[i]);
                                    }

                                }
                                copyDevice = null;
                                cutIds.Clear();
                                copyIds.Clear();
                            }

                        }
                      
                       

                        this.listView.BeginInvoke(new EventHandler(delegate
                        {
                            this.listView.BeginUpdate();
                            for (int i=0;i< lvis.Count;i++)
                            {
                                this.AddListViewItem(lvis[i]);
                                IOManagerUIManager.mediator.IOLogForm.AppendText(" 创建到设备" + Device.IO_DEVICE_NAME + "新IO点" + lvis[i].Para.IO_NAME+"["+ lvis[i].Para.IO_LABEL + "]成功！");

                            }

                            this.listView.EndUpdate();

                        }));
                       
                    }
                }
                catch (Exception ex)
                {
                      IOManagerUIManager.DisplayException(ex);
                }
            }

        }
   

        public  void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Device != null)
            {
                for (int i = 0; i < this.listView.Items.Count; i++)
                {
                    this.listView.Items[i].Selected = false;
                }
            }
        }

        public  void toolStripMenuItem全选_Click(object sender, EventArgs e)
        {
            if (Device != null)
            {
                for(int i=0;i<this.listView.Items.Count;i++)
                {
                    this.listView.Items[i].Selected = true;
                }

            }
        }

        public  void 剪贴toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Device != null)
            {
                copyDevice = null;
                cutIds.Clear();
                copyIds.Clear();
                for (int i = this.listView.SelectedItems.Count - 1; i >= 0; i--)
                {
                    IOListViewItem lvi = this.listView.SelectedItems[i] as IOListViewItem;
                    cutIds.Add(lvi.Para);
                }
                copyDevice = Device;

            }
        }
        #endregion
    }
    public class IOListViewItem : ListViewItem
    {
        public IO_PARA Para = null;
        public IOListViewItem()
        {

        }
        public IOListViewItem(IO_PARA d)
            : base(d.IO_ID)
        {
            Para = d;

            this.SubItems.Add(Para.IO_NAME);
            this.SubItems.Add(Para.IO_LABEL);
            this.SubItems.Add(Para.IO_PARASTRING);
            this.SubItems.Add(Para.IO_DATATYPE);
            this.SubItems.Add(Para.IO_OUTLIES);
            this.SubItems.Add(Para.IO_INITALVALUE);
            this.SubItems.Add(Para.IO_MINVALUE);
            this.SubItems.Add(Para.IO_MAXVALUE);
            if (Para.IO_ENABLERANGECONVERSION == 1)
                this.SubItems.Add("是");
            else
                this.SubItems.Add("否");
            this.SubItems.Add(Para.IO_RANGEMIN);
            this.SubItems.Add(Para.IO_RANGEMAX);
            this.SubItems.Add(Para.IO_POINTTYPE);
            this.SubItems.Add(Para.IO_ZERO);
            this.SubItems.Add(Para.IO_ONE);
            this.SubItems.Add(Para.IO_UNIT);

            if (Para.IO_HISTORY == 1)
                this.SubItems.Add("是");
            else
                this.SubItems.Add("否");
            this.SubItems.Add(Para.IO_ADDRESS);
            if (Para.IO_ENABLEALARM == 1)
                this.SubItems.Add("是");
            else
                this.SubItems.Add("否");
            if (Para.IO_SYSTEM == 1)
                this.SubItems.Add("是");
            else
                this.SubItems.Add("否");

        }
        public void ResetSubItems()
        {
            this.SubItems.Clear();
            if (Para != null)
            {

                this.Text = Para.IO_ID;
                this.SubItems.Add(Para.IO_NAME);
                this.SubItems.Add(Para.IO_LABEL);
                this.SubItems.Add(Para.IO_PARASTRING);
                this.SubItems.Add(Para.IO_DATATYPE);
                this.SubItems.Add(Para.IO_OUTLIES);
                this.SubItems.Add(Para.IO_INITALVALUE);
                this.SubItems.Add(Para.IO_MINVALUE);
                this.SubItems.Add(Para.IO_MAXVALUE);
                if (Para.IO_ENABLERANGECONVERSION == 1)
                    this.SubItems.Add("是");
                else
                    this.SubItems.Add("否");
                this.SubItems.Add(Para.IO_RANGEMIN);
                this.SubItems.Add(Para.IO_RANGEMAX);
                this.SubItems.Add(Para.IO_POINTTYPE);
                this.SubItems.Add(Para.IO_ZERO);
                this.SubItems.Add(Para.IO_ONE);
                this.SubItems.Add(Para.IO_UNIT);

                if (Para.IO_HISTORY == 1)
                    this.SubItems.Add("是");
                else
                    this.SubItems.Add("否");
                this.SubItems.Add(Para.IO_ADDRESS);
                if (Para.IO_ENABLEALARM == 1)
                    this.SubItems.Add("是");
                else
                    this.SubItems.Add("否");
                if (Para.IO_SYSTEM == 1)
                    this.SubItems.Add("是");
                else
                    this.SubItems.Add("否");
            }


        }
    }
}
