using System.Collections.Generic;
using TofuCore.Service;

/*
 * Allows queries to be run on objects in the given ServiceContext
 */
namespace Scripts.Sensors
{
    public abstract class AbstractSensor
    {

        protected ServiceContext Context;

        

        public AbstractSensor(ServiceContext context)
        {
            Context = context;
        }

        public abstract List<ISensable> GetAllSensables();

    }
}
