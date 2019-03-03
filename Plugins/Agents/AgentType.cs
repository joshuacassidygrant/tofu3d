using System.Collections.Generic;

public class AgentType
{
    public string Name;
    public HashSet<string> ExpectedProperties;

    public AgentType(string name, HashSet<string> expectedProperties)
    {
        Name = name;
        ExpectedProperties = expectedProperties;
    }

}
