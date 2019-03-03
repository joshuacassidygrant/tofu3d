using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Configuration;
using TofuCore.ContentInjectable;
using TofuCore.Glops;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions.Fake;
using TofuPlugin.Agents.Factions;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentContainer : GlopContainer, ISensableContainer {

        [Dependency][ContentInjectable] protected FactionContainer FactionContainer;
        [Dependency][ContentInjectable] protected FakeAgentActionFactory ActionFactory;
        [Dependency] protected AgentFactory AgentFactory;


        public Agent Spawn(AgentPrototype prototype, Vector3 location)
        {
            Configuration config = new Configuration();
            Agent agent = AgentFactory.BuildAndRegisterNewAgent(this, location, prototype, config);
            return agent;
        }

        public List<Agent> GetAllAgentsInRangeOfPoint(Vector3 point, float range)
        {
            return GetAgents().Where(x => (point - x.Position).sqrMagnitude <= range * range).ToList();

        }

        public List<Agent> GetAgents()
        {
            return GetContents().Cast<Agent>().ToList();
        }

        public List<ISensable> GetAllSensables() {
            return GetContents().Cast<ISensable>().ToList();
        }

        public List<ISensable> GetAllSensablesWithinRangeOfPoint(Vector3 point, float range)
        {
            return GetAllSensables().Where(x => (point - x.Position).magnitude <= range).ToList();
        }
    }
}