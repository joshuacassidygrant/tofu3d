using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TofuCore.Configuration;
using UnityEditor.VersionControl;
using UnityEngine;

namespace TofuTest
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
            _config.AddProperty("tFloat", 12.5f);
            _config.AddProperty("tBool", true);
        }

        [Test]
        public void TestParseProperties()
        {
            Properties props = new Properties(_config);
            Assert.AreEqual(12, props.GetProperty("tInt", 13));
            Assert.AreEqual("test", props.GetProperty("tString", "default"));
            Assert.AreEqual(12.5f, props.GetProperty("tFloat", 1f));
            Assert.AreEqual(true, props.GetProperty("tBool", false));

        }

        [Test]
        public void TestUnloadedPropertiesShouldFallBackToDefault()
        {
            Properties props = new Properties(_config);
            Assert.AreEqual(13, props.GetProperty("barf", 13));
            Assert.AreEqual("jazz", props.GetProperty("xarf", "jazz"));
            Assert.AreEqual(13.4, props.GetProperty("blart", 13.4f), 0.1f);

        }

        [Test]
        public void TestAddingPropertiesShouldOverwriteSpecifiedProperties()
        {
            Properties props = new Properties(_config);
            Configuration newConfig = new Configuration();
            newConfig.AddProperty("tInt", 23);
            newConfig.AddProperty("tZarf", "aaa");
            props.Configure(newConfig);

            Assert.AreEqual(23, props.GetProperty("tInt", 444));
            Assert.AreEqual("aaa", props.GetProperty("tZarf", "zzz"));
            Assert.AreEqual(12.5f, props.GetProperty("tFloat", 13.4f));
        }

    }
}

