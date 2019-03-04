﻿using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;

namespace TofuTests
{
    public class FrameUpdateServiceTest
    {

        ServiceContext _serviceContext;
        TofuCore.FrameUpdateService.FrameUpdateService _frameUpdateService;
        EventContext _eventContext;
        DummyServiceOne _dummyService;

        [SetUp]
        public void SetUp()
        {
            _serviceContext = new ServiceContext();
            _eventContext = _serviceContext.Fetch("EventContext");
            _eventContext.BindServiceContext(_serviceContext);
            _frameUpdateService = new GameObject().AddComponent<TofuCore.FrameUpdateService.FrameUpdateService>();
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
}
