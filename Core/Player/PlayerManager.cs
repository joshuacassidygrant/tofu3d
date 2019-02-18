
using System.Collections.Generic;
using TofuCore.Glops;

namespace TofuCore.Player
{
    public class PlayerContainer : GlopContainer
    {
        private Dictionary<string, Player> _playersByString;

        public override void Initialize()
        {
            base.Initialize();
            _playersByString = new Dictionary<string, Player>();
        }

        public Player Create(string idName)
        {
            int id = GenerateGlopId();
            Player player = new Player(id, idName, ServiceContext);
            Contents.Add(id, player);
            _playersByString.Add(idName, player);
            return player;

        }

        public Player GetPlayerByName(string idName)
        {
            if (!_playersByString.ContainsKey(idName)) return null;
            return _playersByString[idName];
        }

    }
}
