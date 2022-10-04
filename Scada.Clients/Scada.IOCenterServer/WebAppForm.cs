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

namespace ScadaCenterServer
{
    public partial class WebAppForm : Form
    {
        ChromiumWebBrowser webBrowser = null;
        public WebAppForm()
        {
            InitializeComponent();
        }

        private void WebAppForm_Load(object sender, EventArgs e)
        {
            webBrowser = new ChromiumWebBrowser();
            webBrowser.Dock= DockStyle.Fill;
            webBrowser.Load(@"http://localhost:8010//login");
            this.Controls.Add(webBrowser);
        }
    }
}
