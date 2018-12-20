using UnityEngine;

namespace TofuPlugin.Renderable
{
    public interface IRenderable
    {

        Sprite Sprite {
            get;
        }

        Vector3 Position {
            get;
        }
        //Add animation stuff too.

    }
}
