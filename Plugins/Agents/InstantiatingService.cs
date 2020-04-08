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
        }

        public GameObject DoInstantiate(GameObject obj, Vector3 place)
        {
            
            ObjectsInstantiated++;
            return Instantiate(obj);

        }



    }
}
