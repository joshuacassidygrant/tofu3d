/*
 * A stateless collection of comparers and related functions for use elsewhere in Tofu3D and your project.
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TofuPlugin.Agents.Targetable
{
    public static class TargettableUtilities
    {

        //Gets the closest Vector3 in a list
        public static ITargetable GetClosest(Vector3 point, List<ITargetable> objects)
        {
            List<ITargetable> targetsOrdered = objects.OrderBy(t => (t.Position - point).sqrMagnitude).ToList();
            if (targetsOrdered.Count == 0) return null;
            return targetsOrdered[0];

        }

    }

}
