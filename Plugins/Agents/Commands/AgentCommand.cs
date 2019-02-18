﻿using TofuCore.Command;
using TofuCore.Targetable;
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
                //TODO: check all action preconditions here -- in range, enough power etc
                if (!Action.InRange(Target))
                {
                    //TODO: Get the best mobility action and path towards target (later -- this would need a new pathing system if it had to allow for blinks etc)
                    //TODO: allow for unit size
                    //Action.Agent.GetMobilityActions();
                    //TEMP:
                    Action.Agent.GetMoveAction().TriggerAction(Target);
                    return false;
                }

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
