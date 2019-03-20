using System;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Sensors;
using UnityEngine;

namespace TofuPlugin.Agents.Behaviour
{
    /*
     * Class to determine AI targetting and longterm planning, action use etc.
     * Plug into AIAgentController.
     */
    public abstract class AIBehaviour
    {
        protected ServiceContext ServiceContext;
        protected AgentSensor Sensor;
        protected IControllableAgent Agent;
        
        public void BindAgent(IControllableAgent agent)
        {
            Agent = agent;
        }

        public void SetSensor(AgentSensor sensor)
        {
            Sensor = sensor;
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

        //Behaviour should define a series of priorities for each tag
        //And then apply that to actions based on situations.
        //i.e. "AOE" actions will get more priority if several units
        //are grouped together... healing actions if there is someone directly injured
        //etc.s
        //Also should handle whether it privileges fire-and-forget tactics or focus fire
        //Should NOT find the BEST action every time, just a sensible one.
        //Could crank up "intelligence" to simulate more actions at the cost of processing speed.
        public virtual AgentCommand PickCommand()
        {
            //TODO: Also should handle whether it privileges fire-and-forget tactics or focus fire
            List<ActionTargetableValueTuple> atVs = new List<ActionTargetableValueTuple>();
            List<AgentAction> actions = Agent.Actions.Where(x => x.Ready()).ToList();
            foreach (AgentAction action in actions)
            {
                try
                {
                    atVs.Add(action.TargetingFunction());
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            atVs.Sort((x, y) => y.Value.CompareTo(x.Value));
            if (atVs.Count <= 0)
            {
                return new AgentCommand(null, null, Mathf.RoundToInt(0));
            }
            ActionTargetableValueTuple pick = atVs[0];

            return new AgentCommand(pick.Action, pick.Targetable, Mathf.RoundToInt(pick.Value));

        }


        public abstract Dictionary<string, float> BehaviourCoefficients {
            get;
        }

        public float GetBehaviourCoefficient(string tag)
        {
            if (BehaviourCoefficients.ContainsKey(tag)) return BehaviourCoefficients[tag];
            return AgentConstants.BehaviourCoefficientMinimum;
        }

        public float GetUtilityValue(Dictionary<string, float> actionUsageTagValues, float targetValue)
        {
            Dictionary<string, float> atvLive = new Dictionary<string, float>();
            //Multiply each atvu with the matching coefficient or 1
            foreach (KeyValuePair<string, float> entry in actionUsageTagValues)
            {
               atvLive.Add(entry.Key, entry.Value * GetBehaviourCoefficient(entry.Key));
            }

            //then add them up and multiply by target value
            return atvLive.Values.Sum() * targetValue;
        }

    }
}
