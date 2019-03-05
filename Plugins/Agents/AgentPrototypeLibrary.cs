using System.Collections.Generic;
using System.Linq;
using TofuCore.ResourceLibrary;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentPrototypeLibrary : AbstractResourceLibrary<AgentPrototype>
    {
        [Dependency] protected AgentTypeLibrary AgentTypeLibrary;
        private string _agentTypeKey;

        public AgentPrototypeLibrary(string key, List<AgentPrototype> agentPrototypes)
        {
            _agentTypeKey = key;
            _contents = agentPrototypes.ToDictionary(a => a.Id, a => a);
        }

    }
}
