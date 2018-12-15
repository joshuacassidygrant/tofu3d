using System;

/*
 * Interface for services classes.
 */

namespace TUFFYCore.Service
{
    public interface IService
    {
        void Build();
        void ResolveServiceBindings();
        void Initialize();
        void BindServiceContext(ServiceContext serviceContext, string bindingName);
        string GetServiceName();
    
    }
}
