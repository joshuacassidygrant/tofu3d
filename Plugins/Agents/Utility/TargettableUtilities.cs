/*
 * A stateless collection of comparers and related functions for use elsewhere in Tofu3D and your project.
 */
using System.Collections.Generic;
using System.Linq;
using TofuPlugin.Agents;
using UnityEngine;

namespace TofuPlugin.Agent.Utility
{
    public static class TargettableUtilities
    {

        //Gets the closest Vector3 in a list
        public static ITargettable GetClosest(Vector3 point, List<ITargettable> objects)
        {
            List<ITargettable> targetsOrdered = objects.OrderBy(t => (t.Position - point).sqrMagnitude).ToList();
            if (targetsOrdered.Count == 0) return null;
            return targetsOrdered[0];

        }

    }

}
