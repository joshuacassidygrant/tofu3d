using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;

namespace TofuTest.Events
{
    public class EventLoggerTests
    {
        private EventLogger _eventLogger;
        private EventContext _eventContext;
        private IServiceContext _subServiceContext;
        private IListener _subListener;

        [SetUp]
        public void SetUp()
        {
            _eventLogger = new EventLogger();
            _eventContext = new EventContext();

            _subServiceContext = TestUtilities.BuildSubServiceContextWithServices(new Dictionary<string, object>
            {
                {"IEventLogger", _eventLogger},
                {"IEventContext", _eventContext }
            });

            _eventContext.Build();
            _eventContext.BindServiceContext(_subServiceContext, "IEventContext");
            _eventContext.Initialize();
            _eventContext.ResolveServiceBindings();

            _subListener = Substitute.For<IListener>();

            _eventContext.ContextBindEventListener("Test", _subListener);
            _eventContext.ContextBindEventListener("Zep", _subListener);


        }

        [Test]
        public void TestEventLoggerLogsSimpleEvent()
        {
            _eventContext.TriggerEvent("Test", new EventPayload("null", null));

            Assert.AreEqual(1, _eventLogger.Logs.Count);
            Assert.AreEqual("Test", _eventLogger.Logs[0].Event);
            Assert.AreEqual("null", _eventLogger.Logs[0].PayloadType);

        }

        [Test]
        public void TestEventLoggerCountsSeveralEvents()
        {
            _eventContext.TriggerEvent("Test", new EventPayload("null", null));
            _eventContext.TriggerEvent("Test", new EventPayload("null", null));
            _eventContext.TriggerEvent("Zep", new EventPayload("norf", null));
            _eventContext.TriggerEvent("Zep", new EventPayload("norf", null));
            _eventContext.TriggerEvent("Zep", new EventPayload("null", null));

            Assert.AreEqual(2, _eventLogger.EventsCalledToPayloadTypesCounts["Test"]["null"]);
            Assert.AreEqual(2, _eventLogger.EventsCalledToPayloadTypesCounts["Zep"]["norf"]);
            Assert.AreEqual(1, _eventLogger.EventsCalledToPayloadTypesCounts["Zep"]["null"]);
            Assert.AreEqual(5, _eventLogger.Logs.Count);
        }
    }

}
