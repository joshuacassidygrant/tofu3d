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

        private List<ITangibleContainer> _tangibleContainers = new List<ITangibleContainer>();

        [Dependency] private AgentContainer _agentContainer;
        [Dependency] private IPathableMapService _mapService;

        public void RegisterTargetableContainer(ITangibleContainer tangibleContainer)
        {
            _tangibleContainers.Add(tangibleContainer);
        }

        public bool SpaceAtPosition(ITangible tangible, List<ITangible> ignore)
        {
            //TODO: fix this
            return _mapService.GetPathableMapTile(tangible.Position).Passable 
            && !_tangibleContainers.Any(xc => xc.GetAllTangibles()
                .Any(x => TangibleUtilities.DoTangiblesOverlap(x, tangible) && !ignore.Contains(x)));

        }

        public ITangible GetNearestClearSpace(ITangible position, Vector3 direction, List<ITangible> ignore)
        {
            float scanSize = 180f / AgentConstants.PositionJostleScanSteps;

            //TODO: optimize this
            //Use when attempt direction has failed, we will check other possible directions by rotating the attempt around the origin point.
            //Rotate attempt direction back and forth until we find a moveable direction.
            for (int i = 1; i < AgentConstants.PositionJostleScanSteps / 2; i++)
            {

                //Positive direction
                Vector3 newDirection = Quaternion.Euler(scanSize * i, 0, 0) * direction;
                Vector3 newPoint = position.Position + newDirection;
                TangiblePosition newPos = new TangiblePosition(newPoint);
                if (SpaceAtPosition(newPos, ignore))
                {
                    return newPos;
                }

                //Negative direction
                newDirection = Quaternion.Euler(scanSize * i * -1f, 0, 0) * direction;
                newPoint = position.Position + newDirection;
                newPos = new TangiblePosition(newPoint);
                if (SpaceAtPosition(newPos, ignore))
                {
                    return newPos;
                }
            }

            return position;
        }


    }
}

