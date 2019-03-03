using UnityEngine;
using TofuPlugin.Renderable;
using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Configuration;
using TofuCore.Glops;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AI;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Sensors;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;
using TofuCore.ResourceModule;
using TofuCore.Targetable;
using TofuPlugin.Agents.AgentActions.Fake;
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
        protected string Name;
        /*
         * Add to this only with the AddAction() method to ensure actions are bound to agent
         */
        public List<AgentAction> Actions { get; }
        public AgentType AgentType;
        public Color BaseColor;

        public ITargetable TargetableSelf => this;
        public AIAgentController Controller;


        /*Pathfinding -- REFACTOR */
        private Path _path;
        private float _turnDist = 0.04f;
        private float _stoppingDist = 1f;
        private ITargetable _moveTarget;
        private float _moveTargetDist;
        private float _turnSpeed = 1f;
        private Quaternion _rotation = Quaternion.identity;
        private float _moveSpeed = 2f;

        private HashSet<string> _expectedProperties;
        protected AIBehaviourManager BehaviourManager;
        protected PathRequestService PathRequestService;
        protected AgentTypeLibrary AgentTypeLibrary;

        //TODO: should we also allow other methods of setting size radius? Or move it out of properties?
        public float SizeRadius { get; protected set; }

        //TODO: should be able to take a 3d model instead
        public Sprite Sprite { get; set; }
        private Dictionary<string, bool> AnimationStates = new Dictionary<string, bool>();

        public Vector3 Position { get; set; }
        private Dictionary<string, ResourceModule> _resourceModules;
        public Vector3 NextMoveTarget;

        private AgentSensor _sensor;



        protected EventContext EventContext;

        public int GetId()
        {
            return Id;
        }

        public AgentCommand CurrentCommand {
            get; set;
        }

        public AgentAction CurrentAction {
            get; set;
        }

        public ITargetable CurrentActionTarget {
            get; set;
        }

        public Faction Faction {
            get; set;
        }

        public Properties Properties {
            get {
                return _properties;
            }
            protected set {
                _properties = value;
            }
        }

        public bool Active {
            get; protected set;
        }

        public string GetName()
        {
            return Name;
        }

        private Properties _properties;

        public virtual string GetSortingLayer()
        {
            return "Agent";
        }


        protected AgentSensorFactory SensorFactory;
        protected AbstractAgentActionFactory ActionFactory;
        protected FactionContainer FactionContainer;


        public Agent(AgentPrototype prototype, Vector3 position, AgentType agentType, float sizeRadius = 1f)
        {
            Sprite = prototype?.Sprite;
            Name = prototype?.Name;

            Position = position;
            SizeRadius = sizeRadius;

            //Temp
            AgentType = agentType;
            _expectedProperties = agentType.ExpectedProperties;

            _resourceModules = new Dictionary<string, ResourceModule>();

            Actions = new List<AgentAction>();

            if (prototype == null || ActionFactory == null || prototype.Actions == null || prototype.Actions.Count <= 0) return;

            foreach (PrototypeActionEntry action in prototype.Actions)
            {
                AddAction(ActionFactory.BindAction(this, action.ActionId, action.Configuration));
            }
             
            float hpMax = 0f;




        }

        public void ConsumeConfig(Configuration config)
        {
            OverwriteProperties(config);
            CheckProperties();
        }

        public void ConsumePrototype(AgentPrototype prototype)
        {
            if (prototype == null) return;

            AgentType = AgentTypeLibrary.Get(prototype.AgentType);
            //hpMax = prototype.HpMax;
            BaseColor = prototype.BaseColor;
            SizeRadius = prototype.SizeRadius;
            
            //Load actions based on agent type

            //Load custom actions from prototype
        }

        private void OverwriteProperties(Configuration config)
        {
            // Will overwrite duplicate properties
            if (Properties == null)
            {
                Properties properties = new Properties(config);
            }
            else
            {
                Properties.Configure(config);
            }

        }

        public override void InjectDependencies(ContentInjectablePayload injectables)
        {
            SensorFactory = injectables.Get("AgentSensorFactory");
            ActionFactory = injectables.Get("AgentActionFactory");
            FactionContainer = injectables.Get("FactionContainer");
            EventContext = injectables.Get("EventContext");
            BehaviourManager = injectables.Get("AIBehaviourManager");
            PathRequestService = injectables.Get("PathRequestService");
        }

        public void ReceiveCommand(AgentCommand command) {
            throw new System.NotImplementedException();
        }

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

            if (_moveTarget != null && _path != null)
            {
                FollowPath(frameDelta);
            }
        }

        private void FollowPath(float frameDelta)
        {
            bool followingPath = true;
            int pathIndex = 0;
            Vector3 nextPoint = (_path.LookPoints[0]);

            float speedPercent = 1f;

            Vector2 pos2D = new Vector2(Position.x, Position.y);
            while (_path.TurnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == _path.FinishLineIndex)
                {
                    followingPath = false;
                }
                else
                {
                    pathIndex++;
                    nextPoint = _path.LookPoints[pathIndex];
                }
            }

            if (followingPath)
            {
                if (pathIndex >= _path.slowDownIndex && _stoppingDist > 0)
                {
                    speedPercent = Mathf.Clamp01(_path.TurnBoundaries[_path.FinishLineIndex].DistanceFromPoint(pos2D) / _stoppingDist);
                    if (speedPercent <= 0.01)
                    {
                        followingPath = false;
                    }
                }

                Vector3 direction = (nextPoint - Position).normalized;
                Vector3 add = direction * frameDelta * _moveSpeed * speedPercent;
                Position += add;
            }
        }

        public void SetMoveTarget(ITargetable target, float dist)
        {
            _moveTarget = target;
            _moveTargetDist = dist;
            RequestPathTo(target.Position);
        }

        public void RequestPathTo(Vector3 point)
        {
            PathRequest request = new PathRequest(Position, point, OnPathFound);
            PathRequestService.RequestPath(request);
        }

        public void OnPathFound(Vector3[] waypoints, bool success)
        {
            if (success)
            {
                _path = new Path(waypoints, Position, _turnDist, _stoppingDist);
                /*StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");*/
            }
        }




        public void SetMove(ITargetable target, float dist)
        {
            SetMoveTarget(target, dist);
        }



        /**
         * AI Configuration
         */
        public void SetSensor(AgentSensor sensor)
        {
            _sensor = sensor;
        }

        public void SetController(AIAgentController controller)
        {
            Controller = controller;
            foreach (AgentAction action in Actions)
            {
                (action).AgentSensor = (AgentSensor)controller.GetSensor();
            }

        }

        public override string ToString()
        {
            return base.ToString() + AgentType.ToString() + Id + " at " + Position.ToString();
        }

        public virtual void AutoSetController()
        {
            SetController(new AIAgentController(this, _sensor));
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

        public override void Die()
        {
            base.Die();
            Active = false;
        }

        //TODO: remove this
        private void LoadDefaultActions()
        {
            if (ActionFactory == null)
            {
                Debug.Log("No action factory bound. Cannot load default actions.");
                return;
            }

            //TODO: most of these are testing only
            AddAction(ActionFactory.BindAction(this, "idle"));
            AddAction(ActionFactory.BindAction(this, "move"));
            AddAction(ActionFactory.BindAction(this, "attack"));
            AddAction(ActionFactory.BindAction(this, "ranged"));
            AddAction(ActionFactory.BindAction(this, "heal"));
            AddAction(ActionFactory.BindAction(this, "moveToObjective"));

        }

        /*
         * Faction
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

        //Pathfinding
        //TEMPORARY
        //TODO: add real pathfinding
        private Vector3 _nextPathPoint = Vector3.zero;

        public Vector3 GetNextPathPoint()
        {
            return _nextPathPoint;
        }

        public void SetNextPathPoint(Vector3 point)
        {
            _nextPathPoint = point;
        }

        public override void Initialize()
        {
            base.Initialize();


            ResourceModule Hp = new ResourceModule("HP", hpMax, hpMax, this, EventContext);
            Hp.BindFullDepletionEvent("UnitDies", new EventPayload("Unit", this, EventContext));
            Hp.SetDepletionEventKey("Damaged");
            Hp.SetReplenishEventKey("Healed");
            AssignResourceModule("HP", Hp);




            LoadDefaultActions();

        }

        public void MoveInDirection(Vector3 direction, float time)
        {
            Position = Position + direction * Properties.GetProperty("Speed", 1f) * time;
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


        protected void UpdateActions(float deltaTime)
        {
            foreach (AgentAction action in Actions)
            {
                action.Update(deltaTime);
            }
        }

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

        public Dictionary<string, ResourceModule> GetResourceModules()
        {
            return _resourceModules;
        }

        public void AssignResourceModule(string key, ResourceModule module)
        {
            if (_resourceModules.ContainsKey(key))
            {
                Debug.Log("Can't assign a second resource module to key " + key);
                return;
            }

            _resourceModules.Add(key, module);
        }

        public void RemoveResourceModule(string key)
        {
            _resourceModules.Remove(key);
        }

        public ResourceModule GetResourceModule(string key)
        {
            if (!_resourceModules.ContainsKey(key)) return null;
            return _resourceModules[key];
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        private bool CheckProperties()
        {
            if (Properties == null) return true;

            HashSet<string> _checklist = new HashSet<string>(_expectedProperties);

            foreach (var entry in Properties.GetProperties())
            {
                if (_checklist.Contains(entry.Key))
                {
                    _checklist.Remove(entry.Key);
                }
            }

            if (_checklist.Count == 0) return true;

            Debug.Log("Could not find " + _checklist.Count + " expected properties for unit " + Name + Id);
            return false;
        }

    }
}
