﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Glops;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class FactionManager : GlopManager
    {

        public FactionManager()
        {
            _relationships = new List<FactionRelationshipLevel>();
        }

        private List<FactionRelationshipLevel> _relationships;

        public void Configure(List<FactionRelationshipLevel> relationships)
        {
            relationships.Sort(CompareTo);
            _relationships = relationships;

            //TODO: disallow overlapping range
        }

        private int CompareTo(FactionRelationshipLevel x, FactionRelationshipLevel y)
        {
            return x.Min.CompareTo(y.Min);
        }


        public Faction Create(string idName, string niceName)
        {
            int id = GenerateGlopId();
            Faction faction = new Faction(idName, niceName, id, ServiceContext);
            Contents.Add(id, faction);
            return faction;
        }

        public Faction GetFactionByIdName(string idName)
        {
            return Contents.Values.Cast<Faction>().FirstOrDefault(x => (x.IdName == idName));
        }

        public FactionRelationshipLevel GetFactionRelationship(int amount)
        {

            if (_relationships == null || _relationships.Count == 0)
            {
                return new FactionRelationshipLevel(0, "Unaffiliated");
            }

            //Remove edge cases
            if (amount >= _relationships[_relationships.Count - 1].Min) return _relationships[_relationships.Count - 1];
            if (amount <= _relationships[0].Min) return _relationships[0];

            //NOTE: If we have a very large number of levels, could be efficient to add a max to each
            //and run a binary search.

            for (int i = _relationships.Count - 2; i > 0; i--)
            {
                if (amount >= _relationships[i].Min) return _relationships[i] ;
            }

            return _relationships[0];

        }


    }

}
