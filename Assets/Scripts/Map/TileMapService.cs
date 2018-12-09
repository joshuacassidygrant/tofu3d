using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapService : AbstractService
{
    public new readonly string[] Dependencies = {
    };

    private Map _map;

    private MapRenderer _mapRenderer;

    public override void Build()
    {
        base.Build();

        LoadMap(new Map(25, 25));


    }

    public override void Initialize()
    {
        base.Initialize();
        Render();
    }

    public void Render()
    {
        //Soft dependency on renderer (for testing)
        MapRenderer renderer = (MapRenderer) ServiceContext.Fetch("MapRenderer");
        if (renderer)
        {
            renderer.Render(_map);
        }
    }


    public MapTile GetTile(Vector3 location)
    {
        return _map.GetTile(location);

    }

    public MapTile GetTile(Vector3Int location)
    {
        return _map.GetTile(location);
    }

    public void LoadMap(Map map)
    {
        _map = map;
    }

    public Map GetMap()
    {
        return _map;
    }
}
