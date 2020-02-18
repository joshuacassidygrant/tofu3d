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
        void Prepare();
        void Initialize();
        dynamic BindServiceContext(IServiceContext serviceContext, string bindingName);
        string GetServiceName();
        bool Initialized { get; }
        RebindMode RebindMode { get; }
        
    }
}
