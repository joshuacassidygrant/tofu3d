using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuConfig;
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

            _eventContext.ContextBindEventListener(EventKey.Test, _subListener);
            _eventContext.ContextBindEventListener(EventKey.Test2, _subListener);


        }

        [Test]
        public void TestEventLoggerLogsSimpleEvent()
        {
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));

            Assert.AreEqual(1, _eventLogger.Logs.Count);
            Assert.AreEqual(EventKey.Test, _eventLogger.Logs[0].EventKey);
            Assert.AreEqual("null", _eventLogger.Logs[0].PayloadType);

        }

        [Test]
        public void TestEventLoggerCountsSeveralEvents()
        {
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
            _eventContext.TriggerEvent(EventKey.Test2, new EventPayload("norf", null));
            _eventContext.TriggerEvent(EventKey.Test2, new EventPayload("norf", null));
            _eventContext.TriggerEvent(EventKey.Test2, new EventPayload("null", null));

            Assert.AreEqual(2, _eventLogger.EventsCalledToPayloadTypesCounts[EventKey.Test]["null"]);
            Assert.AreEqual(2, _eventLogger.EventsCalledToPayloadTypesCounts[EventKey.Test2]["norf"]);
            Assert.AreEqual(1, _eventLogger.EventsCalledToPayloadTypesCounts[EventKey.Test2]["null"]);
            Assert.AreEqual(5, _eventLogger.Logs.Count);
        }
    }

}
