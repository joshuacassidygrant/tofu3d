using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Agents.Sensors
{
    /*
     * An ISensableContainer contains a queryable collection of ISensable
     * objects.
     */
    public interface ISensableContainer
    {

        List<ISensable> GetAllSensables();
        List<ISensable> GetAllSensablesWithinRangeOfPoint(Vector3 point, float range);

    }
}
