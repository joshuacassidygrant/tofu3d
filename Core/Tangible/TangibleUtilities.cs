/*
 * A stateless collection of comparers and related functions for use elsewhere in Tofu3D and your project.
 */
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace TofuCore.Tangible
{
    public static class TangibleUtilities
    {

        //Gets the closest Tangible in a list
        public static ITangible GetClosest(ITangible tangible, List<ITangible> objects)
        {
            if (objects.Count == 0) return null;

            //If it is within another ITargetables radius, take the closest ITangible center it's within, ignoring radius
            List<ITangible> targetsWithin =
                objects.Where(t => (t.Position - tangible.Position).magnitude - t.SizeRadius < 0)
                        .OrderBy(t => (t.Position - tangible.Position).sqrMagnitude).ToList();
            if (targetsWithin.Count > 0)
            {
                return targetsWithin[0];
            }
                    
            //Else, return the closest tangible based on other tangible's size radii
            List <ITangible> targetsOrdered = objects.OrderBy(t => 
                (t.Position - tangible.Position).magnitude - t.SizeRadius).ToList();
            if (targetsOrdered.Count == 0) return null;
            return targetsOrdered[0];

        }

        /*
         * This method will return negative numbers for overlapping targets
         */
        private static float GetDistanceBetweenUnclamped(ITangible t1, ITangible t2)
        {
            return (t1.Position - t2.Position).magnitude - t1.SizeRadius - t2.SizeRadius;
        }

        //Finds the distance between two ITangible by the closest points on their radii
        public static float GetDistanceBetween(ITangible t1, ITangible t2)
        {
            return Mathf.Max(GetDistanceBetweenUnclamped(t1,t2), 0f);
        }

        //Checks if two tangibles overlap
        public static bool DoTangiblesOverlap(ITangible t1, ITangible t2)
        {
            return GetDistanceBetweenUnclamped(t1, t2) <= 0;
        }
        
    }

}
