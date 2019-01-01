using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;

namespace TofuPlugin.Agents.AI.Strategy
{
    /*
     * A stupid strategy for testing.
     */
    public class AIStrategyDefault : AIStrategy {


        //Stub
        public override AgentAction PickAction(List<AgentAction> actions) {
            //Will need to get world state here too?
            //A request to UnitManager?

            if (actions.Count > 0) return actions[0];
            return null;
        }



    }
}
