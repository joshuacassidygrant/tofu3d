using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents.Factions;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public interface IFactionBelongable
    {
        Faction Faction { get; }

        FactionRelationshipLevel GetRelationshipWith(IFactionBelongable ifb);

        List<string> GetFactionPermissions(IFactionBelongable ifb);

        bool PermissionToDo(string factionAction, IFactionBelongable ifb);
    }

}
