using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents;
using UnityEngine;

namespace TofuPlugin.ResourceModule
{
    public struct ResourceEventPayload
    {

        public Color Color;
        public ITargettable Target;
        public int Amount;

        public ResourceEventPayload(Color color, ITargettable target, int amount)
        {
            Color = color;
            Target = target;
            Amount = amount;
        }
    }

}
