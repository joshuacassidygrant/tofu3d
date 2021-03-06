﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Service;
using TofuCore.Utility.Heap;
using UnityEngine;

namespace TofuPlugin.Pathfinding
{
    /**
     * Adapted from Sebastian Lague.
     */
    public class Pathfinder : AbstractMonoService
    {

#pragma warning disable 649
        [Dependency] private PathGrid _grid;
#pragma warning restore 649

        public void FindPath(PathRequest request, Action<PathResult> callback)
        {

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            PathNode startNode = _grid.NodeFromWorldPoint(request.pathStart);
            PathNode targetNode = _grid.NodeFromWorldPoint(request.pathEnd);
            if (startNode.Walkable && targetNode.Walkable)
            {
                Heap<PathNode> openSet = new Heap<PathNode>(_grid.MaxSize);
                HashSet<PathNode> closedSet = new HashSet<PathNode>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    PathNode currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        /*sw.Stop();
                        UnityEngine.Debug.Log("Path found in " + sw.Elapsed);*/
                        pathSuccess = true;
                        break;
                    }

                    foreach (PathNode neighbour in _grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.Walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour =
                            currentNode.GCost +
                            Mathf.RoundToInt(GetDistance(currentNode, neighbour) * neighbour.MovementPenalty);
                        if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                        {
                            neighbour.GCost = newMovementCostToNeighbour;
                            neighbour.HCost = GetDistance(neighbour, targetNode);
                            neighbour.Parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }

                if (pathSuccess)
                {
                    waypoints = RetracePath(startNode, targetNode);
                    pathSuccess = waypoints.Length > 0;
                }

                callback(new PathResult(waypoints, pathSuccess, request.callback));

            }
        }

        Vector3[] RetracePath(PathNode startNode, PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();

            PathNode currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }


            //TODO: Get simplify working without having units end up getting caught on map corners
            //Vector3[] wayPoints = SimplifyPath(path);
            Vector3[] wayPoints = NotSimplifyPath(path);

            Array.Reverse(wayPoints);
            return wayPoints;

        }

        Vector3[] NotSimplifyPath(List<PathNode> path)
        {
            return path.Select(x => x.Position).ToArray();
        }

        Vector3[] SimplifyPath(List<PathNode> path)
        {
            if (path.Count < 1) return path.Select(x => x.Position).ToArray();
            List<Vector3> wayPoints = new List<Vector3>();
            wayPoints.Add(path[0].Position);
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i-1].GridX - path[i].GridX, path[i-1].GridY - path[i].GridY);
                if (directionNew != directionOld)
                {
                    wayPoints.Add(path[i].Position);
                }

                directionOld = directionNew;


            }
            
            return wayPoints.ToArray();
        }




        int GetDistance(PathNode n1, PathNode n2)
        {
            int distanceX = Mathf.Abs(n1.GridX - n2.GridX);
            int distanceY = Mathf.Abs(n1.GridY - n2.GridY);

            if (distanceX > distanceY)
            {
                return 14*distanceY + 10*(distanceX - distanceY);
            }
            return 14*distanceX + 10*(distanceY - distanceX);
        }


        void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _grid.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.Grid.GetLength(1); j++)
                {
                    Debug.Log(_grid.Grid[i, j].MovementPenalty / _grid.PenaltyMax);
                    Gizmos.color = _grid.Grid[i,j].Walkable ? Color.Lerp(Color.white, Color.red, _grid.Grid[i, j].MovementPenalty / _grid.PenaltyMax) : Color.black;
                    Gizmos.DrawCube(new Vector3(i /(float) _grid.NodesPerTileSide, 0, j / (float)_grid.NodesPerTileSide),
                        Vector3.one / (_grid.NodesPerTileSide + 1));
                }
            }
        }
    }

}

