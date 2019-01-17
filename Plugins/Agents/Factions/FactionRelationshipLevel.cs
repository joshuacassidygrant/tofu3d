using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class FactionRelationshipLevel
    {

        public int Min;
        public string Name;

        public FactionRelationshipLevel(int min, string name)
        {
            Min = min;
            Name = name;
             
        }
    }

}

