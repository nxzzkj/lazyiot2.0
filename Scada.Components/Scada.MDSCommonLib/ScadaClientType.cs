

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
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Scada.MDSCore
{
    public enum ScadaClientType
    {
        WebSystem = 1, FlowDesign = 2, IoManager = 3, IoMonitor = 4, IoServer = 5,None=6
    }
    public delegate Task CommandHandle(byte[] datas, long commandid, string msg, MDSCommandType command);
    public delegate Task HeartBeatHandle(string appName, bool isconnect);
    public delegate void SendFileHandle(string appName, long total, long current, bool isend);
    public abstract class TcpHostMAC
    {
        private static string mac = "";
        public static string GetMAC()
        {
            string madAddr = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();
            foreach (ManagementObject mo in moc2)
            {
                if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                {
                    madAddr = mo["MacAddress"].ToString();
                    madAddr = madAddr.Replace(":", "").Replace("-", "");
                }
                mo.Dispose();
            }
            mac = madAddr;
            return madAddr;
        }

    }
    /// <summary>
    /// 解析后的数据包
    /// </summary>
    public class SplitPackageMessage:IDisposable
    {
        public byte[] Datas { set; get; }
        public int Length { set; get; }
        public string ServerId { set; get; }
        public MDSCommandType MDSCommandType { set; get; }
        public string Message { set; get; }
        public bool IsInvalid { set; get; }
        public ScadaClientType ClientType = ScadaClientType.None;
        //
        /// <summary>
        /// 数据原始的ID
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 回复数据的ID
        /// </summary>
        public string RepliedMessageId { get; set; }

        /// <summary>
        ///  消息的第一个源服务器的名称。
        /// </summary>
        public string SourceServerName { set; get; }

        /// <summary>
        /// 消息的第一个源服务器的应用程序名称。
        /// </summary>
        public string SourceApplicationName { set; get; }

        /// <summary>
        ///源通信通道（通信器）的唯一Id。
        ///当一个应用程序的多个通信器连接到同一MDS服务器时
        ///同时，此字段可用于指示专用通信器。
        ///此字段由MDS自动设置。
        /// </summary>
        public long SourceCommunicatorId { set; get; }

        /// <summary>
        /// 目标服务器
        /// </summary>
        public string DestinationServerName { set; get; }

        /// <summary>
        /// 目标服务器应用程序
        /// </summary>
        public string DestinationApplicationName { set; get; }

        /// <summary>
        ///目的地通信通道（通信器）Id。
        ///MDS使用此字段向spesific communicator传递消息。
        ///当一个应用程序的多个通信器连接到同一MDS服务器时
        ///同时，该字段可用于指示spesific通信器作为消息的接收器。
        ///如果设置为0（零），则消息可能会传递给任何连接的通讯器
        /// </summary>
        public long DestinationCommunicatorId { set; get; }

        public EndPoint RemoteEndPoint { set; get; }

        public void Dispose()
        {
            RemoteEndPoint = null;
            DestinationApplicationName = "";
            DestinationServerName = "";
            SourceApplicationName = "";
            SourceServerName = "";
            RepliedMessageId = "";
            MessageId = "";
            Message = "";
            ServerId = "";
            Datas = null;
        }
    }
    /// <summary>
    /// 数据包的解析与打包
    /// </summary>
    public class SplitPackage
    {

        /// <summary>
        /// MAC或者是ServerID得固定长度，一般都是serverID就是MAC
        /// </summary>
        private static int mServerIDByteCount = 0;
        public static int ServerIDByteCount
        {
            get
            {
                if (mServerIDByteCount == 0)
                    mServerIDByteCount = Encoding.UTF8.GetBytes(TcpHostMAC.GetMAC()).Count();
                return mServerIDByteCount;
            }
        }
        /// <summary>
        /// 数组比较是否相等
        /// </summary>
        /// <param name="bt1">数组1</param>
        /// <param name="bt2">数组2</param>
        /// <returns>true:相等，false:不相等</returns>
        public static bool CompareArray(byte[] bt1, byte[] bt2)
        {
            var len1 = bt1.Length;
            var len2 = bt2.Length;
            if (len1 != len2)
            {
                return false;
            }
            for (var i = 0; i < len1; i++)
            {
                if (bt1[i] != bt2[i])
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 将一个数据组字节包装成当前的标准数据包
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] AssembleBytes(ArraySegment<byte> datas, string serverId, MDSCommandType commandType, ScadaClientType clientType)
        {
            return AssembleBytes(datas.Array, serverId, commandType, clientType);

        }
        public static byte[] AssembleBytes(byte[] datas, string serverId, MDSCommandType commandType, ScadaClientType clientType)
        {
            try
            {
                byte[] commandTypeBytes = new byte[1] { (byte)commandType };
                byte[] clientTypeBytes = new byte[1] { (byte)clientType };
                byte[] lengthBytes = BitConverter.GetBytes(Convert.ToInt32(datas.Length));
                byte[] macBytes = Encoding.UTF8.GetBytes(serverId);
                byte[] newData = new byte[datas.Length + ServerIDByteCount + lengthBytes.Length + 2];
                //操作符+MAC地址长度+数据长度+数据包数据
                newData[0] = commandTypeBytes[0];//操作符合
                newData[1] = clientTypeBytes[0];//客户端标识
                System.Array.Copy(macBytes, 0, newData, 2, macBytes.Length);//MAC/ServerID
                System.Array.Copy(lengthBytes, 0, newData, 2 + macBytes.Count(), lengthBytes.Length);//MAC/ServerID
                System.Array.Copy(datas, 0, newData, 2 + macBytes.Count() + lengthBytes.Count(), datas.Count());//MAC/ServerID
                return newData;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 传入的数据是通过尾标识进行分隔的数据，所有此数据并不包含尾字节
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="client"></param>
        /// <param name="MAC"></param>
        /// <returns></returns>
        public static SplitPackageMessage RemoveIdentificationBytes(byte[] datas)
        {
            SplitPackageMessage splitPackage = new SplitPackageMessage();
            splitPackage.Message = "数据不符合规则";
            splitPackage.Datas = null;
            splitPackage.Length = 0;
            splitPackage.ServerId = "";
            splitPackage.IsInvalid = false;
            if (datas.Length < 0)
                return splitPackage;
            if (datas.Length - 6 - ServerIDByteCount <= 0)
            {
                splitPackage.Message = "数据不符合规则";
                splitPackage.Datas = null;
                splitPackage.Length = 0;
                splitPackage.ServerId = "";
                return splitPackage;
            }

            //首先判断数据体是否否和数据包的要求
            //准备分隔数据包，由于本系统全部将字符串转换为字节，所以需要通过字符串来分隔
            byte[] commandtypebytes = new byte[1];//操作符
            byte[] clienttypebytes = new byte[1];//客户端标识
            byte[] macbytes = new byte[ServerIDByteCount];//MAC地址
            byte[] reallengthbytes = new byte[4];//实际数据长度字节
            byte[] realdatas = new byte[datas.Length - 6 - ServerIDByteCount];//存储数据的实际长度

            try
            {
                commandtypebytes[0] = datas[0];//操作符标识
                clienttypebytes[0] = datas[1];//操作符标识
                System.Array.Copy(datas, 2, macbytes, 0, macbytes.Length);
                splitPackage.ServerId = Encoding.UTF8.GetString(macbytes);//获取MAC
                //获取数据长度的变量标识
                System.Array.Copy(datas, 2+ macbytes.Length, reallengthbytes, 0, reallengthbytes.Length);
                //获取实际数据
                System.Array.Copy(datas, 2 + macbytes.Length+ reallengthbytes.Length, realdatas, 0, realdatas.Length);

                splitPackage.Length= BitConverter.ToInt32(reallengthbytes, 0);
                splitPackage.Datas = realdatas;
                splitPackage.IsInvalid = true;
                splitPackage.MDSCommandType = (MDSCommandType)Enum.Parse(typeof(MDSCommandType), commandtypebytes[0].ToString());
                splitPackage.ClientType = (ScadaClientType)Enum.Parse(typeof(ScadaClientType), clienttypebytes[0].ToString());
                return splitPackage;
            }
            catch (Exception emx)
            {
                splitPackage.Message = "" + emx.Message;
                splitPackage.Datas = null;
                splitPackage.Length = 0;
                splitPackage.ServerId = "";
                return splitPackage;
            }



        }
    }

}
