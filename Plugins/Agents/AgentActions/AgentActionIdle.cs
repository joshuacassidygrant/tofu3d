
using System.Collections.Generic;
using TofuCore.Service;
using TofuPlugin.Agents.Targettable;

namespace TofuPlugin.Agents.AgentActions
{
    public class AgentActionIdle : AgentAction
    {
        public AgentActionIdle(string id, string name) : base(id, name)
        {
        }

        public override Dictionary<string, float> GetUsageTagValues()
        {
            throw new System.NotImplementedException();
        }

        public override ITargettable TargettingFunction()
        {
            throw new System.NotImplementedException();
        }
    }
}

