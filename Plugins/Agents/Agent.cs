using UnityEngine;
using TofuPlugin.Renderable;
using System.Collections.Generic;
using Scripts;
using TofuPlugin.Agents.AI.Strategy;
using Scripts.Sensors;
using TofuCore.Configuration;
using TofuCore.Glop;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AI;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Sensors;

namespace TofuPlugin.Agents
{
    /*
     * Holds the state for a single instantiated Agent. Subclass to get property checking.
     * Agents are objects that have a position, a name/Id, and are commandable by an attached AI
     * or player input. Agents are managed by an agent manager class and rendered by an agent
     * renderer.
     */
    public class Agent: Glop, IRenderable, ITargettable, IControllableAgent, IConfigurable, ISensable
    {

        public List<AgentAction> Actions { get; }
        public ITargettable TargettableSelf => this;
        public AIAgentController Controller;

        //TODO: should we also allow other methods of setting size radius? Or move it out of properties?
        public float SizeRadius { get; protected set; }

        //TODO: should be able to take a 3d model instead
        public Sprite Sprite { get; set; }
        public Vector3 Position { get; set; }

        public AgentCommand CurrentCommand {
            get; set;
        }

        public AgentAction CurrentAction {
            get; set;
        }

        public ITargettable CurrentActionTarget {
            get; set;
        }

        private readonly Dictionary<string, dynamic> _properties = new Dictionary<string, dynamic>();
        protected AgentSensorFactory SensorFactory;
        protected AbstractAgentActionFactory ActionFactory;


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
                Actions.Add(ActionFactory.BindAction(this, action.ActionId, action.Configuration));
            }


        }

        private void ResolveDependencies()
        {
            SensorFactory = ServiceContext.Fetch("AgentSensorFactory");
            ActionFactory = ServiceContext.Fetch("FakeAgentActionFactory");
        }

        public void ReceiveCommand(AgentCommand command) {
            throw new System.NotImplementedException();
        }


        public override void Update(float frameDelta)
        {
            if (Controller == null) AutoSetController();
            Controller.Update();
            //Controller should target and trigger actions.

            if (CurrentAction != null)
            {
                CurrentAction.TryExecute(frameDelta);
            }
            //Do something
        }

        public virtual void AutoSetController()
        {
            Controller = new AIAgentController(this, SensorFactory.NewAgentSensor(this));
        }


        public void AddAction(AgentAction action)
        {
            action.Agent = this;
            Actions.Add(action);
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
            Debug.Log("Move");
            Position = direction * GetProperty("Speed", 1f) * time;
        }


        //Config
        public virtual void Configure(Configuration config) {
            foreach (ConfigurationProperty entry in config.Properties)
            {
                SetProperty(entry.Key, entry.Value);
            }
        }

        public Dictionary<string, dynamic> GetProperties() {
            return _properties;
        }

        public float GetProperty(string id, float defaultValue) {
            if (!_properties.ContainsKey(id)) {
                return defaultValue;
            }
            return _properties[id];
        }

        public string GetProperty(string id, string defaultValue) {
            if (!_properties.ContainsKey(id) || (string)_properties[id] == null) {
                return defaultValue;
            }

            return _properties[id];
        }

        public int GetProperty(string id, int defaultValue) {
            if (!_properties.ContainsKey(id)) {
                return defaultValue;
            }

            return _properties[id];
        }

        public bool GetProperty(string id, bool defaultValue) {
            if (!_properties.ContainsKey(id)) {
                return defaultValue;
            }

            return _properties[id];
        }

        public void SetProperty(string id, dynamic value) {
            if (_properties.ContainsKey(id)) {
                _properties[id] = value;
                return;
            }

            _properties.Add(id, value);
        }

    }
}
