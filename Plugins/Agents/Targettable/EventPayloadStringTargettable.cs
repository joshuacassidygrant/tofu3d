using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Targettable
{
    public struct EventPayloadStringTargettable
    {

        public ITargettable Target;
        public string Value;

        public EventPayloadStringTargettable(ITargettable target, string value)
        {
            Target = target;
            Value = value;
        }
    }
}

