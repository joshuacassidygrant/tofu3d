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
        public override AgentCommand PickCommand(List<AgentAction> actions)
        {

            if (actions.Count > 0) return null;
            return null;
        }



    }
}
