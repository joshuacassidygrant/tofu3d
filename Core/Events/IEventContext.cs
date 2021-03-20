using System;
using TofuConfig;
using TofuCore.Service;

namespace TofuCore.Events
{
    public interface IEventContext
    {
        void TriggerEvent(dynamic key, EventPayload payload);
        void TriggerEvent(dynamic key);
        TofuEvent GetEvent(dynamic key);
        void ContextBindEventListener(TofuEvent evnt, IListener listener);
        void ContextRemoveEventListener(TofuEvent evnt, IListener listener);
        void PreUpdate();
    }
}