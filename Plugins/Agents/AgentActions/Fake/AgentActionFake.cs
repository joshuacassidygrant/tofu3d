using TofuCore.Service;

namespace TofuPlugin.Agents.AgentActions.Fake {

    public class AgentActionFake : AgentAction {

        public AgentActionFake(string id, string name) : base(id, name)
        {
        }

        public override ITargettable TargettingFunction()
        {
            throw new System.NotImplementedException();
        }
    }

    
}