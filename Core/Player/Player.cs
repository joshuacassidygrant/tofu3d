using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.ResourceModule;
using UnityEngine;

namespace TofuCore.Player
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : Glop
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public Vector3 Position { get; private set; }
        public IControllable Controlling;


        public Player(string name) : base()
        {
            Name = name;
        }


    }

}
