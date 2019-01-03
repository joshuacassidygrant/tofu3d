using System;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.TestSupport
{
    public class DummyServiceOne : AbstractService
    {
        [Dependency] protected DummyLibrary DummyLibrary;
        [Dependency("AnotherDummy")] protected DummyServiceOne AnotherDummy;
    
        public bool Built = false;
        public bool Bound = false;

        public int DummyActionsCalled = 0;
        public float DummyActionsCapturedFloats = 0;
        public int FlarfCount = 0;



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
            return AnotherDummy != null;
        }

        public bool GetInitialized()
        {
            return Initialized;
        }

        public void Flarf()
        {
            FlarfCount++;
        }

        public void DummyEventAction(EventPayload payload)
        {
            DummyActionsCalled++;

            if (payload.ContentType == "Float")
            {
                float contentFloat = ((float)payload.GetContent());
                DummyActionsCapturedFloats += contentFloat;
            }
        }


        public bool HasDummyLibrary() {
            return DummyLibrary != null;
        }
    }

}
