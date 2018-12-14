using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TUFFYCore.Service;
using UnityEngine;

namespace TUFFYTests
{

    public class ContentLibraryTests
    {
        private ServiceContext _serviceContext;
        private DummyLibrary _dummyLibrary;

        [SetUp]
        public void SetUp()
        {
            _serviceContext = new ServiceContext();
            _dummyLibrary = new DummyLibrary("test");
            _dummyLibrary.BindServiceContext(_serviceContext);

        }

        [Test]
        public void LibraryShouldLoadResourcesOnBuild()
        {
            Assert.NotNull(_dummyLibrary);
            Assert.AreEqual(3, _dummyLibrary.CountMembers());
        }

        [Test]
        public void LibraryShouldLoadCorrectResources()
        {
            Assert.True(_dummyLibrary.Contains("12"));
            Assert.True(_dummyLibrary.Contains("15"));
            Assert.True(_dummyLibrary.Contains("16"));
            Assert.False(_dummyLibrary.Contains("11"));

        }

        [Test]
        public void LibraryShouldDeliverRightResourceForKey()
        {

            Assert.AreEqual(12, _dummyLibrary.Get("12"));
            

        }

        [Test]
        public void LibraryShouldGiveDefaultWhenNoResourceFound()
        {

            Assert.AreEqual(0, _dummyLibrary.Get("144"));

        }


    }

}
