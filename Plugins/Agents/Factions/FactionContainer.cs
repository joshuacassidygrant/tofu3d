using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Glops;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class FactionContainer : GlopContainer<Faction>
    {

        public FactionRelationshipLevel Unaffiliated;
        public FactionRelationshipLevel Same;

        public FactionContainer()
        {
            _relationships = new List<FactionRelationshipLevel>();
            Unaffiliated = new FactionRelationshipLevel(0, "Unaffiliated", new List<string>());
            Same = new FactionRelationshipLevel(0, "Same", new List<string>());
        }

        public void SetSame(FactionRelationshipLevel level)
        {
            Same = level;
        }

        public void SetUnaffiliated(FactionRelationshipLevel level)
        {
            Unaffiliated = level;
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
            Faction faction = new Faction(idName, niceName);
            Register(faction);
            return faction;
        }

        public Faction GetFactionByIdName(string idName)
        {
            return GetContents().Cast<Faction>().FirstOrDefault(x => (x.IdName == idName));
        }

        public FactionRelationshipLevel GetFactionRelationship(IFactionBelongable agent, Faction faction)
        {
            if (agent.Faction == faction) return Same;
            if (faction == null) return Unaffiliated;
            return GetFactionRelationship(agent.Faction.GetRelationship(faction));
        }

        public FactionRelationshipLevel GetFactionRelationship(IFactionBelongable agent, IFactionBelongable other)
        {
            return GetFactionRelationship(agent, other.Faction);
        }

        public FactionRelationshipLevel GetFactionRelationship(int amount)
        {

            if (_relationships == null || _relationships.Count == 0)
            {
                return new FactionRelationshipLevel(0, "Unaffiliated", new List<string>());
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
