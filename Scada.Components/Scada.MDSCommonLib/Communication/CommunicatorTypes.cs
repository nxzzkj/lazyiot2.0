 

using System;

namespace Scada.MDSCore.Communication
{
    /// <summary>
    /// Represents types of communicatiors.
    /// </summary>
    [Serializable]
    public enum CommunicatorTypes : byte 
    {
        /// <summary>
        /// An undefined remote application.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// A MDS server.
        /// </summary>
        MdsServer = 1,

        /// <summary>
        /// A client application.
        /// </summary>
        Application = 2,

        /// <summary>
        /// A controller application.
        /// </summary>
        Controller = 3
    }
}
