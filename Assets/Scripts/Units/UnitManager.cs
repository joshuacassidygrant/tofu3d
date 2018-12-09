using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Manages units on screen.
 */
public class UnitManager : AbstractService
{

    private InstantiatingService _instantiatingService;
    private TileMapService _tileMapService;

    private int num;

    public override string[] Dependencies
    {
        get
        {
            return new string[]
            {
                "InstantiatingService",
                "TileMapService"
            };
        }
    }

    private Dictionary<string, Unit> _unitsLibrary;
    private Dictionary<string, Unit> _towersLibrary;
    private Dictionary<int, Unit> _activeUnits;
    private List<UnitSpawner> _unitSpawners;

    public override void Build()
    {
        base.Build();
        LoadUnits();


        _activeUnits = new Dictionary<int, Unit>();
        _unitSpawners = new List<UnitSpawner>();
    }

    public override void Initialize()
    {
        CreateSpawner(_unitsLibrary, _tileMapService.GetTile(new Vector3Int(4, 5, 0)));
        CreateSpawner(_unitsLibrary, _tileMapService.GetTile(new Vector3Int(7, 5, 0)));

    }

    private void LoadUnits()
    {
        /*_unitsLibrary = Resources.LoadAll<Unit>("Creatures").ToDictionary(u => u.Name, u => u);
        _towersLibrary = Resources.LoadAll<Unit>("Towers").ToDictionary(u => u.Name, u => u);*/
    }

    public int CountUnitsInLibrary()
    {
        return _unitsLibrary.Count;
    }

    public int CountActiveUnitsOnScreen()
    {
        return _activeUnits.Count;
    }

    public void CreateSpawner(Dictionary<string, Unit> units, MapTile tile) { 
        if (_instantiatingService != null) {
            UnitSpawner spawner = new GameObject().AddComponent<UnitSpawner>();
            spawner.name = "Spawner";
            spawner.transform.position = tile.Location;
            _unitSpawners.Add(spawner);
            spawner.Init(tile, this);
            spawner.LoadUnits(_unitsLibrary);
            spawner.StartSpawning();
        }
    }

    public Unit SpawnUnit(string unitId, UnitType unitType, MapTile tile)
    {
        Dictionary<string, Unit> pickDictionary;

        /*switch (unitType)
        {
            case UnitType.Creature:
                pickDictionary = _unitsLibrary;
                break;
            case UnitType.Tower:
                pickDictionary = _towersLibrary;
                break;
            default:
                return null;

        }
        if (!pickDictionary.ContainsKey(unitId))
        {
            Debug.Log("No unit of type " + unitType + " in unit library");
        }

        Unit toSpawn = pickDictionary[unitId];*/

        int id = Random.Range(0, 1000000);
        Unit unit = new Unit(id);
        _activeUnits.Add(id, unit);
        return unit;

        //TODO: make this an event!
        /*ServiceContext.TriggerEvent(Events.SpawnUnit, new EventPayload(PayloadContentType.GameObject, ));
        if (_instantiatingService != null)
        {
            GameObject spawned = _instantiatingService.Instantiate(toSpawn.gameObject, tile.Location);
            Unit unit = spawned.GetComponent<Unit>(); //TODO: factor out unit from monobehaviour unitrenderer gameobject to test.
            _activeUnits.Add(id, unit);
            return unit;
        }


        return null;*/
    }


}