using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AI.Strategy;
using TofuPlugin.Agents.Commands;

namespace Scripts.Agents.Strategy {

    /*
     * A stupid strategy for testing.
     */
    public class AIStrategyFake : AIStrategy {


        //Stub
        public override AgentCommand PickCommand()
        {

            return null;
        }



    }
}
