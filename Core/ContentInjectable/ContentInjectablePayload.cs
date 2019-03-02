using System.Collections;
using System.Collections.Generic;
using TofuCore.ContentInjectable;
using UnityEngine;

public class ContentInjectablePayload
{

    private Dictionary<string, IContentInjectable> _contents;

    public ContentInjectablePayload()
    {
        _contents = new Dictionary<string, IContentInjectable>();
    }

    public dynamic Get(string key)
    {
        if (!_contents.ContainsKey(key))
        {
            Debug.Log("Couldn't find key " + key + " in ContentInjectablePayload");
            return null;
        }

        return _contents[key];
    }

    public void Add(string key, IContentInjectable injectable)
    {
        _contents.Add(key, injectable);
    }

}
