using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.ResourceLibrary;
using TofuCore.ResourceModule;
using TofuPlugin.Agents;

namespace TofuPlugin.Agents
{
    public class AgentTypeLibrary : AbstractResourceLibrary<AgentType>
    {
        public AgentTypeLibrary(string path, string prefix = "") : base(path, prefix)
        {
            //TODO: parametrize this out to configure from outside tofu
        }

        public override void LoadResources()
        {
            //TODO: take this out of here
            _contents.Add("Creature", new AgentType("Creature", new HashSet<string>
            {
                "Speed"
            }, new List<string>
            {
                "idle",
                "move",
                "attack",
                "ranged",
                "heal",
                "moveToObjective"
            }, new List<AgentResourceModuleConfig>
            {
                new AgentResourceModuleConfig("HP", "HpMax", "AgentDies", "Agent", "Damaged", "Healed")
            }));

            _contents.Add("Structure", new AgentType("Structure", new HashSet<string>
            {
            }, null, null));


        }

        public struct AgentResourceModuleConfig
        {
            public string Key;
            public string ConfigPropertyMax;
            public string FullDepletionEventName;
            public string PayloadContentTypeName;
            public string DepletionEventName;
            public string ReplenishEventName;

            public AgentResourceModuleConfig(string key, string configPropertyMax, string fullDepletionEvent, string payloadContentType, string depletionEventName, string replenishEventName)
            {
                Key = key;
                ConfigPropertyMax = configPropertyMax;
                FullDepletionEventName = fullDepletionEvent;
                PayloadContentTypeName = payloadContentType;
                DepletionEventName = depletionEventName;
                ReplenishEventName = replenishEventName;
            }

            public ResourceModule GenerateResourceModule(Agent agent, EventContext eventContext)
            {
                ResourceModule resourceModule = new ResourceModule("HP", agent.Properties.GetProperty("HpMax", 0), agent.Properties.GetProperty("HpMax", 0), agent, eventContext);
                resourceModule.BindFullDepletionEvent(FullDepletionEventName, new EventPayload(PayloadContentTypeName, agent, eventContext));
                resourceModule.SetDepletionEventKey(DepletionEventName);
                resourceModule.SetReplenishEventKey(ReplenishEventName);
                return resourceModule;
            }

        }
    }

}
