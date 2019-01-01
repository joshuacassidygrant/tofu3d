using Scripts.Sensors;
using TofuPlugin.Agents.AI.Strategy;
using UnityEngine;

namespace TofuPlugin.Agents.AI
{
    /*
     * This script runs the attached agent's coroutines. It should contain no permanent state.
     */
    public class AIAgentController : AgentController
    {
        private AIStrategy _strategy;

        public AIAgentController(Agent agent, AbstractSensor sensor): base(agent, sensor)
        {
            ClearStrategy();
        }

        public void NextCommand()
        {
            
        }

        public string GetStrategyName()
        {
            return _strategy.GetType().Name;
        }

        public void SetStrategy(AIStrategy strategy)
        {
            _strategy = strategy;
            strategy.SetSensor(Sensor);
        }

        public void ClearStrategy()
        {
            _strategy = new AIStrategyDefault();
        }



    }
}
