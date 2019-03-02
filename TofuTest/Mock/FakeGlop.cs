﻿using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using UnityEngine;

namespace TofuTests.Mock
{
    public class FakeGlop : Glop
    {
        public override void InjectDependencies(Dictionary<string, IContentInjectable> injectables)
        {

        }

        public override void Update(float frameDelta)
        {

        }
    }
}

