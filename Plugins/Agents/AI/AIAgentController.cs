using Scripts.Sensors;
using TofuPlugin.Agents.AI.Behaviour;
using TofuPlugin.Agents.Commands;
using UnityEngine;

namespace TofuPlugin.Agents.AI
{
    /*
     * This script runs the attached agent's coroutines. It should contain no permanent state.
     */
    public class AIAgentController : AgentController
    {
        private AIBehaviour _behaviour;

        public AIAgentController(Agent agent, AbstractSensor sensor): base(agent, sensor)
        {
            ClearStrategy();
        }

        public override void Update()
        {
            base.Update();
            if (_behaviour == null) return;

            if (Agent.CurrentCommand != null && !Agent.CurrentCommand.Executable())
            {
                Interrupt();
            }

            AgentCommand newCommand = NextCommand();

            if (Agent.CurrentCommand == null || 
               (newCommand != null && newCommand.Priority > Agent.CurrentCommand.Priority))
            {
                Agent.CurrentCommand = NextCommand();
                Agent.CurrentAction = null;
            }

            if (Agent.CurrentAction == null)
            {
                Agent.CurrentAction = Agent.CurrentCommand.Action;
                Agent.CurrentActionTarget = Agent.CurrentCommand.Target;
                //Agent.CurrentAction.TriggerAction(Agent.CurrentActionTarget);
            }
            
        }

        public AgentCommand NextCommand()
        {
            AgentCommand nextCommand = _behaviour.PickCommand();
            return nextCommand;
        }

        public string GetStrategyName()
        {
            return _behaviour.GetType().Name;
        }

        public void SetStrategy(AIBehaviour behaviour)
        {
            _behaviour = behaviour;
            _behaviour.BindAgent(Agent);
            behaviour.SetSensor(Sensor);
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
            SetStrategy(new AiBehaviourDefault());
        }



    }
}
