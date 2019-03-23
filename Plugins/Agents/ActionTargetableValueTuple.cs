using TofuCore.Tangible;
using TofuPlugin.Agents.AgentActions;

namespace TofuPlugin.Agents
{
    public struct ActionTargetableValueTuple
    {
        public AgentAction Action;
        public ITangible Tangible;
        public float Value;

        public ActionTargetableValueTuple(AgentAction action, ITangible tangible, float value)
        {
            Action = action;
            Tangible = tangible;
            Value = value;
        }


        public bool IsNull()
        {
            return Equals(NULL);
        }

        public static ActionTargetableValueTuple NULL = new ActionTargetableValueTuple(null, null, 0f);
    }
}

