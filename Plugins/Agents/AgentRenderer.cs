using TofuCore.Service;
using TofuPlugin.Renderable;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentRenderer : AbstractRenderer
    {

        public void Initialize(Agent agent, ServiceContext context)
        {
            base.Initialize(agent, context);
            SetLayer(agent.GetSortingLayer());
        }


    }
}
