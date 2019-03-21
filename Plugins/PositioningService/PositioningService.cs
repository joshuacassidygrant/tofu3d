using System.Collections.Generic;
using System.Linq;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.Targetable;
using TofuPlugin.Agents;
using TofuPlugin.Pathfinding.MapAdaptors;

/*
 * For checking if positions with regards to the Map and other units.
 */
namespace TofuPlugin.PositioningService
{
    public class PositioningService : AbstractService
    {

        private List<ITargetableContainer> _targetableContainers = new List<ITargetableContainer>();

        [Dependency] private AgentContainer _agentContainer;
        [Dependency] private IPathableMapService _mapService;

        public void RegisterTargetableContainer(ITargetableContainer targetableContainer)
        {
            _targetableContainers.Add(targetableContainer);
        }

        public bool SpaceAtPosition(ITargetable targetable, List<ITargetable> ignore)
        {
            //TODO: fix this
            return !_targetableContainers.Any(xc => xc.GetTargetables()
                .Any(x => TargetableUtilities.DoTargetablesOverlap(x, targetable) && !ignore.Contains(x)));

        }


    }
}

