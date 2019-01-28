
using TofuCore.Glops;

namespace TofuCore.Player
{
    public class PlayerContainer : GlopContainer
    {

        public Player Create(string idName)
        {
            int id = GenerateGlopId();
            Player player = new Player(id, idName, ServiceContext);
            Contents.Add(id, player);
            return player;

        }

    }
}
