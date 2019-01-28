using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Targettable;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public interface IControllableAgent
    {
        List<AgentAction> Actions { get; }
        void AddAction(AgentAction action);
        ITargettable TargettableSelf { get; }
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
        ITargettable CurrentActionTarget {
            get;
            set;
        }
    }
}
