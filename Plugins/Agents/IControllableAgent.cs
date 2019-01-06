using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Commands;

namespace TofuPlugin.Agents
{
    public interface IControllableAgent
    {
        List<AgentAction> Actions { get; }
        void AddAction(AgentAction action);
        ITargettable TargettableSelf { get; }
        void ReceiveCommand(AgentCommand command);

    }
}
