using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentManager : AbstractService, ISensableContainer {
        public virtual List<ISensable> GetAllSensables()
        {
            return new List<ISensable>();
        }

        public List<ISensable> GetAllSensablesWithinRangeOfPoint(Vector3 point, float range)
        {
            return GetAllSensables().Where(x => (point - x.Position).magnitude <= range).ToList();
        }
    }
}