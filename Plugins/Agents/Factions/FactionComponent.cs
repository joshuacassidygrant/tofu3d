﻿using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents.Factions;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class FactionComponent: IFactionComponent
    {

        public FactionComponent(IFactionBelongable belongable, FactionContainer factionContainer)
        {
            _boundBelongable = belongable;
            _factionContainer = factionContainer;
        }

        public Faction Faction { get; set; }

        private readonly IFactionBelongable _boundBelongable;
        private readonly FactionContainer _factionContainer;

        public FactionRelationshipLevel GetRelationshipWith(IFactionBelongable other)
        {
            return _factionContainer.GetFactionRelationship(_boundBelongable, other);
        }

        public List<string> GetFactionPermissions(IFactionBelongable agent)
        {
            return GetRelationshipWith(agent).Permissions;
        }

        public bool PermissionToDo(string factionAction, IFactionBelongable agent)
        {
            return GetFactionPermissions(agent).Contains(factionAction);
        }

    }
}

