using System.Collections.Generic;
using TofuCore.ResourceModule;
using UnityEngine;

public interface IResourceModuleOwner
{
    Dictionary<string, IResourceModule> GetResourceModules();
    void AssignResourceModule(string key, IResourceModule module);
    void RemoveResourceModule(string key);
    IResourceModule GetResourceModule(string key);
    Vector3 Position { get; }
    int Id { get; }
}
