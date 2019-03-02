
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
            Player player = new Player(idName);
            Register(player);
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
