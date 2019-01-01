using UnityEngine;

namespace Scripts.Sensors
{
    /*
     * A Sensable object exists in a Vector3 position and defines
     * a sensable radius.
     */
    public interface ISensable
    {
        Vector3 Position { get; }
        float SizeRadius { get; }
    }
}
