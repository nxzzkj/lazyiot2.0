 

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Scada.MDSCore.Settings
{
    /// <summary>
    ///  
    /// </summary>
    public class ApplicationInfoItem
    {
        public string StationName { set; get; }
        /// <summary>
        /// Name of this server.
        /// </summary>
        public string Name { get; set; }
 
        /// <summary>
        /// Predefined communication channels.
        /// </summary>
        public List<CommunicationChannelInfoItem> CommunicationChannels { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationInfoItem()
        {
            CommunicationChannels = new List<CommunicationChannelInfoItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public class CommunicationChannelInfoItem
        {
            /// <summary>
            ///  
            /// </summary>
            public string CommunicationType { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public NameValueCollection CommunicationSettings { get; set; }

            /// <summary>
            ///  
            /// </summary>
            public CommunicationChannelInfoItem()
            {
                CommunicationSettings = new NameValueCollection();
            }
        }
    }
}
