using System;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Exceptions;
using TofuCore.FrameUpdateServices;
using TofuCore.Glops;
using UnityEngine;

/*
 * The ServiceContext provides a container and dependency
 * injection for service classes.
 */
namespace TofuCore.Service
{
    public class ServiceContext : IServiceContext
    {
        public int LastGlopId { get; set; }

        private Dictionary<string, IService> _services;
        private Dictionary<string, string> _aliases;
        private List<IGlopContainer> _glopContainers;
        private ServiceFactory _factory;

        public ServiceContext()
        {
            _services = new Dictionary<string, IService>();
            _aliases = new Dictionary<string, string>();
            _glopContainers = new List<IGlopContainer>();
            _factory = new ServiceFactory(this);
            LastGlopId = 0x1;

            //All service contexts need an event context.
            BindCoreServices();
        }

        /*
         * Binding
         */
        public void Drop(string name)
        {
            _services.Remove(name);
        }

        public void Bind(string name, IService service, bool overwrite = true)
        {
            service = _factory.Build(service);

            if (_services.ContainsKey(name))
            {
                if (!overwrite) throw new ServiceDoubleBindException();
                _services[name] = service;
            }
            else
            {
                _services.Add(name, service);
            }

            if (service is IGlopContainer glopContainer)
            {
                _glopContainers.Add(glopContainer);
            }
        }

        public bool Unbind(string name)
        {
            if (!_services.ContainsKey(name))
            {
                Debug.LogWarning($"Tried to unbind {name} but it wasn't bound");
                return false;
            }

            IService service = Fetch(name);
            service.Cease();
            _services.Remove(name);
            return true;
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
            if (_aliases.ContainsKey(alias))
            {
                _aliases[alias] = key;
            }
            else
            {
                _aliases.Add(alias, key);
            }
        }
        
        //Glop functions
        public Glop FindGlopById(int id)
        {
            foreach (IGlopContainer manager in _glopContainers)
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
            PrepareAll();
            InitializeAll();
        }

        //To be called for late bindings:
        public void FullInitialization(IService service)
        {
            ResolveBindings();
            if (!service.Initialized || service.RebindMode != RebindMode.REBIND_IGNORE)
            {
                service.Initialize();
            }
        }

        private void ResolveBindings()
        {
            foreach (KeyValuePair<String, IService> entry in _services)
            {
                entry.Value.ResolveServiceBindings();
            }
        }

        private void PrepareAll()
        {
            foreach (KeyValuePair<String, IService> entry in _services)
            {
                entry.Value.Prepare();
            }
        }

        private void InitializeAll()
        {
            foreach (KeyValuePair<String, IService> entry in _services)
            {
                if (!entry.Value.Initialized || entry.Value.RebindMode != RebindMode.REBIND_IGNORE) {
                    entry.Value.Initialize();
                }
            }
        }

        private void BindCoreServices()
        {
            new EventContext().BindServiceContext(this);
            AddAlias("EventContext", "IEventContext");

        }

        public Dictionary<string, IGlopContainer> GetGlopContainers()
        {
            Dictionary<string, IGlopContainer> containers = new Dictionary<string, IGlopContainer>();
            foreach (KeyValuePair<string, IService> entry in _services)
            {
                if (entry.Value is IGlopContainer glopContainer)
                {
                    containers.Add(entry.Key, glopContainer);
                }
            }
            return containers;
        }

        private static GameObject NewServiceGameObject(string typeName) {
            GameObject obj = new GameObject();
            obj.name = typeName;
            return obj;
        }

    }
}
