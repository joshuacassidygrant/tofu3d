using System;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.Pathfinding.MapAdaptors;
using UnityEngine;

namespace TofuPlugin.Pathfinding
{
    public class PathGrid : AbstractService
    {
        [Dependency("IPathableMapService")] private IPathableMapService _pathableMapService;
        [Dependency] private EventContext _eventContext;

        public int PenaltyBlur = 3; //Set in configurator
        public int NodesPerTileSide = 4; // Set in configurator

        public float NodeSizeRadius;
        public int MaxSize => _gridSizeX * _gridSizeY;

        private PathNode[,] _grid;
        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;
        private Vector2 _gridWorldSize;


        //Debugging:
        public bool DisplayGridGizmos = true;
        private float _penaltyMin;
        private float _penaltyMax;

        public override void Prepare()
        {
            BindListener(_eventContext.GetEvent("MapLoaded"), OnMapServiceLoaded, _eventContext);
        }

        void ConfigureParameters()
        {
            IPathableMapTile[,] terrainTiles = _pathableMapService.GetPathableMapTiles();
            _nodeDiameter = _pathableMapService.TileSize / NodesPerTileSide;
            NodeSizeRadius = _nodeDiameter / 2;
            _gridSizeX = terrainTiles.GetLength(0) * NodesPerTileSide;
            _gridSizeY = terrainTiles.GetLength(1) * NodesPerTileSide;
            _gridWorldSize = new Vector2(_gridSizeX * _pathableMapService.TileSize, _gridSizeY * _pathableMapService.TileSize);
        }

        void CreatePathGrid()
        {
            _grid = new PathNode[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = Vector3.zero;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter) + Vector3.right * (NodeSizeRadius) +
                                         Vector3.up * (y * _nodeDiameter) + Vector3.up * (NodeSizeRadius);
                    IPathableMapTile tile = _pathableMapService.GetPathableMapTile(worldPoint);
                    if (tile != null)
                    {
                        _grid[x, y] = new PathNode(tile.Passable, worldPoint, x, y, tile.MovePenalty);
                    }
                }
            }

            BlurPenaltyMap(PenaltyBlur);
        }

        public PathNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x * NodesPerTileSide);
            int y = Mathf.RoundToInt(worldPosition.y * NodesPerTileSide);


            return _grid[Mathf.Clamp(x, 0, _gridSizeX - 1), Mathf.Clamp(y, 0, _gridSizeY - 1)];
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
                        neighbours.Add(_grid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        public void BlurPenaltyMap(int blurSize)
        {
            int kernelSize = blurSize * 2 + 1;
            int kernelExtents = (kernelSize - 1) / 2;

            float[,] penaltiesHorizontalPass = new float[_gridSizeX, _gridSizeY];
            float[,] penaltiesVerticalPass = new float[_gridSizeX, _gridSizeY];

            for (int y = 0; y < _gridSizeY; y++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesHorizontalPass[0, y] += _grid[sampleX, y].MovementPenalty;
                }

                for (int x = 1; x < _gridSizeX; x++)
                {
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, _gridSizeX);
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, _gridSizeX -1);
                    penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] -
                                                    _grid[removeIndex, y].MovementPenalty +
                                                    _grid[addIndex, y].MovementPenalty;
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
                _grid[x, 0].MovementPenalty = blurredPenalty;

                for (int y = 1; y < _gridSizeY; y++)
                {
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, _gridSizeY);
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, _gridSizeY - 1);
                    penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] -
                                                    penaltiesHorizontalPass[x, removeIndex] +
                                                    penaltiesHorizontalPass[x, addIndex];
                    blurredPenalty = penaltiesVerticalPass[x, y] / (kernelSize * kernelSize);
                    _grid[x, y].MovementPenalty = blurredPenalty;
                    //UnityEngine.Debug.Log(blurredPenalty);
                    _penaltyMax = Mathf.Min(blurredPenalty, _penaltyMax);
                    _penaltyMin = Mathf.Max(blurredPenalty, _penaltyMin);
                }
            }
        }

        private void OnMapServiceLoaded(EventPayload payload)
        {
            ConfigureParameters();
            CreatePathGrid();
        }

        /* Dead code. Move to debugger gameobject if necessary*/
        /*
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(_gridWorldSize.x/2, _gridWorldSize.y/2, 0));
            if (_grid != null && DisplayGridGizmos)
            {
                foreach (PathNode node in _grid)
                {
                    if (DisplayGridGizmos) {
                        Gizmos.color = Color.Lerp(Color.black, Color.white,
                            Mathf.InverseLerp(_penaltyMin, _penaltyMax, node.MovementPenalty));
                            //Gizmos.color = node.Walkable ? Gizmos.color : Color.red;
                            if (node.Walkable) Gizmos.DrawCube(node.Position, Vector3.one * (_nodeDiameter - 0.1f));
                        
                    }
                }
            }
        }*/
    }
}
