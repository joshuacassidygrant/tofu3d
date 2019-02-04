using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Targetable;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public interface IControllableAgent
    {
        List<AgentAction> Actions { get; }
        void AddAction(AgentAction action);
        ITargetable TargetableSelf { get; }
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
        ITargetable CurrentActionTarget {
            get;
            set;
        }
    }
}
