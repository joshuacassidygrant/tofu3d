using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Events
{
    public class EventLog
    {
        public float TimeStamp;
        public string Event;
        public string PayloadType;

        public EventLog(float timeStamp, string evnt, string payloadType)
        {
            TimeStamp = timeStamp;
            Event = evnt;
            PayloadType = payloadType;
        }

        public override string ToString()
        {
            return TimeStamp.ToString() + ": " + Event + "(" + PayloadType + ")";
        }
    }


}
