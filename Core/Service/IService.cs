using System;

/*
 * Interface for services classes.
 */

namespace TofuCore.Service
{
    public interface IService
    {
        void Build();
        void ResolveServiceBindings();
        void Initialize();
        dynamic BindServiceContext(ServiceContext serviceContext, string bindingName);
        string GetServiceName();
    
    }
}
