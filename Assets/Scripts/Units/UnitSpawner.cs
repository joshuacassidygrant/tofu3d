using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * TODO: this
 */
public class UnitSpawner : MonoBehaviour
{

    private Dictionary<string, Unit> _loadedUnits;
    public float SpawnFrequency = 1f;
    private float _sinceLastSpawn = 0f;
    private UnitManager _unitManager;
    private bool _spawning = false;
    private MapTile _tile;

    public void Init(MapTile tile, UnitManager unitManager)
    {
        _unitManager = unitManager;
        _tile = tile;
    }

    public void LoadUnits(Dictionary<string, Unit> units)
    {
        _loadedUnits = units;
    }

    public void StartSpawning()
    {
        _spawning = true;
        StartCoroutine("Spawning");
    }

    private IEnumerator Spawning()
    {
        while (_spawning)
        {
            /*_sinceLastSpawn += Time.deltaTime;
            if (_sinceLastSpawn >= SpawnFrequency)
            {
                _sinceLastSpawn = 0;
                _unitManager.SpawnUnit(new List<string>(_loadedUnits.Keys)[Random.Range(0, _loadedUnits.Count)], UnitType.Creature, _tile);
            }*/
            yield return null;
        }

    }

    

    

}
