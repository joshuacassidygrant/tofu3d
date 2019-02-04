using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents;
using TofuPlugin.Agents.Targetable;
using UnityEngine;

namespace TofuPlugin.ResourceModule
{
    public struct ResourceEventPayload
    {

        public Color Color;
        public ITargetable Target;
        public int Amount;

        public ResourceEventPayload(Color color, ITargetable target, int amount)
        {
            Color = color;
            Target = target;
            Amount = amount;
        }
    }

}
