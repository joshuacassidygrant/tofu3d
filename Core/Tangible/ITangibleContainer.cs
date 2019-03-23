using System.Collections.Generic;
using TofuCore.Tangible;

public interface ITangibleContainer
{

    List<ITangible> GetAllTangibles();
    List<ITangible> GetAllTangiblesWithinRangeOfPoint(UnityEngine.Vector3 point, float range);
    //TODO: this
}
