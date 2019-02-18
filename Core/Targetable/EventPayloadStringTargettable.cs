using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Targetable
{
    public struct EventPayloadStringTargettable
    {

        public ITargetable Target;
        public string Value;

        public EventPayloadStringTargettable(ITargetable target, string value)
        {
            Target = target;
            Value = value;
        }
    }
}

