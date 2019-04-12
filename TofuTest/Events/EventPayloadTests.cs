using NUnit.Framework;
using TofuCore.Events;
using UnityEditor.VersionControl;

namespace TofuTest.Events
{
    public class EventPayloadTests
    {
        [Test]
        public void TestPayloadConstructs()
        {
            EventPayload payload = new EventPayload("string", "Test");

            Assert.NotNull(payload);
            Assert.AreEqual(payload.GetContent(), "Test");
            Assert.AreEqual("string", payload.ContentType);
        }

        [Test]
        public void TestPayloadGetContentDynamic()
        {
            EventPayload payload = new EventPayload("integer", 4);
            EventPayload payloadFloat = new EventPayload("float", 2.3f);

            Assert.AreEqual(2.3f, payloadFloat.GetContent());
            Assert.AreEqual(4, payload.GetContent());
        }

        [Test]
        public void TestPayloadNullContentAccepted()
        {
            EventPayload payload = new EventPayload("null", null);

            Assert.NotNull(payload);
            Assert.Null(payload.GetContent());
        }
    }
}

