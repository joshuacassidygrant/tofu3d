namespace TofuCore.Player
{
    public interface IController
    {
        bool IsLocalPlayer();
        IControllable Controlling { get; }
        void UpdateController(float delta);
    }
}
