﻿using TofuCore.Events;
using TofuCore.ResourceModule;
using TofuPlugin.Agents;

public struct AgentResourceModuleConfig
{
    public string Key;
    public string ConfigPropertyMax;
    public string FullDepletionEventName;
    public string PayloadContentTypeName;
    public string DepletionEventName;
    public string ReplenishEventName;

    public AgentResourceModuleConfig(string key, string configPropertyMax, string fullDepletionEvent, string payloadContentType, string depletionEventName, string replenishEventName)
    {
        Key = key;
        ConfigPropertyMax = configPropertyMax;
        FullDepletionEventName = fullDepletionEvent;
        PayloadContentTypeName = payloadContentType;
        DepletionEventName = depletionEventName;
        ReplenishEventName = replenishEventName;
    }

    public ResourceModule GenerateResourceModule(Agent agent, EventContext eventContext)
    {
        ResourceModule resourceModule = new ResourceModule("HP", agent.Properties.GetProperty("HpMax", 0), agent.Properties.GetProperty("HpMax", 0), agent, eventContext);
        resourceModule.BindFullDepletionEvent(FullDepletionEventName, new EventPayload(PayloadContentTypeName, agent, eventContext));
        resourceModule.SetDepletionEventKey(DepletionEventName);
        resourceModule.SetReplenishEventKey(ReplenishEventName);
        return resourceModule;
    }

}