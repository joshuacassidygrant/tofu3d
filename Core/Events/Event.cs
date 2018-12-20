namespace TofuCore.Events
{
    public class TofuEvent
    {
        public string Name;
        public int CallCount;

        public TofuEvent(string name)
        {
            Name = name;
            CallCount = 0;
        }

        public TofuEvent HasBeenCalled()
        {
            CallCount++;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is TofuEvent && Name == ((TofuEvent)obj).Name;
        }

        public bool Equals(TofuEvent other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }

}