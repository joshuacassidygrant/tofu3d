using System;

namespace TUFFYCore.Events
{
    public interface IListener {
        void ReceiveEvent(Event evnt, EventPayload payload);
        void BindListener(Event evnt, Action<EventPayload> action, EventContext evntContext);
    }
}
