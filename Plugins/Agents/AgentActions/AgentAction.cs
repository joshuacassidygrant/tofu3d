using System.Collections.Generic;
using System.Dynamic;
using Scripts;
using TofuPlugin.Agents.AI;
using TofuCore.Configuration;

namespace TofuPlugin.Agents.AgentActions
{
    public abstract class AgentAction: IConfigurable
    {
        public Agent Agent;
        public string Id;
        public string Name;
        public float Cooldown;
        public float CurrentCooldown;
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
