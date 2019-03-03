using System.Collections.Generic;
using TofuCore.ResourceLibrary;
using TofuPlugin.Agents;

public class AgentTypeLibrary : AbstractResourceLibrary<AgentType> {
    public AgentTypeLibrary(string path, string prefix = "") : base(path, prefix)
    {
        //TODO: parametrize this out to configure from outside tofu
    }

    public override void LoadResources()
    {
        //TODO: take this out of here
        _contents.Add("Unit", new AgentType("Unit", new HashSet<string>
        {
            "Speed"
        }, new List<string>()
        {
            "idle",
            "move",
            "attack",
            "ranged",
            "heal",
            "moveToObjective"
        }));

        _contents.Add("Structure", new AgentType("Structure", new HashSet<string>
        {
        }, null));


    }
}
