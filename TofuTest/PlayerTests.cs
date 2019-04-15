using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Player;
using TofuCore.ResourceModule;
using UnityEngine;

namespace TofuTests
{
    public class PlayerTests
    {
        private Player _player1;

        [SetUp]
        public void SetUp()
        {
            _player1 = new Player("TestName");
        }

        [Test]
        public void TestPlayerConstructsWithName()
        {
            Assert.NotNull(_player1);
            Assert.AreEqual("TestName", _player1.Name);
            Assert.AreEqual(_player1.Position, Vector3.zero);
        }

        [Test]
        public void TestSetPlayerName()
        {
            _player1.Name = "TestName2";

            Assert.AreEqual("TestName2", _player1.Name);
        }

        [Test]
        public void TestAssignResourceModule()
        {
            IResourceModule subModule = Substitute.For<IResourceModule>();
            _player1.AssignResourceModule("Module1", subModule);

            Assert.NotNull(_player1.GetResourceModule("Module1"));
            Assert.AreEqual(1, _player1.GetResourceModules().Count);
            Assert.True(_player1.GetResourceModules().ContainsKey("Module1"));
        }

        [Test]
        public void TestRemoveResourceModule()
        {
            IResourceModule subModule = Substitute.For<IResourceModule>();
            _player1.AssignResourceModule("Module1", subModule);

            Assert.NotNull(_player1.GetResourceModule("Module1"));

            _player1.RemoveResourceModule("Module1");

            Assert.AreEqual(0, _player1.GetResourceModules().Count);
            Assert.Null(_player1.GetResourceModule("Module1"));
        }

    }
}
