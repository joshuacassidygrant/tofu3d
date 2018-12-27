using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Service;
using TofuCore.TestSupport;
using UnityEngine;

namespace TofuTests
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
            Assert.True(_dummyLibrary.ContainsKey("12"));
            Assert.True(_dummyLibrary.ContainsKey("15"));
            Assert.True(_dummyLibrary.ContainsKey("16"));
            Assert.False(_dummyLibrary.ContainsKey("11"));

        }

        [Test]
        public void LibraryShouldDeliverRightResourceForKey()
        {

            Assert.AreEqual(12, _dummyLibrary.Get("12"));
            

        }

        [Test]
        public void LibraryShouldAddResourceWhenAsked()
        {
            Assert.AreEqual(0, _dummyLibrary.Get("13"));
            _dummyLibrary.LoadResource("13", 13);
            Assert.AreNotEqual(0, _dummyLibrary.Get("13"));
            
        }

        [Test]
        public void LibraryShouldGiveDefaultWhenNoResourceFound()
        {

            Assert.AreEqual(0, _dummyLibrary.Get("144"));

        }

        [Test]
        public void LibraryShouldRemoveResourceWhenAsked()
        {
            Assert.AreEqual(12, _dummyLibrary.Get("12"));
            Assert.True(_dummyLibrary.RemoveResource("12"));
            Assert.AreEqual(0, _dummyLibrary.Get("12"));
        }

        [Test]
        public void LibraryShouldReturnFalseWhenAskedToRemoveANonExistingMember()
        {
            Assert.False(_dummyLibrary.RemoveResource("Babylon"));
        }


    }

}
