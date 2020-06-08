using System;
using TofuConfig;

namespace TofuCore.Events
{
    public interface IListener {
        void ReceiveEvent(TofuEvent evnt, EventPayload payload);
        void BindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext);
        void BindListener(EventKey key, Action<EventPayload> action, IEventContext evntContext);
        void UnbindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext);
        void UnbindListenerDeferred(EventKey key, Action<EventPayload> action, IEventContext evntContext);
    }
}
