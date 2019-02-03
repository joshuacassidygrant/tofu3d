using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Targettable;
using UnityEngine;

namespace TofuPlugin.Agents.AI.Behaviour
{
    /*
     * Class to determine AI targetting and longterm planning, action use etc.
     * Plug into AIAgentController.
     */
    public abstract class AIBehaviour
    {
        protected ServiceContext ServiceContext;
        protected AbstractSensor _sensor;
        protected IControllableAgent Agent;
        


        public void BindAgent(IControllableAgent agent)
        {
            Agent = agent;
        }

        public void SetSensor(AbstractSensor sensor)
        {
            _sensor = sensor;
        }

        public AgentAction FindActionById(string id)
        {
            List<AgentAction> actions = Agent.Actions;
            AgentAction action = Agent.Actions.Find(x => x.Id == id);
            if (action == null)
            {
                Debug.Log("No action found for id: " + id);
                return null;
            }
            return action;
        }

        public abstract AgentCommand PickCommand();
        //Behaviour should define a series of priorities for each tag
        //And then apply that to actions based on situations.
        //i.e. "AOE" actions will get more priority if several units
        //are grouped together... healing actions if there is someone directly injured
        //etc.s
        //Also should handle whether it privileges fire-and-forget tactics or focus fire
        //Should NOT find the BEST action every time, just a sensible one.
        //Could crank up "intelligence" to simulate more actions at the cost of processing speed.



        public abstract Dictionary<string, float> BehaviourCoefficients {
            get;
        }

        public float GetBehaviourCoefficient(string tag)
        {
            if (BehaviourCoefficients.ContainsKey(tag)) return BehaviourCoefficients[tag];
            return 1f;
        }

        public float GetUtilityValue(Dictionary<string, float> actionUsageTagValues, float targetValue)
        {
            //Multiply each atvu with the matching coefficient or 1
            foreach (KeyValuePair<string, float> entry in actionUsageTagValues)
            {
                actionUsageTagValues[entry.Key] = entry.Value * GetBehaviourCoefficient(entry.Key);
            }

            //then add them up and multiply by target value
            return actionUsageTagValues.Values.Sum() * targetValue;
        }

    }
}
