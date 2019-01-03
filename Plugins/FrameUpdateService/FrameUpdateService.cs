using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.FrameUpdateService
{
    public class FrameUpdateService : AbstractMonoService
    {

        [Dependency] protected EventContext EventContext;

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
            EventPayload deltaTimePayload = new EventPayload("Float", seconds, EventContext);
            EventContext.TriggerEvent("FrameUpdate", deltaTimePayload);
        }
    }
}

