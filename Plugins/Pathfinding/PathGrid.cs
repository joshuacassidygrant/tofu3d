using System;
using System.Collections.Generic;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.Pathfinding.MapAdaptors;
using UnityEngine;

namespace TofuPlugin.Pathfinding
{
    public class PathGrid : AbstractService
    {
#pragma warning disable 649
        [Dependency("IPathableMapService")] private IPathableMapService _pathableMapService;
#pragma warning restore 649

        public int PenaltyBlur =2; //Set in configurator
        public int NodesPerTileSide = 3; // Set in configurator

        public float NodeSizeRadius;
        public int MaxSize => _gridSizeX * _gridSizeY;

        public PathNode[,] Grid { get; private set; }
        public float NodeDiameter;
        private int _gridSizeX, _gridSizeY;
        public Vector2 GridWorldSize;
        public bool YZSwizzle = false;


        //Debugging:
        public float PenaltyMin;
        public float PenaltyMax;

        public override void Prepare()
        {
            BindListener(EventKey.MapLoaded, OnMapServiceLoaded, _eventContext);
        }

        void ConfigureParameters()
        {
            IPathableMapTile[,] terrainTiles = _pathableMapService.GetPathableMapTiles();
            NodeDiameter = _pathableMapService.TileSize / NodesPerTileSide;
            NodeSizeRadius = NodeDiameter / 2;
            _gridSizeX = terrainTiles.GetLength(0) * NodesPerTileSide;
            _gridSizeY = terrainTiles.GetLength(1) * NodesPerTileSide;
            GridWorldSize = new Vector2(_gridSizeX * _pathableMapService.TileSize, _gridSizeY * _pathableMapService.TileSize);
            YZSwizzle = _pathableMapService.YZSwizzle;
        }

        void CreatePathGrid()
        {
            Grid = new PathNode[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = Vector3.zero;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * NodeDiameter) + Vector3.right * (NodeSizeRadius) +
                                         Vector3.up * (y * NodeDiameter) + Vector3.up * (NodeSizeRadius);
                    IPathableMapTile tile = _pathableMapService.GetPathableMapTile(worldPoint);
                    if (tile != null)
                    {
                        Grid[x, y] = new PathNode(tile.Passable, worldPoint, x, y, tile.MovePenalty);
                    }
                }
            }

            BlurPenaltyMap(PenaltyBlur);
        }

        public PathNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x * NodesPerTileSide);
            int y;

            if (YZSwizzle)
            {
                y = Mathf.RoundToInt(worldPosition.z * NodesPerTileSide);
            }
            else
            {
                y = Mathf.RoundToInt(worldPosition.y * NodesPerTileSide);
            }

            return Grid[Mathf.Clamp(x, 0, _gridSizeX - 1), Mathf.Clamp(y, 0, _gridSizeY - 1)];
        }

        public List<PathNode> GetNeighbours(PathNode node)
        {
            List<PathNode> neighbours = new List<PathNode>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 & y == 0) continue;
                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;

                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    {
                        neighbours.Add(Grid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        public void BlurPenaltyMap(int blurSize)
        {
            if (blurSize == 0) return;

            int kernelSize = blurSize * 2 + 1;
            int kernelExtents = (kernelSize - 1) / 2;

            float[,] penaltiesHorizontalPass = new float[_gridSizeX, _gridSizeY];
            float[,] penaltiesVerticalPass = new float[_gridSizeX, _gridSizeY];

            for (int y = 0; y < _gridSizeY; y++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesHorizontalPass[0, y] += Grid[sampleX, y].MovementPenalty;
                }

                for (int x = 1; x < _gridSizeX; x++)
                {
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, _gridSizeX);
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, _gridSizeX -1);
                    penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] -
                                                    Grid[removeIndex, y].MovementPenalty +
                                                    Grid[addIndex, y].MovementPenalty;
                }
            }

            for (int x = 0; x < _gridSizeY; x++)
            {
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                    penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
                }

                float blurredPenalty = penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize);
                Grid[x, 0].MovementPenalty = blurredPenalty;

                for (int y = 1; y < _gridSizeY; y++)
                {
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, _gridSizeY);
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, _gridSizeY - 1);
                    penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] -
                                                    penaltiesHorizontalPass[x, removeIndex] +
                                                    penaltiesHorizontalPass[x, addIndex];
                    blurredPenalty = penaltiesVerticalPass[x, y] / (kernelSize * kernelSize);
                    Grid[x, y].MovementPenalty = blurredPenalty;
                    //UnityEngine.Debug.Log(blurredPenalty);
                    PenaltyMax = Mathf.Min(blurredPenalty, PenaltyMax);
                    PenaltyMin = Mathf.Max(blurredPenalty, PenaltyMin);
                }
            }
        }

        private void OnMapServiceLoaded(EventPayload payload)
        {
            ConfigureParameters();
            CreatePathGrid();
        }

    }
}
