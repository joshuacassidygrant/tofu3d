using TofuCore.Command;
using TofuPlugin.Agents.AgentActions;

namespace TofuPlugin.Agents.Commands
{
    public class AgentActionCommand : Command
    {

        public AgentAction Action;
        public ITargettable Target;

        public AgentActionCommand(AgentAction action, ITargettable target)
        {
            Action = action;
            Target = target;
        }

        public override bool TryExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
