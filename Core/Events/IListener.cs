﻿using System;

namespace TofuCore.Events
{
    public interface IListener {
        void ReceiveEvent(TofuEvent evnt, EventPayload payload);
        void BindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext);
        void BindListener(string evntId, Action<EventPayload> action, IEventContext evntContext);
        void UnbindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext);
    }
}
