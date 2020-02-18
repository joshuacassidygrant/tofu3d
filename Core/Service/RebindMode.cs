namespace TofuCore.Service
{
    /*
     * Defines behaviour when we bind a service that is already bound.
     */
    public enum RebindMode
    {
        REBIND_IGNORE,          // On rebind, ignores initialization step.
        REBIND_REINITIALIZE,    // On rebind, performs the initialization step.
        THROW_ERROR             // On rebind, throw and error.
    }
}
