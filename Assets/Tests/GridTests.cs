using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GridTests {


    private ServiceContext _serviceContext;

    [SetUp]
    public void SetUp()
    {
        _serviceContext = new ServiceContext();


    }

    [TearDown]
    public void TearDown()
    {


    }

    [Test]
    public void TestCreateNewMap()
    {
        const int length = 40;
        const int width = 50;
        Map map = new Map(length, width);
        Assert.AreEqual(length, map.Length);
        Assert.AreEqual(width, map.Width);
    }
}
