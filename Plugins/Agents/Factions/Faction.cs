using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Player;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class Faction: Glop
    {

        //Who controls this allegiance

        //List of other allegiances and their relationships
        private Dictionary<Faction, int> _relationships;

        public string IdName;
        public Player Controller;

        public Faction(string idName, string niceName, int id, ServiceContext serviceContext) : base(id, idName, serviceContext)
        {
            Name = niceName;
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

        public void SetController(Player controller)
        {
            Controller = controller;
        }

        public override void Update(float frameDelta)
        {
            //Do something!
        }
    }

}

