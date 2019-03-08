using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Service
{
    public abstract class AbstractGameServiceInitializer : MonoBehaviour
    {

        private GameObject _cameraObject;

        protected ServiceContext ServiceContext;

        void Awake()
        {
            ServiceContext = new ServiceContext();

            HandleUnityObjects();
            RegisterPayloads();
            BindServices();
            InitServices();
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

