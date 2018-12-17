using NUnit.Framework;
using TUFFYCore.Events;
using TUFFYCore.Service;
using TUFFYCore.TestSupport;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;

namespace TUFFYTests
{
    public class EventsTests
    {

        private ServiceContext _serviceContext;
        private EventContext _eventContext;
        private EventTesterService _eventTesterService;

        [SetUp]
        public void SetUp()
        {
            _serviceContext = new ServiceContext();
        
            new EventTesterService().BindServiceContext(_serviceContext);
            new EventContext().BindServiceContext(_serviceContext);

            _eventTesterService = (EventTesterService)_serviceContext.Fetch("EventTesterService");
            _eventContext = (EventContext) _serviceContext.Fetch("EventContext");
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

            _eventContext.TriggerEvent(_eventContext.GetEvent("MollyCoddle"), new EventPayload(PayloadContentType.String, "test"));
            _eventContext.TriggerEvent(_eventContext.GetEvent("MollyCoddle"), new EventPayload(PayloadContentType.String, "test"));

            Assert.AreEqual(2, _eventTesterService.MollycoddleCalled);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);

            _eventContext.TriggerEvent(_eventContext.GetEvent("Zarf"), new EventPayload(PayloadContentType.String, "test"));

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
            _eventContext.TriggerEvent(_eventContext.GetEvent("MollyCoddle"), new EventPayload(PayloadContentType.Boolean, false));
            Assert.AreEqual(1, _eventTesterService.MollycoddleCalled);
            _eventTesterService.UnbindListener(_eventContext.GetEvent("MollyCoddle"), _eventTesterService.Mollycoddled, _eventContext);
            _eventContext.TriggerEvent(_eventContext.GetEvent("MollyCoddle"), new EventPayload(PayloadContentType.Boolean, false));
            _eventContext.TriggerEvent(_eventContext.GetEvent("Zarf"), new EventPayload(PayloadContentType.Boolean, false));
            Assert.AreEqual(1, _eventTesterService.MollycoddleCalled);
        }

        [Test]
        public void CorrectPayloadTypeSuccess()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);
            _eventContext.TriggerEvent(_eventContext.GetEvent("Zarf"), new EventPayload(PayloadContentType.String, "Test"));
            Assert.AreEqual(1, _eventTesterService.ZarfCalled);
            Assert.AreEqual("Test", _eventTesterService.LastZarfPayload);


        }

        [Test]
        public void IncorrectPayloadTypeFail()
        {
            _eventTesterService.BindListener(_eventContext.GetEvent("Zarf"), _eventTesterService.Zarfed, _eventContext);
            Assert.AreEqual(0, _eventTesterService.ZarfCalled);
            _eventContext.TriggerEvent(_eventContext.GetEvent("Zarf"), new EventPayload(PayloadContentType.Integer, 42));
            Assert.AreEqual(1, _eventTesterService.ZarfCalled);
            Assert.AreNotEqual("42", _eventTesterService.LastZarfPayload);
            Assert.Null(_eventTesterService.LastZarfPayload);


        }


    }
}
