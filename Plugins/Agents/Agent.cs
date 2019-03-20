using System;
using UnityEngine;
using TofuPlugin.Renderable;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Configuration;
using TofuCore.Glops;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AI;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Sensors;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;
using TofuCore.ResourceModule;
using TofuCore.Targetable;
using TofuPlugin.Pathfinding;

namespace TofuPlugin.Agents
{
    /*
     * Holds the state for a single instantiated Agent. Subclass to get property checking.
     * Agents are objects that have a position, a name/Id, and are commandable by an attached AI
     * or player input. Agents are managed by an agent manager class and rendered by an agent
     * renderer.
     */
    public class Agent: Glop, IRenderable, ITargetable, IControllableAgent, IConfigurable, ISensable, IResourceModuleOwner
    {
        public string Name { get; private set; }
        public bool Active { get; private set; }

        //Services
        protected AIBehaviourManager BehaviourManager;
        protected PathRequestService PathRequestService;
        protected FactionContainer FactionContainer;
        protected EventContext EventContext;

        /*
         * ITargetable and positioning
         */
        public Vector3 Position { get; set; }
        public float SizeRadius { get; protected set; }
        public ITargetable TargetableSelf => this;

        /*
         * Pathfinding
         */
        //TODO: refactor Pathfinding
        public Path Path;
        private float _turnDist = 0.04f;
        private float _stoppingDist = 1f;
        private float _moveTargetDist;
        private float _turnSpeed = 1f;
        private Quaternion _rotation = Quaternion.identity;
        private float _moveSpeed = 2f;
        public Vector3 NextMoveTarget;

        public ITargetable MoveTarget;
        public float PositionTolerance = 0.05f;
        public float MovementStepMax = 0.5f;
        private bool _pathRequested;
        private int _currentPathIndex;
        private ITargetable _nextMovePoint;

        private float _pathPointDistance = 0.5f;
        //END REFACTOR

        /*
         * Rendering
         */
        //TODO: Refactor. Put Sprite and AnimationStates in a component that can interface with IRenderable and accept either a 3d model or a sprite
        public Sprite Sprite { get; set; }
        private Dictionary<string, bool> AnimationStates = new Dictionary<string, bool>();
        public string GetSortingLayer() { return "Unit"; }

        /*
         * Local modules
         */
        public AgentSensor Sensor { get; set; }
        public Dictionary<string, ResourceModule> ResourceModules { get; private set; }
        public HashSet<string> ExpectedProperties { get; private set; }
        public AIAgentController Controller { get; private set; }
        public List<AgentAction> Actions { get; private set; } //Add to this only with the AddAction() method to ensure actions are bound to agent
        public AgentType AgentType { get; private set; }
        public Faction Faction { get; set; }
        public Properties Properties { get; private set; }

        /*
         * Action/Command storage
         */
        //TODO: refactor. should these go in a module?
        public AgentCommand CurrentCommand {get; set;}
        public AgentAction CurrentAction {get; set;}
        public ITargetable CurrentActionTarget {get; set;}


        /**
         * INITIALIZATION
         */
        public Agent()
        {
            ResourceModules = new Dictionary<string, ResourceModule>();
            Properties = new Properties();
            Actions = new List<AgentAction>();
        }

        public void ConsumeConfig(Configuration config)
        {
            Properties.Configure(config);
            Properties.Check(ExpectedProperties);
        }
        
        // Called by AgentFactory
        public void ConsumePrototype(AgentType type, AgentPrototype prototype, List<AgentAction> boundActions)
        {
            if (prototype == null) return;
            Sprite = prototype.Sprite;
            Name = prototype.Name;
            AgentType = type;
            ExpectedProperties = AgentType.ExpectedProperties;
            SizeRadius = prototype.SizeRadius;

            ConsumeConfig(prototype.Config);

            BindResourceModules();
            Actions = boundActions;

        }

        // Called by AgentFactory from AgentContainer to InjectDependencies
        public override void InjectDependencies(ContentInjectablePayload injectables)
        {
            FactionContainer = injectables.Get("FactionContainer");
            EventContext = injectables.Get("EventContext");
            BehaviourManager = injectables.Get("AIBehaviourManager");
            PathRequestService = injectables.Get("PathRequestService");
        }

        private void BindResourceModules()
        {
            foreach (AgentResourceModuleConfig agentResourceModuleConfig in AgentType
                .ResourceModuleConfigs)
            {
                AssignResourceModule(agentResourceModuleConfig.Key, agentResourceModuleConfig.GenerateResourceModule(this, EventContext));
            }
        }

        public void SetController(AIAgentController controller)
        {
            Controller = controller;
            foreach (AgentAction action in Actions)
            {
                (action).AgentSensor = (AgentSensor)controller.GetSensor();
            }

        }

        public virtual void AutoSetController()
        {
            SetController(new AIAgentController(this, Sensor, BehaviourManager));
        }

        /**
         * UPDATE & COMMON METHODS
         */
        public override void Update(float frameDelta)
        {
            UpdateActions(frameDelta);

            if (Controller == null) AutoSetController();
            Controller.Update();
            //Controller should target and trigger actions.

            if (CurrentCommand != null && CurrentCommand.Action != null)
            {
                //Once action is set and targeted, agent is responsible for carrying it out
                CurrentCommand.TryExecute();
                SetAnimationStates();
                /*if (CurrentCommand.Action.Phase == ActionPhase.FOCUS && CurrentCommand.Action.Name != "Move")
                {
                    EventContext.TriggerEvent("StringPop", new EventPayload("EventPayloadStringTargettable", new EventPayloadStringTargettable(this, "F"), EventContext));
                }*/
            }

            HandleMovement(frameDelta);
        }

        public override string ToString()
        {
            return base.ToString() + AgentType.ToString() + Id + " at " + Position.ToString();
        }

        public override void Die()
        {
            base.Die();
            Active = false;
        }

        /**
         * COMMANDS
         */
        public void ReceiveCommand(AgentCommand command)
        {
            throw new System.NotImplementedException();
        }

        /**
         * ACTIONS
         */

        protected void UpdateActions(float deltaTime)
        {
            foreach (AgentAction action in Actions)
            {
                action.Update(deltaTime);
            }
        }

        public void AddAction(AgentAction action)
        {
            if (action.Agent != this)
            {
                Debug.Log("Must bind action to this agent first!");
                return;
            }
            Actions.Add(action);
        }

        /**
         * PATHFINDING AND MOVEMENT
         */
        private void HandleMovement(float frameDelta)
        {
            if (MoveTarget == null || Vector3.Distance(Position, MoveTarget.Position) <= PositionTolerance || (Path != null && _currentPathIndex >= Path.LookPoints.Length)) return; //No move target or at current target; return.

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
            if (_nextMovePoint == null || Vector3.Distance(Position, _nextMovePoint.Position) <= PositionTolerance)
            {
                /*
                 * Find a chunk distance to take from path.
                 */
                float pointDistance = MovementStepMax;
                //TODO: Distance should be attenuated from current position in relation to path end to allow for more gentle corrections to be made up close.
     
                /*
                 * Grab a move point up to chunk value away.
                 */
                Vector3 nextWayPoint = Path.LookPoints[_currentPathIndex];
                if (Vector3.Distance(Position, nextWayPoint) <= PositionTolerance)
                {
                    _nextMovePoint = new TargetablePosition(nextWayPoint);
                    _currentPathIndex++;
                }
                else
                {
                    _nextMovePoint = new TargetablePosition(Vector3.LerpUnclamped(Position, nextWayPoint, pointDistance));
                }


            }

            Move(frameDelta);
            
        }

        private void FollowPath(float frameDelta)
        {
            bool followingPath = true;
            int pathIndex = 0;
            Vector3 nextPoint = (Path.LookPoints[0]);

            float speedPercent = 1f;

            Vector2 pos2D = new Vector2(Position.x, Position.y);
            while (Path.TurnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == Path.FinishLineIndex)
                {
                    followingPath = false;
                }
                else
                {
                    pathIndex++;
                    nextPoint = Path.LookPoints[pathIndex];
                }
            }

            if (followingPath)
            {
                if (pathIndex >= Path.slowDownIndex && _stoppingDist > 0)
                {
                    speedPercent = Mathf.Clamp01(Path.TurnBoundaries[Path.FinishLineIndex].DistanceFromPoint(pos2D) / _stoppingDist);
                    if (speedPercent <= 0.01)
                    {
                        followingPath = false;
                    }
                }

                Vector3 direction = (nextPoint - Position).normalized;
                Vector3 add = direction * _pathPointDistance * _moveSpeed * speedPercent;
               // SetNextMovePoint(Position + add);
                //Position += add;
            }
        }

        public void SetMoveTarget(ITargetable target, float dist)
        {
            MoveTarget = target;
            _moveTargetDist = dist;
            RequestPathTo(target.Position);
        }

        private void Move(float deltaTime)
        {
            Vector3 direction = (_nextMovePoint.Position - Position).normalized;
            Vector3 add = direction * deltaTime * _moveSpeed;
            Position += add;
        }

        public void RequestPathTo(Vector3 point)
        {
            PathRequest request = new PathRequest(Position, point, OnPathFound);
            PathRequestService.RequestPath(request);
        }

        public void OnPathFound(Vector3[] waypoints, bool success)
        {
            _pathRequested = false;
            if (success)
            {
                Path = new Path(waypoints, Position, _turnDist, _stoppingDist);
                _currentPathIndex = 0;
            }
        }

        //Pathfinding
        //TEMPORARY
        //TODO: REMOVE THIS
        private Vector3 _nextPathPoint = Vector3.zero;

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
        }

        //TEMP
        public AgentAction GetMoveAction()
        {
            return Actions.FirstOrDefault(x => x.Id == "move");
        }

        /**
         * FACTION METHODS
         */

        public FactionRelationshipLevel GetRelationshipWith(Agent agent)
        {
            return FactionContainer.GetFactionRelationship(this, agent);
        }

        public List<string> GetFactionPermissions(Agent agent)
        {
            return GetRelationshipWith(agent).Permissions;
        }

        public bool PermissionToDo(string factionAction, Agent agent)
        {
            return GetFactionPermissions(agent).Contains(factionAction);
        }



        /**
         * IRENDERABLE
         */
        public Dictionary<string, bool> GetAnimationStateBools()
        {
            return AnimationStates;
        }

        protected virtual void SetAnimationStates()
        {
            ClearAnimationStates();

            //TODO: make a better way to send these
            if (CurrentCommand.Action.Name == "Move")
            {
                SetAnimationState("Walking", true);
            }
            else if (CurrentCommand.Action.Phase == ActionPhase.FOCUS)
            {
                SetAnimationState("Focusing" ,true);
            }
            else if (CurrentCommand.Action.Phase == ActionPhase.READY && CurrentCommand.Action.CanUse())
            {
                SetAnimationState("Acting", true);
            }
        }

        protected void SetAnimationState(string key, bool value)
        {
            if (!AnimationStates.ContainsKey(key))
            {
                AnimationStates.Add(key, value);
            }
            else
            {
                AnimationStates[key] = value;
            }
        }

        protected void ClearAnimationStates()
        {
            string[] keys = AnimationStates.Keys.ToArray();
            foreach (string key in keys)
            {
                AnimationStates[key] = false;
            }
        }

        /**
         * RESOURCE MODULES
         */

        public Dictionary<string, ResourceModule> GetResourceModules()
        {
            return ResourceModules;
        }

        public void AssignResourceModule(string key, ResourceModule module)
        {
            if (ResourceModules.ContainsKey(key))
            {
                Debug.Log("Can't assign a second resource module to key " + key);
                return;
            }

            ResourceModules.Add(key, module);
        }

        public void RemoveResourceModule(string key)
        {
            ResourceModules.Remove(key);
        }

        public ResourceModule GetResourceModule(string key)
        {
            if (!ResourceModules.ContainsKey(key)) return null;
            return ResourceModules[key];
        }
    }
}
