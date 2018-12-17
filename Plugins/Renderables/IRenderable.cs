using UnityEngine;

namespace TUFFYPlugins.Renderable
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
