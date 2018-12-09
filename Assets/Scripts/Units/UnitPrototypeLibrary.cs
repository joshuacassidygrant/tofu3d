using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitPrototypeLibrary : AbstractResourceLibrary {

    private Dictionary<string, UnitPrototype> _creaturePrototypes;

    public override void LoadResources()
    {
        string fullPath = Prefix + "/" + Path;
        _creaturePrototypes = Resources.LoadAll<UnitPrototype>(fullPath).ToDictionary(u => u.Id, u => u);
    }

    public UnitPrototype GetUnit(UnitType unitType, string id)
    {
        if (_creaturePrototypes.ContainsKey(id)) return _creaturePrototypes[id];

        return null;
    }

    public int CountMembers(UnitType unitType)
    {
        return _creaturePrototypes.Count;
    }

    public UnitPrototypeLibrary() : base(typeof(UnitPrototype), "Creatures")
    {


    }
}
