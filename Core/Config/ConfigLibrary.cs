using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using TofuConfig;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Config
{
    public class ConfigLibrary : AbstractService
    {
        private Dictionary<ConfigKey, string> _config; 

        public ConfigLibrary()
        {
            _config = new Dictionary<ConfigKey, string>();
        }

        public void LoadConfig(string json)
        {
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (KeyValuePair<string, string> entry in values)
            {
                ConfigKey key;
                bool succ = Enum.TryParse(entry.Key, true, out key);
                if (!succ)
                {
                    Debug.LogWarning($"No entry in ConfigKey enum for {entry.Key}");
                } 
                else if (_config.ContainsKey(key))
                {
                    Debug.LogWarning($"Overwriting config entry for ${key} from ${_config[key]} to {entry.Value}");
                    _config.Remove(key);
                }

                _config.Add(key, entry.Value);
            }
        }

        public bool TryGetValueString(ConfigKey key, out string val)
        {
            if (!_config.ContainsKey(key))
            {
                val = null;
                return false;
            }

            val = _config[key];
            return true;
        }

        public bool TryGetValueInteger(ConfigKey key, out int val)
        {
            if (!_config.ContainsKey(key)) {
                val = 0;
                return false;
            }

            return int.TryParse(_config[key], NumberStyles.Integer, null, out val);

        }

        public bool TryGetValueFloat(ConfigKey key, out float val)
        {
            if (!_config.ContainsKey(key)) {
                val = 0;
                return false;
            }

            return float.TryParse(_config[key], NumberStyles.Float, null, out val);
        }

        public float GetValueFloat(ConfigKey key, float defaultVal = 0f)
        {
            float val = defaultVal;
            return TryGetValueFloat(key, out val) ? val : defaultVal;
        }

        public int GetValueInteger(ConfigKey key, int defaultVal = 0) {
            int val = defaultVal;
            return TryGetValueInteger(key, out val) ? val : defaultVal;
        }

        public string GetValueString(ConfigKey key, string defaultVal = "") {
            string val = defaultVal;
            return TryGetValueString(key, out val) ? val : defaultVal;
        }


    }
}
