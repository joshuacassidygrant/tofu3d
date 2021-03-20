using System;
using System.Collections.Generic;
using TofuConfig;
using UnityEditor;
using UnityEngine;

namespace TofuCore.Events
{
    public class EventList
    {

        private Dictionary<string, TofuEvent> _events = new Dictionary<string, TofuEvent>();

        public void Register(dynamic key)
        {
            try
            {
                string keyString = key.ToString();
                _events.Add(keyString, new TofuEvent(keyString));

            } catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        //Note: this may not be useful, since we're binding event names at request time.
        public void RegisterAll(List<EventKey> keys)
        {
            foreach (EventKey key in keys)
            {
                Register(key);
            }
        }

        public void Deregister(dynamic key)
        {
            try
            {
                string keyString = key.ToString();
                _events.Remove(keyString);

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public bool IsRegistered(dynamic key)
        {
            try
            {
                string keyString = key.ToString();
                return _events.ContainsKey(key);

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            return false;
        }

        public TofuEvent Get(dynamic key)
        {
            string keyString = key.ToString();

            if (!IsRegistered(keyString))
            {
                //Debug.Log("No event registered for " + name + ", creating one");
                TofuEvent evnt = new TofuEvent(keyString);
                _events.Add(keyString, evnt);
                return evnt;
            }

            return _events[keyString];
        }
    }

}