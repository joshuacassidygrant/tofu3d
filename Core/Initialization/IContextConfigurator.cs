namespace TofuCore.Service
{
    /*
     * Takes in a service context, modifies, and returns it. Avoid other side effects.
     */ 
    public interface IContextConfigurator
    {

        void Configure(ServiceContext context);

    }

}