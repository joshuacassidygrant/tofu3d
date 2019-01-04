using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Assets.tofu3d.TofuTest.Mock;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Glop;
using TofuCore.Service;
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
            GlopManager glopManager = new GlopManager();
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
        }

    }
}

