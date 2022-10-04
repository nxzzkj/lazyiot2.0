using ScadaFlowDesign.Core;
using Scada.FlowGraphEngine.GraphicsMap;
using Scada.FlowGraphEngine.GraphicsShape;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;



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
    public partial class DebugForm : Form
    {
        IOFlowProject Project = null;
        FlowGraphAbstract View = null;
        public DebugForm(IOFlowProject flowProject, FlowGraphAbstract view)
        {
            InitializeComponent();
            Project = flowProject;
            View =view;
            this.WindowState = FormWindowState.Maximized;
            this.Load += DebugForm_Load;
            
        }
        ChromiumWebBrowser webBrowser = null;
        private void DebugForm_Load(object sender, EventArgs e)
        {
            webBrowser = new ChromiumWebBrowser
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webBrowser);
            if (Project != null)
            {
                if (View == null)
                {
                    foreach (FlowGraphAbstract view in Project.GraphList)
                    {
                        BuilderView(view);
                    }
                }
                else
                {
                    BuilderView(View);
                }

            }
        }
        private void BuilderView(FlowGraphAbstract view)
        {
            if (view.Site != null)
            {
                SVG_Color backColor = new SVG_Color(Color.FromArgb(Color.Gray.A & view.mBackgroundColor.A, Color.Gray.R & view.mBackgroundColor.R, Color.Gray.G & view.mBackgroundColor.G, Color.Gray.B & view.mBackgroundColor.B));
                if (view.mBackgroundType == CanvasBackgroundType.Gradient)
                {
                    backColor = new SVG_Color(Color.FromArgb(Color.Gray.A & view.mGradientTop.A, Color.Gray.R & view.mGradientTop.R, Color.Gray.G & view.mGradientTop.G, Color.Gray.B & view.mGradientTop.B));
                }
                string name = view.GID;

                StringBuilder sb = view.Site.ExportSVG();
                StreamWriter sw = new StreamWriter(Application.StartupPath + "/web/debug.html", false, Encoding.UTF8);
                sw.Write(@"<!DOCTYPE html>

<html>
<head>
<meta http-equiv='Content-Type' content='text/html;charset = utf-8' />
    <meta name='viewport' content='width=device-width' />
    <meta name='renderer' content='webkit|ie-comp'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge,chrome =1'>
    <title> " + view.ViewTitle + (view.Index ? "(主视图)" : "") + @"</title>
    <link href='Content/lib/layui/css/layui.css' rel='stylesheet' />
    <link href='Content/css/okadmin.css' rel='stylesheet' />
    <link href='Content/css/common.css' rel='stylesheet' />
    <script src='Content/lib/layui/layui.js'></script>
    <script src='Content/js/global.js'></script>
    <script src='Content/js/jquery-3.4.1.js'></script>
    <script src='Content/js/jquery.timers-1.2.js'></script>
    <script  src='Content/js/svg-pan-zoom.js'></script>
    <script src='Content/js/SVG.js'></script>
    <script src='Content/js/Scada.js'></script>

    <script src='Content/js/zy.media.min.js'></script>
</head>
<body style='margin: 0;padding: 0;background-color:" + backColor + @";'  >
 <script src='Content/lib/echarts/echarts.min.js'></script>
    <div id='container' class='layui-layout-body'  style='width:100%; height:100%'>
        " + sb.ToString().Replace("href='/Content", "href='Content").Replace("src='/Content", "src='Content") + @"
    </div>
 <script>
     SCADA.ScadaFlow();
 
   </script>
 
</body>
</html>");
                sw.Close();

                
                webBrowser.Load(Application.StartupPath + "/web/debug.html");

           
            }
        }
      
    private void Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
          
        }
    }
}