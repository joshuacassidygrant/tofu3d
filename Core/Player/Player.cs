using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using UnityEngine;

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
