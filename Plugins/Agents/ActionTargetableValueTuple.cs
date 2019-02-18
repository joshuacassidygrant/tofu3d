using TofuCore.Targetable;
using TofuPlugin.Agents.AgentActions;

namespace TofuPlugin.Agents
{
    public struct ActionTargetableValueTuple
    {
        public AgentAction Action;
        public ITargetable Targetable;
        public float Value;

        public ActionTargetableValueTuple(AgentAction action, ITargetable targetable, float value)
        {
            Action = action;
            Targetable = targetable;
            Value = value;
        }


        public bool IsNull()
        {
            return Equals(NULL);
        }

        public static ActionTargetableValueTuple NULL = new ActionTargetableValueTuple(null, null, 0f);
    }
}

