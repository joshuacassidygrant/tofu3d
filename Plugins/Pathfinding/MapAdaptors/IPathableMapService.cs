using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Pathfinding.MapAdaptors
{
    public interface IPathableMapService
    {

        IPathableMapTile GetPathableMapTile(Vector3 worldPoint);
        IPathableMapTile[,] GetPathableMapTiles();
        float TileSize { get; }


    }

}
