using System;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Configuration;
using TofuCore.Service;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
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
        [Dependency] protected AgentActionFactory AgentActionFactory;
        


        /**
         * Ensures an agent has all needed configurations and is bound to its container
         */
        public Agent BuildAndRegisterNewAgent(AgentContainer container, Vector3 location, AgentPrototype prototype, Configuration config)
        {
            Agent agent = new Agent();
            container.Register(agent);
            List<AgentAction> boundActions = LoadActionsAndBind(agent, prototype);
            agent.ConsumePrototype(AgentTypeLibrary.Get(prototype.AgentType), prototype, boundActions);
            agent.ConsumeConfig(config);
            agent.Initialize();

            AgentSensor sensor = AgentSensorFactory.NewAgentSensor(agent);
            agent.SetSensor(sensor);

            agent.Position = location;
            return agent;
        }


        private List<AgentAction> LoadActionsAndBind(Agent agent, AgentPrototype prototype)
        {
            List<AgentAction> boundActions = new List<AgentAction>();

            AgentType agentType = AgentTypeLibrary.Get(prototype.AgentType);
            foreach (string actionId in agentType.DefaultActions)
            {
                boundActions.Add(AgentActionFactory.BindAction(agent, actionId));
            }

            foreach (PrototypeActionEntry actionEntry in prototype.Actions)
            {
                AgentAction action = AgentActionFactory.BindAction(agent, actionEntry.ActionId);
                boundActions.Add(action);
                action.Configure(actionEntry.Configuration);
            }

            return boundActions;
        }


    }
}
