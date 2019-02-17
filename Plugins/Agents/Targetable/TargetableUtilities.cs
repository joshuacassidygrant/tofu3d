/*
 * A stateless collection of comparers and related functions for use elsewhere in Tofu3D and your project.
 */
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace TofuPlugin.Agents.Targetable
{
    public static class TargetableUtilities
    {

        //Gets the closest Targetable in a list
        public static ITargetable GetClosest(ITargetable targetable, List<ITargetable> objects)
        {
            if (objects.Count == 0) return null;

            //If it is within another ITargetables radius, take the closest ITargetable center it's within, ignoring radius
            List<ITargetable> targetsWithin =
                objects.Where(t => (t.Position - targetable.Position).magnitude - t.SizeRadius < 0)
                        .OrderBy(t => (t.Position - targetable.Position).sqrMagnitude).ToList();
            if (targetsWithin.Count > 0)
            {
                return targetsWithin[0];
            }
                    
            //Else, return the closest targetable based on other targetable's size radii
            List <ITargetable> targetsOrdered = objects.OrderBy(t => 
                (t.Position - targetable.Position).magnitude - t.SizeRadius).ToList();
            if (targetsOrdered.Count == 0) return null;
            return targetsOrdered[0];

        }

        /*
         * This method will return negative numbers for overlapping targets
         */
        private static float GetDistanceBetweenUnclamped(ITargetable t1, ITargetable t2)
        {
            return (t1.Position - t2.Position).magnitude - t1.SizeRadius - t2.SizeRadius;
        }

        //Finds the distance between two ITargetable by the closest points on their radii
        public static float GetDistanceBetween(ITargetable t1, ITargetable t2)
        {
            return Mathf.Max(GetDistanceBetweenUnclamped(t1,t2), 0f);
        }

        //Checks if two targetables overlap
        public static bool DoTargetablesOverlap(ITargetable t1, ITargetable t2)
        {
            return GetDistanceBetweenUnclamped(t1, t2) <= 0;
        }
        
    }

}
