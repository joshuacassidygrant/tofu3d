using UnityEngine;

namespace TofuPlugin.Renderable
{
    public interface IRenderable
    {
        int GetId();

        string GetSortingLayer();

        Sprite Sprite {
            get;
        }

        Vector3 Position {
            get;
        }
        //Add animation stuff too.

    }
}
