using System.Collections;
using System.Collections.Generic;
using TofuCore.Targetable;
using TofuPlugin.Pathfinding;
using UnityEngine;

namespace TofuPlugin.Agents.Components
{
    public class AgentMobilityComponent
    {
        //CONSTANTS TODO: MOVE
        const float POSITION_TOLERANCE = 0.1f;
        private const float MOVE_STEP_MAX = 0.5f;

        public Agent Agent { get;}
        public Path Path { get; private set; }
        public ITargetable MoveTarget { get; private set; }

        private PathRequestService _pathRequestService;
        //TODO: add this. See move action.
        //private PositioningService _positioningService;

        private bool _pathRequested;
        private ITargetable _nextMovePoint;
        private int _currentPathIndex;

        //The distance to get within:
        private float _moveTargetDist;
        private float MoveSpeed => Agent.Properties.GetProperty("Speed", 0f);


        public AgentMobilityComponent(Agent agent, PathRequestService pathRequestService)
        {
            Agent = agent;
            _pathRequestService = pathRequestService;
        }

        public void Update(float deltaTime)
        {
            HandleMovement(deltaTime);
        }

        private void HandleMovement(float frameDelta)
        {
            if (MoveTarget == null || Vector3.Distance(Agent.Position, MoveTarget.Position) <= POSITION_TOLERANCE || (Path != null && _currentPathIndex >= Path.LookPoints.Length)) return; //No move target or at current target; return.

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
            if (_nextMovePoint == null || Vector3.Distance(Agent.Position, _nextMovePoint.Position) <= POSITION_TOLERANCE)
            {
                /*
                 * Find a chunk distance to take from path.
                 */
                float pointDistance = MOVE_STEP_MAX;
                //TODO: Distance should be attenuated from current position in relation to path end to allow for more gentle corrections to be made up close.

                /*
                 * Grab a move point up to chunk value away.
                 */
                Vector3 nextWayPoint = Path.LookPoints[_currentPathIndex];
                if (Vector3.Distance(Agent.Position, nextWayPoint) <= POSITION_TOLERANCE)
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
            Agent.Position += add;
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

