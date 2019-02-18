using TofuCore.Glops;
using TofuCore.Service;

namespace TofuCore.Player
{
    public class Player : Glop
    {
        //TODO this
        //Probably abstract for AI vs. human player

        

        public Player(int id, string name, ServiceContext context) : base(id, context)
        {
        }

        public override void Update(float frameDelta)
        {
        }
    }

}
