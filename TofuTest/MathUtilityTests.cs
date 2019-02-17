using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Utility;
using UnityEngine;

public class MathUtilityTests  {

    [Test]
    public void TestRoundToVectorInt()
    {
        Assert.AreEqual(new Vector3Int(2 ,3, 5), MathUtilities.RoundToVector3Int(new Vector3(2.1f, 2.9f, 5.4f)));
    }

    [Test]
    public void TestCastVector3IntToVector3()
    {
        Assert.AreEqual(new Vector3(4f, 5f, 2f), MathUtilities.Vector3IntToVector3(new Vector3Int(4, 5, 2)));

    }

}
