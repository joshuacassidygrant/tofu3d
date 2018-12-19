using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * All events are sent with an event payload, a data type
 * that encodes what it can be decoded to for type checking.
 */
namespace TUFFYCore.Events
{
    public class EventPayload
    {

        public string ContentType { get; }

        private dynamic _content;
        private EventPayloadTypeContainer _payloadTypeContainer;

        public EventPayload(string contentType, dynamic content, EventContext eventContext)
        {
            ContentType = contentType;
            _content = content;

            _payloadTypeContainer = eventContext.GetPayloadTypeContainer();
            
            if (!_payloadTypeContainer.CheckContentAs(content, contentType))
            {
                Debug.Log("Can't store content " + content.ToString() + " as " + contentType);
                //TODO: do something
            }

            
        }


        public dynamic GetContent()
        {
            return _content;
        }


    }
}
