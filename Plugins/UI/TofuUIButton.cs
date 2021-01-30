using System;
using System.Collections;
using System.Collections.Generic;
using TofuConfig;
using TofuPlugin.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TofuPlugin.UI
{
    [RequireComponent(typeof(Button))]
    public class TofuUIButton : TofuUiBase
    {

        public string EventName;
        private EventKey _eventKey;

        protected Button Button;

        void Awake()
        {
              Button = GetComponent<Button>();
        }

        void Start()
        {
            if (Enum.TryParse(EventName, false, out _eventKey))
            {
                Button.onClick.AddListener(OnClick);
                BindServiceContext();
            }

        }

        public void SetEvent(string name)
        {
            EventName = name;
            if (Enum.TryParse(EventName, false, out _eventKey))
            {
                Button.onClick.AddListener(OnClick);
                BindServiceContext();
            }
        }

        private void OnClick()
        {
            EventContext.TriggerEvent(_eventKey);
        }

        protected void SetTrigger(string eventName)
        {
            EventName = eventName;
        }


    }
}

