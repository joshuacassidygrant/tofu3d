using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class FactionRelationshipLevel
    {

        public int Min;
        public string Name;
        public readonly List<string> Permissions;

        public FactionRelationshipLevel(int min, string name, List<string> permissions)
        {
            Min = min;
            Name = name;
            Permissions = permissions;
             
        }

        public bool Can(string permission)
        {
            return Permissions.Contains(permission);
        }

        //Add queryable list of things you can do at this level?  
    }

}

