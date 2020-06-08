using System.Collections.Generic;
using TofuConfig;
using UnityEditor;
using UnityEngine;

namespace TofuCore.Events
{
    public class EventList
    {

        private Dictionary<EventKey, TofuEvent> _events = new Dictionary<EventKey, TofuEvent>();

        public void Register(EventKey key)
        {
            _events.Add(key, new TofuEvent(key));
        }

        //Note: this may not be useful, since we're binding event names at request time.
        public void RegisterAll(List<EventKey> keys)
        {
            foreach (EventKey key in keys)
            {
                Register(key);
            }
        }

        public void Deregister(EventKey key)
        {
            _events.Remove(key);
        }

        public bool IsRegistered(EventKey key)
        {
            return _events.ContainsKey(key);
        }

        public TofuEvent Get(EventKey key)
        {
            if (!IsRegistered(key))
            {
                //Debug.Log("No event registered for " + name + ", creating one");
                TofuEvent evnt = new TofuEvent(key);
                _events.Add(key, evnt);
                return evnt;
            }

            return _events[key];
        }
    }

}