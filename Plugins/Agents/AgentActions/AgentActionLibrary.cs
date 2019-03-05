using System;
using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuCore.ResourceLibrary;

namespace TofuPlugin.Agents
{
    public class AgentActionLibrary : AbstractResourceLibrary<Func<AgentAction>>
    {
        public AgentActionLibrary(Dictionary<string, Func<AgentAction>> agentActions)
        {
            _contents = agentActions;
        }

        public override void LoadResources()
        {
            //Resources are loaded in constructor
        }
    }
}
