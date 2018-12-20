using System;
using System.Collections.Generic;

/*
 * Bound to service context and provides any set up
 * that service classes need.
 */
namespace TofuCore.Service
{
    public class ServiceFactory
    {
        //private readonly ServiceContext _serviceContext;

        private static readonly Dictionary<string, Func<IService, IService>> InitFilters = new Dictionary<string, Func<IService, IService>>
        {
            {"UnitManager", FilterUnitManager}
        };

        public ServiceFactory (ServiceContext serviceContext)
        {
            //_serviceContext = serviceContext;
        }

        public IService Build(IService service)
        {
            string serviceName = service.GetType().ToString();
            service.Build();

            if (InitFilters.ContainsKey(serviceName))
            {
                service = InitFilters[serviceName](service);
            }

        
            return service;
        }

        private static IService FilterUnitManager(IService unitManager)
        {
            //Perform additional setup for this class here.
            return unitManager;
        }
    }
}
