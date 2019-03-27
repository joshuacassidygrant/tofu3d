using System;
using System.Collections;
using System.Collections.Generic;
using TofuCore.ResourceLibrary;
using UnityEngine;


namespace TofuCore.TestSupport
{

    public class DummyLibrary : AbstractResourceLibrary<int>
    {

        public DummyLibrary(string path)
        {
        }

        public void Load(List<int> vals)
        {
            foreach (int val in vals)
            {
                _contents.Add(val.ToString(), val);
            }
        }

    }

}

