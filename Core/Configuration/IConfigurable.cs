using System.Collections.Generic;

/*
 * IConfigurable objects hold a properties object with string, int/float/string parameters
 */
namespace TofuCore.Configuration
{
    public interface IConfigurable
    {

        void Configure(Configuration config);
        Dictionary<string, dynamic> GetProperties();
        string GetProperty(string id, string defaultValue);
        int GetProperty(string id, int defaultValue);
        float GetProperty(string id, float defaultValue);
        bool GetProperty(string id, bool defaultValue);
        void SetProperty(string id, dynamic value);
    }
}
