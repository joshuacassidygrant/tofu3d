using System;

public interface IListener {
    void ReceiveEvent(Events evnt, EventPayload payload);
    void BindListener(Events evnt, Action<EventPayload> action, EventContext evntContext);
}
