

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
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Scada.Controls
{
    /// <summary>
    /// Class NativeMethods.
    /// </summary>
    internal class NativeMethods
    {
        /// <summary>
        /// Enum ComboBoxButtonState
        /// </summary>
        public enum ComboBoxButtonState
        {
            /// <summary>
            /// The state system none
            /// </summary>
            STATE_SYSTEM_NONE,
            /// <summary>
            /// The state system invisible
            /// </summary>
            STATE_SYSTEM_INVISIBLE = 32768,
            /// <summary>
            /// The state system pressed
            /// </summary>
            STATE_SYSTEM_PRESSED = 8
        }

        /// <summary>
        /// Struct RECT
        /// </summary>
        public struct RECT
        {
            /// <summary>
            /// The left
            /// </summary>
            public int Left;

            /// <summary>
            /// The top
            /// </summary>
            public int Top;

            /// <summary>
            /// The right
            /// </summary>
            public int Right;

            /// <summary>
            /// The bottom
            /// </summary>
            public int Bottom;

            /// <summary>
            /// Gets the rect.
            /// </summary>
            /// <value>The rect.</value>
            public Rectangle Rect
            {
                get
                {
                    return new Rectangle(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);
                }
            }

            /// <summary>
            /// Gets the size.
            /// </summary>
            /// <value>The size.</value>
            public Size Size
            {
                get
                {
                    return new Size(this.Right - this.Left, this.Bottom - this.Top);
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT" /> struct.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="top">The top.</param>
            /// <param name="right">The right.</param>
            /// <param name="bottom">The bottom.</param>
            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT" /> struct.
            /// </summary>
            /// <param name="rect">The rect.</param>
            public RECT(Rectangle rect)
            {
                this.Left = rect.Left;
                this.Top = rect.Top;
                this.Right = rect.Right;
                this.Bottom = rect.Bottom;
            }

            /// <summary>
            /// Froms the xywh.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            /// <returns>NativeMethods.RECT.</returns>
            public static NativeMethods.RECT FromXYWH(int x, int y, int width, int height)
            {
                return new NativeMethods.RECT(x, y, x + width, y + height);
            }

            /// <summary>
            /// Froms the rectangle.
            /// </summary>
            /// <param name="rect">The rect.</param>
            /// <returns>NativeMethods.RECT.</returns>
            public static NativeMethods.RECT FromRectangle(Rectangle rect)
            {
                return new NativeMethods.RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }

        /// <summary>
        /// Struct PAINTSTRUCT
        /// </summary>
        public struct PAINTSTRUCT
        {
            /// <summary>
            /// The HDC
            /// </summary>
            public IntPtr hdc;

            /// <summary>
            /// The f erase
            /// </summary>
            public int fErase;

            /// <summary>
            /// The rc paint
            /// </summary>
            public NativeMethods.RECT rcPaint;

            /// <summary>
            /// The f restore
            /// </summary>
            public int fRestore;

            /// <summary>
            /// The f inc update
            /// </summary>
            public int fIncUpdate;

            /// <summary>
            /// The reserved1
            /// </summary>
            public int Reserved1;

            /// <summary>
            /// The reserved2
            /// </summary>
            public int Reserved2;

            /// <summary>
            /// The reserved3
            /// </summary>
            public int Reserved3;

            /// <summary>
            /// The reserved4
            /// </summary>
            public int Reserved4;

            /// <summary>
            /// The reserved5
            /// </summary>
            public int Reserved5;

            /// <summary>
            /// The reserved6
            /// </summary>
            public int Reserved6;

            /// <summary>
            /// The reserved7
            /// </summary>
            public int Reserved7;

            /// <summary>
            /// The reserved8
            /// </summary>
            public int Reserved8;
        }

        /// <summary>
        /// Struct ComboBoxInfo
        /// </summary>
        public struct ComboBoxInfo
        {
            /// <summary>
            /// The cb size
            /// </summary>
            public int cbSize;

            /// <summary>
            /// The rc item
            /// </summary>
            public NativeMethods.RECT rcItem;

            /// <summary>
            /// The rc button
            /// </summary>
            public NativeMethods.RECT rcButton;

            /// <summary>
            /// The state button
            /// </summary>
            public NativeMethods.ComboBoxButtonState stateButton;

            /// <summary>
            /// The HWND combo
            /// </summary>
            public IntPtr hwndCombo;

            /// <summary>
            /// The HWND edit
            /// </summary>
            public IntPtr hwndEdit;

            /// <summary>
            /// The HWND list
            /// </summary>
            public IntPtr hwndList;
        }

        /// <summary>
        /// The wm paint
        /// </summary>
        public const int WM_PAINT = 15;

        /// <summary>
        /// The wm setredraw
        /// </summary>
        public const int WM_SETREDRAW = 11;

        /// <summary>
        /// The false
        /// </summary>
        public static readonly IntPtr FALSE = IntPtr.Zero;

        /// <summary>
        /// The true
        /// </summary>
        public static readonly IntPtr TRUE = new IntPtr(1);

        /// <summary>
        /// Gets the ComboBox information.
        /// </summary>
        /// <param name="hwndCombo">The HWND combo.</param>
        /// <param name="info">The information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref NativeMethods.ComboBoxInfo info);

        /// <summary>
        /// Gets the window rect.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="lpRect">The lp rect.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, ref NativeMethods.RECT lpRect);

        /// <summary>
        /// Begins the paint.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="ps">The ps.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref NativeMethods.PAINTSTRUCT ps);

        /// <summary>
        /// Ends the paint.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="ps">The ps.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref NativeMethods.PAINTSTRUCT ps);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        [DllImport("user32.dll")]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }
}
