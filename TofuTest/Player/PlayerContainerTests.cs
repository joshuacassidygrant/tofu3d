using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Player;
using TofuCore.Service;
using UnityEngine;

namespace TofuTest.PlayerTests {

    public class PlayerContainerTests
    {
        private PlayerContainer _playerContainer;
        private IEventContext _subEventContext;
        private IServiceContext _subServiceContext;

        [SetUp]
        public void SetUp()
        {
            _subEventContext = Substitute.For<IEventContext>();
            _subEventContext.GetEvent(EventKey.FrameUpdate).Returns(new TofuEvent(EventKey.FrameUpdate));

            _subServiceContext = TestUtilities.BuildSubServiceContextWithServices(new Dictionary<string, object>
            {
                {"IEventContext", _subEventContext}
            });

            _playerContainer = new PlayerContainer();
            _playerContainer.Build();
            _playerContainer.BindServiceContext(_subServiceContext);
            _playerContainer.ResolveServiceBindings();
            _playerContainer.Initialize();
        }

        [Test]
        public void TestPlayerContainerConstructs()
        {
            Assert.NotNull(_playerContainer);
            Assert.True(_playerContainer.CheckDependencies());
        }

        [Test]
        public void TestPlayerContainerCreatesAndRegistersANewPlayer()
        {
            _subServiceContext.LastGlopId.Returns(5);
            Player player = _playerContainer.Create("TestName");
            
            Assert.AreEqual("TestName", player.Name);
            Assert.AreEqual(5, player.Id);
            Assert.AreEqual(false, player.Garbage);
        }

        [Test]
        public void TestPlayerContainerFailsToCreatePlayerWithNullName()
        {
            try
            {
                Player player = _playerContainer.Create(null);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Debug.Log(e);
                Assert.Pass();
            }

        }

        [Test]
        public void TestFindPlayerByName()
        {
            _subServiceContext.LastGlopId.Returns(5);
            Player player = _playerContainer.Create("TestName");

            Assert.NotNull(_playerContainer.GetPlayerByName("TestName"));
            Assert.Null(_playerContainer.GetPlayerByName("Test2"));
        }
    }

}
