using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.ResourceLibrary;
using TofuCore.ResourceModule;
using TofuPlugin.Agents;

namespace TofuPlugin.Agents
{
    public class AgentTypeLibrary : AbstractResourceLibrary<AgentType>
    {
        public AgentTypeLibrary(Dictionary<string, AgentType> contents)
        {
            _contents = contents;
        }

        
    }

}
