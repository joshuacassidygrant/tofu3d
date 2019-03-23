using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Tangible
{
    public class TangibleDefault : ITangible
    {
        public Vector3 Position => Vector3.zero;
        public bool Active => true;
        public float SizeRadius => 0f;
    }
}

