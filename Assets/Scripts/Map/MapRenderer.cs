using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRenderer : AbstractMonoService
{
    private Tilemap _tileMap;
    private Map _map;
    public Tile Passable;
    public Tile NonPassable;

    public void Render(Map map)
    {
        _map = map;
        Passable = Resources.Load<Tile>("Tiles/passable");
        NonPassable = Resources.Load<Tile>("Tiles/impassable");

        _tileMap = GetComponentInChildren<Tilemap>();

        _tileMap.ClearAllTiles();
        MapTile[,] tiles = _map.GetTiles();


        for (int x = 0; x < _map.Length; x++)
        {
            for (int y = 0; y < _map.Width; y++)
            {
                if (tiles[x, y].Passable)
                {
                    _tileMap.SetTile(new Vector3Int(x, y, 0), Passable);
                }
                else
                {
                    _tileMap.SetTile(new Vector3Int(x, y, 0), NonPassable);
                }
            }
        }
    }
}
