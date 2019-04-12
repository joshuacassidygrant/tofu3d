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

        private readonly object _content;

        public EventPayload(string contentType, object content)
        {
            ContentType = contentType;
            _content = content;
            
        }


        public dynamic GetContent()
        {
            return _content;
        }


    }
}
