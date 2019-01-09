using System;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Configuration;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
using UnityEngine;

namespace TofuPlugin.Agents.AgentActions.Fake
{
    public class AgentActionFactory : AbstractAgentActionFactory
    {

        private Dictionary<string, Func<AgentAction>> Actions;

        public override void LoadAgentActions() {
            Actions = new Dictionary<string, Func<AgentAction>>();

        }

        public override AgentAction BindAction(Agent agent, string actionId) {
            if (!Actions.ContainsKey(actionId))
            {
                Debug.Log("Can't find action with key " + actionId);
                return null;
            }

            AgentAction action = Actions[actionId].Invoke();
            action.Agent = agent;
            return action;
        }

        public override AgentAction BindAction(Agent agent, string actionId, Configuration config)
        {
            AgentAction action = BindAction(agent, actionId);
            action.Configure(config);
            return action;

        }

        public override void AddAction(string key, Func<AgentAction> actionCreator) {
            if (Actions.ContainsKey(key)) {
                Debug.Log("Already have an action with key " + key);
                return;
            }

            Actions.Add(key, actionCreator);
        }

    }

}
