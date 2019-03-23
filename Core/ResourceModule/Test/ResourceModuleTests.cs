using NUnit.Framework;
using TofuCore.Events;
using TofuCore.ResourceModule;
using TofuCore.Tangible;
using TofuCore.TestSupport;
using UnityEngine;

public class ResourceModuleTests {

    private EventContext _eventContext;

    [SetUp]
    public void SetUp()
    {
        _eventContext = new EventContext();
        _eventContext.Initialize();
        _eventContext.Build();
        _eventContext.GetPayloadTypeContainer().RegisterPayloadContentType("ResourceEventPayload", (x => x is ResourceEventPayload));


    }

    [Test]
    public void TestResourceModuleShouldBeConstructed()
    {
        ResourceModule resourceModule = new ResourceModule("Test", 100f, 40f, new DummyResourceModuleOwner(), _eventContext);

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
        ResourceModule resourceModule = new ResourceModule("Test", 0, 40f, new DummyResourceModuleOwner(), _eventContext);
        
        Assert.AreEqual(0, resourceModule.Percent);
    }

    [Test]
    public void TestEventShouldBeSentWhenTriggeredOnDepletion()
    {
        DummyServiceOne ds1 = new DummyServiceOne();
        
        ds1.BindListener(_eventContext.GetEvent("DummyCalled"), ds1.DummyEventAction, _eventContext);

        ResourceModule resourceModule = new ResourceModule("Test", 10f, 4f, new DummyResourceModuleOwner(), _eventContext);
        resourceModule.Deplete(1f, "DummyCalled", new EventPayload("String", "depleted", _eventContext));

        Assert.AreEqual(0, ds1.DummyActionsCalled);

        resourceModule.Deplete(9.1f, "DummyCalled", new EventPayload("String", "depleted", _eventContext));
        Assert.AreEqual(1, ds1.DummyActionsCalled);

    }

    [Test]
    public void TestSpendShouldFailWhenNotEnoughResource()
    {
        ResourceModule resourceModule = new ResourceModule("Buns", 100f, 100f, new DummyResourceModuleOwner(), _eventContext);

        bool succ = resourceModule.Spend(90f);
        Assert.True(succ);

        Assert.False(resourceModule.CanSpend(11f));
        Assert.True(resourceModule.CanSpend(10f));

        succ = resourceModule.Spend(10f);
        Assert.True(succ);
    }

    [Test]
    public void TestSetMaxShouldSetMaxButNotVal()
    {
        ResourceModule resourceModule = new ResourceModule("Zoms", 100f, 100f, new DummyResourceModuleOwner(), _eventContext);
        
        Assert.AreEqual(100, resourceModule.IMax);

        resourceModule.SetMax(150f);

        Assert.AreEqual(150, resourceModule.IMax);
        Assert.AreEqual(100, resourceModule.IValue);
    }

    [Test]
    public void TestSetMaxRetainPercentShouldSetMaxAndVal()
    {
        ResourceModule resourceModule = new ResourceModule("Zoms", 100f, 100f, new DummyResourceModuleOwner(), _eventContext);

        resourceModule.SetMaxRetainPercent(200f);
        
        Assert.AreEqual(200, resourceModule.IValue);
        Assert.AreEqual(200, resourceModule.IMax);
        Assert.AreEqual(1f, resourceModule.Percent);
    }

}
