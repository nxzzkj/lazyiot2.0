 

using System;

namespace Scada.MDSCore.Client.MDSServices
{
    /// <summary>
    /// This attribute is used to add information to a parameter or return value of a MDSServiceMethod.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class MDSServiceMethodParameterAttribute : Attribute
    {
        /// <summary>
        /// A brief description of parameter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Creates a new MDSServiceMethodParameterAttribute.
        /// </summary>
        /// <param name="description"></param>
        public MDSServiceMethodParameterAttribute(string description)
        {
            Description = description;
        }
    }
}
