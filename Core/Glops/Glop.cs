using System.Collections;
using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Glops
{
    /*
     * A GLOP is a Generalized Local Object or Process. Subclass this
     * and manage it with a GlopManager.
     */
    public abstract class Glop {
        public int Id;
        public string Name;
        protected ServiceContext ServiceContext;

        protected Glop(int id, string name, ServiceContext context)
        {
            Id = id;
            Name = name;
            ServiceContext = context;
        }

        public virtual void Die()
        {
            //Do something!
        }

        public abstract void Update(float frameDelta);


    }

}
