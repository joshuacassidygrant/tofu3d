using System.Collections;
using System.Collections.Generic;
using TUFFYCore.Events;
using TUFFYCore.Service;
using UnityEngine;

namespace TUFFYCore.TestSupport
{
    public class DummyServiceOne : AbstractService
    {
        [Dependency] private DummyLibrary _dummyLibrary;
        [Dependency("AnotherDummy")] private DummyServiceOne _anotherDummy;

        public bool Built = false;
        public bool Bound = false;

        public int DummyActionsCalled = 0;
        public float DummyActionsCapturedFloats = 0;



        public override void Build()
        {
            base.Build();
            Built = true;
        }

        public override void ResolveServiceBindings()
        {
            base.ResolveServiceBindings();
            Bound = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public bool AnotherDummyBound()
        {
            return _anotherDummy != null;
        }

        public bool GetInitialized()
        {
            return Initialized;
        }

        public void DummyEventAction(EventPayload payload)
        {
            DummyActionsCalled++;

            if (payload.ContentType == PayloadContentType.Float)
            {
                float contentFloat = ((float)payload.GetContent());
                DummyActionsCapturedFloats += contentFloat;
            }
        }
    }

}
