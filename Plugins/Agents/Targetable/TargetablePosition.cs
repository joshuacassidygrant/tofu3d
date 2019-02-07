using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Targetable
{
    public class TargetablePosition : ITargetable
    {
        private readonly Vector3 _position;
        float x => _position.x;
        float y => _position.y;
        float z => _position.z;


        public TargetablePosition(Vector3 position)
        {
            _position = position;
        }

        public Vector3 Position => _position;
        public bool Active => true;
        public float SizeRadius => 0f;
    }

}