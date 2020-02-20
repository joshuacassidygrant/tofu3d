using System.Collections.Generic;
using Newtonsoft.Json;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.ResourceModule;
using UnityEngine;

namespace TofuCore.Player
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : Glop, IResourceModuleOwner
    {
        [JsonProperty] private readonly Dictionary<string, IResourceModule> _resourceModules;
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public Vector3 Position { get; private set; }

        public Player(string name) : base()
        {
            _resourceModules = new Dictionary<string, IResourceModule>();
            Name = name;
        }


        public override void Update(float frameDelta)
        {
        }


        public Dictionary<string, IResourceModule> GetResourceModules()
        {
            return _resourceModules;
        }

        public void AssignResourceModule(string key, IResourceModule module)
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

        public IResourceModule GetResourceModule(string key)
        {
            if (!_resourceModules.ContainsKey(key)) return null;
            return _resourceModules[key];
        }
    }

}
