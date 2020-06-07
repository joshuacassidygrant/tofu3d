using System.Collections.Generic;
using UnityEngine;

public class AVFXRequest
{
    public List<EffectAVFXEntry> Entries;
    public Vector3 Origin;
    public Vector3 Target;

    public AVFXRequest(List<EffectAVFXEntry> entries, Vector3 origin, Vector3 target)
    {
        Entries = entries;
        Origin = origin;
        Target = target;
    }

}
