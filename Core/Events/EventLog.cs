using System.Collections;
using System.Collections.Generic;
using TofuConfig;
using UnityEngine;

namespace TofuCore.Events
{
    public class EventLog
    {
        public float TimeStamp;
        public EventKey EventKey;
        public string PayloadType;

        public EventLog(float timeStamp, EventKey evnt, string payloadType)
        {
            TimeStamp = timeStamp;
            EventKey = evnt;
            PayloadType = payloadType;
        }

        public override string ToString()
        {
            return TimeStamp.ToString() + ": " + EventKey + "(" + PayloadType + ")";
        }
    }


}
