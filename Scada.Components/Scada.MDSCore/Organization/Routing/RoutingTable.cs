 

using System.Collections.Generic;
using System.Linq;
using Scada.MDSCore.Communication.Messages;
using Scada.MDSCore.Settings;

namespace Scada.MDSCore.Organization.Routing
{
    /// <summary>
    /// Represents routing table that contains all routing rules.
    /// </summary>
    public class RoutingTable
    {
        /// <summary>
        /// All routing rules.
        /// </summary>
        public List<RoutingRule> Rules { get; private set; }

        /// <summary>
        /// Settings.
        /// </summary>
        private readonly MDSSettings _settings;

        /// <summary>
        /// Creates a new RoutingTable object.
        /// </summary>
        public RoutingTable()
        {
            _settings = MDSSettings.Instance;
            Rules = new List<RoutingRule>();
            CreateRuleList();
        }

        /// <summary>
        /// Checks all routing rules and apply proper rule to the message
        /// </summary>
        /// <param name="message">Message to apply routing</param>
        public void ApplyRouting(MDSDataTransferMessage message)
        {
            if (Rules.Any(rule => rule.ApplyRule(message)))
            {
                return;
            }
        }

        /// <summary>
        /// Creates Rules list from settings.
        /// </summary>
        private void CreateRuleList()
        {
            foreach (var routeInfoItem in _settings.Routes)
            {
                var rule = new RoutingRule
                           {
                               Name = routeInfoItem.Name,
                               Filters = new RoutingFilter[routeInfoItem.Filters.Count],
                               Destinations = new RoutingDestination[routeInfoItem.Destinations.Count],
                               DistributionType = routeInfoItem.DistributionType
                           };

                for (var i = 0; i < rule.Filters.Length; i++)
                {
                    rule.Filters[i] = new RoutingFilter
                                      {
                                          SourceServer = routeInfoItem.Filters[i].SourceServer,
                                          SourceApplication = routeInfoItem.Filters[i].SourceApplication,
                                          DestinationServer = routeInfoItem.Filters[i].DestinationServer,
                                          DestinationApplication = routeInfoItem.Filters[i].DestinationApplication,
                                          TransmitRule = routeInfoItem.Filters[i].TransmitRule
                                      };
                    
                    if(rule.Filters[i].DestinationServer == "this")
                    {
                        rule.Filters[i].DestinationServer = MDSSettings.Instance.ThisServerName;
                    }

                    if (rule.Filters[i].DestinationServer == "this")
                    {
                        rule.Filters[i].DestinationServer = MDSSettings.Instance.ThisServerName;
                    }
                }

                for (var i = 0; i < rule.Destinations.Length; i++)
                {
                    rule.Destinations[i] = new RoutingDestination
                                           {
                                               Server = routeInfoItem.Destinations[i].Server,
                                               Application = routeInfoItem.Destinations[i].Application,
                                               RouteFactor = routeInfoItem.Destinations[i].RouteFactor
                                           };
                }

                Rules.Add(rule);
            }
        }
    }
}
