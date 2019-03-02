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
     */
    public abstract class Glop {
        public int Id;

        public virtual void Die()
        {
            Garbage = true;
        }

        public abstract void InjectDependencies(Dictionary<string, IContentInjectable> injectables);

        /**
         * To be called after dependencies are injected and Glop is registered to container.
         */
        public virtual void Initialize()
        {

        }

        public bool Garbage { get; protected set; }


        public abstract void Update(float frameDelta);


    }

}
