using System;
using System.Collections.Generic;
using TofuCore.Service;
using TofuCore.Configuration;
using UnityEngine;

namespace TofuPlugin.Agents.AgentActions
{
    public class AgentActionFactory : AbstractService
    {
        [Dependency] protected AgentActionLibrary AgentActionLibrary;


        protected Dictionary<string, Func<AgentAction>> Actions = new Dictionary<string, Func<AgentAction>>();

        public void LoadAgentActions()
        {
            Actions = AgentActionLibrary.GetCatalogue();
        }

        public AgentAction BindAction(Agent agent, string actionId)
        {
            if (!Actions.ContainsKey(actionId))
            {
                Debug.Log("AgentActionFactory contains no definition for " + actionId);
                return null;
            }

            AgentAction action = Actions[actionId].Invoke();
            action.Agent = agent;
            action.InjectServiceContext(ServiceContext);
            action.BindDependencies();

            return action;
        }

        public AgentAction BindAction(Agent agent, string actionId,
            Configuration config)
        {
            AgentAction action = BindAction(agent, actionId);

            if (action == null) return null;

            action.Configure(config);

            return action;
        }

        public void AddAction(string key, Func<AgentAction> actionCreator)
        {
            if (Actions.ContainsKey(key))
            {
                Debug.Log("Already have an action with key " + key);
                return;
            }

            Actions.Add(key, actionCreator);
        }


        public override void Initialize()
        {
            base.Initialize();
            LoadAgentActions();

        }

    }
}
