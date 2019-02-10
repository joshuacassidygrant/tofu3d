using System.Collections.Generic;
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

        Dictionary<string, bool> GetAnimationStateBools();
        //Add animation stuff too.

    }
}
