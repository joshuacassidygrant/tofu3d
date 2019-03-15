using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.Pathfinding.MapAdaptors;

namespace TofuPlugin.Pathfinding.Test
{
    public class PathGridTests
    {
        private ServiceContext _serviceContext;
        private PathGrid _pathGrid;
        private EventContext _eventContext;
        private FakePathableMapService _fakePathableMapService;

        [SetUp]
        public void SetUp()
        {
            _serviceContext = new ServiceContext();
            _eventContext = _serviceContext.Fetch("EventContext");
            _eventContext.GetPayloadTypeContainer().RegisterPayloadContentType("Null", o => o == null);
            _pathGrid = new PathGrid().BindServiceContext(_serviceContext);
            _fakePathableMapService = new FakePathableMapService().BindServiceContext(_serviceContext, "IPathableMapService");
            _serviceContext.FullInitialization();

        }

        [Test]
        public void TestGridConstructorShouldConstructAndSetParameters()
        {
            _pathGrid.NodeSizeRadius = 10f;
            _pathGrid.PenaltyBlur = 5;
            _pathGrid.NodesPerTileSide = 6;

            Assert.NotNull(_pathGrid);
            Assert.AreEqual(10f, _pathGrid.NodeSizeRadius, 0.01f);
            Assert.AreEqual(5, _pathGrid.PenaltyBlur);
            Assert.AreEqual(6, _pathGrid.NodesPerTileSide);
        }

        [Test]
        public void TestGridShouldGenerateFromSmallMap()
        {
            Assert.Null(_pathGrid.Grid);

            _fakePathableMapService.SetSimpleMap3x3();

            Assert.NotNull(_pathGrid.Grid);
        }

        [Test]
        public void TestShouldBeAbleToGetNodesFromSmallGrid()
        {
            _pathGrid.NodesPerTileSide = 2;
            _fakePathableMapService.SetSimpleMap3x3();

            string expected = "000011111111110000";
            
            Assert.AreEqual(expected, GetPassableValues(_pathGrid.Grid));
            

        }

        private string GetPassableValues(PathNode[,] tiles)
        {
            string val = "";
            foreach (PathNode tile in tiles)
            {
                val += tile.Walkable ? "1" : "0";
            }

            return val;
        }
    }

}
