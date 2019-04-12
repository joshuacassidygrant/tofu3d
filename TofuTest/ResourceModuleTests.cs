using NSubstitute;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.ResourceModule;

namespace TofuTest.ResourceModules
{
    public class ResourceModuleTests
    {

        private IEventContext _subEventContext;

        [SetUp]
        public void SetUp()
        {
            _subEventContext = Substitute.For<IEventContext>();
            _subEventContext.GetEvent("DummyCalled").Returns(new TofuEvent("DummyCalled"));
        }

        [Test]
        public void TestResourceModuleShouldBeConstructed()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 100f, 40f, new DummyResourceModuleOwner(), _subEventContext);

            Assert.AreEqual("Test", resourceModule.Name);
            Assert.AreEqual(100f, resourceModule.FMax);
            Assert.AreEqual(40f, resourceModule.FValue);
            Assert.AreEqual(100, resourceModule.IMax);
            Assert.AreEqual(40, resourceModule.IValue);
            Assert.AreEqual(0.4f, resourceModule.Percent);
        }

        [Test]
        public void TestResourcePercentReturns0WhenMaxIs0()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 0, 40f, new DummyResourceModuleOwner(), _subEventContext);
            Assert.AreEqual(0, resourceModule.Percent);
        }

        [Test]
        public void TestEventShouldBeSentWhenTriggeredOnDepletion()
        {
            IListener subListener = Substitute.For<IListener>();
            subListener.BindListener("DummyCalled", null, _subEventContext);
            ResourceModule resourceModule = new ResourceModule("Test", 10f, 4f, new DummyResourceModuleOwner(), _subEventContext);

            resourceModule.Deplete(1f, "DummyCalled", new EventPayload("String", "depleted"));

            _subEventContext.Received(0).TriggerEvent("DummyCalled", Arg.Any<EventPayload>());

            resourceModule.Deplete(9.1f, "DummyCalled", new EventPayload("String", "depleted"));

            _subEventContext.Received(1).TriggerEvent("DummyCalled", Arg.Any<EventPayload>());
        }

        [Test]
        public void TestSpendShouldFailWhenNotEnoughResource()
        {
            ResourceModule resourceModule = new ResourceModule("Buns", 100f, 100f, new DummyResourceModuleOwner(), _subEventContext);

            bool succ = resourceModule.Spend(90f);

            Assert.True(succ);
            Assert.False(resourceModule.CanSpend(11f));
            Assert.True(resourceModule.CanSpend(10f));

            succ = resourceModule.Spend(10f);

            Assert.True(succ);
        }

        [Test]
        public void TestSetMaxShouldSetMaxButNotVal()
        {
            ResourceModule resourceModule = new ResourceModule("Zoms", 100f, 100f, new DummyResourceModuleOwner(), _subEventContext);

            Assert.AreEqual(100, resourceModule.IMax);

            resourceModule.SetMax(150f);

            Assert.AreEqual(150, resourceModule.IMax);
            Assert.AreEqual(100, resourceModule.IValue);
        }

        [Test]
        public void TestSetMaxRetainPercentShouldSetMaxAndVal()
        {
            ResourceModule resourceModule = new ResourceModule("Zoms", 100f, 100f, new DummyResourceModuleOwner(), _subEventContext);

            resourceModule.SetMaxRetainPercent(200f);

            Assert.AreEqual(200, resourceModule.IValue);
            Assert.AreEqual(200, resourceModule.IMax);
            Assert.AreEqual(1f, resourceModule.Percent);
        }

    }

}