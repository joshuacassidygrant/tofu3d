using System.Collections;
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

        private static List<FactionRelationshipLevel> _relationships;

        public void Configure(List<FactionRelationshipLevel> relationships)
        {
            relationships.Sort(CompareTo);
            _relationships = relationships;
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

        public static FactionRelationshipLevel GetFactionRelationship(int amount)
        {
            foreach (FactionRelationshipLevel rel in _relationships)
            {
                if (rel.Min >= amount) return rel;
            }
            return new FactionRelationshipLevel(0, "Unaffiliated");
        }


    }

}
