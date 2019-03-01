using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Pathfinding.MapAdaptors
{
    public interface IPathableMapTile
    {

        bool Passable { get; }
        float MovePenalty { get; }

    }
}

