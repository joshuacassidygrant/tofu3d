namespace TofuCore.Tangible
{
    public struct EventPayloadStringTangible
    {
        public ITangible Target;
        public string Value;

        public EventPayloadStringTangible(ITangible target, string value)
        {
            Target = target;
            Value = value;
        }
    }
}

