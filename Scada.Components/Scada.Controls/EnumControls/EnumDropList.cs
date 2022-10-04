using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Controls.EnumControls
{
    public partial class EnumDropList : UserControl
    {
        public Action<Enum> SelectedChanged;
        public EnumDropList()
        {
            InitializeComponent();
        }


        public void SetItems<T>()
        {
            this.comboBox.Items.Clear();
            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                this.comboBox.Items.Add(item);

            }
            if (this.comboBox.Items.Count > 0)
                this.comboBox.SelectedIndex = 0;


        }
        public Enum SelectedItem
        {
            get
            {
                if (this.comboBox.SelectedItem == null)
                    return null;
                return (Enum)this.comboBox.SelectedItem;

            }
            set
            {

                for (int i = 0; i < this.comboBox.Items.Count; i++)
                {
                    if (this.comboBox.Items[i].ToString() == value.ToString())
                    {
                        this.comboBox.SelectedIndex = i;
                        break;
                    }
                }


            }
        }
        public int SelectedIndex
        {
            set { this.comboBox.SelectedIndex = value; }
            get { return this.comboBox.SelectedIndex; }
        }
        /// <summary>
        /// 获取 enum 的描述信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tField"></param>
        /// <returns></returns>
        private   string GetEnumDesc<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(DescriptionAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (DescriptionAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Description ?? string.Empty;
            }
            return description;
        }

        private   string GetEnumCategory<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(CategoryAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (CategoryAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Category ?? string.Empty;
            }
            return description;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string desc = GetEnumDesc(SelectedItem);
            if (!string.IsNullOrEmpty(desc))
            {
                this.labelDesc.Text = desc;
            }
            if (SelectedChanged!=null)
            {
                SelectedChanged(SelectedItem);

               
            }
        }
    }
}
