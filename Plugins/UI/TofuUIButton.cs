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

        private Button _button;

        void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            BindServiceContext();
        }

        private void OnClick()
        {
            EventContext.TriggerEvent(EventName);
        }


    }
}

