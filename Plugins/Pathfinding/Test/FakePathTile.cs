using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Pathfinding.MapAdaptors;
using UnityEngine;

namespace TofuPlugin.Pathfinding.Test
{
    public class FakePathTile : IPathableMapTile
    {

        public bool Passable { get; }
        public float MovePenalty { get; }

        public FakePathTile(bool passable, float movePenalty = 0)
        {
            Passable = passable;
            MovePenalty = movePenalty;
        }
    }
}

