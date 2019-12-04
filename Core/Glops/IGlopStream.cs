using System.Collections.Generic;

namespace TofuCore.Glops
{
    public interface IGlopStream
    {
        List<Glop> GetContents();
        Glop GetGlopById(int id);
    }
}
