using TofuCore.Glops;
using TofuCore.Service;

namespace TofuCore.Glops
{
    public interface IGlopModule
    {
        void Bind(Glop glop, IServiceContext context);
        void ResolveAfterDeserialize(Glop glop, IServiceContext context);
    }
}
