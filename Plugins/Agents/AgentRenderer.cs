using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.Renderable;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace TofuPlugin.Agents
{
    public class AgentRenderer : AbstractRenderer
    {
        public void Initialize(Agent agent, ServiceContext context)
        {
            base.Initialize(agent, context);
            
            BindListener(EventContext.GetEvent("AgentDies"), UnitDestroyed, EventContext);


            SpriteRenderer.color = agent.BaseColor;

            if (Renderable == null)
            {
                Destroy(this);
            }
            SetLayer(agent.GetSortingLayer());

            //Temp
            Anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Creatures/TinySpiritController");

        }



        protected void UnitDestroyed(EventPayload eventPayload)
        {

            if (eventPayload.ContentType != "Agent") return;
            Agent agent = eventPayload.GetContent();
            if (agent.Id == Renderable.GetId())
            {
                UnbindListener(EventContext.GetEvent("UnitDies"), UnitDestroyed, EventContext);
                ToDestroy = true;
            }
        }



    }
}
