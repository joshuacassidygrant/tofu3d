namespace TofuCore.Command
{

    public abstract class Command
    {


        public abstract bool TryExecute();

    }

    public enum CommandCode
    {
        SUCCESS,
        FAIL,
        PROCESS,
        DELAY
    }
}
