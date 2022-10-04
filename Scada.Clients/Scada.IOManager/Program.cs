﻿using IOManager.Core;
using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
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
namespace IOManager
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
         

            bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                if (IPAddressSelector.Instance().ShowDialog() == DialogResult.OK)
                {
                    Application.EnableVisualStyles();
                    System.Windows.Forms.Application.DoEvents();

                   
                    Application.ThreadException += Application_ThreadException;
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    LoginForm form = new LoginForm();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new IOMainForm());
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

        private static  void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
         
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private static  void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
           
            MessageBox.Show(e.Exception.Message);
        }
    }
}
