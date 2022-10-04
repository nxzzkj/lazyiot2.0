 
using System;

namespace Scada.MDSCore.Client.MDSServices
{
    /// <summary>
    /// Any MDSService class must has this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MDSServiceAttribute : Attribute
    {
        /// <summary>
        /// A brief description of Service.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Service Version. This property can be used to indicate the code version (especially the version of service methods).
        /// This value is sent to user application on an exception, so, user/client application can know that service version is changed.
        /// Default value: NO_VERSION.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Creates a new MDSServiceAttribute object.
        /// </summary>
        public MDSServiceAttribute()
        {
            Version = "NO_VERSION";
        }
    }
}
