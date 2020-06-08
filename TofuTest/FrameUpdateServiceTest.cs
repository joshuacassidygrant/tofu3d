using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using TofuConfig;
using TofuCore.Events;
using TofuCore.FrameUpdateServices;
using TofuCore.Service;
using UnityEngine;

namespace TofuTest
{
    public class FrameUpdateServiceTest
    {

        private FrameUpdateService _frameUpdateService;
        private IEventContext _subEventContext;
        private IServiceContext _subServiceContext;

        [SetUp]
        public void SetUp()
        {
            _subEventContext = Substitute.For<IEventContext>();
            _subServiceContext = TestUtilities.BuildSubServiceContextWithServices(new Dictionary<string, object>()
            {
                {"IEventContext", _subEventContext}
            });

            _subEventContext.When(x => x.TriggerEvent(Arg.Any<EventKey>(), Arg.Any<EventPayload>())).Do(x => { });
            _frameUpdateService = new GameObject().AddComponent<FrameUpdateService>();
            _frameUpdateService.BindServiceContext(_subServiceContext);
            _frameUpdateService.ResolveServiceBindings();
        }

        [Test]
        public void TestFrameUpdateServiceHasAllDependencies()
        {
            Assert.True(_frameUpdateService.CheckDependencies());
        }

        [Test]
        public void TestFrameUpdateForceUpdateTriggersEvent()
        {
            _subEventContext.Received(0).TriggerEvent(EventKey.FrameUpdate, Arg.Any<EventPayload>());
            _frameUpdateService.ForceUpdate(0.1f);
            _frameUpdateService.ForceUpdate(0.12f);
            _frameUpdateService.ForceUpdate(0.15f);
            _subEventContext.Received(3).TriggerEvent(EventKey.FrameUpdate, Arg.Any<EventPayload>());
        }

    }
}

