using System.Collections.Generic;
using TUFFYCore.Service;
using UnityEngine;

/*
 * Event listeners are bound and triggered within
 * this class.
 */
namespace TUFFYCore.Events
{
    public class EventContext : AbstractService {

        private Dictionary<Events, List<IListener>> _eventListeners;

        public override void Initialize()
        {
            base.Initialize();
            _eventListeners = new Dictionary<Events, List<IListener>>();
        }

        public void TriggerEvent(Events evnt, EventPayload payload)
        {
            if (!_eventListeners.ContainsKey(evnt) || _eventListeners[evnt].Count == 0) return;

            foreach (IListener listener in _eventListeners[evnt])
            {
                listener.ReceiveEvent(evnt, payload);
            }

        }

        public void BindEventListener(Events evnt, IListener listener)
        {
        
            if (!_eventListeners.ContainsKey(evnt))
            {
                _eventListeners.Add(evnt, new List<IListener>());
            }

            _eventListeners[evnt].Add(listener);
        }

        public void RemoveEventListener(Events evnt, IListener listener)
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
