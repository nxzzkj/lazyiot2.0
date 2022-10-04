using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.License
{
    public partial class LicenseForm : Form
    {
        private string str = "";
        public LicenseForm() : base()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);//双缓冲
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true); //双缓冲

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); //禁止擦除背景.
            this.UpdateStyles();
            if (CheckDesingModel.IsDesingMode())
            {
                CheckKey();
            }
                this.Paint += LicenseForm_Paint;
            this.Load += LicenseForm_Load;
        }
        private void CheckKey()
        {

            try
            {
                str = DESEncrypt.Decrypt(Encoding.UTF8.GetString(License.ApplicationLicense), License.LicenseKey + "my820403@126.com");
            }
            catch
            {
                str = "未经授权使用本商业源码需负法律责任！\r\n商业版源码请联系作者马勇 电话18695221159 ";

            }
        }
        private void LicenseForm_Load(object sender, EventArgs e)
        {
            CheckKey();
        }

        private void LicenseForm_Paint(object sender, PaintEventArgs e)
        {
            
                e.Graphics.DrawString(str, new Font("黑体", 14), new SolidBrush(Color.Red), this.ClientRectangle, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

             
        }
    }
}
