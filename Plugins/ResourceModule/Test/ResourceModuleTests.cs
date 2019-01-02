using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Events;
using TofuPlugin.ResourceModule;
using UnityEngine;

public class ResourceModuleTests {

    private EventContext _eventContext;

    [SetUp]
    public void SetUp()
    {
        _eventContext = new EventContext();
        
    }

    [Test]
    public void TestResourceModuleShouldBeConstructed()
    {
        ResourceModule resourceModule = new ResourceModule("Test", 100f, 40f, _eventContext);

        Assert.AreEqual("Test", resourceModule.Name);
        Assert.AreEqual(100f, resourceModule.FMax);
        Assert.AreEqual(40f, resourceModule.FValue);
        Assert.AreEqual(100, resourceModule.IMax);
        Assert.AreEqual(40, resourceModule.IValue);
        Assert.AreEqual(0.4f, resourceModule.Percent);



    }

    [Test]
    public void TestResourcePercentReturns0WhenMaxIs0()
    {
        ResourceModule resourceModule = new ResourceModule("Test", 0, 40f, _eventContext);
        
        Assert.AreEqual(0, resourceModule.Percent);
    }

    [Test]
    public void TestEventSentWhenTriggeredOnDepletion()
    {
        //TODO: this
    }


}
