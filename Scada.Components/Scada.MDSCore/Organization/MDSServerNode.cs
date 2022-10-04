 

using System.Collections.Generic;

namespace Scada.MDSCore.Organization
{
    /// <summary>
    /// Represents a MDS server on network.
    /// </summary>
    public class MDSServerNode
    {
        /// <summary>
        /// Name of the remote application
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Adjacent server nodes of this node.
        /// </summary>
        public SortedList<string, MDSServerNode> Adjacents { get; set; }

        /// <summary>
        /// Stores best paths to the all server nodes from this node
        /// </summary>
        public SortedList<string, List<MDSServerNode>> BestPathsToServers { get; set; }

        /// <summary>
        /// Constructur.
        /// </summary>
        /// <param name="name">name of server</param>
        public MDSServerNode(string name)
        {
            Name = name;
        }
    }
}
