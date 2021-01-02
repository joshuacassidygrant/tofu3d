using System.Collections.Generic;
using TofuCore.Glops;

namespace TofuCore.Service
{
    public interface IServiceContext
    {
        void Drop(string name);
        void Bind(string name, IService service, bool overwrite);
        dynamic Fetch(string name);
        IGlopContainer FetchContainer(string name);
        bool Has(string name);
        void AddAlias(string key, string alias);
        Glop FindGlopById(int id);
        void FullInitialization();
        void FullInitialization(IService service);
        int LastGlopId { get; set; }
        Dictionary<string, IGlopContainer> GetGlopContainers();
        void SetLastGlopId(int id);
    }
}