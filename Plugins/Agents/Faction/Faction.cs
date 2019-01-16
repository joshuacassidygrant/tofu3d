using System.Collections;
using System.Collections.Generic;
using TofuCore.Glop;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents.Faction
{
    public class Faction: Glop
    {

        //Who controls this allegiance

        //List of other allegiances and their relationships
        private Dictionary<Faction, int> _relationships;

        public Faction(string idName, int id, ServiceContext serviceContext) : base(id, idName, serviceContext)
        {
            _relationships = new Dictionary<Faction, int>();
        }

        public void SetMutualRelationship (Faction faction, int value)
        {
            SetRelationship(faction, value);
            faction.SetRelationship(faction, value);

        }

        public void SetRelationship(Faction faction, int value)
        {
            if (!_relationships.ContainsKey(faction))
            {
                _relationships.Add(faction, value);
            }
            else
            {
                _relationships[faction] = value;
            }
        }

        public override void Update(float frameDelta)
        {
            //Do something!
        }
    }

}

