using NUnit.Compatibility;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TUFFYCore.Exceptions;
using TUFFYCore.Service;
using TUFFYCore.TestSupport;

namespace TUFFYTests
{
    public class ServiceTests
    {

        [Test]
        public void ServiceShouldThrowExceptionOnDoubleInitialize()
        {
            DummyServiceOne dummy = new DummyServiceOne();

            Assert.False(dummy.GetInitialized());

            dummy.Initialize();

            Assert.True(dummy.GetInitialized());

            try
            {
                dummy.Initialize();
                Assert.Fail();
            } catch (MultipleInitializationException e)
            {
                //Pass!
            }

        }



        [Test]
        public void ServiceShouldBindAllDependencies()
        {
            //Arrange
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);

            //Assert
            Assert.False(((DummyServiceOne)s.Fetch("DummyServiceOne")).CheckDependencies());

            //Act
            s.FullInitialization();

            //Assert
            Assert.False(((DummyServiceOne)s.Fetch("DummyServiceOne")).CheckDependencies());

            //Act
            new DummyLibrary("aaa").BindServiceContext(s);
            new DummyServiceOne().BindServiceContext(s, "AnotherDummy");
            s.ResolveBindings();

            //Assert
            Assert.True(((DummyServiceOne)s.Fetch("DummyServiceOne")).CheckDependencies());
        }
    }
}

