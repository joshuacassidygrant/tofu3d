using System;

namespace TUFFYCore.Service
{
    public interface IService
    {
        String[] Dependencies { get; }
        void Build();
        void ResolveServiceBindings();
        void Initialize();
        void BindServiceContext(ServiceContext serviceContext, string bindingName);
        string GetServiceName();
    
    }
}
