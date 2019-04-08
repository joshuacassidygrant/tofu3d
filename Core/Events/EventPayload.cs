using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * All events are sent with an event payload, a data type
 * that encodes what it can be decoded to for type checking.
 */
namespace TofuCore.Events
{
    public class EventPayload
    {

        public string ContentType { get; }

        private dynamic _content;
        private IEventPayloadTypeContainer _payloadTypeContainer;

        public EventPayload(string contentType, dynamic content, IEventContext eventContext)
        {
            ContentType = contentType;
            _content = content;
            
            if (!eventContext.CheckPayloadContentAs(content, contentType))
            {
                Debug.LogWarning("Can't store content " + content.ToString() + " as " + contentType);
            }
        }


        public dynamic GetContent()
        {
            return _content;
        }


    }
}
