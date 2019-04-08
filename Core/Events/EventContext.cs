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
        private IEventPayloadTypeContainer _eventPayloadTypeContainer;
        private Dictionary<TofuEvent, IListener> _eventListenersToRemove;
        [Dependency] protected EventLogger EventLogger;

        public override void Build()
        {
            base.Build();
            _eventListeners = new Dictionary<TofuEvent, List<IListener>>();
            _events = new EventList();
            _eventPayloadTypeContainer = new EventPayloadTypeContainer();
        }

        public IEventPayloadTypeContainer GetPayloadTypeContainer()
        {
            return _eventPayloadTypeContainer;
        }

        public void TriggerEvent(string eventKey, EventPayload payload)
        {
            FlushListeners();
            TofuEvent evnt = GetEvent(eventKey);

            if (!_eventListeners.ContainsKey(evnt) || _eventListeners[evnt].Count == 0) return;
            _events.Get(evnt.Name).HasBeenCalled();

            foreach (IListener listener in _eventListeners[evnt])
            {
                listener.ReceiveEvent(evnt, payload);
            }
            FlushListeners();

            if (EventLogger != null)
            {
                EventLogger.LogEvent(Time.time, eventKey, payload.ContentType);
            }


        }

        public TofuEvent GetEvent(string name)
        {
            return _events.Get(name);
        }

        public void HelperBindEventListener(TofuEvent evnt, IListener listener)
        {
            if (!_eventListeners.ContainsKey(evnt))
            {
                _eventListeners.Add(evnt, new List<IListener>());
            }

            _eventListeners[evnt].Add(listener);
        }

        public void RemoveEventListener(TofuEvent evnt, IListener listener)
        {
            if (!_eventListeners.ContainsKey(evnt) || !_eventListeners[evnt].Contains(listener))
            {
                Debug.Log("Not bound.");
                return;
            }

            if (_eventListenersToRemove == null)
            {
                _eventListenersToRemove = new Dictionary<TofuEvent, IListener>();
            }

            _eventListenersToRemove.Add(evnt, listener);


        }

        public bool CheckPayloadContentAs(dynamic content, string type)
        {
            return _eventPayloadTypeContainer.CheckContentAs(content, type);
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
