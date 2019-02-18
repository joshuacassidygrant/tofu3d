using System.Collections.Generic;
using TofuCore.ResourceModule;
using UnityEngine;

public interface IResourceModuleOwner
{
    Dictionary<string, ResourceModule> GetResourceModules();
    void AssignResourceModule(string key, ResourceModule module);
    void RemoveResourceModule(string key);
    ResourceModule GetResourceModule(string key);
    Vector3 GetPosition();
}
