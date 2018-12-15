using System.Collections;
using System.Collections.Generic;
using TUFFYCore.Service;
using UnityEngine;

public class DummyServiceOne : AbstractService
{
    [Dependency] private DummyLibrary _dummyLibrary;
    [Dependency("AnotherDummy")] private DummyServiceOne _anotherDummy;

    public bool Built = false;
    public bool Bound = false;
    public bool Initialized = false;


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
        Initialized = true;
    }

    public bool AnotherDummyBound()
    {
        return _anotherDummy != null;
    }
}
