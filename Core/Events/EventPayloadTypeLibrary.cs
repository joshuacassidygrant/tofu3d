using System;
using System.Collections.Generic;
using TofuCore.ResourceLibrary;
using UnityEngine;

namespace TofuCore.Events
{
    public interface IEventPayloadTypeLibrary
    {
        bool CheckContentAs(string type, object content);
        bool ValidatePayload(EventPayload payload);
    }

    public class EventPayloadTypeLibrary : AbstractResourceLibrary<Func<object, bool>>, IEventPayloadTypeLibrary
    {
        public EventPayloadTypeLibrary(Dictionary<string, Func<object, bool>> contents)
        {
            _contents = contents;
        }

        public bool CheckContentAs(string type, object content)
        {
            if (!_contents.ContainsKey(type))
            {
                Debug.Log("No payload type for " + type + " found");
                return false;
            }

            return _contents[type].Invoke(content);
        }

        public bool ValidatePayload(EventPayload payload)
        {
            return CheckContentAs(payload.ContentType, payload.GetContent());
        }
    }

}
 