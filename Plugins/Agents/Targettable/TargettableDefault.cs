using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Targettable
{
    public class TargettableDefault : ITargettable
    {
        public Vector3 Position {
            get {
                return Vector3.zero;
            }
        }

        public bool Active {
            get {
                return true;
            }
        }
    }
}

