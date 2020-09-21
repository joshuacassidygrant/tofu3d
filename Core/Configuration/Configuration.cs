using System.Collections.Generic;

namespace TofuCore.Configuration
{
    /*
     * A Configuration object is consumed by an IConfigurable to generate a properties dictionary
     */
    [System.Serializable]
    public class Configuration
    {
        public List<ConfigurationProperty> Properties = new List<ConfigurationProperty>();
        public Dictionary<string, string> _properties = new Dictionary<string, string>();

        public void AddProperty(string id, dynamic value)
        {
            Properties.Add(new ConfigurationProperty(id, value.ToString()));
            _properties.Add(id, value.ToString());
        }

        public List<ConfigurationProperty>.Enumerator GetEnumerator()
        {
            return Properties.GetEnumerator();
        }

        public Configuration(Dictionary<string, string> kvps)
        {
            if (kvps == null)
            {
                return;
            }

            foreach (KeyValuePair<string, string> kvp in kvps)
            {
                AddProperty(kvp.Key, kvp.Value);
            }
        }

        public Configuration(Dictionary<string, dynamic> kvps)
        {
            if (kvps == null) {
                return;
            }

            foreach (KeyValuePair<string, dynamic> kvp in kvps) {
                AddProperty(kvp.Key, kvp.Value);
            }
        }


        public float GetProperty(string id, float defaultValue) {
            if (!_properties.ContainsKey(id)) {
                return defaultValue;
            }

            float result;
            if (float.TryParse(_properties[id], out result)) return result;

            return defaultValue;
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

            int result;
            if (int.TryParse(_properties[id], out result)) return result;

            return defaultValue;
        }

        public bool GetProperty(string id, bool defaultValue) {
            if (!_properties.ContainsKey(id)) {
                return defaultValue;
            }


            bool result;
            if (bool.TryParse(_properties[id], out result)) return result;

            return defaultValue;
        }


        public Configuration()
        {

        }
    }

    [System.Serializable]
    public class ConfigurationProperty
    {
        public string Key; 
        public string Value;

        public ConfigurationProperty(string id, string value)
        {
            Key = id;
            Value = value;
        }

        public int ValueAsInt(int def)
        {
            int result;

            if (int.TryParse(Value, out result))
            {
                return result;
            }

            return def;
        }

        public float ValueAsFloat(float def)
        {
            float result;

            if (float.TryParse(Value, out result))
            {
                return result;
            }
            return def;
        }

        public bool ValueAsBool(bool def)
        {
            bool result;

            if (bool.TryParse(Value, out result)) {
                return result;
            }

            return def;
        }
    }
}
