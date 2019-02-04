using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Targetable
{
    public class TargetableDefault : ITargetable
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

