
using System.Collections.Generic;
using TofuCore.Glops;
using UnityEngine;

namespace TofuCore.Player
{
    public class PlayerContainer : GlopContainer<Player>
    {
        private Dictionary<string, Player> _playersByString = new Dictionary<string, Player>();

        public string GetCurrentPlayerId()
        {
            // TODO
            return "player1";
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

        public IControllable GetCurrentPlayerControllable()
        {
            return GetPlayerByName(GetCurrentPlayerId()).Controlling;
        }
    }
}
