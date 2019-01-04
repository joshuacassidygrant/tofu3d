﻿using System.Collections.Generic;
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
    public class EventContext : AbstractService
    {


        private Dictionary<TofuEvent, List<IListener>> _eventListeners;
        private EventList _events;
        private EventPayloadTypeContainer _eventPayloadTypeContainer;

        public override void Build()
        {
            base.Build();
            _eventListeners = new Dictionary<TofuEvent, List<IListener>>();
            _events = new EventList();
            _eventPayloadTypeContainer = new EventPayloadTypeContainer();
        }

        public EventPayloadTypeContainer GetPayloadTypeContainer()
        {
            return _eventPayloadTypeContainer;
        }

        public void TriggerEvent(string eventKey, EventPayload payload)
        {
            TofuEvent evnt = GetEvent(eventKey);

            if (!_eventListeners.ContainsKey(evnt) || _eventListeners[evnt].Count == 0) return;
            _events.Get(evnt.Name).HasBeenCalled();

            foreach (IListener listener in _eventListeners[evnt])
            {
                listener.ReceiveEvent(evnt, payload);
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

            _eventListeners[evnt].Remove(listener);

        }
    }
}
