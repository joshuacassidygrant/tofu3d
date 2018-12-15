using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TUFFYCore.Events;
using TUFFYCore.Exceptions;
using UnityEngine;

/*
 * see also: AbstractMonoService. Inherit from this to
 * register in service context and inject to other services.
 */
namespace TUFFYCore.Service
{
    public abstract class AbstractService : IService, IListener
    {
        protected ServiceContext ServiceContext;

        private Dictionary<Event, List<Action<EventPayload>>> _boundListeners;

        /*
     * Build sets up all internal workings of the class.
     */
        public virtual void Build()
        {
            //Do something!
        }


        /*
     * ResolveServiceBindings ensures that the class has access to all serviceContext it needs
     * To receive bindings, a service MUST have fields named as a private version of the 
     * dependency name (e.g. _serviceName for ServiceName)
     */
        public virtual void ResolveServiceBindings()
        {
            if (ServiceContext == null)
            {
                Debug.Log("Service context not bound in " + GetType().Name);
                return;
            }

            var dependencyFields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof(Dependency), false).Any());

            foreach (var fieldInfo in dependencyFields)
            {
                object[] atts = fieldInfo.GetCustomAttributes(typeof(Dependency), false);

                string name = ((Dependency)atts[0]).Name;
                if (name == null) name = fieldInfo.FieldType.Name;
                
                if (ServiceContext.Has(name))
                {
                    fieldInfo.SetValue(this, ServiceContext.Fetch(name));
                } else
                {
                    Debug.Log("Can't find service with name " + name + " to bind to " + GetType().Name);
                }

            }

        }

        /*
        * Called after Build and ResolveServiceBindings.
        */
        public virtual void Initialize()
        {
            //Do something!
        }

        public void BindServiceContext(ServiceContext serviceContext, string bindingName = null)
        {
            if (bindingName == null) bindingName = GetType().Name;
            ServiceContext = serviceContext;

            try
            {
                serviceContext.Bind(bindingName, this);
            }
            catch (ServiceDoubleBindException e)
            {
                Debug.Log("Error: Cannot bind two services to " + bindingName + " " + e);
            }
        }

        public string GetServiceName()
        {
            return GetType().ToString();
        }

        public bool CheckDependencies()
        {
            /*foreach (string dependency in Dependencies)
            {
                if (!ServiceContext.Has(dependency))
                {
                    return false;
                }
            }

            return true;*/
            return true;
        }

        private string toPrivateFieldName(string typeName)
        {
            string privateName = "_" + Char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
            return privateName;
        }

        public void ReceiveEvent(Event evnt, EventPayload payload)
        {
            if (_boundListeners == null) return;

            if (!_boundListeners.ContainsKey(evnt))
            {
                Debug.Log("Expected event " + evnt + " but found no action bound");
            }

            foreach (Action<EventPayload> action in _boundListeners[evnt])
            {
                action.Invoke(payload);
            }
        }

        public void BindListener(Event evnt, Action<EventPayload> action, EventContext evntContext)
        {
            if (_boundListeners == null) _boundListeners = new Dictionary<Event, List<Action<EventPayload>>>();

            if (!_boundListeners.ContainsKey(evnt)) _boundListeners.Add(evnt, new List<Action<EventPayload>>());

            evntContext.BindEventListener(evnt, this);
            _boundListeners[evnt].Add(action);
        }

        public void UnbindListener(Event evnt, Action<EventPayload> action, EventContext evntContext)
        {
            evntContext.RemoveEventListener(evnt, this);
            _boundListeners[evnt].Remove(action);
        }
    }
}