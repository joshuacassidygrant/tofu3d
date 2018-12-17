using NUnit.Framework;
using TUFFYCore.Events;
using TUFFYCore.Service;
using TUFFYPlugins.FrameUpdateService;
using UnityEngine;

public class FrameUpdateServiceTest {

    ServiceContext _serviceContext;
    FrameUpdateService _frameUpdateService;
    EventContext _eventContext;
    DummyServiceOne _dummyService;

    [SetUp]
    public void SetUp()
    {
        _serviceContext = new ServiceContext();
        _eventContext = new EventContext();
        _eventContext.BindServiceContext(_serviceContext);
        _frameUpdateService = new GameObject().AddComponent<FrameUpdateService>();
        _frameUpdateService.BindServiceContext(_serviceContext);
        _dummyService = new DummyServiceOne();
        _dummyService.BindServiceContext(_serviceContext);
        _serviceContext.FullInitialization();
    }

    [Test]
    public void TestFrameUpdateServiceShouldHaveBoundDependencies()
    {
        Assert.True(_frameUpdateService.CheckDependencies());
    }

    [Test]
    public void TestFrameUpdateForceUpdateTriggersEvent()
    {
        _dummyService.BindListener(_eventContext.GetEvent("FrameUpdate"), _dummyService.DummyEventAction, _eventContext);
        _frameUpdateService.ForceUpdate(0.1f);
        _frameUpdateService.ForceUpdate(0.12f);
        _frameUpdateService.ForceUpdate(0.15f);
        Assert.AreEqual(3, _dummyService.DummyActionsCalled);
        Assert.AreEqual(0.37f, _dummyService.DummyActionsCapturedFloats);
    }



}
