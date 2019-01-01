using Scripts.Sensors;

namespace TofuPlugin.Agents
{
    public abstract class AgentController {

        protected IControllableAgent Agent;
        protected AbstractSensor Sensor;

        public AgentController(Agent agent, AbstractSensor sensor)
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


    }
}
