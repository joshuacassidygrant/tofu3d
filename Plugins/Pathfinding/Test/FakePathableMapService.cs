using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework.Constraints;
using TofuCore.Events;
using TofuCore.Service;
using TofuCore.Utility;
using TofuPlugin.Pathfinding.MapAdaptors;
using UnityEngine;

namespace TofuPlugin.Pathfinding.Test
{
    public class FakePathableMapService: AbstractService, IPathableMapService
    {
        [Dependency] private EventContext _eventContext;
        public FakePathTile[,] Tiles;
        public float TileSize => 1f;

        public void SetMapTiles(FakePathTile[,] tiles)
        {
            Tiles = tiles;
            _eventContext.TriggerEvent("MapLoaded", new EventPayload("Null", null, _eventContext));
        }

        public IPathableMapTile GetPathableMapTile(Vector3 worldPoint) {
            Vector3Int locInt = MathUtilities.RoundDownToVector3Int(worldPoint);

            return GetTile(locInt);
        }

        public IPathableMapTile GetTile(Vector3Int location)
        {
            if (location.x >= Tiles.GetLength(0) || location.x < 0) return null;
            if (location.y >= Tiles.GetLength(1) || location.y < 0) return null;
            return Tiles[location.x, location.y];
        }

        public IPathableMapTile[,] GetPathableMapTiles()
        {
            return Tiles;
        }

        public IPathableMapTile X()
        {
            return new FakePathTile(false);
        }

        public IPathableMapTile _()
        {
            return new FakePathTile(true);
        }

        public void SetLinearMap2x4()
        {
            SetMapTiles(LoadFromString(2, 4,
                 "XX_X"
                 +"XX_X"));

        }

        public void SetSimpleMap3x3()
        {
            SetMapTiles(LoadFromString(3, 3, 
              ""
             + "XX_"
             + "___"
             + "_XX"));
        }

        public FakePathTile[,] LoadFromString(int width, int height, string tileList)
        {
            Debug.Log("loading string " + tileList);
            FakePathTile[,] tiles = new FakePathTile[width, height];

            string text = tileList.Replace("\r\n", "");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = (height - y - 1) * width + x;
                    try
                    {
                        tiles[x, y] = ConstructFakePathTileFromToken(text[index]);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.Log("No tile found at " + x + "," + y);
                    }

                }
            }


            return tiles;
        }

        private FakePathTile ConstructFakePathTileFromToken(char token)
        {
            switch (token)
            {
                case 'X':
                    return new FakePathTile(false);
                case '_':
                    return new FakePathTile(true);
                default:
                    return new FakePathTile(false);
            }
        }

    }


}
