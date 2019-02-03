using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Targettable;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public struct ActionTargettableValueTuple
    {
        public AgentAction Action;
        public ITargettable Targettable;
        public float Value;

        public ActionTargettableValueTuple(AgentAction action, ITargettable targettable, float value)
        {
            Action = action;
            Targettable = targettable;
            Value = value;
        }


        public bool IsNull()
        {
            return Equals(NULL);
        }

        public static ActionTargettableValueTuple NULL = new ActionTargettableValueTuple(null, null, 0f);
    }
}

