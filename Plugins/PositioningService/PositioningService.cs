using System.Collections.Generic;
using System.Linq;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.Tangible;
using TofuPlugin.Agents;
using TofuPlugin.Pathfinding.MapAdaptors;

/*
 * For checking if positions with regards to the Map and other units.
 */
namespace TofuPlugin.PositioningServices
{
    public class PositioningService : AbstractService
    {

        private List<ITangibleContainer> _targetableContainers = new List<ITangibleContainer>();

        [Dependency] private AgentContainer _agentContainer;
        [Dependency] private IPathableMapService _mapService;

        public void RegisterTargetableContainer(ITangibleContainer tangibleContainer)
        {
            _targetableContainers.Add(tangibleContainer);
        }

        public bool SpaceAtPosition(ITangible tangible, List<ITangible> ignore)
        {
            //TODO: fix this
            return !_targetableContainers.Any(xc => xc.GetAllTangibles()
                .Any(x => TangibleUtilities.DoTangiblesOverlap(x, tangible) && !ignore.Contains(x)));

        }


    }
}

