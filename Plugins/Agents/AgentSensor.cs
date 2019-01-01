using System.Collections.Generic;
using Scripts.Sensors;
using TofuCore.Service;

namespace TofuPlugin.Agents
{
    public class AgentSensor : AbstractSensor
    {

        protected Agent Agent;


        public AgentSensor(ServiceContext context, Agent agent) : base(context) {
            Agent = agent;
        }

        public override List<ISensable> GetAllSensables()
        {
            return new List<ISensable>();
        }
    }
}
