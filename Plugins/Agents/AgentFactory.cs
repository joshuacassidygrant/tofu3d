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
        
        

    }
}
