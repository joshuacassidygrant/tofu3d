using NSubstitute;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.ResourceModule;

namespace TofuTest.ResourceModules
{
    public class ResourceModuleTests
    {

        private IEventContext _subEventContext;
        private IResourceModuleOwner _subResourceModuleOwner;

        [SetUp]
        public void SetUp()
        {
            _subEventContext = Substitute.For<IEventContext>();
            _subEventContext.GetEvent("DepleteEvent").Returns(new TofuEvent("DepleteEvent"));
            _subEventContext.GetEvent("DepleteEvent2").Returns(new TofuEvent("DepleteEvent2"));

            _subResourceModuleOwner = Substitute.For<IResourceModuleOwner>();
        }

        [Test]
        public void TestResourceModuleShouldBeConstructed()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 100f, 40f, "vitality", _subResourceModuleOwner, _subEventContext);

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
            ResourceModule resourceModule = new ResourceModule("Test", 0, 40f, "vitality", _subResourceModuleOwner, _subEventContext);
            Assert.AreEqual(0, resourceModule.Percent);
        }

        [Test]
        public void TestResourcePercentReturns0WhenMaxIsVeryCloseTo0()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 0.0001f, 40f, "vitality", _subResourceModuleOwner, _subEventContext);
            Assert.AreEqual(0, resourceModule.Percent);
        }

        [Test]
        public void TestResourcePercentReturnsHigherThan1WhenOverMax()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 100f,150f, "vitality", _subResourceModuleOwner, _subEventContext);
            Assert.AreEqual(1.5, resourceModule.Percent);
        }

        [Test]
        public void TestEventsShouldBeSentWhenTriggeredOnDepletion()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 10f, 4f, "vitality", _subResourceModuleOwner, _subEventContext);
            resourceModule.BindFullDepletionEvent("DepleteEvent", new EventPayload("null", null));
            resourceModule.SetChangeDeltaEventKey("ChangeEvent");
            resourceModule.SetStateChangeEventKey("StateChangeEvent");

            resourceModule.Deplete(1f, "DepleteEvent2", new EventPayload("String", "depleted"));

            _subEventContext.Received(1).TriggerEvent("ChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(1).TriggerEvent("StateChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(0).TriggerEvent("DepleteEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(0).TriggerEvent("DepleteEvent2", Arg.Any<EventPayload>());

            resourceModule.Deplete(9f, "DepleteEvent2", new EventPayload("String", "depleted"));

            _subEventContext.Received(2).TriggerEvent("ChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(2).TriggerEvent("StateChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(1).TriggerEvent("DepleteEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(1).TriggerEvent("DepleteEvent2", Arg.Any<EventPayload>());

            resourceModule.Deplete(1f, null, null);

            _subEventContext.Received(3).TriggerEvent("ChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(3).TriggerEvent("StateChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(2).TriggerEvent("DepleteEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(1).TriggerEvent("DepleteEvent2", Arg.Any<EventPayload>());
        }

        [Test]
        public void TestEventsShouldBeSentWhenTriggeredOnReplenishAndSpend()
        {
            ResourceModule resourceModule = new ResourceModule("Test", 10f, 4f, "vitality", _subResourceModuleOwner, _subEventContext);
            resourceModule.SetReplenishEventKey("ReplenishEvent");
            resourceModule.SetChangeDeltaEventKey("ChangeEvent");
            resourceModule.SetStateChangeEventKey("StateChangeEvent");

            resourceModule.Spend(2f);
            _subEventContext.Received(1).TriggerEvent("ChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(1).TriggerEvent("StateChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(0).TriggerEvent("ReplenishEvent", Arg.Any<EventPayload>());

            resourceModule.Replenish(4f);
            _subEventContext.Received(2).TriggerEvent("ChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(2).TriggerEvent("StateChangeEvent", Arg.Any<EventPayload>());
            _subEventContext.Received(1).TriggerEvent("ReplenishEvent", Arg.Any<EventPayload>());
            Assert.AreEqual(6, resourceModule.IValue);
            Assert.AreEqual(0.6f, resourceModule.Percent, 0.001f);
        }

        [Test]
        public void TestCanSpendFalse()
        {
            ResourceModule resourceModule =
                new ResourceModule("Buns", 100f, 100f, "vitality", _subResourceModuleOwner, _subEventContext);

            bool succ = resourceModule.Spend(90f);

            Assert.True(succ);
            Assert.False(resourceModule.CanSpend(11f));
            Assert.True(resourceModule.CanSpend(10f));
        }

        [Test]
        public void TestSpendShouldFailWhenNotEnoughResource() {
            ResourceModule resourceModule =
                new ResourceModule("Buns", 5f, 5f, "vitality", _subResourceModuleOwner, _subEventContext);

            Assert.False(resourceModule.Spend(10f));
        }

        [Test]
        public void TestSpendShouldSucceedWhenJustEnoughResource()
        {
            ResourceModule resourceModule =
                new ResourceModule("Buns", 5f, 5f, "vitality", _subResourceModuleOwner, _subEventContext);

            Assert.True(resourceModule.Spend(5f));
        }

        [Test]
        public void TestSetMaxShouldSetMaxButNotVal()
        {
            ResourceModule resourceModule = new ResourceModule("Zoms", 100f, 100f, "vitality", _subResourceModuleOwner, _subEventContext);

            Assert.AreEqual(100, resourceModule.IMax);

            resourceModule.SetMax(150f);

            Assert.AreEqual(150, resourceModule.IMax);
            Assert.AreEqual(100, resourceModule.IValue);
        }

        [Test]
        public void TestSetMaxRetainPercentShouldSetMaxAndVal()
        {
            ResourceModule resourceModule = new ResourceModule("Zoms", 100f, 100f, "vitality", _subResourceModuleOwner, _subEventContext);

            resourceModule.SetMaxRetainPercent(200f);

            Assert.AreEqual(200, resourceModule.IValue);
            Assert.AreEqual(200, resourceModule.IMax);
            Assert.AreEqual(1f, resourceModule.Percent);
        }

        [Test]
        public void TestReplenishShouldOnlyOverrunWhenTrue()
        {
            ResourceModule resourceModule = new ResourceModule("Glif", 100f, 99f, "vitality", _subResourceModuleOwner, _subEventContext);
            resourceModule.Replenish(2f);

            Assert.AreEqual(100f, resourceModule.FValue);

            resourceModule.Replenish(10f, true);
            
            Assert.AreEqual(110f, resourceModule.FValue);
        }

        [Test]
        public void TestSetValueShouldSetValue()
        {
            ResourceModule resourceModule = new ResourceModule("Blans", 100f, 100f, "vitality", _subResourceModuleOwner, _subEventContext);

            resourceModule.SetValue(1f);
            Assert.AreEqual(1f, resourceModule.FValue);
            Assert.AreEqual(1, resourceModule.IValue);
            Assert.AreEqual(0.01, resourceModule.Percent, 0.001);

            resourceModule.SetValue(-100f);
            Assert.AreEqual(-100f, resourceModule.FValue);
            Assert.AreEqual(-100, resourceModule.IValue);
            Assert.AreEqual(-1, resourceModule.Percent);

            resourceModule.SetValue(1500f);
            Assert.AreEqual(1500f, resourceModule.FValue);
            Assert.AreEqual(1500, resourceModule.IValue);
            Assert.AreEqual(15f, resourceModule.Percent);

            resourceModule.SetValue(0f);
            Assert.AreEqual(0f, resourceModule.FValue);
            Assert.AreEqual(0, resourceModule.IValue);
            Assert.AreEqual(0f, resourceModule.Percent);
            
            resourceModule.SetValue(1.4f);
            Assert.AreEqual(1.4f, resourceModule.FValue);
            Assert.AreEqual(1, resourceModule.IValue);
        }

    }

}