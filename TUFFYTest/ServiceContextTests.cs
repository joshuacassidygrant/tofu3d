using NUnit.Compatibility;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TUFFYCore.Service;
using TUFFYCore.TestSupport;

namespace TUFFYTests
{
    public class ServiceContextTests {

        [Test]
        public void ServiceShouldBindToClassName()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);
            var ds1 = (DummyServiceOne) s.Fetch("DummyServiceOne");
            var nullService = s.Fetch("DummyServiceNull");

            //Assert
            Assert.NotNull(ds1);
            Assert.Null(nullService);
        }

        [Test]
        public void ServiceShouldUnbindWhenDropped()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);

            //Asset
            Assert.NotNull(s.Fetch("DummyServiceOne"));

            //Act
            s.Drop("DummyServiceOne");

            //Assert
            Assert.Null(s.Fetch("DummyServiceOne"));
        }

        [Test]
        public void ServiceShouldBindByStringWhenProvided()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s, "TestBinding");

            //Assert
            Assert.Null(s.Fetch("DummyServiceOne"));
            Assert.NotNull(s.Fetch("TestBinding"));
        }

        [Test]
        public void ServiceShouldBeBuiltBoundAndInitialized()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s, "AnotherDummy");
            new DummyServiceOne().BindServiceContext(s);
            DummyServiceOne ds1 = (DummyServiceOne)s.Fetch("DummyServiceOne");
            DummyServiceOne ds2 = (DummyServiceOne)s.Fetch("AnotherDummy");

            //Assert
            Assert.True(ds1.Built);
            Assert.False(ds1.GetInitialized());
            Assert.False(ds1.Bound);
            Assert.False(ds1.AnotherDummyBound());
            Assert.True(ds2.Built);
            Assert.False(ds2.GetInitialized());
            Assert.False(ds2.Bound);


            //Act
            s.FullInitialization();

            //Assert
            Assert.True(ds1.Bound);
            Assert.True(ds1.Built);
            Assert.True(ds1.GetInitialized());
            Assert.True(ds1.AnotherDummyBound());

        }

        [Test]
        public void ServiceContextShouldNotOverwriteBinding()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);
            new DummyServiceOne().BindServiceContext(s);

            //Will fail if exception not caught
            Assert.True(true);

        }

        [Test]
        public void NewServiceContextShouldBindNewDummyService()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);
            s.FullInitialization();

            //Assert
            Assert.True(((DummyServiceOne)s.Fetch("DummyServiceOne")).Bound);

            //Act
            s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);
       
            //Assert
            Assert.False(((DummyServiceOne)s.Fetch("DummyServiceOne")).Bound);

        }


    }
}

