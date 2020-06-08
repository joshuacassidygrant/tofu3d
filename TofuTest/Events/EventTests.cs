using NUnit.Framework;
using TofuConfig;
using TofuCore.Events;

namespace TofuTest.Events
{
    public class EventTests
    {
        [Test]
        public void TestEventConstructor()
        {
            TofuEvent evnt = new TofuEvent(EventKey.Test);

            Assert.AreEqual(EventKey.Test, evnt.Key);
        }

        [Test]
        public void TestEventKeepsCallCount()
        {
            TofuEvent evnt = new TofuEvent(EventKey.Test);

            Assert.AreEqual(0, evnt.CallCount);

            evnt.HasBeenCalled();

            Assert.AreEqual(1, evnt.CallCount);
        }

        [Test]
        public void TestTwoEventsAreEqualWithEqualNames()
        {
            TofuEvent evnt1 = new TofuEvent(EventKey.Test);
            TofuEvent evnt2 = new TofuEvent(EventKey.Test);
            TofuEvent evnt3 = new TofuEvent(EventKey.Test2);
            evnt2.HasBeenCalled();

            Assert.AreEqual(evnt1, evnt2);
            Assert.AreNotEqual(evnt3, evnt1);
        }

        [Test]
        public void TestTwoEventsEqualHashWithEqualNames()
        {
            TofuEvent evnt1 = new TofuEvent(EventKey.Test);
            TofuEvent evnt2 = new TofuEvent(EventKey.Test);
            TofuEvent evnt3 = new TofuEvent(EventKey.Test2);
            evnt2.HasBeenCalled();

            Assert.AreEqual(evnt1.GetHashCode(), evnt2.GetHashCode());
            Assert.AreNotEqual(evnt3.GetHashCode(), evnt1.GetHashCode());
        }
    }

}