using NUnit.Compatibility;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TofuCore.Exceptions;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;

namespace TofuTests
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
                Debug.Log(e);
            }

        }

        [Test]
        public void ServiceShouldBindAllDependencies()
        {
            //Arrange
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);

            //Assert
            Assert.False((s.Fetch("DummyServiceOne")).CheckDependencies());

            //Act
            s.FullInitialization();

            //Assert
            Assert.False((s.Fetch("DummyServiceOne")).CheckDependencies());

            //Act
            new DummyLibrary("aaa").BindServiceContext(s);
            new DummyServiceOne().BindServiceContext(s, "AnotherDummy");
            s.ResolveBindings();

            //Assert
            Assert.True(((DummyServiceOne)s.Fetch("DummyServiceOne")).CheckDependencies());
        }
    }
}

