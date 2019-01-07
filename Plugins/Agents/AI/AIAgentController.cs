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
                Debug.Log(Agent.CurrentCommand);
                Debug.Log("Command picked" + Agent.CurrentCommand.ToString());
            }

            if (Agent.CurrentAction == null)
            {
                Agent.CurrentAction = Agent.CurrentCommand.Action;
                Agent.CurrentActionTarget = Agent.CurrentCommand.Target;
            }
        }

        public AgentCommand NextCommand()
        {
            AgentCommand nextCommand = _strategy.PickCommand(Agent.Actions);
            Debug.Log(_strategy);
            Debug.Log(nextCommand);
            return nextCommand;
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
