using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Renderable
{
    public interface IRenderable
    {
        int Id { get; }

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
