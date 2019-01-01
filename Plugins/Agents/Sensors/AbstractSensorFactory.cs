using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents;
using Scripts.Sensors;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents.Sensors
{
    public abstract class AbstractSensorFactory : AbstractService {

        public abstract AbstractSensor NewAgentSensor(Agent agent);
    }

}
