﻿using TofuPlugin.Agents.AI.Behaviour;
using TofuPlugin.Agents.Behaviour;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Sensors;

namespace TofuPlugin.Agents.AI
{
    /*
     * This script runs the attached agent's coroutines. It should contain no permanent state.
     */
    public class AIAgentController : AgentController
    {
        private AIBehaviour _behaviour;
        protected AIBehaviourManager BehaviourManager;

        public AIAgentController(Agent agent, AgentSensor sensor, AIBehaviourManager behaviourManager) : base(agent, sensor)
        {
            ClearBehaviour();
            BehaviourManager = behaviourManager;
            Agent = agent;
            Sensor = sensor;
            SetBehaviour(behaviourManager.ChooseStrategy(agent));
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
            }
            
        }

        public AgentCommand NextCommand()
        {
            AgentCommand nextCommand = _behaviour.PickCommand();
            return nextCommand;
        }

        public AIBehaviour GetBehaviour()
        {
            return _behaviour;
        }

        public string GetBehaviourName()
        {
            return _behaviour.GetType().Name;
        }

        public void SetBehaviour(AIBehaviour behaviour)
        {
            _behaviour = behaviour;
            _behaviour.BindAgent(Agent);
            behaviour.SetSensor(Sensor);
            //Update();
        }
        
        public void Interrupt()
        {
            Agent.CurrentCommand = null;
            Agent.CurrentAction = null;
            Agent.CurrentActionTarget = null;
        }

        public void ClearBehaviour()
        {
            SetBehaviour(new AiBehaviourDefault());
        }



    }
}