using System.Collections.Generic;

namespace TofuPlugin.Agents.AgentActions
{
    public class AgentActionIdle : AgentAction
    {
        public AgentActionIdle(string id, string name) : base(id, name)
        {
        }

        public override Dictionary<string, float> GetUsageTagValues()
        {
            return new Dictionary<string, float>();
        }

        public override ActionTargetableValueTuple TargetingFunction()
        {
            return new ActionTargetableValueTuple(this, Agent, 0);
        }
    }
}

