using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ServicesTest {

    [Test]
    public void ServicesTestSimplePasses()
    {
        ServiceContext s = new ServiceContext();
        s.Bind("UnitManager", new UnitManager());
        var unitManager = (UnitManager) s.Fetch("UnitManager");
        var nullService = s.Fetch("DummyService");
        Assert.NotNull(unitManager);
        Assert.Null(nullService);
    }

}

