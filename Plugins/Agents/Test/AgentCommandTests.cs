using NUnit.Framework;
using TofuPlugin.Agents.AgentActions.Test;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Targetable;

namespace TofuPlugin.Agents.AgentActions.Tests
{
    public class AgentCommandTests
    {
        private AgentAction _action1;
        

        [SetUp]
        public void SetUp()
        {
            _action1 = new AgentActionFake("fake1", "FakeAction");
        }

        [Test]
        public void TestAgentCommandConstructorShouldWork()
        {
            AgentCommand command = new AgentCommand(_action1, new TargetableDefault(), 10);
            
            Assert.AreEqual(_action1, command.Action);
            Assert.AreEqual(10, command.Priority);

        }
    }

}
