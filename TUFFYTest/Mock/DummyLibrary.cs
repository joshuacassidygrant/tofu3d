using System.Collections;
using System.Collections.Generic;
using TUFFYCore.ResourceLibrary;
using UnityEngine;

public class DummyLibrary : AbstractResourceLibrary<int> {

    public DummyLibrary(string path) : base(path)
    {
    }

    public override void LoadResources()
    {
        _contents.Add("12", 12);
        _contents.Add("15", 15);
        _contents.Add("16", 16);
    }
}
