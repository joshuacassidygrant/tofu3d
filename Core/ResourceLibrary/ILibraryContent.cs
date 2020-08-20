using System.Collections.Generic;

namespace TofuCore.ResourceLibrary
{
    public interface IContentList<T>
    {

        Dictionary<string, T> Get();

    }

}
