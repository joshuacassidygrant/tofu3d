using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.UI;
using UnityEngine;

namespace TofuPlugin.UI
{
    public class TofuUiBase : MonoBehaviour
    {
        protected UIBindingService UiBindingService;
        protected ServiceContext ServiceContext;
        protected EventContext EventContext;

        protected void BindServiceContext() {
            if (UiBindingService == null)
            {
                UiBindingService = FindObjectOfType<UIBindingService>();
            }

            if (UiBindingService != null)
            {
                ServiceContext = UiBindingService.GetServiceContext();
                EventContext = ServiceContext.Fetch("EventContext");
            }
        }
    }
}

