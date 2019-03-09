using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Pathfinding.MapAdaptors;
using UnityEngine;

namespace TofuPlugin.Pathfinding.Test
{
    public class FakePathService: IPathableMapService
    {
        public FakePathTile[,] Tiles;

        public void SetMapTiles(FakePathTile[,] tiles)
        {
            Tiles = tiles;
        }

        public IPathableMapTile GetPathableMapTile(Vector3 worldPoint)
        {
            throw new System.NotImplementedException();
        }

        public IPathableMapTile[,] GetPathableMapTiles()
        {
            return Tiles;
        }

        public float TileSize { get; set; }
    }


}
