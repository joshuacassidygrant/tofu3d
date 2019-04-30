using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.Tangible;

/*
 * Allows queries to be run on objects in the given ServiceContext
 */
namespace TofuCore.Sensors
{
    public abstract class AbstractSensor
    {
        //TODO: this should be an array of sensible managers
        //TODO: this should use the positioning service
        protected GlopContainer<ITangible> Manager;
        protected IServiceContext Context;

        

        public AbstractSensor(IServiceContext context)
        {
            Context = context;
        }

        public abstract List<ITangible> GetAllTargetables();

    }
}
