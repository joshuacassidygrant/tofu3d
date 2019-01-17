using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using UnityEngine;

namespace TofuCore.Player
{
    public class Player : Glops.Glop
    {
        //TODO this
        //Probably abstract for AI vs. human player

        public Player(int id, string name, ServiceContext context) : base(id, name, context)
        {
        }

        public override void Update(float frameDelta)
        {
            throw new System.NotImplementedException();
        }
    }

}
