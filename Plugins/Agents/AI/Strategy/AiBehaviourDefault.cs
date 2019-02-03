using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Targettable;

namespace TofuPlugin.Agents.AI.Behaviour
{
    /*
     * A stupid behaviour for testing.
     */
    public class AiBehaviourDefault : AIBehaviour {


        //Stub
        public override AgentCommand PickCommand() {

            AgentAction action = FindActionById("idle");

            if (action == null)
            {
                return new AgentCommand(new AgentActionIdle("idle", "Idle"), new TargettableDefault(), 0);
            }

            return new AgentCommand(action, new TargettableDefault(), 0);

        }



    }
}
