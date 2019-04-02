using System.Collections.Generic;
using TofuCore.Configuration;
using TofuPlugin.Agents.AgentActions;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

namespace TofuPlugin.Agents
{
    [CreateAssetMenu(fileName = "AgentPrototype", menuName = "Data/AgentPrototype", order = 1)]
    public class AgentPrototype : ScriptableObject {

        public string Name;
        public string Id;
        public Sprite Sprite;
        public RuntimeAnimatorController Animator;
        public string AgentType;
        public int HpMax;
        public Color BaseColor;
        public float SizeRadius;

        public List<PrototypeActionEntry> Actions;
        public Configuration Config;

        public static AgentPrototype GetNew()
        {
            return CreateInstance<AgentPrototype>();
        }
    }
}
