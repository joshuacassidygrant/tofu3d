using NSubstitute.Routing.Handlers;
using UnityEngine;


namespace TofuCore.ResourceHolder {

    // v0.0.7: this class and interface are meant to supplant the weird ResourceModule implementation;
    // removes float representation/storage and event handling

    public interface IResourceHolder
    {
        string Name { get;}
        IResourceHolderOwner Owner { get; set; }

        int Value { get; set; }
        int Max { get; set; }

        float Percent { get;}
        int Deplete(int amount, bool forceIfResultLessThan0 = false);
        int Replenish(int amount, bool keepOverrun = false);
        int SetMaxRetainPercent(int amount);

        bool CanSpend(int amount);

    }

    public interface IResourceHolderOwner
    {
        IResourceHolder GetResourceHolder(string name);
    }

    public class ResourceHolder: IResourceHolder
    {
        public string Name { get; }
        public IResourceHolderOwner Owner { get; set; }
        public int Value { get; set; }
        public int Max { get; set; }
        public float Percent => Max == 0 ? 0 : (float) Value / Max;

        public int Deplete(int amount, bool forceIfResultLessThan0 = false)
        {
            return Value = !forceIfResultLessThan0 && !CanSpend(amount) ? Value : Value - amount;
        }

        public int Replenish(int amount, bool keepOverrun = false)
        {
            return Value = !keepOverrun && amount > Max - Value ? Max : Value + amount;
        }

        public int SetMaxRetainPercent(int amount)
        {
            float percent = Percent;
            Max = amount;
            return Value = Mathf.CeilToInt(Max * percent);
        }

        public bool CanSpend(int amount)
        {
            return amount <= Value;
        }

        public ResourceHolder(string name, int max, int val, IResourceHolderOwner owner = null)
        {
            Value = val;
            Max = max;
            Name = name;
            Owner = owner;
        }
    }


}