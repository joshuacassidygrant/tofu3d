﻿using UnityEngine;
using TofuPlugin.Renderable;
using System.Collections.Generic;
using Scripts;
using TofuPlugin.Agents.AI.Strategy;
using Scripts.Sensors;
using TofuCore.Configuration;
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
    public class Agent: IRenderable, ITargettable, IControllableAgent, IConfigurable, ISensable
    {
        public int Id;
        public string Name;
        public List<AgentAction> Actions { get; }
        public ITargettable TargettableSelf => this;
        public AIAgentController Controller;

        //TODO: should we also allow other methods of setting size radius? Or move it out of properties?
        public float SizeRadius { get; }

        //TODO: should be able to take a 3d model instead
        public Sprite Sprite { get; set; }
        public Vector3 Position { get; set; }

        private readonly Dictionary<string, dynamic> _properties = new Dictionary<string, dynamic>();
        private readonly AbstractSensorFactory _sensorFactory;

        public Agent(int id, AgentPrototype prototype, Vector3 position, AbstractSensorFactory abstractSensorFactory,
            AbstractAgentActionFactory actionFactory = null, float sizeRadius = 1f)
        {
            Id = id;
            Name = prototype.Name;
            Sprite = prototype.Sprite;
            Position = position;
            SizeRadius = sizeRadius;
            _sensorFactory = abstractSensorFactory;

            Actions = new List<AgentAction>();

            if (actionFactory == null || prototype.Actions == null || prototype.Actions.Count <= 0) return;

            foreach (PrototypeActionEntry action in prototype.Actions)
            {
                Actions.Add(actionFactory.BindAction(this, action.ActionId, action.Configuration));
            }


        }

        public void ReceiveCommand(AgentActionCommand command) {
            throw new System.NotImplementedException();
        }


        public virtual void Update(float frameDelta)
        {
            if (Controller == null) Controller = new AIAgentController(this, _sensorFactory.NewAgentSensor(this));
            //Do something
        }



        public void AddAction(AgentAction action)
        {
            action.Agent = this;
            Actions.Add(action);
        }

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