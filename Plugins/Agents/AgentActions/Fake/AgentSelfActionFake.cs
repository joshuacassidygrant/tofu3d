using System.Collections.Generic;
using TofuCore.Tangible;

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
            return new ActionTargetableValueTuple(this, Agent.TangibleSelf, 1f);
        }

        protected override IEnumerable<ITangible> GetTargets()
        {
            throw new System.NotImplementedException();
        }

        protected override float ValueFunction(ITangible t)
        {
            throw new System.NotImplementedException();
        }
    }

    
}