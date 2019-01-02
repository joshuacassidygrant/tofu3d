using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using UnityEngine;


namespace TofuPlugin.ResourceModule
{
    public class ResourceModule
    {


        public readonly string Name;
        

        public int IValue => Mathf.RoundToInt(_value);
        public int IMax => Mathf.RoundToInt(_max);
        public float FValue => _value;
        public float FMax => _max;

        public float Percent
        {
            get
            {
                if (_max == 0) return 0;
                return _value / _max;
            }
        }

        private float _value;
        private float _max;
        private readonly EventContext _eventContext;

        public ResourceModule(string name, float max, float val, EventContext eventContext)
        {
            Name = name;
            _value = val;
            _max = max;
            _eventContext = eventContext;
        }

        public void Deplete(float amount, string depletionEventKey = null, EventPayload depletionPayload = null)
        {
            _value -= amount;
            if (depletionEventKey != null && _value <= 0)
            {
                _eventContext.TriggerEvent(depletionEventKey, depletionPayload);
            }
        }

        public bool Spend(float amount)
        {
            if (CanSpend(amount))
            {
                _value -= amount;
                return true;
            }

            return false;
        }

        public bool CanSpend(float amount)
        {
            return _value - amount >= 0;
        }

        public void SetMax(float amount)
        {
            _max = amount;
        }

        public void SetMaxRetainPercent(float amount)
        {
            float percent = Percent;
            SetMax(amount);
            _value = FMax * percent;
        }

    }

}
