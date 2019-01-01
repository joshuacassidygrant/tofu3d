using TofuPlugin.Renderable;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class UnitRenderer : AbstractRenderer
    {

        public void Initialize(Agent agent)
        {
            base.Initialize(agent);
            SetLayer("Agent");
        }


    }
}
