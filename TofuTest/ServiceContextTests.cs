using NUnit.Compatibility;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;

namespace TofuTests
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

        [Test]
        public void ServiceShouldDynamicallyCastType()
        {
            //Act
            ServiceContext s = new ServiceContext();
            new DummyServiceOne().BindServiceContext(s);
            DummyServiceOne dummy = s.Fetch("DummyServiceOne");

            //Assert
            Assert.NotNull(dummy);

            //Act
            s.Fetch("DummyServiceOne").Flarf();
            
            //Assert
            Assert.AreEqual(1, s.Fetch("DummyServiceOne").FlarfCount);

            try
            {
                s.Fetch("DummyServiceOne").UnwrittenFunction();

                //Assert
                Assert.Fail();
            } catch (Exception e)
            {
                Debug.Log("SUCCESS:");
                Debug.Log(e);
            }

        }

        [Test]
        public void TestContextShouldBeAbleToFindGlopInMultipleManagers()
        {
            /*FakeSubGlopManager fake1 = new FakeSubGlopManager();
            FakeSubGlopManager fake2 = new FakeSubGlopManager();

            fake1.BindServiceContext(_context);
            fake2.BindServiceContext(_context, "OtherOne");
            _context.FullInitialization();

            int start = _context.LastGlopId;

            for (int i = 0; i < 10; i++)
            {
                fake1.SpawnFakeGlop();
                fake2.SpawnFakeGlop();
                fake2.SpawnFakeGlop();
                fake1.SpawnFakeGlop();
            }

            Assert.NotNull(_context.FindGlopById(start));
            Assert.NotNull(_context.FindGlopById(start + 20));
            Assert.NotNull(_context.FindGlopById(start + 21));
            Assert.NotNull(_context.FindGlopById(start + 22));
            Assert.NotNull(_context.FindGlopById(start + 38));*/
        }


    }
}

