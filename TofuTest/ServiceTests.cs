using NUnit.Compatibility;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TofuCore.Exceptions;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;

namespace TofuTest
{
    public class ServiceTests
    {


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
            DummyLibrary dl1 = new DummyLibrary("aaa").BindServiceContext(s);
            DummyServiceOne ds1 = new DummyServiceOne().BindServiceContext(s, "AnotherDummy");
            s.FullInitialization(dl1);
            s.FullInitialization(ds1);

            //Assert
            Assert.True(((DummyServiceOne)s.Fetch("DummyServiceOne")).CheckDependencies());
        }

        [Test]
        public void ServiceShouldBindBaseClasses()
        {
            ServiceContext s = new ServiceContext();
            DummyServiceSub sub = new DummyServiceSub();
            sub.BindServiceContext(s);

            new DummyLibrary("t").BindServiceContext(s, "DummyLibrary");
            new DummyLibrary("t2").BindServiceContext(s, "DummyLibrary2");

            s.FullInitialization();

            Assert.True(sub.HasDummyLibrary2());
            Assert.True(sub.HasDummyLibrary());
           
        }
    }
}

