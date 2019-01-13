using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class TargettablePosition : ITargettable
    {
        Vector3 _position;
        float x => _position.x;
        float y => _position.y;
        float z => _position.z;


        public TargettablePosition(Vector3 position)
        {
            _position = position;
        }

        public Vector3 Position {
            get {
                return _position;
            }
        }

        public bool Active {
            get {
                return true;
            }
        }
    }

}