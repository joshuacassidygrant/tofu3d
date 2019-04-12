using System;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;

namespace TofuTest.Events
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

            _subPayloadTypeLibrary.ValidatePayload(Arg.Any<EventPayload>()).Returns(true);

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
            _eventContext.ContextBindEventListener("Event1", _subListener1);
            _eventContext.TriggerEvent("Event1", new EventPayload("null", null));
            _eventContext.TriggerEvent("Event1", new EventPayload("null", null));
            _eventContext.TriggerEvent("Event1", new EventPayload("null", null));

            //Assert
            _subListener1.Received(3).ReceiveEvent(_eventContext.GetEvent("Event1"), Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventListenerUnbinds()
        {
            _eventContext.ContextBindEventListener("Event1", _subListener1);
            _eventContext.TriggerEvent("Event1", new EventPayload("null", null));
            _eventContext.ContextRemoveEventListener("Event1", _subListener1);
            _eventContext.TriggerEvent("Event1", new EventPayload("null", null));
            _eventContext.TriggerEvent("Event1", new EventPayload("null", null));

            //Assert
            _subListener1.Received(1).ReceiveEvent(_eventContext.GetEvent("Event1"), Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventWithNoListenersCalls()
        {
            _eventContext.TriggerEvent("Ziff", new EventPayload("null", null));

            //Assert
            _subListener1.Received(0).ReceiveEvent(_eventContext.GetEvent("Ziff"), Arg.Any<EventPayload>());
            _subListener2.Received(0).ReceiveEvent(_eventContext.GetEvent("Ziff"), Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventWithNullIdFails()
        {
            try
            {
                _eventContext.TriggerEvent(null, new EventPayload("null", null));
                Assert.Fail();
            }
            catch (ArgumentNullException e)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestEventWithNullPayloadOkay()
        {
            _eventContext.TriggerEvent("Zarp", null);

            _subPayloadTypeLibrary.Received(1).ValidatePayload(Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventIllegalPayload()
        {
            _subPayloadTypeLibrary.ValidatePayload(Arg.Any<EventPayload>()).Returns(false);
            try
            {
                _eventContext.TriggerEvent("Faf", new EventPayload("null", null));
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.Pass();
            }
        }
    }
}
