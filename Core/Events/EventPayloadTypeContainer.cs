using System;
using System.Collections.Generic;
using TUFFYCore.Exceptions;
using UnityEngine;

namespace TUFFYCore.Events
{
    public class EventPayloadTypeContainer
    {

        private Dictionary<string, Func<dynamic, bool>> _typeDefaultChecks =
            new Dictionary<string, Func<dynamic, bool>>
        {
                {"Boolean", x => x is bool },
                {"Float", x => x is float },
                {"Integer", x => x is int },
                {"String", x => x is string },
                {"GameObject", x => x is GameObject },
                //{PayloadContentType.Unit, x => x is Unit }

        };

        public void RegisterPayloadContentType(string id, Func<dynamic, bool> check)
        {
            if (IsRegistered(id))
            {
                throw new PayloadTypeDoubleRegisterException();
            }

            _typeDefaultChecks.Add(id, check);
        }

        public void DeregisterPayloadContentType(string id)
        {
            if (IsRegistered(id)) _typeDefaultChecks.Remove(id);
        }

        public bool IsRegistered(string id)
        {
            return _typeDefaultChecks.ContainsKey(id);
        }

        public Func<dynamic, bool> GetDefaultCheck(string id)
        {
            if (IsRegistered(id)) return _typeDefaultChecks[id];
            return null;
        }
        
        public bool CheckContentAs(dynamic content, string id)
        {
            if (!IsRegistered(id))
            {
                Debug.Log(id + " not registered as an event payload content type");
            }

            return _typeDefaultChecks[id](content);
        }

    }

}
