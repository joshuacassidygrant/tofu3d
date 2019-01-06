using System.Collections.Generic;
using System.Dynamic;
using Scripts;
using TofuPlugin.Agents.AI;
using TofuCore.Configuration;

namespace TofuPlugin.Agents.AgentActions
{
    /*
     * AgentActions are functions that can be held by Agents and triggered by Commands
     * when specific criteria are met. Actions may take a set time to execute (i.e. an 
     * attack enemy action may factor in a short swing) or take a variable time (i.e.
     * walking along a path may take any number of frames to complete. 
     * 
     * A single AgentAction refers to a distinct entry within an Agent's action list.
     */ 
    public abstract class AgentAction: IConfigurable
    {
        public Agent Agent;
        public string Id;
        public string Name;
        
        /*
         * Cooldown refers to the time until this agent can use this action again
         */
        public float Cooldown;
        public float CurrentCooldown;

        /* 
         * DownTime refers to the time AFTER this is triggered before the agent can 
         * use another action.
         */
        public float DownTime;
        public float CurrentDownTime;

        /*
         * FocusTime refers to the amount of time required to focus on triggering an 
         * action. This could be 0 or very low for something quick (an attack, for 
         * instance) or longer for something more complicated (casting a ritual).
         * Some actions might allow focus for a variable amount of time up to a maximum
         * time.
         */
        public float FocusTime;
        public float CurrentFocusTime;
        public bool VariableFocus;
        public float MaximiumFocus;

        //The range at which this action can be activated
        public float Range;

        private Dictionary<string, dynamic> _properties = new Dictionary<string, dynamic>();

        protected AgentAction(string id, string name)
        {
            Id = id;
            Name = name;

        }

        public abstract ITargettable TargettingFunction();

        public virtual bool CanUse()
        {
            return 
                (Cooldown <= 0 &&
                TargettingFunction() != null);
        }

        public virtual void UseEffect(ITargettable target)
        {
            CurrentCooldown = Cooldown;
        }

        public virtual void Configure(Configuration config)
        {
            SetProperty("Cooldown", 0f);
            SetProperty("Range", 0f);
            //Do something
        }

        public Dictionary<string, dynamic> GetProperties()
        {
            return _properties;
        }

        public float GetProperty(string id, float defaultValue)
        {
            if (!_properties.ContainsKey(id))
            {
                return defaultValue;
            }
            return _properties[id];
        }

        public string GetProperty(string id, string defaultValue)
        {
            if (!_properties.ContainsKey(id) || (string)_properties[id] == null)
            {
                return defaultValue;
            }

            return _properties[id];
        }

        public int GetProperty(string id, int defaultValue)
        {
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

        public void SetProperty(string id, dynamic value)
        {
            if (_properties.ContainsKey(id))
            {
                _properties[id] = value;
                return;
            }

            _properties.Add(id, value);
        }

    }
}
