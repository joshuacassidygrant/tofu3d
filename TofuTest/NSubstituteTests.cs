/**
 * Tests for NSubstitute functionality and integration.
 */

using NSubstitute;
using NUnit.Framework;
using TofuCore.Service;

namespace TofuTests
{
    public class NSubstituteTests
    {

        [Test]
        public void TestNSubstituteFakesService()
        {
            IService sub = Substitute.For<IService>();
            sub.GetServiceName().Returns("Substitute");

            Assert.AreEqual("Substitute", sub.GetServiceName());
        }
    }
}
