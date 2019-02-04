using UnityEngine;
using TofuPlugin.Renderable;
using System.Collections.Generic;
using Scripts;
using TofuPlugin.Agents.AI.Behaviour;
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
using TofuPlugin.Agents.Targetable;

namespace TofuPlugin.Agents
{
    /*
     * Holds the state for a single instantiated Agent. Subclass to get property checking.
     * Agents are objects that have a position, a name/Id, and are commandable by an attached AI
     * or player input. Agents are managed by an agent manager class and rendered by an agent
     * renderer.
     */
    public class Agent: Glop, IRenderable, ITargetable, IControllableAgent, IConfigurable, ISensable
    {

        /*
         * Add to this only with the AddAction() method to ensure actions are bound to agent
         */
        public List<AgentAction> Actions { get; }
        public ITargetable TargetableSelf => this;
        public AIAgentController Controller;


        //TODO: should we also allow other methods of setting size radius? Or move it out of properties?
        public float SizeRadius { get; protected set; }

        //TODO: should be able to take a 3d model instead
        public Sprite Sprite { get; set; }
        public Vector3 Position { get; set; }

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

        private Properties _properties;

        public virtual string GetSortingLayer()
        {
            return "Agent";
        }

        protected AgentSensorFactory SensorFactory;
        protected AbstractAgentActionFactory ActionFactory;
        protected FactionContainer FactionManager;


        public Agent(int id, AgentPrototype prototype, Vector3 position, ServiceContext context, float sizeRadius = 1f) : base(id, prototype.Name, context)
        {
            Sprite = prototype.Sprite;
            Position = position;
            SizeRadius = sizeRadius;

            ResolveDependencies();

            Actions = new List<AgentAction>();

            if (ActionFactory == null || prototype.Actions == null || prototype.Actions.Count <= 0) return;

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

            if (CurrentCommand != null)
            {
                //Once action is set and targeted, agent is responsible for carrying it out
                CurrentCommand.TryExecute();
                if (CurrentCommand.Action.Phase == ActionPhase.FOCUS && CurrentCommand.Action.Name != "Move")
                {
                    EventContext.TriggerEvent("StringPop", new EventPayload("EventPayloadStringTargettable", new EventPayloadStringTargettable(this, "F"), EventContext));
                }
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

        public void MoveInDirection(Vector3 direction, float time)
        {
            Position = Position + direction * Properties.GetProperty("Speed", 1f) * time;
        }


        protected void UpdateActions(float deltaTime)
        {
            foreach (AgentAction action in Actions)
            {
                action.Update(deltaTime);
            }
        }






    }
}
