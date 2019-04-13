using System;
using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Events;
using UnityEngine;

namespace TofuTest.Events
{
    public class EventPayloadTypeLibraryTests
    {
        private EventPayloadTypeLibrary _payloadLibrary;

        [SetUp]
        public void SetUp()
        {
            _payloadLibrary = new EventPayloadTypeLibrary( new Dictionary<string, Func<object, bool>>
            {
                {"Float", x => x is float },
                {"Integer", x => x is int },
                {"String", x => x is string },
                {"GameObject", x => x is GameObject },
            });
        }
        
        [Test]
        public void TestRegisteredTypes()
        {
            EventPayload floatPayload = new EventPayload("Float", 1.2f);
            EventPayload intPayload = new EventPayload("Integer", 1);
            EventPayload stringPayload = new EventPayload("String", "Test");
            EventPayload gameObjectPayload = new EventPayload("GameObject", new GameObject("Test"));

            Assert.True(_payloadLibrary.ValidatePayload(floatPayload));
            Assert.True(_payloadLibrary.ValidatePayload(intPayload));
            Assert.True(_payloadLibrary.ValidatePayload(stringPayload));
            Assert.True(_payloadLibrary.ValidatePayload(gameObjectPayload));
        }

        [Test]
        public void TestInvalidAndUnregisteredTypes()
        {
            EventPayload unregisteredType = new EventPayload("Boolean", true);
            EventPayload invalidType = new EventPayload("String", 2);

            Assert.False(_payloadLibrary.ValidatePayload(unregisteredType));
            Assert.False(_payloadLibrary.ValidatePayload(invalidType));
        }

        [Test]
        public void TestCheckContentTypeAs()
        {
            Assert.True(_payloadLibrary.CheckContentAs("String", "Test"));
            Assert.False(_payloadLibrary.CheckContentAs("Integer", 2.4f));

        }
    }

}
