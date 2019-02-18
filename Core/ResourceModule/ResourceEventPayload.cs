using TofuCore.Targetable;
using UnityEngine;

namespace TofuCore.ResourceModule
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
