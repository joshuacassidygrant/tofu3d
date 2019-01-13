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
        public override AgentCommand PickCommand() {

            return new AgentCommand(FindActionById("idle"), new TargettableDefault(), 0);
        }



    }
}
