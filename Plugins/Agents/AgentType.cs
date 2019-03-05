using System.Collections.Generic;

namespace TofuPlugin.Agents
{
    /**
     * AgentType can be used to define a default name, properties expected in a config object, and a list of default actions to load.
     */
    public class AgentType
    {
        public string Name;
        public HashSet<string> ExpectedProperties;
        public List<string> DefaultActions;
        public List<AgentResourceModuleConfig> ResourceModuleConfigs;


        public AgentType(string name, HashSet<string> expectedProperties, List<string> defaultActions, List<AgentResourceModuleConfig> resourceModuleConfigs)
        {
            Name = name;
            ExpectedProperties = expectedProperties;
            DefaultActions = defaultActions;
            ResourceModuleConfigs = resourceModuleConfigs;

        }

    }
}

