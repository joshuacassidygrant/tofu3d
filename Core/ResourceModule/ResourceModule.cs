using System;
using System.Security.Policy;
using TofuCore.Events;
using TofuCore.Glops;
using UnityEngine;


namespace TofuCore.ResourceModule
{
    public interface IResourceModule
    {
        int IValue { get; }
        int IMax { get; }
        float FValue { get; }
        float FMax { get; }
        float Percent { get; }
        IResourceModuleOwner Owner { get; }
        void Deplete(float amount, string additionalDepletionEventKey = null, EventPayload additionalPayload = null);
        bool Spend(float amount);
        bool CanSpend(float amount);
        void Replenish(float amount, bool keepOverrun = false);
        void BindFullDepletionEvent(string key, EventPayload payload);
        void SetDepletionEventKey(string key);
        void SetReplenishEventKey(string key);
        void SetChangeDeltaEventKey(string key);
        void SetStateChangeEventKey(string key);
        void SetMax(float amount);
        void SetValue(float amount);
        void SetMaxRetainPercent(float amount);
        Material LoadMaterial();
        string GetCurrentMaxRatioString();
    }

    public class ResourceModule : IResourceModule
    {
        public readonly string Name;

        public int IValue => Mathf.RoundToInt(_value);
        public int IMax => Mathf.RoundToInt(_max);
        public float FValue => _value;
        public float FMax => _max;
        public string MaterialName;

        private string _depletionEventKey;
        private string _fullDepletionEventKey;
        private EventPayload _fullDepletionEventPayload;
        private string _replenishEventKey;
        private string _changeDeltaEventKey;
        private string _stateChangeEventKey;


        public float Percent
        {
            get
            {
                if (Math.Abs(_max) < 0.01f) return 0;
                return _value / _max;
            }
        }

        public IResourceModuleOwner Owner { get; private set; }

        private float _value;
        private float _max;
        private readonly IEventContext _eventContext;

        public ResourceModule(string name, float max, float val, string materialName, IResourceModuleOwner owner, IEventContext eventContext)
        {
            Name = name;
            _value = val;
            _max = max;
            MaterialName = materialName;
            _eventContext = eventContext;
            Owner = owner;
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
                _eventContext.TriggerEvent(_depletionEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.white, Owner, (int)Math.Round(amount))));
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
                _eventContext.TriggerEvent(_replenishEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.green, Owner, (int)Math.Round(amount))));
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

        public void SetChangeDeltaEventKey(string key)
        {
            _changeDeltaEventKey = key;
        }

        public void SetStateChangeEventKey(string key)
        {
            _stateChangeEventKey = key;
        }

        /**
         * Getters/Setters
         */
        public void SetMax(float amount)
        {
            _max = amount;
            _value = Mathf.Min(_max, _value);
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

        public Material LoadMaterial()
        {
            return Resources.Load<Material>($"Materials/{MaterialName}");
        }

        /**
         * Helpers
         */

        private void FireChangeEvent()
        {
            if (_eventContext == null)
            {
                Debug.LogWarning("No EventContext bound.");
                return;
            } 
            if (string.IsNullOrEmpty(_changeDeltaEventKey)) return;
            _eventContext.TriggerEvent(_changeDeltaEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.white, Owner, IValue)));
            _eventContext.TriggerEvent((_stateChangeEventKey), new EventPayload("ResourceStateEventPayload", new ResourceStateEventPayload(Color.white, Owner, IValue, IMax)));

        }


        /**
         * Display
         */

        public string GetCurrentMaxRatioString()
        {
            return $"{IValue}/{IMax}";
        }

    }

}
