using NSubstitute;
using NUnit.Framework;
using TofuCore.ResourceHolder;

namespace TofuTest.ResourceHolderTests
{
    public class ResourceHolderTests
    {

        private IResourceHolderOwner _subResourceModuleOwner;

        [SetUp]
        public void SetUp()
        {
            _subResourceModuleOwner = Substitute.For<IResourceHolderOwner>();
        }

        [Test]
        public void TestResourceModuleShouldBeConstructed()
        {
            ResourceHolder rh = new ResourceHolder("Test", 100, 40);

            Assert.AreEqual("Test", rh.Name);
            Assert.AreEqual(100, rh.Max);
            Assert.AreEqual(40, rh.Value);
            Assert.AreEqual(0.4f, rh.Percent, 0.01f);
            Assert.AreEqual(null, rh.Owner);
        }

        [Test]
        public void TestResourcePercentReturns0WhenMaxIs0()
        {
            ResourceHolder rh = new ResourceHolder("Test", 0, 40);
            Assert.AreEqual(0, rh.Percent);
        }


        [Test]
        public void TestResourcePercentReturnsHigherThan1WhenOverMax()
        {
            ResourceHolder rh = new ResourceHolder("Test", 100,150);
            Assert.AreEqual(1.5, rh.Percent);
        }

        [Test]
        public void TestCanSpendFalse()
        {
            ResourceHolder rh =
                new ResourceHolder("Buns", 100, 100);

            int val = rh.Deplete(90);

            Assert.AreEqual(val, 10);
            Assert.False(rh.CanSpend(11));
            Assert.True(rh.CanSpend(10));
        }

        [Test]
        public void TestSpendShouldFailWhenNotEnoughResourceAndNotForced() {
            ResourceHolder rh =
                new ResourceHolder("Buns", 5, 5);

            Assert.AreEqual(rh.Deplete(10), 5);
        }

        [Test]
        public void TestSpendShouldDebtWhenNotEnoughResourceAndForced() {
            ResourceHolder rh =
                new ResourceHolder("Buns", 5, 5);

            Assert.AreEqual(rh.Deplete(10, true), -5);
        }

        [Test]
        public void TestSpendShouldSucceedWhenJustEnoughResource()
        {
            ResourceHolder rh =
                new ResourceHolder("Buns", 5, 5);

            Assert.AreEqual(rh.Deplete(5), 0);
        }

        [Test]
        public void TestSetMaxShouldSetMaxButNotVal()
        {
            ResourceHolder rh = new ResourceHolder("Zoms", 100, 100);

            Assert.AreEqual(100, rh.Max);

            rh.Max = 150;

            Assert.AreEqual(150, rh.Max);
            Assert.AreEqual(100, rh.Value);
            Assert.AreEqual(0.66f, rh.Percent, 0.01f);
        }

        [Test]
        public void TestSetMaxRetainPercentShouldSetMaxAndVal()
        {
            ResourceHolder rh = new ResourceHolder("Zoms", 100, 100);

            rh.SetMaxRetainPercent(200);

            Assert.AreEqual(200, rh.Value);
            Assert.AreEqual(200, rh.Max);
            Assert.AreEqual(1f, rh.Percent);
        }

        [Test]
        public void TestReplenishShouldOnlyOverrunWhenTrue()
        {
            ResourceHolder rh = new ResourceHolder("Glif", 100, 99);
            rh.Replenish(2);

            Assert.AreEqual(100, rh.Value);

            rh.Replenish(10, true);
            
            Assert.AreEqual(110, rh.Value);
        }

        [Test]
        public void TestCanBindOwner()
        {
            ResourceHolder rh = new ResourceHolder("Zoms", 100, 100);
            rh.Owner = _subResourceModuleOwner;
        }

    }

}