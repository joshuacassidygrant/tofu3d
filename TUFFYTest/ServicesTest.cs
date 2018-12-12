using NUnit.Framework;
using TUFFYCore.Service;

namespace Tests
{
    public class ServicesTest {

        [Test]
        public void ServicesTestSimplePasses()
        {
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);
            var ds1 = (DummyServiceOne) s.Fetch("DummyServiceOne");
            var nullService = s.Fetch("DummyServiceNull");
            Assert.NotNull(ds1);
            Assert.Null(nullService);
        }

        [Test]
        public void BindLibraryWithName()
        {
            ServiceContext s = new ServiceContext();
        }

    }
}

