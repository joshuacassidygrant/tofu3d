using System.Collections;
using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Events
{
    public interface IEventLogger
    {
        void LogEvent(float timeStamp, string evnt, string payloadType);
        void DumpCallCounts();
        void DumpCallHistory();
    }

    public class EventLogger: AbstractService, IEventLogger
    {
        public Dictionary<string, Dictionary<string, int>> EventsCalledToPayloadTypesCounts = new Dictionary<string, Dictionary<string, int>>();
        public List<EventLog> Logs = new List<EventLog>();

        //Note: this is only called for events that HAVE listeners
        public void LogEvent(float timeStamp, string evnt, string payloadType)
        {
            Logs.Add(new EventLog(timeStamp, evnt, payloadType));

            if (EventsCalledToPayloadTypesCounts.ContainsKey(evnt))
            {
                if (EventsCalledToPayloadTypesCounts[evnt].ContainsKey(payloadType))
                {
                    EventsCalledToPayloadTypesCounts[evnt][payloadType]++;
                }
                else
                {
                    EventsCalledToPayloadTypesCounts[evnt].Add(payloadType, 1);
                }
            }
            else
            {
                EventsCalledToPayloadTypesCounts.Add(evnt, new Dictionary<string, int> { {payloadType, 1} });
            }
        }

        public void DumpCallCounts()
        {
            string logString = "";
            foreach (KeyValuePair<string, Dictionary<string, int>> entry in EventsCalledToPayloadTypesCounts)
            {
                logString += entry.Key + ":\n";
                foreach (KeyValuePair<string, int> subEntry in entry.Value)
                {
                    logString += "--" + subEntry.Key + ": " + subEntry.Value + "\n";
                }
            }

            Debug.Log(logString);
        }

        public void DumpCallHistory()
        {
            string logString = "";
            foreach (EventLog log in Logs)
            {
                logString += log.ToString() + "\n";
            }
            Debug.Log(logString);
        }



    }

}
