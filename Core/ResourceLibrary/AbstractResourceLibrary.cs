using System;
using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

/*
 * AbstractResourceLibrary provides functionality to load in and retrieve content.
 */
namespace TofuCore.ResourceLibrary
{
    public abstract class AbstractResourceLibrary<T> : AbstractService
    {

        protected Type Type;

        protected Dictionary<string, T> _contents;


        protected AbstractResourceLibrary()
        {
            Type = typeof(T);
            _contents = new Dictionary<string, T>();
        }

        public override void Build()
        {
            base.Build();
        }

        public virtual void LoadResource(string id, T resource)
        {
            try {
                _contents.Add(id, resource);
            } catch (Exception e) {
                Debug.Log(e);
            }
        }

        public virtual bool RemoveResource(string id)
        {
            return _contents.Remove(id);
        }

        public Dictionary<string, T> GetCatalogue()
        {
            return _contents;
        }

        public T Get(string id)
        {
            if (ContainsKey(id)) return _contents[id];

            return default(T);
        }

        public int CountMembers()
        {
            return _contents.Count;
        }

        public T GetRandom()
        {
            return _contents[new List<string>(_contents.Keys)[UnityEngine.Random.Range(0, _contents.Keys.Count)]];
        }


        public bool ContainsKey(string id)
        {
            return _contents.ContainsKey(id);
        }

    }
}