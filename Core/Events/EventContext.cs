﻿using System.Collections.Generic;
using TUFFYCore.Service;
using UnityEngine;

/*
 * Event listeners are bound and triggered within
 * this class.
 *
 * Must load an array of event names with LoadEvents to use. 
 */
namespace TUFFYCore.Events
{
    public class EventContext : AbstractService {

        private Dictionary<Event, List<IListener>> _eventListeners;
        private EventList _events;

        public override void Build()
        {
            base.Build();
            _eventListeners = new Dictionary<Event, List<IListener>>();
            _events = new EventList();

        }


        public void TriggerEvent(Event evnt, EventPayload payload)
        {
            if (!_eventListeners.ContainsKey(evnt) || _eventListeners[evnt].Count == 0) return;
            _events.Get(evnt.Name).HasBeenCalled();

            foreach (IListener listener in _eventListeners[evnt])
            {
                listener.ReceiveEvent(evnt, payload);
            }

        }

        public Event GetEvent(string name)
        {
            return _events.Get(name);
        }

        public void BindEventListener(Event evnt, IListener listener)
        {

            if (!_eventListeners.ContainsKey(evnt))
            {
                _eventListeners.Add(evnt, new List<IListener>());
            }

            _eventListeners[evnt].Add(listener);
        }

        public void RemoveEventListener(Event evnt, IListener listener)
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