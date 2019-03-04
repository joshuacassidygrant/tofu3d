using TofuCore.Configuration;

namespace TofuPlugin.Agents.AgentActions
{
    [System.Serializable]
    public class PrototypeActionEntry
    {
        public string ActionId;
        public Configuration Configuration;

        public PrototypeActionEntry(string actionId, Configuration config = null)
        {
            ActionId = actionId;
            Configuration = config;
        }
    }
}
