using System.Collections.Generic;
using TofuCore.Service;
using TofuPlugin.Agents.Targettable;

namespace TofuPlugin.Agents.AgentActions.Fake {

    public class AgentActionFake : AgentAction {

        public AgentActionFake(string id, string name) : base(id, name)
        {
        }

        public override Dictionary<string, float> GetUsageTagValues()
        {
            throw new System.NotImplementedException();
        }

        public override ActionTargettableValueTuple TargettingFunction()
        {
            throw new System.NotImplementedException();
        }
    }

    
}