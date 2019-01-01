using System;
using System.Collections.Generic;
using Scripts;
using TofuCore.Configuration;
using UnityEngine;

namespace TofuPlugin.Agents.AgentActions.Fake
{
    public class AgentActionFactoryFake : AbstractAgentActionFactory {

        protected Dictionary<string, Func<AgentAction>> Actions = new Dictionary<string, Func<AgentAction>>();

        

        public override AgentAction BindAction(Agent agent, string actionId) {
            if (!Actions.ContainsKey(actionId)) {
                Debug.Log("UnitActionFactory contains no definition for " + actionId);
                return null;
            }

            AgentAction action = Actions[actionId].Invoke();
            action.Agent = agent;
            return action;

        }

        public override AgentAction BindAction(Agent agent, string actionId,
            Configuration config) {
            AgentAction action = BindAction(agent, actionId);

            if (action == null) return null;

            action.Configure(config);

            return action;
        }

        public override void LoadAgentActions()
        {
            Actions = new Dictionary<string, Func<AgentAction>>();
        }

        public void AddAction(string actionId, Func<AgentAction> action)
        {
            Actions.Add(actionId, action);
        }

    }
}
