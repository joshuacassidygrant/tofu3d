using TofuConfig;

namespace TofuCore.Events
{
    public class TofuEvent
    {
        public readonly string Key;
        public int CallCount;

        public TofuEvent(string key)
        {
            Key = key;
            CallCount = 0;
        }

        public TofuEvent HasBeenCalled()
        {
            CallCount++;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is TofuEvent && Key == ((TofuEvent)obj).Key;
        }

        public bool Equals(TofuEvent other)
        {
            return string.Equals(Key, other.Key);
        }

        public override int GetHashCode()
        {
            return (Key.GetHashCode());
        }
    }

}