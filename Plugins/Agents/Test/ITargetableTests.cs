using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using TofuCore.Service;
using TofuPlugin.Agents;
using TofuPlugin.Agents.Targetable;
using UnityEngine;

public class ITargetableTests
{
    private ServiceContext _context;
    private ITargetable _t1;
    private ITargetable _t2;
    private ITargetable _t3;
    private ITargetable _t4;
    private ITargetable _t5;
    private ITargetable _t6;
    private ITargetable _t7;
    private ITargetable _t8;
    private ITargetable _t9;

    [SetUp]
    public void SetUp()
    {
        _context =  new ServiceContext();

        _t1 = new Agent(1, null, Vector3.zero, _context, 1f);
        _t2 = new Agent(2, null, new Vector3(2f, 2f), _context, 1.95f); //Closest to 0 by size radius
        _t3 = new Agent(3, null, new Vector3(-1f, -0.5f), _context, 0.1f); //Closest by center point
        _t4 = new Agent(4, null, new Vector3(5f, 5f), _context, 1f);
        _t5 = new Agent(5, null, new Vector3(10f, 10f), _context, 2f);
        _t6 = new Agent(6, null, new Vector3(4f, 4f), _context, 9f); //Far center, but overlaps 0
        _t7 = new Agent(7, null, new Vector3(3f, 3f), _context, 4.25f); //Closer center and overlaps 0
        _t8 = new Agent(8, null, new Vector3(10f, 15f), _context, 1.5f);
        _t9 = new Agent(7, null, new Vector3(3f, 3f), _context, 4.22f); //Closer center but does not overlaps


    }

    [Test]
    public void TestReturnsClosestTargetableFromList()
    {
        List<ITargetable> targets = new List<ITargetable>() {_t3, _t4, _t5};
        Assert.AreEqual(_t3, TargetableUtilities.GetClosest(_t1, targets));
    }

    [Test]
    public void TestReturnsClosestTargetableFromListTakingIntoAccountSize()
    {
        List<ITargetable> targets = new List<ITargetable>() { _t2, _t3, _t4, _t5 };
        Assert.AreEqual(_t2, TargetableUtilities.GetClosest(_t1, targets));
    }


    [Test]
    public void TestReturnTargetableInsideRadius()
    {
        List<ITargetable> targets = new List<ITargetable>() { _t2, _t3, _t4, _t5, _t6 };
        Assert.AreEqual(_t6, TargetableUtilities.GetClosest(_t1, targets));
    }


    [Test]
    public void TestReturnsClosestTargetableOfMultipleInsideRadius()
    {
        List<ITargetable> targets = new List<ITargetable>() { _t2, _t3, _t4, _t5, _t6, _t7, _t9 };
        Assert.AreEqual(_t7, TargetableUtilities.GetClosest(_t1, targets));
    }

    [Test]
    public void TestGetDistanceBetween()
    {
        Assert.AreEqual(1.5f, TargetableUtilities.GetDistanceBetween(_t5, _t8));
    }

    [Test]
    public void TestGetDistanceBetweenOverlap()
    {
        Assert.AreEqual(0f, TargetableUtilities.GetDistanceBetween(_t5, _t6));
    }

    [Test]
    public void TestGetDistanceBetweenInside()
    {
        Assert.AreEqual(0, TargetableUtilities.GetDistanceBetween(_t6, _t7));
    }

    

}
