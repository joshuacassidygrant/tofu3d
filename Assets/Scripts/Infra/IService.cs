using System;

public interface IService
{
    String[] Dependencies { get; }
    void Build();
    void ResolveServiceBindings();
    void Initialize();
    void BindServiceContext(ServiceContext serviceContext);
    string GetServiceName();
    
}
