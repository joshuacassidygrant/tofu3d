using System;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Sensors;
using TofuCore.Service;
using TofuCore.Tangible;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentSensor : AbstractSensor
    {

        public IAgent Agent;
        public float SenseRange { get; set; }

        //TEMPORRARY
        private IAgentContainer AgentContainer;


        public AgentSensor(IServiceContext context, IAgent agent) : base(context) {
            Agent = agent;
            AgentContainer = Context.Fetch("IAgentContainer");
        }

        public override List<ITangible> GetAllTargetables()
        {
            return new List<ITangible>();
        }

        public List<Agent> GetAgentsInRange(float range) {
            //TODO: this!
            return AgentContainer.GetAllAgentsInRangeOfPoint(Agent.Position, range).ToList();
        }

        public List<Agent> GetAgentsInRangeWithFactionPermission(float range, string permission)
        {
            return null;
            //return GetAgentsInRange(range, (other => Agent.FactionComponent.PermissionToDo(permission, other)));
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
