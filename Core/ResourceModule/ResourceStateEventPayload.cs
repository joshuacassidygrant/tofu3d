using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.ResourceModule
{
    public struct ResourceStateEventPayload
    {

        public Color Color;
        public IResourceModuleOwner Target;
        public int Amount;
        public int Max;

        public ResourceStateEventPayload(Color color, IResourceModuleOwner target, int amount, int max)
        {
            Color = color;
            Target = target;
            Amount = amount;
            Max = max;
        }

    }

}
