using System.Collections.Generic;
using TofuCore.Service;
using TofuPlugin.Agents.Targetable;

namespace TofuPlugin.Agents.AgentActions.Test {

    public class AgentActionFake : AgentAction {

        public AgentActionFake(string id, string name) : base(id, name)
        {
        }

        public override Dictionary<string, float> GetUsageTagValues()
        {
            throw new System.NotImplementedException();
        }

        public override ActionTargetableValueTuple TargetingFunction()
        {
            throw new System.NotImplementedException();
        }
    }

    
}