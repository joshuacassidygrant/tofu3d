using System;
using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

/*
 * Event listeners are bound and triggered within
 * this class.
 *
 * Must load an array of event names with LoadEvents to use. 
 */
namespace TofuCore.Events
{
    public class EventContext : AbstractService, IEventContext
    {
        private Dictionary<TofuEvent, List<IListener>> _eventListeners;
        private EventList _events;
        private Dictionary<TofuEvent, IListener> _eventListenersToRemove;
        [Dependency] protected IEventPayloadTypeLibrary EventPayloadLibrary;
        [Dependency] protected IEventLogger EventLogger;
        

        // Set Up
        public override void Build()
        {
            base.Build();
            _eventListeners = new Dictionary<TofuEvent, List<IListener>>();
            _events = new EventList();
        }

        // Run from FrameUpdateService BEFORE calling frame update events.
        public void PreUpdate()
        {
            FlushListeners();
        }

        

        // Event Management
        public TofuEvent GetEvent(string name)
        {
            return _events.Get(name);
        }

        public void TriggerEvent(string eventKey)
        {
            TriggerEvent(eventKey, new EventPayload("Null", null));
        }

        public void TriggerEvent(string eventKey, EventPayload payload)
        {
            if (EventPayloadLibrary != null)
            {
                if (!EventPayloadLibrary.ValidatePayload(payload))
                {
                    throw new ArgumentException("Invalid payload!");
                }
            }

            FlushListeners();
            TofuEvent evnt = GetEvent(eventKey);

            if (!_eventListeners.ContainsKey(evnt) || _eventListeners[evnt].Count == 0) return;
            _events.Get(evnt.Name).HasBeenCalled();

            List<IListener> listeners = new List<IListener>(_eventListeners[evnt]);

            foreach (IListener listener in listeners)
            {
                listener.ReceiveEvent(evnt, payload);
            }

            if (EventLogger != null)
            {
                if (payload == null)
                {
                    EventLogger.LogEvent(Time.time, eventKey, "NULL");

                }
                else
                {
                    EventLogger.LogEvent(Time.time, eventKey, payload.ContentType);
                }
            }


        }

        //Event Listener Management

        public void ContextBindEventListener(string evntId, IListener listener)
        {
            ContextBindEventListener(GetEvent(evntId), listener);
        }

        public void ContextBindEventListener(TofuEvent evnt, IListener listener)
        {
            if (listener == this)
            {
                Debug.LogWarning("Trying to bind EventContext as its own listener.");
            }

            if (!_eventListeners.ContainsKey(evnt))
            {
                _eventListeners.Add(evnt, new List<IListener>());
            }

            _eventListeners[evnt].Add(listener);
        }

        public void ContextRemoveEventListener(string evntId, IListener listener)
        {
            ContextRemoveEventListener(GetEvent(evntId), listener);
        }

        public void ContextRemoveEventListener(TofuEvent evnt, IListener listener)
        {
            if (!_eventListeners.ContainsKey(evnt) || !_eventListeners[evnt].Contains(listener))
            {
                //Debug.Log("Not bound.");
                return;
            }

            if (_eventListenersToRemove == null)
            {
                _eventListenersToRemove = new Dictionary<TofuEvent, IListener>();
            }

            if (!_eventListenersToRemove.ContainsKey(evnt))
            {
                _eventListenersToRemove.Add(evnt, listener);
            }


        }

        private void FlushListeners()
        {
            if (_eventListenersToRemove == null) return;

            foreach (KeyValuePair<TofuEvent, IListener> entry in _eventListenersToRemove)
            {
                _eventListeners[entry.Key].Remove(entry.Value);
            }
            _eventListenersToRemove = null;

        }
    }
}
