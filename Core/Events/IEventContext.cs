using System;
using TofuConfig;
using TofuCore.Service;

namespace TofuCore.Events
{
    public interface IEventContext
    {
        void TriggerEvent(EventKey key, EventPayload payload);
        void TriggerEvent(EventKey key);
        TofuEvent GetEvent(EventKey key);
        void ContextBindEventListener(TofuEvent evnt, IListener listener);
        void ContextRemoveEventListener(TofuEvent evnt, IListener listener);
        void PreUpdate();
    }
}