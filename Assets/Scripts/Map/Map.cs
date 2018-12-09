using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map
{

    public int Length;
    public int Width;

    private MapTile[,] tiles;
    

    public Map(int length, int width)
    {
        Length = length;
        Width = width;
        tiles = new MapTile[length,width];
        
        //TEST -- randomizes tiles
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                bool passable = Random.Range(0, 1f) > 0.5f;
                MapTile tile = new MapTile(new Vector3Int(x, y, 0), passable);
                tiles[x, y] = tile;
            }
        }

    }

    public MapTile[,] GetTiles()
    {
        return tiles;
    }

    public MapTile GetTile(Vector3 location)
    {
        Vector3Int locInt = MathUtilities.RoundToVector3Int(location);
        return GetTile(locInt);
    }

    public MapTile GetTile(Vector3Int location)
    {
        if (location.x > tiles.GetLength(0) || location.x < 0) return null;
        if (location.y > tiles.GetLength(1) || location.y < 0) return null;

        return tiles[location.x, location.y];
    }


}
