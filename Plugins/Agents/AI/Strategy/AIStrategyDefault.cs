using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;

namespace TofuPlugin.Agents.AI.Strategy
{
    /*
     * A stupid strategy for testing.
     */
    public class AIStrategyDefault : AIStrategy {


        //Stub
        public override AgentCommand PickCommand(List<AgentAction> actions) {

            return new AgentCommand(new AgentActionIdle("idle", "Idle"), new TargettableDefault());
        }



    }
}
