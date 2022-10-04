

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls
{
    /// <summary>
    /// 鼠标全局钩子
    /// </summary>
    public static class MouseHook
    {
        /// <summary>
        /// The wm mousemove
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;
        /// <summary>
        /// The wm lbuttondown
        /// </summary>
        private const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// The wm rbuttondown
        /// </summary>
        private const int WM_RBUTTONDOWN = 0x204;
        /// <summary>
        /// The wm mbuttondown
        /// </summary>
        private const int WM_MBUTTONDOWN = 0x207;
        /// <summary>
        /// The wm lbuttonup
        /// </summary>
        private const int WM_LBUTTONUP = 0x202;
        /// <summary>
        /// The wm rbuttonup
        /// </summary>
        private const int WM_RBUTTONUP = 0x205;
        /// <summary>
        /// The wm mbuttonup
        /// </summary>
        private const int WM_MBUTTONUP = 0x208;
        /// <summary>
        /// The wm lbuttondblclk
        /// </summary>
        private const int WM_LBUTTONDBLCLK = 0x203;
        /// <summary>
        /// The wm rbuttondblclk
        /// </summary>
        private const int WM_RBUTTONDBLCLK = 0x206;
        /// <summary>
        /// The wm mbuttondblclk
        /// </summary>
        private const int WM_MBUTTONDBLCLK = 0x209;

        /// <summary>
        /// 点
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            /// <summary>
            /// The x
            /// </summary>
            public int x;
            /// <summary>
            /// The y
            /// </summary>
            public int y;
        }

        /// <summary>
        /// 钩子结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            /// <summary>
            /// The pt
            /// </summary>
            public POINT pt;
            /// <summary>
            /// The h WND
            /// </summary>
            public int hWnd;
            /// <summary>
            /// The w hit test code
            /// </summary>
            public int wHitTestCode;
            /// <summary>
            /// The dw extra information
            /// </summary>
            public int dwExtraInfo;
        }
        

        // 全局的鼠标事件
        /// <summary>
        /// Occurs when [on mouse activity].
        /// </summary>
        public static event MouseEventHandler OnMouseActivity;

      
        /// <summary>
        /// The h mouse hook
        /// </summary>
        private static int _hMouseHook = 0; // 鼠标钩子句柄
        /// <summary>
        /// 启动全局钩子
        /// </summary>
        /// <exception cref="System.Exception">SetWindowsHookEx failed.</exception>
        /// <exception cref="Exception">SetWindowsHookEx failed.</exception>
        public static void Start()
        {
            // 安装鼠标钩子
            if (_hMouseHook != 0)
            {
                Stop();
            }
            // 生成一个HookProc的实例.
            WindowsHook.HookMsgChanged += WindowsHook_HookMsgChanged;
            _hMouseHook = WindowsHook.StartHook(HookType.WH_MOUSE_LL);

            //假设装置失败停止钩子
            if (_hMouseHook == 0)
            {
                Stop();
            }
        }

        static void WindowsHook_HookMsgChanged(string strHookName, int nCode, IntPtr msg, IntPtr lParam)
        {
            // 假设正常执行而且用户要监听鼠标的消息
            if (nCode >= 0 && OnMouseActivity != null)
            {
                MouseButtons button = MouseButtons.None;
                int clickCount = 0;

                switch ((int)msg)
                {
                    case WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONUP:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONUP:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                }
                if (button != MouseButtons.None && clickCount > 0)
                {
                    // 从回调函数中得到鼠标的信息
                    MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                    MouseEventArgs e = new MouseEventArgs(button, clickCount, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y, 0);
                    OnMouseActivity(null, e);
                }
            }
        }

        /// <summary>
        /// 停止全局钩子
        /// </summary>
        /// <exception cref="System.Exception">UnhookWindowsHookEx failed.</exception>
        /// <exception cref="Exception">UnhookWindowsHookEx failed.</exception>
        public static void Stop()
        {
            bool retMouse = true;

            if (_hMouseHook != 0)
            {
                retMouse = WindowsHook.StopHook(_hMouseHook);
                _hMouseHook = 0;
            }

            // 假设卸下钩子失败
            if (!(retMouse))
                throw new Exception("UnhookWindowsHookEx failed.");
        }


    }
}
