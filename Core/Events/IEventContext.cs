using System;
using TofuCore.Service;

namespace TofuCore.Events
{
    public interface IEventContext
    {
        IEventPayloadTypeContainer GetPayloadTypeContainer();
        void TriggerEvent(string eventKey, EventPayload payload);
        TofuEvent GetEvent(string name);
        void HelperBindEventListener(TofuEvent evnt, IListener listener);
        void RemoveEventListener(TofuEvent evnt, IListener listener);
        void ResolveServiceBindings();
        void Prepare();
        void Initialize();
        dynamic BindServiceContext(ServiceContext serviceContext, string bindingName = null);
        string GetServiceName();
        bool CheckDependencies();
        void ReceiveEvent(TofuEvent evnt, EventPayload payload);
    }
}