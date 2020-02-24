using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents.Factions;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public interface IFactionBelongable
    {
        IFactionComponent FactionComponent { get; }
        Faction Faction { get; set; }
        int FactionId { get; set; }


    }

}
