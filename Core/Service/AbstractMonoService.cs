using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TofuCore.Events;
using TofuCore.Exceptions;
using UnityEngine;

/*
 * Should be identical to AbstractService class but inherit from
 * monobehaviour. Inherit from this to have monobehaviour derived
 * services.
 */
namespace TofuCore.Service
{
    public abstract class AbstractMonoService : MonoBehaviour, IService, IListener, IContentInjectable
    {
        protected bool Initialized = false;
        protected ServiceContext ServiceContext;
        protected Dictionary<string, IContentInjectable> ContentInjectables;
        private Dictionary<TofuEvent, List<Action<EventPayload>>> _boundListeners;


        /*
        * Build sets up all internal workings of the class. Since it's called on GameServiceInitializer's
        * Awake() method, it should happen before Start()!
        */
        public virtual void Build()
        {
            //
        }

        /*
        * ResolveServiceBindings ensures that the class has access to all serviceContext it needs.
        * To set a binding, declare it with a [Dependency] attribute on a protected field. If a
        * service is to be bound to a specialized string, use [Dependency("SpecialName")]
        */
        public virtual void ResolveServiceBindings()
        {
            //TODO: t stuff

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

                string name = ((Dependency) atts[0]).Name;
                if (name == null) name = fieldInfo.FieldType.Name;

                if (ServiceContext.Has(name))
                {
                    fieldInfo.SetValue(this, ServiceContext.Fetch(name));
                }
                else
                {
                    Debug.Log("Can't find service with name " + name + " to bind to " + GetType().Name);
                }
            }

            ContentInjectables = new Dictionary<string, IContentInjectable>();
            var contentInjectableFields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => (p.GetCustomAttributes(typeof(ContentInjectable), false)).Any());

            foreach (var fieldInfo in contentInjectableFields)
            {
                string name = fieldInfo.FieldType.Name;

                if (fieldInfo.GetValue(this) == null || !(fieldInfo.GetValue(this) is IContentInjectable))
                {
                    Debug.Log("Trying to set " + name + " as a ContentInjectable in " + this.ToString() + " but it is null or doesn't implement IContentInjectable.");
                }

                ContentInjectables.Add(fieldInfo.FieldType.Name, fieldInfo.GetValue(this) as IContentInjectable);
            }
        }


        /*
     * Called after Build and ResolveServiceBindings.
     */
        public virtual void Initialize()
        {
            if (Initialized)
            {
                Debug.Log("Trying to initialize " + GetType().ToString() + " multiple times!");
                throw new MultipleInitializationException();
            }

            Initialized = true;
        }

        public dynamic BindServiceContext(ServiceContext serviceContext, string bindingName = null)
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
                return this;
            }

            return this;
        }

        public string GetServiceName()
        {
            return GetType().ToString();
        }

        public bool CheckDependencies()
        {
            var dependencyFields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof(Dependency), false).Any());

            foreach (var fieldInfo in dependencyFields)
            {
                if (fieldInfo.GetValue(this) == null) return false;
            }

            return true;
        }

        private string toPrivateFieldName(string typeName)
        {
            return "_" + Char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
        }

        public void ReceiveEvent(TofuEvent evnt, EventPayload payload)
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

        public void BindListener(TofuEvent evnt, Action<EventPayload> action, EventContext evntContext)
        {
            if (_boundListeners == null) _boundListeners = new Dictionary<TofuEvent, List<Action<EventPayload>>>();

            if (!_boundListeners.ContainsKey(evnt)) _boundListeners.Add(evnt, new List<Action<EventPayload>>());

            evntContext.HelperBindEventListener(evnt, this);
            _boundListeners[evnt].Add(action);
        }

        public void UnbindListener(TofuEvent evnt, Action<EventPayload> action, EventContext evntContext)
        {
            evntContext.RemoveEventListener(evnt, this);
            _boundListeners[evnt].Remove(action);
        }
    }
}