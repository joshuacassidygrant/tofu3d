using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Glops;
using UnityEngine;

namespace TofuTest
{
    public class GlopTests
    {

        [Test]
        public void TestSpawnGlop()
        {
            Glop glop = Substitute.For<Glop>();

            Assert.False(glop.Garbage);
            Assert.AreEqual(0, glop.Id);
        }

        [Test]
        public void TestGarbageGlop()
        {
            Glop glop = Substitute.For<Glop>();
            glop.When(x => x.Die()).CallBase();
            glop.Die();

            Assert.True(glop.Garbage);
        }

    }

}
