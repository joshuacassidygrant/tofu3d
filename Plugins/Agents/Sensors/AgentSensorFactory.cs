using System.Collections;
using System.Collections.Generic;
using Scripts.Sensors;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents.Sensors
{

    public class AgentSensorFactory: AbstractSensorFactory {

        public override AbstractSensor NewAgentSensor(Agent agent) {
            return new AgentSensor(ServiceContext, agent);
        }

    }

}
