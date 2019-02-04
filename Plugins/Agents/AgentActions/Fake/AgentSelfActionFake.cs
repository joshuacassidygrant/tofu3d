using System.Collections.Generic;
using TofuCore.Service;
using TofuPlugin.Agents.Targetable;

namespace TofuPlugin.Agents.AgentActions.Fake {

    public class AgentSelfActionFake : AgentAction
    {

        public AgentSelfActionFake(string id, string name) : base(id, name)
        {
        }

        public override Dictionary<string, float> GetUsageTagValues()
        {
            return new Dictionary<string, float>();
        }

        public override ActionTargetableValueTuple TargetingFunction()
        {
            return new ActionTargetableValueTuple(this, Agent.TargetableSelf, 1f);
        }
    }

    
}