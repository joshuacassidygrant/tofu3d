using TofuCore.Utility.Heap;
using UnityEngine;

namespace TofuPlugin.Pathfinding
{

    public class PathNode: IHeapable<PathNode>
    {
        public bool Walkable;
        public Vector3 Position;
        public int GridX;
        public int GridY;
        public PathNode Parent;
        public float MovementPenalty;
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public PathNode(bool walkable, Vector3 worldPos, int gridX, int gridY, float penalty)
        {
            Walkable = walkable;
            Position = worldPos;
            MovementPenalty = penalty;
            GridX = gridX;
            GridY = gridY;
        }

        public int CompareTo(PathNode other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }

            return -compare;
        }

        public int HeapIndex { get; set; }
    }
}
