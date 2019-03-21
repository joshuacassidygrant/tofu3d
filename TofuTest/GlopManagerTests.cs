using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Glops;
using TofuCore.Service;
using TofuTests.Mock;
using UnityEngine;

namespace TofuTests
{
    public class GlopManagerTests
    {
        private ServiceContext _context;
        private EventContext _eventContext;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();
            _eventContext = new EventContext();
            _eventContext.BindServiceContext(_context);
        }


        [Test]
        public void TestGlopManagerShouldInitialize()
        {
            GlopContainer<Glop> glopManager = new GlopContainer<Glop>();
            glopManager.BindServiceContext(_context);
            _context.FullInitialization();

            Assert.AreEqual(0, glopManager.GetContents().Count);
        }

        [Test]
        public void TestGlopManagerShouldGiveCorrectId()
        {
            FakeSubGlopManager fake1 = new FakeSubGlopManager();
            FakeSubGlopManager fake2 = new FakeSubGlopManager();

            fake1.BindServiceContext(_context);
            fake2.BindServiceContext(_context);
            
            Assert.AreEqual(1, fake1.UseNextId());
            Assert.AreEqual(2, fake2.UseNextId());
            Assert.AreEqual(3, fake1.UseNextId());
            Assert.AreEqual(4, fake1.UseNextId());
            Assert.AreEqual(5, fake1.UseNextId());
            Assert.AreEqual(6, fake1.UseNextId());
            Assert.AreEqual(7, fake1.UseNextId());
            Assert.AreEqual(8, fake1.UseNextId());
            Assert.AreEqual(9, fake1.UseNextId());
            Assert.AreEqual(0xA, fake1.UseNextId());
            Assert.AreEqual(0xB, fake1.UseNextId());

        }

        [Test]
        public void TestContextShouldBeAbleToFindGlopInMultipleManagers()
        {
            FakeSubGlopManager fake1 = new FakeSubGlopManager();
            FakeSubGlopManager fake2 = new FakeSubGlopManager();

            fake1.BindServiceContext(_context);
            fake2.BindServiceContext(_context, "OtherOne");

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
            Assert.NotNull(_context.FindGlopById(start + 38));
        }

    }
}

