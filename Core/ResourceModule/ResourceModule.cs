using System;
using System.Security.Policy;
using Newtonsoft.Json;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Glops;
using UnityEngine;
using UnityEngine.Experimental.XR;


namespace TofuCore.ResourceModule
{
    public interface IResourceModule
    {
        int IValue { get; }
        int IMax { get; }
        float FValue { get; }
        float FMax { get; }
        float Percent { get; }
        IResourceModuleOwner Owner { get; set; }
        void Deplete(float amount, EventKey additionalDepletionEventKey = EventKey.None, EventPayload additionalPayload = null);
        bool Spend(float amount);
        bool CanSpend(float amount);
        void Replenish(float amount, bool keepOverrun = false);
        void BindFullDepletionEvent(EventKey key, EventPayload payload);
        void SetDepletionEventKey(EventKey key);
        void SetReplenishEventKey(EventKey key);
        void SetChangeDeltaEventKey(EventKey key);
        void SetStateChangeEventKey(EventKey key);
        void SetMax(float amount);
        void SetValue(float amount);
        void SetMaxRetainPercent(float amount);
        Color BaseColor { get; }
        Color GlowColor { get; }
        Color SecondaryColor { get; }
        string GetCurrentMaxRatioString();
        string MaterialName { get; set; }
        void BindEventContext(IEventContext eventContext);
        void SetColors(Color baseColor, Color secColor, Color glowColor);
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ResourceModule : IResourceModule
    {
        [JsonProperty] public readonly string Name;
        [JsonProperty] public float Value { get; private set; }
        [JsonProperty] public float Max { get; private set; }

        [JsonIgnore] public IResourceModuleOwner Owner { get; set; }

        [JsonIgnore] public int IValue => Mathf.RoundToInt(Value);
        [JsonIgnore] public int IMax => Mathf.RoundToInt(Max);
        [JsonIgnore] public float FValue => Value;
        [JsonIgnore] public float FMax => Max;
        [JsonIgnore] public string MaterialName { get; set; }

        [JsonIgnore] private EventKey _depletionEventKey;
        [JsonIgnore] private EventKey _fullDepletionEventKey;
        [JsonIgnore] private EventPayload _fullDepletionEventPayload;
        [JsonIgnore] private EventKey _replenishEventKey;
        [JsonIgnore] private EventKey _changeDeltaEventKey;
        [JsonIgnore] private EventKey _stateChangeEventKey;

        [JsonIgnore]
        public float Percent
        {
            get
            {
                if (Math.Abs(Max) < 0.01f) return 0;
                return Value / Max;
            }
        }


        [JsonIgnore] private IEventContext _eventContext;

        public ResourceModule(string name, float max, float val, IResourceModuleOwner owner, IEventContext eventContext)
        {
            Name = name;
            Value = val;
            Max = max;

            _eventContext = eventContext;
            Owner = owner;
        }

        public ResourceModule(string name, float max, float val)
        {
            Name = name;
            Value = val;
            Max = max;
        }

        public void SetColors(Color baseColor, Color secColor, Color glowColor)
        {
            BaseColor = baseColor;
            GlowColor = glowColor;
            SecondaryColor = secColor;
        }

        /**
         * Depletes the resource module. Unlike "Spend", "Deplete" can go below 0. If deplete hits 0 or below, it can fire a special event (passed in)
         * as well as a deplete event with a resource module payload.
         */
        public void Deplete(float amount, EventKey additionalDepletionEventKey = EventKey.None, EventPayload additionalPayload = null)
        {

            Value -= amount;
            FireChangeEvent();

            if (_depletionEventKey != null)
            {
                _eventContext.TriggerEvent(_depletionEventKey, new EventPayload("ResourceEventPayload", new ResourceEventPayload(Color.white, Owner, (int)Math.Round(amount))));
            }

            if (Value <= 0)
            {
                if (_fullDepletionEventKey != null && _fullDepletionEventPayload != null)
                {
                    _eventContext.TriggerEvent(_fullDepletionEventKey, _fullDepletionEventPayload);
                }

                if (additionalDepletionEventKey != null && Value <= 0)
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
                Value -= amount;
                FireChangeEvent();
                return true;
            }

            return false;
        }

        public bool CanSpend(float amount)
        {
            return Value - amount >= 0;
        }

        /**
         * Adds to the resource's pool. If keepOverrun is true, can add past the maximum.
         */
        public void Replenish(float amount, bool keepOverrun = false)
        {
            if (!keepOverrun)
            {
                Value = Mathf.Min(Value + amount, FMax);
            }
            else
            {
                Value = Value + amount;
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
        public void BindFullDepletionEvent(EventKey key, EventPayload payload)
        {
            _fullDepletionEventKey = key;
            _fullDepletionEventPayload = payload;
        }

        public void SetDepletionEventKey(EventKey key)
        {
            _depletionEventKey = key;
        }

        public void SetReplenishEventKey(EventKey key)
        {
            _replenishEventKey = key;
        }

        public void SetChangeDeltaEventKey(EventKey key)
        {
            _changeDeltaEventKey = key;
        }

        public void SetStateChangeEventKey(EventKey key)
        {
            _stateChangeEventKey = key;
        }

        /**
         * Getters/Setters
         */
        public void SetMax(float amount)
        {
            Max = amount;
            Value = Mathf.Min(Max, Value);
        }

        public void SetValue(float amount)
        {
            Value = amount;
            FireChangeEvent();
        }

        public void SetMaxRetainPercent(float amount)
        {
            float percent = Percent;
            SetMax(amount);
            Value = FMax * percent;
            FireChangeEvent();
        }

        public Color BaseColor { get; private set; }
        public Color GlowColor { get; private set; }
        public Color SecondaryColor { get; private set; }

        public Material LoadMaterial()
        {
            return Resources.Load<Material>($"Materials/{MaterialName}");
        }

        public void BindEventContext(IEventContext eventContext)
        {
            _eventContext = eventContext;
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
            if (_changeDeltaEventKey == EventKey.None) return;
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
