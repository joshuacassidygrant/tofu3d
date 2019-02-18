using System.Collections.Generic;

namespace TofuPlugin.Agents.AgentActions.Test {

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