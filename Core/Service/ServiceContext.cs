using System;
using System.Collections.Generic;
using TofuCore.Exceptions;
using UnityEngine;

/*
 * The ServiceContext provides a container and dependency
 * injection for service classes.
 */
namespace TofuCore.Service
{
    public class ServiceContext
    {
        private Dictionary<string, IService> _services;
        private ServiceFactory _factory;

        public ServiceContext()
        {
            _services = new Dictionary<string, IService>();
            _factory = new ServiceFactory(this);
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
        }

        public dynamic Fetch(string name)
        {
            if (Has(name))
            {
                return _services[name];
            }

            Debug.Log("Can't find type " + name);
            return null;
        }

        public bool Has(String name)
        {
            if (_services.ContainsKey(name))
            {
                return true;
            }

            return false;
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
