using TofuPlugin.Renderable;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentRenderer : AbstractRenderer
    {

        public void Initialize(Agent agent)
        {
            base.Initialize(agent);
            SetLayer(agent.GetSortingLayer());
        }


    }
}
