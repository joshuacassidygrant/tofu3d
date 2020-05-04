using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Tangible
{
    public class TangibleDefault : ITangible
    {
        public string Name => "Default";
        public Vector3 Position => Vector3.zero;
        public bool Active => true;
        public float SizeRadius => 0f;
        public int Id => 0;
        public bool Hidden => false;
        public List<string> Tags => new List<string>();
        public Sprite Sprite => null;
    }
}

