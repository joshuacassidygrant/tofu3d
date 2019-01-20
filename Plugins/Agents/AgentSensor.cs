using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentSensor : AbstractSensor
    {

        protected Agent Agent;


        public AgentSensor(ServiceContext context, Agent agent) : base(context) {
            Agent = agent;
            Manager = Context.Fetch("AgentManager");
        }

        public override List<ISensable> GetAllSensables()
        {
            return new List<ISensable>();
        }

        public List<Agent> GetAgentsInRange(float range) {
            AgentManager agentManager = (AgentManager)Manager;
            return agentManager.GetAllAgentsInRangeOfPoint(Agent.Position, range).ToList();
        }

        public List<Agent> GetAgentsInRangeWithFactionPermission(float range, string permission)
        {
            return GetAgentsInRange(range, (other => Agent.PermissionToDo(permission, other)));
        }

        public List<Agent> GetAgentsInRange(float range, Func<Agent, bool> predicate) {
            return GetAgentsInRange(range).Where(predicate).ToList();
        }

        public List<Agent> GetOtherAgentsInRange(float range) {
            return GetAgentsInRange(range, (other => other != Agent));
        }

        public List<Agent> GetOtherAgentsInRange(float range, Func<Agent, bool> predicate) {
            return GetAgentsInRange(range, (agent => agent != Agent && predicate(agent)));
        }

        public Agent GetHighestValueAgentInRange(float range, Func<Agent, bool> predicate, Func<Agent, double> fitness) {
            return GetOtherAgentsInRange(range, predicate).OrderByDescending(fitness).FirstOrDefault();
        }

        public Agent GetLowestValueAgentInRange(float range, Func<Agent, bool> predicate, Func<Agent, double> fitness) {
            return GetOtherAgentsInRange(range, predicate).OrderBy(fitness).FirstOrDefault();
        }

        public Agent GetClosestAgentInRange(float range, Func<Agent, bool> predicate = null) {
            if (predicate == null)
                return GetLowestValueAgentInRange(range, x => true,
                    x => Vector3.Distance(Agent.Position, x.Position));
            return GetLowestValueAgentInRange(range, predicate, x => Vector3.Distance(Agent.Position, x.Position));
        }

        public Agent GetFurthestAgentInRange(float range, Func<Agent, bool> predicate = null) {
            if (predicate == null)
                return GetHighestValueAgentInRange(range, x => true,
                    x => Vector3.Distance(Agent.Position, x.Position));
            return GetHighestValueAgentInRange(range, predicate, x => Vector3.Distance(Agent.Position, x.Position));
        }
    }
}
