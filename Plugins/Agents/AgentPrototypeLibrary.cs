using System.Linq;
using TofuCore.ResourceLibrary;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentPrototypeLibrary : AbstractResourceLibrary<AgentPrototype>
    {
        [Dependency] protected AgentTypeLibrary AgentTypeLibrary;
        private string _agentTypeKey;
        public AgentType AgentType;

        public override void LoadResources()
        {
            if (Prefix != "") Prefix = Prefix + "/";
            string fullPath = Prefix + Path;
            //Debug.Log("Loading resources from " + fullPath);
            _contents = Resources.LoadAll<AgentPrototype>(fullPath).ToDictionary(u => u.Id, u => u);
        }

        public AgentPrototypeLibrary(string folder, string agentTypeKey, string prefix) : base(folder, prefix)
        {
            _agentTypeKey = agentTypeKey;
        }

        public override void Initialize()
        {
            base.Initialize();
            AgentType = AgentTypeLibrary.Get(_agentTypeKey);

        }
    }
}
