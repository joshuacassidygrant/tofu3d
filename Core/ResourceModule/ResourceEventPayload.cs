using TofuCore.Targetable;
using UnityEngine;

namespace TofuCore.ResourceModule
{
    public struct ResourceEventPayload
    {

        public Color Color;
        public IResourceModuleOwner Target;
        public int Amount;

        public ResourceEventPayload(Color color, IResourceModuleOwner target, int amount)
        {
            Color = color;
            Target = target;
            Amount = amount;
        }
    }

}
