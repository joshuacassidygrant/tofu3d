using System;
using TofuCore.Events;
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
        private string _changeEventKey;

        private IResourceModuleOwner _owner;

        public float Percent
        {
            get
            {
                if (Math.Abs(_max) < 0.01f) return 0;
                return _value / _max;
            }
        }

        private float _value;
        private float _max;
        private readonly IEventContext _eventContext;

        public ResourceModule(string name, float max, float val, IResourceModuleOwner owner, IEventContext eventContext)
        {
            Name = name;
            _value = val;
            _max = max;
            _eventContext = eventContext;
            _owner = owner;
        }

        /**
         * Depletes the resource module. Unlike "Spend", "Deplete" can go below 0. If deplete hits 0 or below, it can fire a special event (passed in)
         * as well as a deplete event with a resource module payload.
         */
        public void Deplete(float amount, string additionalDepletionEventKey = null, EventPayload additionalPayload = null)
        {

            _value -= amount;
            FireChangeEvent();

            if (_depletionEventKey != null)
            {
                _eventContext.TriggerEvent(_depletionEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.white, _owner, (int)Math.Round(amount))));
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

        /**
         * Spends the resource. True if resource >= to amount to spend; false if not. No change if not spent.
         */
        public bool Spend(float amount)
        {
            if (CanSpend(amount))
            {
                _value -= amount;
                FireChangeEvent();
                return true;
            }

            return false;
        }

        public bool CanSpend(float amount)
        {
            return _value - amount >= 0;
        }

        /**
         * Adds to the resource's pool. If keepOverrun is true, can add past the maximum.
         */
        public void Replenish(float amount, bool keepOverrun = false)
        {
            if (!keepOverrun)
            {
                _value = Mathf.Min(_value + amount, FMax);
            }
            else
            {
                _value = _value + amount;
            }

            FireChangeEvent();
            if (_replenishEventKey != null)
            {
                _eventContext.TriggerEvent(_replenishEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.green, _owner, (int)Math.Round(amount))));
            }
        }

        /**
         * For binding events:
         */
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

        public void SetChangeEventKey(string key)
        {
            _changeEventKey = key;
        }

        /**
         * Getters/Setters
         */
        public void SetMax(float amount)
        {
            _max = amount;
        }

        public void SetValue(float amount)
        {
            _value = amount;
            FireChangeEvent();
        }

        public void SetMaxRetainPercent(float amount)
        {
            float percent = Percent;
            SetMax(amount);
            _value = FMax * percent;
            FireChangeEvent();
        }

        /**
         * Helpers
         */

        private void FireChangeEvent()
        {
            if (string.IsNullOrEmpty(_changeEventKey)) return;
            _eventContext.TriggerEvent(_changeEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.white, _owner, IValue)));

        }

    }

}
