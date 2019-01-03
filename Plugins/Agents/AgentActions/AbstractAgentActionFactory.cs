using System;
using TofuCore.Service;
using Scripts;
using TofuCore.Configuration;

namespace TofuPlugin.Agents.AgentActions
{
    public abstract class AbstractAgentActionFactory : AbstractService
    {
        public abstract void LoadAgentActions();

        public abstract AgentAction BindAction(Agent agent, string actionId);

        public abstract AgentAction BindAction(Agent agent, string actionId,
            Configuration config);

        public override void Build()
        {
            base.Build();
            LoadAgentActions();
        }

        public abstract void AddAction(string key, Func<AgentAction> actionCreator);
    }
}
