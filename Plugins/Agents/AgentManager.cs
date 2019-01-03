using System.Collections.Generic;
using System.Linq;
using Scripts.Sensors;
using TofuCore.Glop;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentManager : GlopManager, ISensableContainer {

        public List<ISensable> GetAllSensables() {
            return Contents.Values.Cast<ISensable>().ToList();
        }

        public List<ISensable> GetAllSensablesWithinRangeOfPoint(Vector3 point, float range)
        {
            return GetAllSensables().Where(x => (point - x.Position).magnitude <= range).ToList();
        }
    }
}