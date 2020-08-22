using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TofuConfig;
using TofuCore.ContentInjectable;
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
        public bool Initialized => _initialized;
        protected IServiceContext ServiceContext;
        protected ContentInjectablePayload ContentInjectables;
        private Dictionary<TofuEvent, List<Action<EventPayload>>> _boundListeners;
        private Dictionary<TofuEvent, Action<EventPayload>> _listenersToUnbind;
        [Dependency] protected EventContext _eventContext;

        public virtual RebindMode RebindMode => RebindMode.REBIND_REINITIALIZE;


        private bool _initialized = false;

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

            ContentInjectables = new ContentInjectablePayload();
            var contentInjectableFields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => (p.GetCustomAttributes(typeof(ContentInjectable.ContentInjectable), false)).Any());

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
         * Called BEFORE Initialize, but after resolve service bindings. Set up eventListeners and do other things that require other services before they are initialized
         */
        public virtual void Prepare()
        {
            //Do something!
        }

        /*
        * Called after Build and ResolveServiceBindings.
        */
        public virtual void Initialize()
        {
            if (Initialized) return;
            _initialized = true;
        }

        public dynamic BindServiceContext(IServiceContext serviceContext, string bindingName = null)
        {
            if (bindingName == null) bindingName = GetType().Name;
            ServiceContext = serviceContext;
            if (RebindMode == RebindMode.REBIND_IGNORE && serviceContext.Has(bindingName))
            {
                return ServiceContext.Fetch(bindingName);
            }

            if (RebindMode == RebindMode.REBIND_REINITIALIZE && serviceContext.Has(bindingName))
            {
                IService oldService = serviceContext.Fetch(bindingName);
                oldService.Cease();
            }

            try {
                serviceContext.Bind(bindingName, this, true);
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

        public void Cease() {
            if (_boundListeners != null)
            {
                foreach (TofuEvent eventKey in _boundListeners.Keys) {
                    foreach (Action<EventPayload> action in _boundListeners[eventKey]) {
                        UnbindListener(eventKey, action, _eventContext);
                    }
                }
            }
        }

        public void ReceiveEvent(TofuEvent evnt, EventPayload payload)
        {
            if (_boundListeners == null) return;

            if (!_boundListeners.ContainsKey(evnt))
            {
                Debug.Log("Expected event " + evnt + " but found no action bound");
            }

            _listenersToUnbind = new Dictionary<TofuEvent, Action<EventPayload>>();

            foreach (Action<EventPayload> action in _boundListeners[evnt])
            {
                action.Invoke(payload);
            }

            foreach (KeyValuePair<TofuEvent, Action<EventPayload>> kvp in _listenersToUnbind)
            {
                UnbindListener(kvp.Key, kvp.Value, _eventContext);
            }

            _listenersToUnbind = null;
        }

        public void BindListener(EventKey key, Action<EventPayload> action, IEventContext evntContext)
        {
            BindListener(evntContext.GetEvent(key), action, evntContext);
        }

        public void BindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext)
        {
            if (_boundListeners == null) _boundListeners = new Dictionary<TofuEvent, List<Action<EventPayload>>>();

            if (!_boundListeners.ContainsKey(evnt)) _boundListeners.Add(evnt, new List<Action<EventPayload>>());

            evntContext.ContextBindEventListener(evnt, this);
            _boundListeners[evnt].Add(action);
        }

        public void UnbindListener(TofuEvent evnt, Action<EventPayload> action, IEventContext evntContext)
        {
            evntContext.ContextRemoveEventListener(evnt, this);
            _boundListeners[evnt].Remove(action);
        }

        public void UnbindListenerDeferred(EventKey key, Action<EventPayload> action, IEventContext evntContext)
        {
            if (_listenersToUnbind != null)
            {
                _listenersToUnbind.Add(evntContext.GetEvent(key), action);
            }
            else
            {
                UnbindListener(evntContext.GetEvent(key), action, evntContext);
            }
        }
    }
}