using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.LazyOSApp
{
    public class CEFKeyBoardHander : IKeyboardHandler
    {
        AppForm AppForm = null;
        public CEFKeyBoardHander(AppForm appform)
        {
            AppForm = appform;
        }
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode))
            {
                var key = (Keys)windowsKeyCode;
                switch (key)
                {
                    case Keys.F12:
                        if ( AppForm.FormBorderStyle == FormBorderStyle.None)
                        {
                            AppForm.FormBorderStyle = FormBorderStyle.FixedDialog;     //设置窗体为无边框样式
                            AppForm.WindowState = FormWindowState.Maximized;    //最大化窗体
                            AppForm.TopMost = false;
                            AppForm.ToolStrip.Visible = true;
                        }
                        else
                        {
                            AppForm.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
                            AppForm.WindowState = FormWindowState.Maximized;    //最大化窗体
                            AppForm.TopMost = true;
                            AppForm.ToolStrip.Visible = false;
                        }
               
                        break;

                    case Keys.F5:

                        if (modifiers == CefEventFlags.ControlDown)
                        {
                      
                            browser.Reload(true); //强制忽略缓存

                        }
                        else
                        {
                    
                            browser.Reload();
                        }
                        break;


                }
            }
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }
    }
}
