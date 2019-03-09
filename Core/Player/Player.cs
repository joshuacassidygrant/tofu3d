using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.ResourceModule;
using UnityEngine;

namespace TofuCore.Player
{
    public class Player : Glop, IResourceModuleOwner
    {
        private Dictionary<string, ResourceModule.ResourceModule> _resourceModules;

        public string Name { get; private set; }
        public Vector3 Position { get; private set; }

        public Player(string name) : base()
        {
            _resourceModules = new Dictionary<string, ResourceModule.ResourceModule>();
            Name = name;
        }


        public override void InjectDependencies(ContentInjectablePayload injectables)
        {
        }

        public override void Update(float frameDelta)
        {
        }


        public Dictionary<string, ResourceModule.ResourceModule> GetResourceModules()
        {
            return _resourceModules;
        }

        public void AssignResourceModule(string key, ResourceModule.ResourceModule module)
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

        public ResourceModule.ResourceModule GetResourceModule(string key)
        {
            if (!_resourceModules.ContainsKey(key)) return null;
            return _resourceModules[key];
        }
    }

}
