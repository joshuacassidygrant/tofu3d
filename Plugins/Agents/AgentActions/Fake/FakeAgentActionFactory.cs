using System.Collections;
using System.Collections.Generic;
using TofuCore.Configuration;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
using UnityEngine;

namespace TofuPlugin.Agents.AgentActions.Fake
{
    public class FakeAgentActionFactory : AbstractAgentActionFactory {


        public override void LoadAgentActions() {
            throw new System.NotImplementedException();
        }

        public override AgentAction BindAction(Agent agent, string actionId) {
            return new AgentSelfActionFake("1", "self");
        }

        public override AgentAction BindAction(Agent agent, string actionId, Configuration config) {
            return new AgentSelfActionFake("2", "act");
        }
    }

}
