
using TofuCore.Service;
using TofuPlugin.Agents.Targettable;

namespace TofuPlugin.Agents.AgentActions
{
    public class AgentActionIdle : AgentAction
    {
        public AgentActionIdle(string id, string name) : base(id, name)
        {
        }

        public override ITargettable TargettingFunction()
        {
            throw new System.NotImplementedException();
        }
    }
}

