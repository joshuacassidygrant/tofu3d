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

        public void AddProperty(string id, dynamic value)
        {
            Properties.Add(new ConfigurationProperty(id, value));
        }

        public List<ConfigurationProperty>.Enumerator GetEnumerator()
        {
            return Properties.GetEnumerator();
        } 
    }

    [System.Serializable]
    public class ConfigurationProperty
    {
        public string Key; 
        public dynamic Value;

        public ConfigurationProperty(string id, dynamic value)
        {
            Key = id;
            Value = value;
        }
    }
}
