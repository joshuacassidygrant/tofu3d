using System.Collections.Generic;
using TofuCore.Configuration;
using TofuPlugin.Agents.AgentActions;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentPrototype : ScriptableObject {

        public string Name;
        public string Id;
        public Sprite Sprite;

        public List<PrototypeActionEntry> Actions;
        public Configuration Config;
    }
}
