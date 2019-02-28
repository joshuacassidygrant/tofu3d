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

        /*
         * Add to this only with the AddAction() method to ensure actions are bound to agent
         */
        public List<AgentAction> Actions { get; }
        public ITargetable TargetableSelf => this;
        public AIAgentController Controller;
        protected string Name;


        //TODO: should we also allow other methods of setting size radius? Or move it out of properties?
        public float SizeRadius { get; protected set; }

        //TODO: should be able to take a 3d model instead
        public Sprite Sprite { get; set; }
        private Dictionary<string, bool> AnimationStates = new Dictionary<string, bool>();

        public Vector3 Position { get; set; }
        private Dictionary<string, ResourceModule> _resourceModules;
        public Vector3 NextMoveTarget;

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
        protected FactionContainer FactionManager;


        public Agent(int id, AgentPrototype prototype, Vector3 position, ServiceContext context, float sizeRadius = 1f) : base(id, context)
        {
            Sprite = prototype?.Sprite;
            Name = prototype?.Name;

            Position = position;
            SizeRadius = sizeRadius;
            
            ResolveDependencies();
            _resourceModules = new Dictionary<string, ResourceModule>();

            Actions = new List<AgentAction>();

            if (prototype == null || ActionFactory == null || prototype.Actions == null || prototype.Actions.Count <= 0) return;

            foreach (PrototypeActionEntry action in prototype.Actions)
            {
                AddAction(ActionFactory.BindAction(this, action.ActionId, action.Configuration));
            }

        }

        private void ResolveDependencies()
        {
            SensorFactory = ServiceContext.Fetch("AgentSensorFactory");
            ActionFactory = ServiceContext.Fetch("AgentActionFactory");
            FactionManager = ServiceContext.Fetch("FactionContainer");
            EventContext = ServiceContext.Fetch("EventContext");
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
            //Do something
        }

        public virtual void AutoSetController()
        {
            Controller = new AIAgentController(this, SensorFactory.NewAgentSensor(this));
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

        /*
         * Faction
         */
        
        public FactionRelationshipLevel GetRelationshipWith(Agent agent)
        {
            return FactionManager.GetFactionRelationship(this, agent);
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

        public virtual void SetMove(ITargetable target, float dist)
        {
            //
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

        public override string ToString()
        {
            return "Agent" + Id;
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

    }
}
