 

using System;

namespace Scada.MDSCore.Client.MDSServices
{
    /// <summary>
    /// Any MDSService class must use this attribute on it's remote methods.
    /// If a method has not MDSServiceMethod attribute, it can not be invoked by remote applications.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MDSServiceMethodAttribute : Attribute
    {
        /// <summary>
        /// A brief description (and may be usage) of method.
        /// </summary>
        public string Description { get; set; }
    }
}
