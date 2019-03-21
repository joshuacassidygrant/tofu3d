using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.Targetable;
using TofuPlugin.Agents.Sensors;

/*
 * Allows queries to be run on objects in the given ServiceContext
 */
namespace TofuPlugin.Agents.Sensors
{
    public abstract class AbstractSensor
    {
        //TODO: this should be an array of sensible managers
        //TODO: this should use the positioning service
        protected GlopContainer<ITargetable> Manager;
        protected ServiceContext Context;

        

        public AbstractSensor(ServiceContext context)
        {
            Context = context;
        }

        public abstract List<ISensable> GetAllSensables();

    }
}
