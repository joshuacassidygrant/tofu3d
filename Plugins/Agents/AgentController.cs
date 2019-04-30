using TofuCore.Sensors;

namespace TofuPlugin.Agents
{
    public abstract class AgentController {

        protected IControllableAgent Agent;
        protected AgentSensor Sensor;

        public AgentController(Agent agent, AgentSensor sensor)
        {
            Agent = agent;
            Sensor = sensor;
        }

        public AbstractSensor GetSensor()
        {
            return Sensor;
        }
        

        public void GiveCommand()
        {

        }

        public virtual void Update()
        {

        }


    }
}
