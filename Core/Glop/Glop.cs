using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Glop
{
    /*
     * A GLOP is a Generalized Local Object or Process. Subclass this
     * and manage it with a GlopManager.
     */
    public abstract class Glop {
        public int Id;
        public string Name;

        protected Glop(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract void Update(float frameDelta);
    }

}
