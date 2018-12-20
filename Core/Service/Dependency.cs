using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Service
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class Dependency : System.Attribute
    {
        public string Name;


        public Dependency(string name)
        {
            Name = name;
        }

        public Dependency()
        {

        }
    }
}

