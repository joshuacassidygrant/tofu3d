using System.Collections.Generic;
using TofuCore.Tangible;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Pathfinding;
using TofuPlugin.PositioningServices;
using UnityEngine;

namespace TofuPlugin.Agents.Components
{
    public class AgentMobilityComponent
    {

        public Agent Agent { get;}
        public Path Path { get; private set; }
        public ITangible MoveTarget { get; private set; }
        

        private PathRequestService _pathRequestService;
        private PositioningService _positioningService;

        private bool _pathRequested;
        private ITangible _nextMovePoint;
        private int _currentPathIndex;
        private Vector3 _moveTargetPositionOnLastPathRequest;

        //The distance to get within:
        private float _moveTargetDist;
        private float MoveSpeed => Agent.Properties.GetProperty("Speed", 0f);


        public AgentMobilityComponent(Agent agent, PathRequestService pathRequestService, PositioningService positioningService)
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
            if (MoveTarget == null 
                || Vector3.Distance(Agent.Position, MoveTarget.Position) <= AgentConstants.PositionTolerance 
                || (Path != null && _currentPathIndex >= Path.LookPoints.Length) 
                || Agent.CurrentAction.Phase == ActionPhase.FOCUS) return; //No move target or at current target; return.
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
                    _nextMovePoint = new TangiblePosition(nextWayPoint);
                    _currentPathIndex++;
                }
                else
                {
                    _nextMovePoint = new TangiblePosition(Vector3.LerpUnclamped(Agent.Position, nextWayPoint, pointDistance));
                }

                if (Vector3.Distance(MoveTarget.Position, _moveTargetPositionOnLastPathRequest) >
                    AgentConstants.RepathDistance)
                {
                    RequestPathTo(MoveTarget.Position);
                }
            }

            Move(frameDelta);

        }

        public void SetMoveTarget(ITangible target, float dist)
        {
            MoveTarget = target;
            _moveTargetDist = dist;
            RequestPathTo(target.Position);
        }

        public void UnsetMoveTarget()
        {
            MoveTarget = null;
            Path = null;
        }

        private void Move(float deltaTime)
        {
            Vector3 direction = (_nextMovePoint.Position - Agent.Position).normalized;
            Vector3 add = direction * deltaTime * MoveSpeed;
            ITangible newPos = new TangiblePosition(Agent.Position + add);

            if (_positioningService.SpaceAtPosition(newPos, new List<ITangible> { Agent }))
            {
                Agent.Position = newPos.Position;
            }
            else
            {
                newPos = _positioningService.GetNearestClearSpace(Agent, add, new List<ITangible> {Agent});
                Agent.Position = newPos.Position;
            }

            
        }


        public void RequestPathTo(Vector3 point)
        {
            PathRequest request = new PathRequest(Agent.Position, point, OnPathFound);
            _moveTargetPositionOnLastPathRequest = point;
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

    }

}

