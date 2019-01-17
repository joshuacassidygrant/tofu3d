using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using UnityEngine;

namespace TofuTests.Mock
{
    public class FakeGlop : Glop
    {
        public FakeGlop(int id, ServiceContext context) : base(id, "test", context)
        {
        }

        public override void Update(float frameDelta)
        {

        }
    }
}

