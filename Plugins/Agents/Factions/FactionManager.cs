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
        }

        public void Configure()
        {
            //Set breakpoints for certain faction level interactions
            // i.e. <20 is hostile, 20+ is allied, 
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
    }

}
