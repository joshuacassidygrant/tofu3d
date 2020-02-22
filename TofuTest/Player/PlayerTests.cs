using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Player;
using TofuCore.ResourceModule;
using UnityEngine;

namespace TofuTest.PlayerTests
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

    }
}
