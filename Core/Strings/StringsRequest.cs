using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringsRequest
{

    public object[] Values;
    public string StringId;

    public StringsRequest(string id, params object[] values)
    {
        Values = values;
        StringId = id;
    }

}
