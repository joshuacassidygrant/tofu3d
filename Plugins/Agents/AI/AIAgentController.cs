using Scripts.Sensors;
using TofuPlugin.Agents.AI.Strategy;
using TofuPlugin.Agents.Commands;
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

        public override void Update()
        {
            base.Update();

            if (_strategy == null) return;
            
            if(Agent.CurrentCommand == null)
            {
                Agent.CurrentCommand = NextCommand();
            }

            if (Agent.CurrentAction == null)
            {
                Agent.CurrentAction = Agent.CurrentCommand.Action;
                Agent.CurrentActionTarget = Agent.CurrentCommand.Target;
                Agent.CurrentAction.TriggerAction(Agent.CurrentActionTarget);
            }
        }

        public AgentCommand NextCommand()
        {
            AgentCommand nextCommand = _strategy.PickCommand();
            return nextCommand;
        }

        public string GetStrategyName()
        {
            return _strategy.GetType().Name;
        }

        public void SetStrategy(AIStrategy strategy)
        {
            _strategy = strategy;
            _strategy.BindAgent(Agent);
            strategy.SetSensor(Sensor);
            Update();
        }
        
        public void Interrupt()
        {
            Agent.CurrentCommand = null;
            Agent.CurrentAction = null;
            Agent.CurrentActionTarget = null;
        }

        public void ClearStrategy()
        {
            SetStrategy(new AIStrategyDefault());
        }



    }
}
