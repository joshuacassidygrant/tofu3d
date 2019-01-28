using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;

/*
 * Allows queries to be run on objects in the given ServiceContext
 */
namespace Scripts.Sensors
{
    public abstract class AbstractSensor
    {
        //TODO: this should be an array of sensible managers
        protected GlopContainer Manager;
        protected ServiceContext Context;

        

        public AbstractSensor(ServiceContext context)
        {
            Context = context;
        }

        public abstract List<ISensable> GetAllSensables();

    }
}
