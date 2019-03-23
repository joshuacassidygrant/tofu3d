using System.Collections.Generic;
using TofuCore.Tangible;

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