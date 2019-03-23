using System.Collections.Generic;
using TofuCore.Tangible;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public interface IControllableAgent
    {
        List<AgentAction> Actions { get; }
        void AddAction(AgentAction action);
        ITangible TangibleSelf { get; }
        void ReceiveCommand(AgentCommand command);
        Vector3 Position {
            get;
        }
        AgentCommand CurrentCommand {
            get;
            set;
        }
        AgentAction CurrentAction {
            get;
            set;
        }
        ITangible CurrentActionTarget {
            get;
            set;
        }
    }
}
