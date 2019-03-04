namespace TofuPlugin.Agents.Sensors
{

    public class AgentSensorFactory: AbstractSensorFactory {

        public AgentSensor NewAgentSensor(Agent agent) {
            return new AgentSensor(ServiceContext, agent);
        }

    }

}
