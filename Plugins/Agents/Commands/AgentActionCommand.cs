﻿using TofuCore.Command;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Targetable;

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
        public ITargetable Target;
        public int Priority;

        public AgentCommand(AgentAction action, ITargetable target, int priority)
        {
            Action = action;
            Target = target;
            Priority = priority;
        }

        public virtual bool Executable()
        {
            return Target != null && Target.Active;
        }

        public override bool TryExecute()
        {
            if (Action.Ready())
            {
                Action.TriggerAction(Target);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Action.Name + " targetting " + Target.ToString();
        }
    }
}
