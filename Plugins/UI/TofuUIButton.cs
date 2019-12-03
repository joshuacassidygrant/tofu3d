using System.Collections;
using System.Collections.Generic;
using TofuPlugin.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TofuPlugin.UI
{
    [RequireComponent(typeof(Button))]
    public class TofuUIButton : TofuUiBase
    {

        public string EventName;

        protected Button Button;

        void Awake()
        {
              Button = GetComponent<Button>();
        }

        void Start()
        {
            Button.onClick.AddListener(OnClick);
            BindServiceContext();
        }

        private void OnClick()
        {
            EventContext.TriggerEvent(EventName);
        }

        protected void SetTrigger(string eventName)
        {
            EventName = eventName;
        }


    }
}

