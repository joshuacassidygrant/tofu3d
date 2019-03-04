using System.Collections;
using System.Collections.Generic;
using TofuCore.Configuration;
using TofuCore.Service;
using TofuPlugin.Agents;
using TofuPlugin.Agents.Sensors;
using UnityEngine;

/**
 * Responsible for full configuration of new Agent objects
 */
namespace  TofuPlugin.Agents
{
    public class AgentFactory : AbstractService
    {
        [Dependency] protected AgentSensorFactory AgentSensorFactory;
        [Dependency] protected AgentTypeLibrary AgentTypeLibrary;


        /**
         * Ensures an agent has all needed configurations and is bound to its container
         */
        public Agent BuildAndRegisterNewAgent(AgentContainer container, Vector3 location, AgentPrototype prototype, Configuration config)
        {
            Agent agent = new Agent();
            container.Register(agent);
            agent.ConsumePrototype(AgentTypeLibrary.Get(prototype.AgentType), prototype);
            agent.ConsumeConfig(config);
            agent.Initialize();

            AgentSensor sensor = AgentSensorFactory.NewAgentSensor(agent);
            agent.SetSensor(sensor);

            agent.Position = location;
            return agent;
        }

    }
}
