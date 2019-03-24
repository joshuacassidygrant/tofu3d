using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.Pathfinding;
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


            //string colorLabel = agent.Properties.GetProperty("BaseColor", "white");
            SpriteRenderer.color = Color.white;

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
            if (agent.Id == Renderable.Id)
            {
                UnbindListener(EventContext.GetEvent("UnitDies"), UnitDestroyed, EventContext);
                ToDestroy = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Renderable is Agent)
            {
                Agent a = (Agent) Renderable;
                Path p = a.Mobility.Path;

                if (p == null) return;

                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(p.LookPoints[0], new Vector3(0.2f, 0.2f, 0.5f));
                Gizmos.DrawLine(a.Position, p.LookPoints[0]);

                for (int i = 1; i < p.LookPoints.Length; i++)
                {
                    Gizmos.DrawLine(p.LookPoints[i-1], p.LookPoints[i]);
                    Gizmos.DrawCube(p.LookPoints[i], new Vector3(0.2f, 0.2f, 0.5f));
                }
            }
        }

    }
}
