using System.Runtime.Remoting.Messaging;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuTests
{
    public class FrameUpdateServiceTest
    {

        private TofuCore.FrameUpdateService.FrameUpdateService _frameUpdateService;
        private IEventContext _subEventContext;
        private IServiceContext _subServiceContext;

        [SetUp]
        public void SetUp()
        {

            _subServiceContext = Substitute.For<IServiceContext>();
            _subServiceContext.Fetch("EventContext").Returns(_subEventContext);
            _subEventContext = Substitute.For<IEventContext>();
            _subEventContext.When(x => x.TriggerEvent(Arg.Any<string>(), Arg.Any<EventPayload>())).Do(x => { });
            _subEventContext.CheckPayloadContentAs(Arg.Any<float>(), "Float").Returns(true);
            _frameUpdateService = new GameObject().AddComponent<TofuCore.FrameUpdateService.FrameUpdateService>();
            _frameUpdateService.BindServiceContext(_subServiceContext);


        }

        [Test]
        public void TestFrameUpdateForceUpdateTriggersEvent()
        {
            _frameUpdateService.ForceUpdate(0.1f);
            _frameUpdateService.ForceUpdate(0.12f);
            _frameUpdateService.ForceUpdate(0.15f);
            _subEventContext.Received(3).TriggerEvent("DummyCalled", Arg.Any<EventPayload>());
        }



    }
}

