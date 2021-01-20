using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

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

            _subServiceContext = TestUtilities.BuildSubServiceContextWithServices(new Dictionary<string, object>
            {
                {"IEventLogger", _subEventLogger},
                {"IEventPayloadTypeLibrary", _subPayloadTypeLibrary}
            });

            _subPayloadTypeLibrary.ValidatePayload(Arg.Any<EventPayload>()).Returns(true);

            _eventContext.BindServiceContext(_subServiceContext);
            _eventContext.Build();
            _eventContext.ResolveServiceBindings();
            _eventContext.Initialize();
        }

        [Test]
        public void TestEventAutoRegisters()
        {
            Assert.NotNull(_eventContext.GetEvent(EventKey.Test));
            Assert.NotNull(_eventContext.GetEvent(EventKey.Test2));
        }


        [Test]
        public void TestEventListenerBinds()
        {
            _eventContext.ContextBindEventListener(EventKey.Test, _subListener1);
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));

            //Assert
            _subListener1.Received(3).ReceiveEvent(_eventContext.GetEvent(EventKey.Test), Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventListenerUnbinds()
        {
            _eventContext.ContextBindEventListener(EventKey.Test, _subListener1);
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
            _eventContext.ContextRemoveEventListener(EventKey.Test, _subListener1);
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));

            //Assert
            _subListener1.Received(1).ReceiveEvent(_eventContext.GetEvent(EventKey.Test), Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventWithNoListenersCalls()
        {
            _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));

            //Assert
            _subListener1.Received(0).ReceiveEvent(_eventContext.GetEvent(EventKey.Test), Arg.Any<EventPayload>());
            _subListener2.Received(0).ReceiveEvent(_eventContext.GetEvent(EventKey.Test), Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventWithNullPayloadOkay()
        {
            _eventContext.TriggerEvent(EventKey.Test, null);

            _subPayloadTypeLibrary.Received(1).ValidatePayload(Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventIllegalPayload()
        {
            _subPayloadTypeLibrary.ValidatePayload(Arg.Any<EventPayload>()).Returns(false);
            try
            {
                _eventContext.TriggerEvent(EventKey.Test, new EventPayload("null", null));
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Debug.Log(e);
                Assert.Pass();
            }
        }
    }
}
