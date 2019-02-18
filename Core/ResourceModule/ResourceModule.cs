using System;
using TofuCore.Events;
using TofuCore.Targetable;
using UnityEngine;


namespace TofuCore.ResourceModule
{
    public class ResourceModule
    {


        public readonly string Name;
        

        public int IValue => Mathf.RoundToInt(_value);
        public int IMax => Mathf.RoundToInt(_max);
        public float FValue => _value;
        public float FMax => _max;

        private string _depletionEventKey;
        private string _fullDepletionEventKey;
        private EventPayload _fullDepletionEventPayload;
        private string _replenishEventKey;

        private IResourceModuleOwner _owner;

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

        public ResourceModule(string name, float max, float val, IResourceModuleOwner owner, EventContext eventContext)
        {
            Name = name;
            _value = val;
            _max = max;
            _eventContext = eventContext;
            _owner = owner;
        }

        public void Deplete(float amount, string additionalDepletionEventKey = null, EventPayload additionalPayload = null)
        {

            _value -= amount;

            if (_depletionEventKey != null)
            {
                _eventContext.TriggerEvent(_depletionEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.white, new TargetablePosition(_owner.GetPosition()), (int)Math.Round(amount)), _eventContext));
            }

            if (_value <= 0)
            {
                if (_fullDepletionEventKey != null && _fullDepletionEventPayload != null)
                {
                    _eventContext.TriggerEvent(_fullDepletionEventKey, _fullDepletionEventPayload);
                }

                if (additionalDepletionEventKey != null && _value <= 0)
                {
                    _eventContext.TriggerEvent(additionalDepletionEventKey, additionalPayload);
                }
            }

        }

        public void BindFullDepletionEvent(string key, EventPayload payload)
        {
            _fullDepletionEventKey = key;
            _fullDepletionEventPayload = payload;
        }

        public void SetDepletionEventKey(string key)
        {
            _depletionEventKey = key;
        }

        public void SetReplenishEventKey(string key)
        {
            _replenishEventKey = key;
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

        public void SetValue(float amount)
        {
            _value = amount;
        }

        public void SetMaxRetainPercent(float amount)
        {
            float percent = Percent;
            SetMax(amount);
            _value = FMax * percent;
        }

        public void Replenish(float amount)
        {
            _value = Mathf.Min(_value + amount, FMax);
            if (_replenishEventKey != null)
            {
                _eventContext.TriggerEvent(_replenishEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.green, new TargetablePosition(_owner.GetPosition()), (int)Math.Round(amount)), _eventContext));
            }
        }

    }

}
