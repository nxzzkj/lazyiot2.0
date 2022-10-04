
 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
//在调试过程中如果发现相关的bug或者代码错误等问题可直接微信联系作者。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOManager.Controls
{
    /// <summary>
    /// 定义的一个下拉多选框组件
    /// </summary>
    public class ComboBoxCheckBox : ComboBox
    {
        private const int WM_LBUTTONDOWN = 0x201, WM_LBUTTONDBLCLK = 0x203;
        ToolStripControlHost listBoxHost;
        ToolStripDropDown dropDown;
        public ComboBoxCheckBox()
        {
            CheckedListBox listbox = new CheckedListBox();
            listbox.ItemCheck += Listbox_ItemCheck;
            listbox.BorderStyle = BorderStyle.None;

            listBoxHost = new ToolStripControlHost(listbox);
            dropDown = new ToolStripDropDown();
            dropDown.Width = this.Width;
            dropDown.Items.Add(listBoxHost);
        }




        private void Listbox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            SetCurSelected();
        }

        private void SetCurSelected()
        {

            this.Items.Clear();
            this.Items.Add(GetCheckedText());
            this.SelectedIndex = 0;

        }
        public string GetCheckedID()
        {
            string separator = ",";
            string selval = "";
            for (int i = 0; i < ListBox.CheckedItems.Count; i++)
            {
                selval += ((CheckBoxItem)ListBox.CheckedItems[i]).ID + separator;

            }
            if (selval != "")
            {
                selval = selval.Remove(selval.Length - 1, 1);
            }
            return selval;
        }
        public void  SetChecked(List<string> strs)
        {
            for (int i = 0; i < ListBox.Items.Count; i++)
            {
                if(strs.Contains(((CheckBoxItem)ListBox.Items[i]).ID))
                {
                    ListBox.SetItemChecked(i, true);

                }
              

            }
        }
        public int CheckedCount
        {
            get { return ListBox.CheckedItems.Count; }
        }
        public void Add(CheckBoxItem item)
        {
            ListBox.Items.Add(item);
        }
        public void Clear()
        {
            ListBox.Items.Clear();
            this.Items.Clear();
        }
        public string GetCheckedText()
        {
            string separator = ",";
            string selval = "";
            for (int i = 0; i < ListBox.CheckedItems.Count; i++)
            {
                selval += ((CheckBoxItem)ListBox.CheckedItems[i]).Text + separator;

            }
            if (selval != "")
            {
                selval = selval.Remove(selval.Length - 1, 1);
            }
            return selval;
        }

        public CheckedListBox ListBox
        {
            get { return listBoxHost.Control as CheckedListBox; }
        }
        private void ShowDropDown()
        {
            if (dropDown != null)
            {
                listBoxHost.Size = new Size(DropDownWidth - 2, DropDownHeight);
                dropDown.Show(this, 0, this.Height);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_LBUTTONDOWN)
            {
                ShowDropDown();
                return;
            }
            base.WndProc(ref m);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dropDown != null)
                {
                    dropDown.Dispose();
                    dropDown = null;
                }
            }
            base.Dispose(disposing);
        }
    }

    public class CheckBoxItem
    {
        public string ID { set; get; }
        public string Text { set; get; }
        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
