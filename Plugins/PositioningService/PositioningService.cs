using System.Collections.Generic;
using System.Linq;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.Tangible;
using TofuPlugin.Agents;
using TofuPlugin.Pathfinding.MapAdaptors;
using TofuTest;
using UnityEngine;

/*
 * For checking if positions with regards to the Map and other units.
 */
namespace TofuPlugin.PositioningServices
{

    public class PositioningService : AbstractService
    {
        public float ScanSize = 180f / AgentConstants.PositionJostleScanSteps;

        public List<ITangibleContainer> TangibleContainers { get; private set; }

        [Dependency] private IPathableMapService _mapService;

        public PositioningService()
        {
            TangibleContainers = new List<ITangibleContainer>();
        }

        public void RegisterTargetableContainer(ITangibleContainer tangibleContainer)
        {
            TangibleContainers.Add(tangibleContainer);
        }

        public bool SpaceAtPosition(ITangible tangible, List<ITangible> ignore)
        {
            //TODO: fix this
            //TODO: currently, only checks 1 map tile, not all map tiles in radius.
            return _mapService.GetPathableMapTile(tangible.Position).Passable 
            && !TangibleContainers.Any(xc => xc.GetAllTangibles()
                .Any(x => TangibleUtilities.DoTangiblesOverlap(x, tangible) && !ignore.Contains(x)));

        }

        public ITangible GetNearestClearSpace(ITangible position, Vector3 direction, List<ITangible> ignore)
        {

            //TODO: optimize this
            //Use when attempt direction has failed, we will check other possible directions by rotating the attempt around the origin point.
            //Rotate attempt direction back and forth until we find a moveable direction.
            for (int i = 1; i < (AgentConstants.PositionJostleScanSteps / 2) + 1; i++)
            {

                //Positive direction
                Vector3 newDirection = Quaternion.Euler(0, 0, ScanSize * i) * direction;
                Vector3 newPoint = position.Position + newDirection;
                TangiblePosition newPos = new TangiblePosition(newPoint);
                if (SpaceAtPosition(newPos, ignore))
                {
                    return newPos;
                }

                //Negative direction
                newDirection = Quaternion.Euler(0, 0, ScanSize * i * -1f) * direction;
                newPoint = position.Position + newDirection;
                newPos = new TangiblePosition(newPoint);
                if (SpaceAtPosition(newPos, ignore))
                {
                    return newPos;
                }
            }

            //TODO: something when it breaks out like this
            return position;
        }


    }
}

