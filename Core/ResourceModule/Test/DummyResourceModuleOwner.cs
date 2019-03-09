using System.Collections;
using System.Collections.Generic;
using TofuCore.ResourceModule;
using UnityEngine;

public class DummyResourceModuleOwner : IResourceModuleOwner
{
    public Vector3 Position { get; }
    private Dictionary<string, ResourceModule> _resourceModules;

    public Dictionary<string, ResourceModule> GetResourceModules()
    {
        return _resourceModules;
    }

    public void AssignResourceModule(string key, ResourceModule module)
    {
        if (_resourceModules.ContainsKey(key))
        {
            Debug.Log("Can't assign a second resource module to key " + key);
            return;
        }

        _resourceModules.Add(key, module);
    }

    public void RemoveResourceModule(string key)
    {
        _resourceModules.Remove(key);
    }

    public ResourceModule GetResourceModule(string key)
    {
        if (!_resourceModules.ContainsKey(key)) return null;
        return _resourceModules[key];
    }

}
