using UnityEngine;

namespace TofuCore.Utility
{
    public static class MathUtilities {

        public static Vector3Int RoundToVector3Int(Vector3 v3)
        {
            return new Vector3Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y), Mathf.RoundToInt(v3.z));
        }

        public static Vector3Int RoundDownToVector3Int(Vector3 v3)
        {
            return new Vector3Int(Mathf.FloorToInt(v3.x), Mathf.FloorToInt(v3.y), Mathf.FloorToInt(v3.z));
        }

        public static Vector3 Vector3IntToVector3(Vector3Int v3i)
        {
            return new Vector3(v3i.x, v3i.y, v3i.z);
        }
    }
}
