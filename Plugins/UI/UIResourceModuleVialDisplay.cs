using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Player;
using TofuCore.ResourceModule;
using TofuPlugin.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceModuleVialDisplay : TofuUiBase
{
    public string EventKey;
    public int OwnerGlopId;

    public Transform Inner;
    public float MaxScale;

    private void Start()
    {
        if (!string.IsNullOrEmpty(EventKey))
        {
            BindServiceContext();
            BindEventListener();
        }

    }



    private void BindEventListener()
    {
        EventContext.BindListener(EventContext.GetEvent(EventKey), OnReceiveUpdate, EventContext);
    }

    private void OnReceiveUpdate(EventPayload payload)
    {
        if (payload.ContentType != "ResourceStateEventPayload") return;
        ResourceStateEventPayload resourceState = payload.GetContent();
        if (resourceState.Target.Id != OwnerGlopId) return;

        float percent = (float)resourceState.Amount / resourceState.Max;
        Rerender(percent);
    }

    private void Rerender(float percent)
    {
        percent = Mathf.Min(1f, percent);
        Inner.localScale = new Vector3(Inner.localScale.x, Inner.localScale.y, MaxScale * percent);

    }
}
