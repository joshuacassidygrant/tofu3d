using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TofuCore.Events;
using TofuCore.Player;
using TofuCore.ResourceModule;
using TofuPlugin.UI;
using UnityEngine;
using UnityEngine.UI;

public class UiTextDynamic : TofuUiBase
{

    public string EventKey;
    public string PayloadType;
    public int DecimalPointsForFloat = 0;
    public string PlayerNameSelector;

    private Text _text;

	private void Start ()
    {
        if (!string.IsNullOrEmpty(EventKey))
        {
            BindServiceContext();
            BindEventListener();
        }

        _text = GetComponentInChildren<Text>();
    }

    private void BindEventListener()
    {
        EventContext.BindListener(EventContext.GetEvent(EventKey), OnReceiveUpdate, EventContext);
    }

    private void OnReceiveUpdate(EventPayload payload)
    {
        if (payload.ContentType != PayloadType) return;
        string value = HandleType(payload);
        _text.text = value;

    }

    private string HandleType(EventPayload payload)
    {
        //TODO: improve this.
        switch (payload.ContentType)
        {
            case "Float":
                float fVal = payload.GetContent();
                float pow = Mathf.Pow(10, DecimalPointsForFloat);
                int intVal = Mathf.FloorToInt(Mathf.FloorToInt(fVal * pow) / pow);
                return intVal.ToString();
            case "Integer":
                int iVal = payload.GetContent();
                return iVal.ToString();
            case "String":
                return payload.GetContent();
            case "ResourceEventPayload":
                ResourceEventPayload rePayload = payload.GetContent();
                Player player = rePayload.Target as Player;
                if (player != null && player.GetName() == PlayerNameSelector)
                {
                    return rePayload.Amount.ToString();
                }

                return _text.text;
            default:
                return "Type not found";
        }
        
    }

}
