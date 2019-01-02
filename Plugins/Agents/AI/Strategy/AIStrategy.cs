﻿using System.Collections.Generic;
using Scripts.Sensors;
using TofuPlugin.Agents.AgentActions;

namespace TofuPlugin.Agents.AI.Strategy
{
    /*
     * Class to determine AI targetting and longterm planning, action use etc.
     * Plug into AIAgentController.
     */
    public abstract class AIStrategy
    {
        private AbstractSensor _sensor;


        public void SetSensor(AbstractSensor sensor)
        {
            _sensor = sensor;
        }

        public abstract AgentAction PickAction(List<AgentAction> actions);

    }
}