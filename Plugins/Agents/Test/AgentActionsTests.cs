using System.Collections.Generic;
using NUnit.Framework;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AgentActions.Fake;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;

namespace Tests
{
    public class AgentActionsTests
    {
        private AgentPrototype _prototype;
        private FakeAgentActionFactory _fakeActionFactory;
        private AgentSensorFactory _sensorFactory;
        private Agent _agent;
        private Agent _unit2;
        private ServiceContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();

            _fakeActionFactory = new FakeAgentActionFactory();
            _fakeActionFactory.BindServiceContext(_context);
            _fakeActionFactory.Build();
            _fakeActionFactory.Initialize();
            _fakeActionFactory.AddAction("act", () => new AgentActionFake("act", "Act"));
            _fakeActionFactory.AddAction("self", () => new AgentSelfActionFake("self", "SelfAct"));

            _sensorFactory = new AgentSensorFactory();
            _sensorFactory.BindServiceContext(_context);

            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();

            _prototype.Id = "test";
            _prototype.Name = "test";
            _prototype.Actions =
                new List<PrototypeActionEntry>
                {
                    new PrototypeActionEntry("act"),
                    new PrototypeActionEntry("self")
                };
            _agent = new Agent(1, _prototype, Vector3.zero, _context);
            _unit2 = new Agent(2, _prototype, Vector3.left, _context);
        }

        [Test]
        public void TwoUnitsShouldNotShareTheSameAction() {

            //TODO: this
            Assert.AreNotSame(_agent.Actions[0], _unit2.Actions[0]);
        }

        [Test]
        public void TestUnitHasAction()
        {
            Assert.AreEqual(2, _agent.Actions.Count);
            AgentAction fakeAction = _agent.Actions[0];
            Assert.AreEqual(_agent.Id, ((Agent)fakeAction.Agent).Id);
        }

        [Test]
        public void TestUnitSelfAction()
        {
            AgentAction selfAction = _agent.Actions[1];
            ITargettable target = selfAction.TargettingFunction();
            Assert.AreEqual(target, _agent);
        }


    

    }
}
