using System;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.UI;
using UnityEngine;

namespace TofuPlugin.UI
{
    public class TofuUiBase : MonoBehaviour, IListener
    {
        protected UIBindingService UiBindingService;
        protected IServiceContext ServiceContext;
        protected EventContext EventContext;
        private Dictionary<TofuEvent, List<Action<EventPayload>>> _boundListeners = new Dictionary<TofuEvent, List<Action<EventPayload>>>();
        private Dictionary<TofuEvent, Action<EventPayload>> _listenersToUnbind;

        protected void BindServiceContext()
        {
            if (ServiceContext != null) return;

            if (UiBindingService == null)
            {
                UiBindingService = FindObjectOfType<UIBindingService>();
            }

            if (UiBindingService != null)
            {
                ServiceContext = UiBindingService.GetServiceContext();
                EventContext = ServiceContext.Fetch("EventContext");
            }
        }

        public void ReceiveEvent(TofuEvent evnt, EventPayload payload) {
            if (_boundListeners == null) return;

            if (!_boundListeners.ContainsKey(evnt)) {
                Debug.Log("Expected event " + evnt + " but found no action bound");
            }

            foreach (Action<EventPayload> action in _boundListeners[evnt]) {
                action.Invoke(payload);
            }

            _listenersToUnbind = new Dictionary<TofuEvent, Action<EventPayload>>();


            foreach (KeyValuePair<TofuEvent, Action<EventPayload>> kvp in _listenersToUnbind) {
                UnbindListener(kvp.Key, kvp.Value, EventContext);
            }

            _listenersToUnbind = null;
        }

        public void BindListener(string eventId, Action<EventPayload> action, IEventContext evntContext) {
            BindListener(evntContext.GetEvent(eventId), action, evntContext);
        }


        public void BindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext) {
            if (_boundListeners == null) _boundListeners = new Dictionary<TofuEvent, List<Action<EventPayload>>>();

            if (!_boundListeners.ContainsKey(evnt)) _boundListeners.Add(evnt, new List<Action<EventPayload>>());

            evntContext.ContextBindEventListener(evnt, this);
            _boundListeners[evnt].Add(action);
        }

        public void UnbindListener(string eventId, Action<EventPayload> action, IEventContext evntContext) {
            UnbindListener(evntContext.GetEvent(eventId), action, evntContext);
        }

        public void UnbindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext) {
            evntContext.ContextRemoveEventListener(evnt, this);
            _boundListeners[evnt].Remove(action);

        }

        public void UnbindListenerDeferred(string eventId, Action<EventPayload> action, IEventContext evntContext) {
            if (_listenersToUnbind != null) {
                _listenersToUnbind.Add(evntContext.GetEvent(eventId), action);
            } else {
                UnbindListener(evntContext.GetEvent(eventId), action, evntContext);
            }
        }
    }
}

