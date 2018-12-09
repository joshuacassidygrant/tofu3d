using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilities {

    public static Vector3Int RoundToVector3Int(Vector3 v3)
    {
        return new Vector3Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y), Mathf.RoundToInt(v3.z));
    }
}
