using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEventPayloadTypeContainer
{
    void RegisterPayloadContentType(string id, Func<object, bool> check);
    void DeregisterPayloadContentType(string id);
    bool IsRegistered(string id);
    Func<object, bool> GetDefaultCheck(string id);
    bool CheckContentAs(dynamic content, string id);
}

