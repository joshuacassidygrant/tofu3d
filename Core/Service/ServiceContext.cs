using System;
using System.Collections.Generic;
using TofuCore.Exceptions;
using TofuCore.Glops;
using UnityEngine;

/*
 * The ServiceContext provides a container and dependency
 * injection for service classes.
 */
namespace TofuCore.Service
{
    public class ServiceContext
    {
        public int LastGlopId;

        private Dictionary<string, IService> _services;
        private Dictionary<string, string> _aliases;
        private List<GlopContainer> _glopManagers;
        private ServiceFactory _factory;

        public ServiceContext()
        {
            _services = new Dictionary<string, IService>();
            _aliases = new Dictionary<string, string>();
            _glopManagers = new List<GlopContainer>();
            _factory = new ServiceFactory(this);
            LastGlopId = 0x1;
        }

    /*
     * Binding
     */
        public void Drop(string name)
        {
            _services.Remove(name);
        }

        public void Bind(string name, IService service)
        {
            if (_services.ContainsKey(name)) throw new ServiceDoubleBindException();
            service = _factory.Build(service);
            _services.Add(name, service);

            if (service is GlopContainer)
            {
                _glopManagers.Add((GlopContainer)service);
            }
        }

        public dynamic Fetch(string name)
        {
            if (Has(name))
            {
                if (_services.ContainsKey(name))
                {
                    return _services[name];
                } else
                {
                    return _services[_aliases[name]];
                }
            }

            Debug.Log("Can't find type " + name);
            return null;
        }

        public bool Has(string name)
        {
            if (_services.ContainsKey(name) || (_aliases.ContainsKey(name) && _services.ContainsKey(_aliases[name])))
            {
                return true;
            }

            return false;
        }

        public void AddAlias(string key, string alias)
        {
            _aliases.Add(alias, key);
        }
        
        //Glop functions
        public Glops.Glop FindGlopById(int id)
        {
            foreach (GlopContainer manager in _glopManagers)
            {
                if (manager.HasId(id)) return manager.GetGlopById(id);
            }
            return null;
        }

    /*
     * Initialization
     */
        public void FullInitialization()
        {
            ResolveBindings();
            InitializeAll();
        }

        public void ResolveBindings()
        {
            foreach (KeyValuePair<String, IService> entry in _services)
            {
                entry.Value.ResolveServiceBindings();
            }
        }

        public void InitializeAll()
        {
            foreach (KeyValuePair<String, IService> entry in _services)
            {
                entry.Value.Initialize();
            }
        }

    

    }
}
