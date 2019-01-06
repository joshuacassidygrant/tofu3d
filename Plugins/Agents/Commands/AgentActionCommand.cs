using TofuCore.Command;
using TofuPlugin.Agents.AgentActions;

namespace TofuPlugin.Agents.Commands
{
    /*
     *  An AgentCommand is a goal set by a Controller (either AI or human),
     *  responsible for finding and triggering AgentActions able to fulfill 
     *  that goal.
     *  
     *  AgentCommands can interrupt themselves or be interrupted by a controller 
     *  when a situation arises.
     */
    public class AgentCommand : Command
    {

        public AgentAction Action;
        public ITargettable Target;

        public AgentCommand(AgentAction action, ITargettable target)
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
