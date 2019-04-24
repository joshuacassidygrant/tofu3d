using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuCore.Service;
using TofuCore.Tangible;
using TofuPlugin.Agents;
using TofuPlugin.Pathfinding.MapAdaptors;
using TofuPlugin.PositioningServices;
using UnityEditor.VersionControl;
using UnityEngine;

namespace TofuTest.PositioningServiceTest
{
    public class PositioningServiceTests
    {
        private PositioningService _positioningService;
        private IPathableMapService _subPathableMapService;
        private ITangibleContainer _tangibleContainer1;
        private ITangibleContainer _tangibleContainer2;
        private IServiceContext _subServiceContext;
        private ITangible _pawn;
        private List<ITangible> _ignoreList;


        [SetUp]
        public void SetUp()
        {
            _positioningService = new PositioningService();
            _subPathableMapService = Substitute.For<IPathableMapService>();
            _tangibleContainer1 = Substitute.For<ITangibleContainer>();
            _tangibleContainer2 = Substitute.For<ITangibleContainer>();


            _subServiceContext = TestUtilities.BuildSubServiceContextWithServices(new Dictionary<string, object>()
            {
                {"IPathableMapService", _subPathableMapService }
            });

            _positioningService.Build();
            _positioningService.BindServiceContext(_subServiceContext);
            _positioningService.ResolveServiceBindings();
            _positioningService.Initialize();

            _positioningService.RegisterTargetableContainer(_tangibleContainer1);
            _positioningService.RegisterTargetableContainer(_tangibleContainer2);

            _pawn = Substitute.For<ITangible>();
            _pawn.Position.Returns(Vector3.zero);
            _pawn.SizeRadius.Returns(1f);
            _ignoreList = new List<ITangible> {_pawn};

        }

        [Test]
        public void TestPositioningServiceBuildsAndInitializes()
        {
            Assert.NotNull(_positioningService);
            Assert.True(_positioningService.CheckDependencies());
            Assert.AreEqual(2, _positioningService.TangibleContainers.Count);
            Assert.True(_positioningService.TangibleContainers.Contains(_tangibleContainer1));
        }

        [Test]
        public void TestClearSpaceClearMap()
        {
            _subPathableMapService.GetPathableMapTile(Arg.Any<Vector3>()).Passable.Returns(true);
            _tangibleContainer1.GetAllTangibles().Returns(new List<ITangible>());
            _tangibleContainer2.GetAllTangibles().Returns(new List<ITangible>());

            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.zero, 1f), null));
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.zero, 0f), null));
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(10f, 10f, 10f), 0f), null));
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.forward, 3f), null));
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.up, 1f), null));
        }

        [Test]
        public void TestMapUnpassableFailsClearSpace()
        {
            _subPathableMapService.GetPathableMapTile(Arg.Any<Vector3>()).Passable.Returns(false);
            _tangibleContainer1.GetAllTangibles().Returns(new List<ITangible>());
            _tangibleContainer2.GetAllTangibles().Returns(new List<ITangible>());

            Assert.False(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.zero, 1f), null));
        }

        [Test]
        public void TestMapPassableButWithObstructionsFailsClearSpace()
        {
            _subPathableMapService.GetPathableMapTile(Arg.Any<Vector3>()).Passable.Returns(true);

            ITangible obstruction = Substitute.For<ITangible>();
            obstruction.Position.Returns(Vector3.one);
            obstruction.SizeRadius.Returns(1f);
            List<ITangible> oneObstructionList = new List<ITangible> {obstruction};
            
            _tangibleContainer1.GetAllTangibles().Returns(oneObstructionList);
            _tangibleContainer2.GetAllTangibles().Returns(new List<ITangible>());

            Assert.False(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.zero, 1f), _ignoreList));

            _tangibleContainer1.GetAllTangibles().Returns(new List<ITangible>());
            _tangibleContainer2.GetAllTangibles().Returns(oneObstructionList);

            Assert.False(_positioningService.SpaceAtPosition(new TangiblePosition(Vector3.zero, 1f), _ignoreList));
        }

        [Test]
        public void TestMapPassableObstructionsEdgeCases()
        {
            _subPathableMapService.GetPathableMapTile(Arg.Any<Vector3>()).Passable.Returns(true);

            ITangible obstruction = Substitute.For<ITangible>();
            ITangible obstruction2 = Substitute.For<ITangible>();
            obstruction.Position.Returns(Vector3.one);
            obstruction.SizeRadius.Returns(0.5f);
            obstruction2.Position.Returns(Vector3.zero);
            obstruction2.SizeRadius.Returns(0.5f);
            List<ITangible> obstructionList1 = new List<ITangible> { obstruction };
            List<ITangible> obstructionList2 = new List<ITangible> { obstruction2 };
            _tangibleContainer1.GetAllTangibles().Returns(obstructionList1);
            _tangibleContainer2.GetAllTangibles().Returns(obstructionList2);

            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0.36f), _ignoreList));
            Assert.False(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0.37f), _ignoreList));

            obstruction.SizeRadius.Returns(0.6f);
            Assert.False(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0.36f), _ignoreList));

            obstruction.Position.Returns(new Vector3(2f, 2f, 0));
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0.36f), _ignoreList));



        }

        [Test]
        public void TestOverlappingObstruction()
        {
            _subPathableMapService.GetPathableMapTile(Arg.Any<Vector3>()).Passable.Returns(true);

            ITangible obstruction = Substitute.For<ITangible>();
            ITangible obstruction2 = Substitute.For<ITangible>();
            obstruction.Position.Returns(Vector3.one);
            obstruction.SizeRadius.Returns(0.5f);
            obstruction2.Position.Returns(Vector3.zero);
            obstruction2.SizeRadius.Returns(0.5f);
            List<ITangible> obstructionList1 = new List<ITangible> { obstruction };
            List<ITangible> obstructionList2 = new List<ITangible> { obstruction2 };
            _tangibleContainer1.GetAllTangibles().Returns(obstructionList1);
            _tangibleContainer2.GetAllTangibles().Returns(obstructionList2);

            obstruction2.Position.Returns(new Vector3(0.5f, 0.5f, 0.5f), Vector3.zero);
            Assert.False(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0.1f), _ignoreList));
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0f), _ignoreList));

            obstruction2.SizeRadius.Returns(0f);
            Assert.True(_positioningService.SpaceAtPosition(new TangiblePosition(new Vector3(0.5f, 0.5f, 0.5f), 0.1f), _ignoreList));
        }

        [Test]
        public void TestGetNearestClearSpaceIncrementsHalfCircle()
        {
            float degreeDelta = 180f / AgentConstants.PositionJostleScanSteps;

            _subPathableMapService.GetPathableMapTile(Arg.Any<Vector3>()).Passable.Returns(true);


            ITangible obstruction = Substitute.For<ITangible>();
            obstruction.Position.Returns(Vector3.up);
            obstruction.SizeRadius.Returns(0.05f);
            List<ITangible> obstructionList1 = new List<ITangible> { obstruction };
            _tangibleContainer1.GetAllTangibles().Returns(obstructionList1);
            _tangibleContainer2.GetAllTangibles().Returns(new List<ITangible>());


            Vector3 pos;
            Vector3 test;

            for (int i = 1; i <= AgentConstants.PositionJostleScanSteps; i++)
            {
                int sign = (i % 2 == 0) ? -1 : 1;
                float degs = 90f + Mathf.CeilToInt(i / 2f) * sign * degreeDelta;
                float rads = (degs) * Mathf.PI / 180;
                pos = _positioningService.GetNearestClearSpace(_pawn, Vector3.up, _ignoreList).Position;
                test = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0f);
                Assert.True(Vector3.Distance(pos, test) < 0.1f);
                
                //Try next position
                ITangible newObstruction = Substitute.For<ITangible>();
                newObstruction.Position.Returns(pos);
                newObstruction.SizeRadius.Returns(0.05f);
                obstructionList1.Add(newObstruction);
            }

            //Obstructed in full 180 degree angle in front; moves nowhere.
            Assert.AreEqual(Vector3.zero, _positioningService.GetNearestClearSpace(_pawn, Vector3.up, _ignoreList).Position);



        }





    }

}
