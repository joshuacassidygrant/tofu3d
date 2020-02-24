using System.Collections;
using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Glops
{
    /*
     * A GLOP is a Generalized Local Object or Process. Subclass this
     * and manage it with a GlopManager.
     *
     * GLOPs receive updates from their managers, generally with a framedelta
     * float telling how much time has passed since the last frame.
     *
     * GLOPs should be contain serializable data, serializable integer references
     * to other glops by ID. Other data must be constructible from factories and
     * contained references.
     */
    public abstract class Glop {
        public int Id { get; set;}

        public virtual void Die()
        {
            Garbage = true;
        }

        /**
         * To be called after dependencies are injected and Glop is registered to container.
         */
        public virtual void Initialize()
        {
            // Do something!
        }

        public virtual void InitializeFromLoad()
        {
            // Do something!
        }

        public bool Garbage { get; protected set; }


        public virtual void Update(float frameDelta)
        {
            // Update based on passed time
        }


    }

}
