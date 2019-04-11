﻿using System;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;

namespace TofuTests
{
    public class EventContextTests
    {
        // Object under test
        private EventContext _eventContext;

        private IServiceContext _subServiceContext;
        private IListener _subListener1;
        private IListener _subListener2;
        private IEventLogger _subEventLogger;
        private IEventPayloadTypeLibrary _subPayloadTypeLibrary;


        [SetUp]
        public void SetUp()
        {

            _eventContext = new EventContext();

            _subEventLogger = Substitute.For<IEventLogger>();
            _subPayloadTypeLibrary = Substitute.For<IEventPayloadTypeLibrary>();
            _subListener1 = Substitute.For<IListener>();
            _subListener2 = Substitute.For<IListener>();
            

            _subServiceContext = Substitute.For<IServiceContext>();
            _subServiceContext.Has("IEventLogger").Returns(true);
            _subServiceContext.Has("IEventPayloadTypeLibrary").Returns(true);
            _subServiceContext.Fetch("IEventLogger").Returns(_subEventLogger);
            _subServiceContext.Fetch("IEventPayloadTypeLibrary").Returns(_subPayloadTypeLibrary);

            _eventContext.BindServiceContext(_subServiceContext);
            _eventContext.Build();
            _eventContext.ResolveServiceBindings();
            _eventContext.Initialize();
        }

        [Test]
        public void TestEventContextHasAllDependenciesSatisfied()
        {
            Assert.True(_eventContext.CheckDependencies());   
        }

        [Test]
        public void TestEventAutoRegisters()
        {
            Assert.NotNull(_eventContext.GetEvent("Zarf"));
            Assert.NotNull(_eventContext.GetEvent("Bamg"));
        }

        [Test]
        public void TestNullEventIllegal()
        {
            try
            {
                Assert.NotNull(_eventContext.GetEvent(null));
                Assert.Fail();
            }
            catch (ArgumentNullException e)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestEventListenerBinds()
        {
            //TODO:
            _subListener1.BindListener("Event1", null, _eventContext); //Can we rid ourselves of this extra call to event context?
        }



        /*[TearDown]
        public void TearDown()
        {
            _serviceContext = null;
            _eventTesterService = null;
            _eventContext = null;
        }

        [Test]
        public void TestDependenciesShouldBeInPlace()
        {
            Assert.NotNull(_serviceContext);
            Assert.NotNull(_eventTesterService);
            Assert.NotNull(_eventContext);
        }

        [Test]
        public void CorrectEventTypeShouldBeCalledOnlyOnceEachCall()
        {

            _eventTesterService.BindListener(_eventContext.GetEvent("MollyCoddle"), _eventTesterService.Mollycoddled, _eventContext);
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);

            Assert.AreEqual(0, _eventTesterService.MollycoddleCalled);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);

            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("String", "test"));
            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("String", "test"));

            Assert.AreEqual(2, _eventTesterService.MollycoddleCalled);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);

            _eventContext.TriggerEvent("Zarf", new EventPayload("String", "test"));

            Assert.AreEqual(2, _eventTesterService.MollycoddleCalled);
            Assert.AreEqual(1, _eventTesterService.ZarfCalled);
            Assert.AreEqual(2, _eventContext.GetEvent("MollyCoddle").CallCount);
            Assert.AreEqual(1, _eventContext.GetEvent("Zarf").CallCount);

        }

        [Test]
        public void EventUnboundShouldNotBeCalled()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("MollyCoddle"), _eventTesterService.Mollycoddled, _eventContext);
            Assert.AreEqual(0, _eventTesterService.MollycoddleCalled);
            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("Boolean", false));
            Assert.AreEqual(1, _eventTesterService.MollycoddleCalled);
            _eventTesterService.UnbindListener(_eventContext.GetEvent("MollyCoddle"), _eventTesterService.Mollycoddled, _eventContext);
            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("Boolean", false));
            _eventContext.TriggerEvent("Zarf", new EventPayload("Boolean", false));
            Assert.AreEqual(1, _eventTesterService.MollycoddleCalled);
        }

        [Test]
        public void CorrectPayloadTypeSuccess()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);
            _eventContext.TriggerEvent("Zarf", new EventPayload("String", "Test"));
            Assert.AreEqual(1, _eventTesterService.ZarfCalled);
            Assert.AreEqual("Test", _eventTesterService.LastZarfPayload);


        }

        [Test]
        public void IncorrectPayloadTypeFail()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42));
            Assert.AreEqual(1, _eventTesterService.ZarfCalled);
            Assert.AreNotEqual("42", _eventTesterService.LastZarfPayload);
            Assert.Null(_eventTesterService.LastZarfPayload);


        }

        [Test]
        public void UnregisteredPayloadTypeShouldWarnButNotFail()
        {
            try
            {
                _eventTesterService.BindListener(_eventContext.GetEvent("Flom"), _eventTesterService.Zarfed, _eventContext);
                _eventContext.TriggerEvent("Flom", new EventPayload("AnUndeclaredType", "zoom"));

            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void EventLoggerServiceShouldCollectTriggeredEvents()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42));
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42));
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42));

            Assert.AreEqual(3, _subEventLogger.Logs.Count);
            Assert.AreEqual("Zarf", _subEventLogger.Logs[0].Event);
            Assert.AreEqual("Integer", _subEventLogger.Logs[0].PayloadType);
        }
        */
    }
}
