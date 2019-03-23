using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Service;
using TofuCore.Tangible;
using UnityEngine;

namespace TofuTests
{
    public class ITargetableTests
    {
        private ServiceContext _context;
        private ITangible _t1;
        private ITangible _t2;
        private ITangible _t3;
        private ITangible _t4;
        private ITangible _t5;
        private ITangible _t6;
        private ITangible _t7;
        private ITangible _t8;
        private ITangible _t9;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();

            _t1 = new TangiblePosition(Vector3.zero, 1f);
            _t2 = new TangiblePosition(new Vector3(2f, 2f), 1.95f); //Closest to 0 by size radius
            _t3 = new TangiblePosition(new Vector3(-1f, -0.5f), 0.1f); //Closest by center point
            _t4 = new TangiblePosition(new Vector3(5f, 5f), 1f);
            _t5 = new TangiblePosition(new Vector3(10f, 10f), 2f);
            _t6 = new TangiblePosition(new Vector3(4f, 4f), 9f); //Far center, but overlaps 0
            _t7 = new TangiblePosition(new Vector3(3f, 3f), 4.25f); //Closer center and overlaps 0
            _t8 = new TangiblePosition(new Vector3(10f, 15f), 1.5f);
            _t9 = new TangiblePosition(new Vector3(3f, 3f), 4.22f); //Closer center but does not overlaps


        }

        [Test]
        public void TestReturnsClosestTargetableFromList()
        {
            List<ITangible> targets = new List<ITangible>() { _t3, _t4, _t5 };
            Assert.AreEqual(_t3, TangibleUtilities.GetClosest(_t1, targets));
        }

        [Test]
        public void TestReturnsClosestTargetableFromListTakingIntoAccountSize()
        {
            List<ITangible> targets = new List<ITangible>() { _t2, _t3, _t4, _t5 };
            Assert.AreEqual(_t2, TangibleUtilities.GetClosest(_t1, targets));
        }


        [Test]
        public void TestReturnTargetableInsideRadius()
        {
            List<ITangible> targets = new List<ITangible>() { _t2, _t3, _t4, _t5, _t6 };
            Assert.AreEqual(_t6, TangibleUtilities.GetClosest(_t1, targets));
        }


        [Test]
        public void TestReturnsClosestTargetableOfMultipleInsideRadius()
        {
            List<ITangible> targets = new List<ITangible>() { _t2, _t3, _t4, _t5, _t6, _t7, _t9 };
            Assert.AreEqual(_t7, TangibleUtilities.GetClosest(_t1, targets));
        }

        [Test]
        public void TestGetDistanceBetween()
        {
            Assert.AreEqual(1.5f, TangibleUtilities.GetDistanceBetween(_t5, _t8));
        }

        [Test]
        public void TestGetDistanceBetweenOverlap()
        {
            Assert.AreEqual(0f, TangibleUtilities.GetDistanceBetween(_t5, _t6));
        }

        [Test]
        public void TestGetDistanceBetweenInside()
        {
            Assert.AreEqual(0, TangibleUtilities.GetDistanceBetween(_t6, _t7));
        }
    }
}
