using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TofuCore.Events
{
    public class EventList
    {

        private Dictionary<string, TofuEvent> _events = new Dictionary<string, TofuEvent>();

        public void Register(string name)
        {
            _events.Add(name, new TofuEvent(name));
        }

        //Note: this may not be useful, since we're binding event names at request time.
        public void RegisterAll(List<string> names)
        {
            foreach (string name in names)
            {
                Register(name);
            }
        }

        public void Deregister(string name)
        {
            _events.Remove(name);
        }

        public bool IsRegistered(string name)
        {
            return _events.ContainsKey(name);
        }

        public TofuEvent Get(string name)
        {
            if (!IsRegistered(name))
            {
                //Debug.Log("No event registered for " + name + ", creating one");
                TofuEvent evnt = new TofuEvent(name);
                _events.Add(name, evnt);
                return evnt;
            }

            return _events[name];
        }
    }

}