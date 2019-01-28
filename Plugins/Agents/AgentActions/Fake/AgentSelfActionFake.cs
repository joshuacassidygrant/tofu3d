using TofuCore.Service;
using TofuPlugin.Agents.Targettable;

namespace TofuPlugin.Agents.AgentActions.Fake {

    public class AgentSelfActionFake : AgentAction
    {

        public AgentSelfActionFake(string id, string name) : base(id, name)
        {
        }

        public override ITargettable TargettingFunction()
        {
            return Agent.TargettableSelf;
        }
    }

    
}