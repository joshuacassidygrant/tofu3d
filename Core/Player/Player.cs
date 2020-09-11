using System.Runtime.InteropServices;
using Newtonsoft.Json;
using TofuCore.Glops;
using UnityEngine;

namespace TofuCore.Player
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : Glop, IController
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public Vector3 Position { get; private set; }
        public IControllable Controlling { get; set; }

        private bool _local;

        public Player(string name) : base()
        {
            Name = name;
        }


        public bool IsLocalPlayer()
        {
            return _local;
        }

        public void UpdateController(float delta)
        {
            //
        }
    }

}
