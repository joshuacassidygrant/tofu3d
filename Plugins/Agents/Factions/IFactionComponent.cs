using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public interface IFactionComponent
    {


        Faction Faction { get; set; }

        FactionRelationshipLevel GetRelationshipWith(IFactionBelongable other);

        List<string> GetFactionPermissions(IFactionBelongable other);

        bool PermissionToDo(string factionAction, IFactionBelongable other);
    }

}
