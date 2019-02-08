using System;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;

namespace TofuTests
{
    public class EventsTests
    {

        private ServiceContext _serviceContext;
        private EventContext _eventContext;
        private EventTesterService _eventTesterService;
        private EventLogger _eventLogger;

        [SetUp]
        public void SetUp()
        {
            _serviceContext = new ServiceContext();

            _eventTesterService = new EventTesterService().BindServiceContext(_serviceContext);
            _eventLogger = new EventLogger().BindServiceContext(_serviceContext);

            _eventContext = _serviceContext.Fetch("EventContext");
            _serviceContext.FullInitialization();
        }

        [TearDown]
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

            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("String", "test", _eventContext));
            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("String", "test", _eventContext));

            Assert.AreEqual(2, _eventTesterService.MollycoddleCalled);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);

            _eventContext.TriggerEvent("Zarf", new EventPayload("String", "test", _eventContext));

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
            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("Boolean", false, _eventContext));
            Assert.AreEqual(1, _eventTesterService.MollycoddleCalled);
            _eventTesterService.UnbindListener(_eventContext.GetEvent("MollyCoddle"), _eventTesterService.Mollycoddled, _eventContext);
            _eventContext.TriggerEvent("MollyCoddle", new EventPayload("Boolean", false, _eventContext));
            _eventContext.TriggerEvent("Zarf", new EventPayload("Boolean", false, _eventContext));
            Assert.AreEqual(1, _eventTesterService.MollycoddleCalled);
        }

        [Test]
        public void CorrectPayloadTypeSuccess()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);
            _eventContext.TriggerEvent("Zarf", new EventPayload("String", "Test", _eventContext));
            Assert.AreEqual(1, _eventTesterService.ZarfCalled);
            Assert.AreEqual("Test", _eventTesterService.LastZarfPayload);


        }

        [Test]
        public void IncorrectPayloadTypeFail()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42, _eventContext));
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
                _eventContext.TriggerEvent("Flom", new EventPayload("AnUndeclaredType", "zoom", _eventContext));

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
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42, _eventContext));
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42, _eventContext));
            _eventContext.TriggerEvent("Zarf", new EventPayload("Integer", 42, _eventContext));

            Assert.AreEqual(3, _eventLogger.Logs.Count);
            Assert.AreEqual("Zarf", _eventLogger.Logs[0].Event);
            Assert.AreEqual("Integer", _eventLogger.Logs[0].PayloadType);
        }

    }
}
