using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Configuration;
using UnityEngine;

namespace TofuTests
{
    public class ConfigTests
    {

        private Configuration _config;

        [SetUp]
        public void SetUp()
        {
            _config = new Configuration();
            _config.AddProperty("tInt", 12);
            _config.AddProperty("tString", "test");
            _config.AddProperty("tFLoat", 12.5f);
            _config.AddProperty("tBool", true);
        }

        [Test]
        public void TestParseProperties()
        {
            
            //TODO
        }

    }
}

