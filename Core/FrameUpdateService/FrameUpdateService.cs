using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.FrameUpdateServices
{
    public class FrameUpdateService : AbstractMonoService
    {

        [Dependency] protected IEventContext EventContext;

        void Update()
        {
            UpdateTime(Time.deltaTime);

        }

        public void ForceUpdate(float seconds)
        {
            UpdateTime(seconds);
        }

        void UpdateTime(float seconds)
        {
            EventPayload deltaTimePayload = new EventPayload("Float", seconds);
            EventContext.Tick();
            EventContext.TriggerEvent("FrameUpdate", deltaTimePayload);
        }
    }
}

