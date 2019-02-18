using System.Collections.Generic;
using TofuCore.Targetable;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;

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
