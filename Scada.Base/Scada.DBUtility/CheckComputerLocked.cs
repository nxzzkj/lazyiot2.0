

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
using System.Threading;
using System.Threading.Tasks;

namespace Scada.DBUtility
{
    public enum WTS_INFO_CLASS
    {
        WTSInitialProgram,
        WTSApplicationName,
        WTSWorkingDirectory,
        WTSOEMId,
        WTSSessionId,
        WTSUserName,
        WTSWinStationName,
        WTSDomainName,
        WTSConnectState,
        WTSClientBuildNumber,
        WTSClientName,
        WTSClientDirectory,
        WTSClientProductId,
        WTSClientHardwareId,
        WTSClientAddress,
        WTSClientDisplay,
        WTSClientProtocolType,
        WTSIdleTime,
        WTSLogonTime,
        WTSIncomingBytes,
        WTSOutgoingBytes,
        WTSIncomingFrames,
        WTSOutgoingFrames,
        WTSClientInfo,
        WTSSessionInfo,
        WTSSessionInfoEx,
        WTSConfigInfo,
        WTSValidationInfo,   // Info Class value used to fetch Validation Information through the WTSQuerySessionInformation
        WTSSessionAddressV4,
        WTSIsRemoteSession
    }
    public enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,              // User logged on to WinStation
        WTSConnected,           // WinStation connected to client
        WTSConnectQuery,        // In the process of connecting to client
        WTSShadow,              // Shadowing another WinStation
        WTSDisconnected,        // WinStation logged on without client
        WTSIdle,                // Waiting for client to connect
        WTSListen,              // WinStation is listening for connection
        WTSReset,               // WinStation is being reset
        WTSDown,                // WinStation is down due to error
        WTSInit,                // WinStation in initialization
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct WTSINFOEXW
    {
        public int Level;
        public WTSINFOEX_LEVEL_W Data;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct WTSINFOEX_LEVEL_W
    {
        public WTSINFOEX_LEVEL1_W WTSInfoExLevel1;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WTSINFOEX_LEVEL1_W
    {
        public int SessionId;
        public WTS_CONNECTSTATE_CLASS SessionState;
        public int SessionFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string WinStationName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string DomainName;
        public LARGE_INTEGER LogonTime;
        public LARGE_INTEGER ConnectTime;
        public LARGE_INTEGER DisconnectTime;
        public LARGE_INTEGER LastInputTime;
        public LARGE_INTEGER CurrentTime;
        public uint IncomingBytes;
        public uint OutgoingBytes;
        public uint IncomingFrames;
        public uint OutgoingFrames;
        public uint IncomingCompressedBytes;
        public uint OutgoingCompressedBytes;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct LARGE_INTEGER //此结构体在C++中使用的为union结构，在C#中需要使用FieldOffset设置相关的内存起始地址
    {
        [FieldOffset(0)]
        uint LowPart;
        [FieldOffset(4)]
        int HighPart;
        [FieldOffset(0)]
        long QuadPart;
    }
    /// <summary>
    /// 监控电脑是否在锁屏状态
    /// </summary>
    public   class  ComputerStatusMonitor
    {
        [DllImport("Wtsapi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WTSQuerySessionInformationW(IntPtr hServer, uint SessionId, WTS_INFO_CLASS WTSInfoClass, ref IntPtr ppBuffer, ref uint pBytesReturned);
        [DllImport("Wtsapi32.dll", CharSet = CharSet.Unicode)]
        public static extern void WTSFreeMemory(IntPtr pMemory);
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint WTSGetActiveConsoleSessionId();
        public Action ComputerLocked;
        public Action ComputerUnLocked;
        private bool  isMonitor = false;
        private Task monitorTask;
        static CancellationTokenSource cts = new CancellationTokenSource();
        public void Start()
        {
            isMonitor = true;
            monitorTask = Task.Run(() =>
            {

                uint dwSessionID = WTSGetActiveConsoleSessionId();
                uint dwBytesReturned = 0;
                int dwFlags = 0;
                IntPtr pInfo = IntPtr.Zero;

                while (isMonitor)
                {
                    if (cts.Token.IsCancellationRequested)//如果检测到取消请求
                    {
                        break;
                    }
                    WTSQuerySessionInformationW(IntPtr.Zero, dwSessionID, WTS_INFO_CLASS.WTSSessionInfoEx, ref pInfo, ref dwBytesReturned);
                    var shit = Marshal.PtrToStructure<WTSINFOEXW>(pInfo);

                    if (shit.Level == 1)
                    {
                        dwFlags = shit.Data.WTSInfoExLevel1.SessionFlags;
                    }
                    switch (dwFlags)
                    {
                        case 0: if(ComputerLocked!=null) ComputerLocked(); break;
                        case 1: if(ComputerUnLocked!=null) ComputerUnLocked(); break;
                        default: ; break;
                    }

                    Thread.Sleep(30000);
                }

            });

        }
        
        public void Close()
        {
            isMonitor = false;
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }
}
