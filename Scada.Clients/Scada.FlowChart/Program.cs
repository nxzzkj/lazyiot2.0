using CefSharp;
using CefSharp.WinForms;
using Scada.DBUtility;
using ScadaFlowDesign.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading;
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
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           
            bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                try
                {
                    var settings = new CefSettings();
                    settings.Locale = "zh-CN";
                    settings.CefCommandLineArgs.Add("disable-gpu", "1");//去掉gpu，否则chrome显示有问题
                    Cef.Initialize(settings);

                }
                catch
                {

                }
                if (IPAddressSelector.Instance().ShowDialog() == DialogResult.OK)
                {
                    Application.EnableVisualStyles();
                    System.Windows.Forms.Application.DoEvents();

                    Application.ApplicationExit += Application_ApplicationExit;
                    Application.ThreadException += Application_ThreadException;
                    Application.ThreadExit += Application_ThreadExit;
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                    LoginForm form = new LoginForm();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        IOFlowManager.StartFlowManager();

                    }
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                MessageBox.Show(null, "有一个和本程序相同的应用程序已经在运行，请不要同时运行多个本程序。\n\n这个程序即将退出。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //   提示信息，可以删除。   
                Application.Exit();//退出程序   
            }



        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionSave();

        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {
         
        }
        /// <summary>
        /// 如果异常了则将保存异常文件
        /// </summary>
        private static void ExceptionSave()
        {
            for (int i = 0; i < IOFlowManager.Projects.Count; i++)
            {
                IOFlowManager.Save(IOFlowManager.Projects[i], Path.GetDirectoryName( IOFlowManager.Projects[i].FileFullName)+"_"+DateTime.Now.ToString("yyyyMMddHHmmss")+ ".flow");

            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ExceptionSave();
            IOFlowManager.AddLogToMainLog(e.Exception.Message);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            
           
           

        }
    }
}
