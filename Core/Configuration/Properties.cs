using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Configuration
{
    /*
     * A properties object holds a key-value pair dictionary of values that extend an IConfigurable object.
     */
    public class Properties
    {

        Dictionary<string, string> _properties;

        public Properties()
        {
            _properties = new Dictionary<string, string>();
        }

        public Properties(Configuration config)
        {
            _properties = new Dictionary<string, string>();
            if (config == null) return;
            Configure(config);
        }

        public void Configure(Configuration config)
        {
            if (config == null) return;

            foreach (ConfigurationProperty entry in config.Properties)
            {
                SetProperty(entry.Key, entry.Value);
            }
        }

        public Dictionary<string, string> GetProperties()
        {
            return _properties;
        }

        public float GetProperty(string id, float defaultValue)
        {
            if (!_properties.ContainsKey(id))
            {
                return defaultValue;
            }

            float result;
            if (float.TryParse(_properties[id], out result)) return result;

            return defaultValue;
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
            if (!_properties.ContainsKey(id))
            {
                return defaultValue;
            }

            int result;
            if (int.TryParse(_properties[id], out result)) return result;

            return defaultValue;
        }

        public bool GetProperty(string id, bool defaultValue)
        {
            if (!_properties.ContainsKey(id))
            {
                return defaultValue;
            }


            bool result;
            if (bool.TryParse(_properties[id], out result)) return result;

            return defaultValue;
        }

        public void SetProperty(string id, dynamic value)
        {
            if (_properties.ContainsKey(id))
            {
                _properties[id] = value.ToString();
                return;
            }

            _properties.Add(id, value.ToString());
        }

        public bool Check(HashSet<string> expected)
        {
            HashSet<string> _checklist = new HashSet<string>(expected);

            foreach (var entry in _properties) 
            {
                if (_checklist.Contains(entry.Key))
                {
                    _checklist.Remove(entry.Key);
                }
            }

            if (_checklist.Count == 0) return true;

            Debug.Log("Could not find " + _checklist.Count + " expected properties");
            return false;
        }
    }

}
