using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class InstantiatingService : AbstractMonoService
    {

        public int ObjectsInstantiated = 0;
        [Dependency] protected EventContext EventContext;

        public override void Initialize()
        {
            base.Initialize();
            BindListener(EventContext.GetEvent("SpawnUnit"), SpawnUnit, EventContext);
        }

        public GameObject DoInstantiate(GameObject obj, Vector3 place)
        {
            ObjectsInstantiated++;
            return obj;

        }


        /*
     * Event-triggered
     */

        public void SpawnUnit(EventPayload payload)
        {
            if (payload.ContentType != "Agent")
            {
                Debug.Log("Incorrect content type");
                return;
            }
            
            Agent agent = (Agent) payload.GetContent();

            UnitRenderer renderer = DoInstantiate(new GameObject() {name = agent.Name + agent.Id}, agent.Position).AddComponent<UnitRenderer>();
            renderer.Initialize(agent);
        }
    }
}
