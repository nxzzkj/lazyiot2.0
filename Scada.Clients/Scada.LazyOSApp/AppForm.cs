using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.LazyOSApp
{
    public partial class AppForm : Form
    {
        ChromiumWebBrowser webBrowser = null;
        public AppForm()
        {
            InitializeComponent();
        }

        private void AppForm_Load(object sender, EventArgs e)
        {
            webBrowser = new ChromiumWebBrowser();
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.Load(@"http://localhost:8010//login");
            webBrowser.KeyboardHandler = new CEFKeyBoardHander(this);
            this.panel.Controls.Add(webBrowser);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser.Load(@"http://localhost:8010//Home");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ((IWebBrowser)webBrowser).Reload(true); //强制忽略缓存

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;    //最大化窗体
            this.ToolStrip.Visible = false;
            this.TopMost = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ((IWebBrowser)webBrowser).Back();
        }
    }
}
