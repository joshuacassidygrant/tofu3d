using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Glops;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentContainer : GlopContainer, ISensableContainer {

        public Agent Spawn(AgentPrototype prototype, Vector3 location)
        {
            Agent agent = new Agent(prototype, location);
            Register(agent);
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