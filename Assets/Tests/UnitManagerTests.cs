using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitManagerTests
{
    private ServiceContext _serviceContext;
    private ServiceFactory _serviceFactory;
    private UnitManager _unitManager;
    private UnitPrototypeLibrary _unitPrototypeLibrary;

    [SetUp]
    public void SetUp()
    {
        _serviceContext = new ServiceContext();

        _unitManager = new UnitManager();
        _unitPrototypeLibrary = new UnitPrototypeLibrary();
        _unitPrototypeLibrary.SetPrefix("Test");

        _unitManager.BindServiceContext(_serviceContext);
        new TileMapService().BindServiceContext(_serviceContext);
        _unitPrototypeLibrary.BindServiceContext(_serviceContext);

        _serviceContext.FullInitialization();
    }

    [Test]
    public void TestUnitManagerInit() 
    {
        Assert.NotNull(_serviceContext.Fetch("UnitManager"));
        int resourceUnitCount = _unitPrototypeLibrary.CountMembers(UnitType.Creature);
        Assert.NotNull(_unitPrototypeLibrary.GetUnit(UnitType.Creature, "test"));
        Assert.Null(_unitPrototypeLibrary.GetUnit(UnitType.Creature, "Adzuki"));
        Assert.True(resourceUnitCount == 2);
    }

    [Test]
    public void TestUnitManagerSpawnUnit()
    {
        MapTile tile = new MapTile(Vector3Int.down, true);
        _unitManager.SpawnUnit("Test", UnitType.Creature, tile);
        _unitManager.SpawnUnit("Test", UnitType.Creature, tile);
        _unitManager.SpawnUnit("Test", UnitType.Creature, tile);

        Assert.AreEqual(3, _unitManager.CountActiveUnitsOnScreen());
    }
}
