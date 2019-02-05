using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Targetable;

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
                return new AgentCommand(action, new TargetableDefault(), 0);
            }

            return new AgentCommand(action, new TargetableDefault(), 0);

        }

        public override Dictionary<string, float> BehaviourCoefficients { get; }
    }
}
