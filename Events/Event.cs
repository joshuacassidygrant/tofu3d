public class Event
{
    public string Name;
    public int CallCount;

    public Event(string name)
    {
        Name = name;
        CallCount = 0;
    }

    public Event HasBeenCalled()
    {
        CallCount++;
        return this;
    }

    public override bool Equals(object obj)
    {
        return obj is Event && Name == ((Event) obj).Name;
    }

    public bool Equals(Event other)
    {
        return string.Equals(Name, other.Name);
    }

    public override int GetHashCode()
    {
        return (Name != null ? Name.GetHashCode() : 0);
    }
}
