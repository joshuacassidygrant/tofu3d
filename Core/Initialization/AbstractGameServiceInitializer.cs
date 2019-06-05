using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using UnityEngine;

namespace TofuCore.Service
{
    public abstract class AbstractGameServiceInitializer : MonoBehaviour
    {


        protected ServiceContext ServiceContext;

        void Awake()
        {
            ServiceContext = new ServiceContext();

            HandleUnityObjects();
            RegisterPayloads();
            BindServices();
            InitServices();

            IEventContext eventContext = ServiceContext.Fetch("EventContext");
            eventContext.TriggerEvent("GameServicesInitialized", null);
        }

        protected abstract void HandleUnityObjects();
        protected abstract void RegisterPayloads();
        protected abstract void BindServices();
        protected abstract void InitServices();

        //For testing purposes
        public ServiceContext GetServiceContext()
        {
            return ServiceContext;
        }

    }
}

