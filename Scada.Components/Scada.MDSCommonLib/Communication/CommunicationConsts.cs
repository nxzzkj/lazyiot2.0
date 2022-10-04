

namespace Scada.MDSCore.Communication
{
    /// <summary>
    ///  
    /// </summary>
    public sealed class CommunicationConsts
    {
        /// <summary>
        ///  默认链接IP地址
        /// </summary>
        public const string DefaultIpAddress = "127.0.0.1";

        /// <summary>
        /// 默认监听的端口号
        /// </summary>
        public const int DefaultMDSPort = 10905;

        /// <summary>
        ///数据最大字节
        /// </summary>
        public const uint MaxMessageSize = 52428800; //50M
    }
}
