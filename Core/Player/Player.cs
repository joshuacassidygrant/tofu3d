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

        public Player(int id, string name, ServiceContext context) : base(id, context)
        {
            _resourceModules = new Dictionary<string, ResourceModule.ResourceModule>();
        }

        public override void Update(float frameDelta)
        {
        }

        public Dictionary<string, ResourceModule.ResourceModule> GetResourceModules()
        {
            throw new System.NotImplementedException();
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

        public Vector3 GetPosition()
        {
            //TODO: Maybe give this a position?
            return Vector3.zero;
        }
    }

}
