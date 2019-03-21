using System.Collections;
using System.Collections.Generic;
using TofuCore.Targetable;
using TofuPlugin.Pathfinding;
using TofuPlugin.PositioningService;
using UnityEngine;

namespace TofuPlugin.Agents.Components
{
    public class AgentMobilityComponent
    {

        public Agent Agent { get;}
        public Path Path { get; private set; }
        public ITargetable MoveTarget { get; private set; }

        private PathRequestService _pathRequestService;
        private PositioningService.PositioningService _positioningService;

        private bool _pathRequested;
        private ITargetable _nextMovePoint;
        private int _currentPathIndex;

        //The distance to get within:
        private float _moveTargetDist;
        private float MoveSpeed => Agent.Properties.GetProperty("Speed", 0f);


        public AgentMobilityComponent(Agent agent, PathRequestService pathRequestService, PositioningService.PositioningService positioningService)
        {
            Agent = agent;
            _pathRequestService = pathRequestService;
            _positioningService = positioningService;
        }

        public void Update(float deltaTime)
        {
            HandleMovement(deltaTime);
        }

        private void HandleMovement(float frameDelta)
        {
            if (MoveTarget == null || Vector3.Distance(Agent.Position, MoveTarget.Position) <= AgentConstants.PositionTolerance || (Path != null && _currentPathIndex >= Path.LookPoints.Length)) return; //No move target or at current target; return.

            if (Path == null)
            {
                if (!_pathRequested)
                {
                    RequestPathTo(MoveTarget.Position);
                    _pathRequested = true;
                }

                return;
            }

            //Path and _moveTarget must be true, and the agent is not at _moveTarget
            if (_nextMovePoint == null || Vector3.Distance(Agent.Position, _nextMovePoint.Position) <= AgentConstants.PositionTolerance)
            {
                /*
                 * Find a chunk distance to take from path.
                 */
                float pointDistance = AgentConstants.MoveStepMax;
                //TODO: Distance should be attenuated from current position in relation to path end to allow for more gentle corrections to be made up close.

                /*
                 * Grab a move point up to chunk value away.
                 */
                Vector3 nextWayPoint = Path.LookPoints[_currentPathIndex];
                if (Vector3.Distance(Agent.Position, nextWayPoint) <= AgentConstants.PositionTolerance)
                {
                    _nextMovePoint = new TargetablePosition(nextWayPoint);
                    _currentPathIndex++;
                }
                else
                {
                    _nextMovePoint = new TargetablePosition(Vector3.LerpUnclamped(Agent.Position, nextWayPoint, pointDistance));
                }


            }

            Move(frameDelta);

        }

        public void SetMoveTarget(ITargetable target, float dist)
        {
            MoveTarget = target;
            _moveTargetDist = dist;
            RequestPathTo(target.Position);
        }

        private void Move(float deltaTime)
        {
            Vector3 direction = (_nextMovePoint.Position - Agent.Position).normalized;
            Vector3 add = direction * deltaTime * MoveSpeed;
            ITargetable newPos = new TargetablePosition(Agent.Position + add);

            if (_positioningService.SpaceAtPosition(newPos, new List<ITargetable> { Agent }))
            {
                Agent.Position = newPos.Position;
            }
            

        }

        public void RequestPathTo(Vector3 point)
        {
            PathRequest request = new PathRequest(Agent.Position, point, OnPathFound);
            _pathRequestService.RequestPath(request);
        }

        public void OnPathFound(Vector3[] waypoints, bool success)
        {
            _pathRequested = false;
            if (success)
            {
                Path = new Path(waypoints);
                _currentPathIndex = 0;
            }
        }

        //Pathfinding
        //TEMPORARY
        //TODO: REMOVE THIS
        /*private Vector3 _nextPathPoint = Vector3.zero;

        public Vector3 GetNextPathPoint()
        {
            return _nextPathPoint;
        }

        public void SetNextPathPoint(Vector3 point)
        {
            _nextPathPoint = point;
        }

        public void MoveTo(Vector3 position)
        {
            Position = position;
        }*/


    }

}

