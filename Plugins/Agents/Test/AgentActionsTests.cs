using System.Collections.Generic;
using NUnit.Framework;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AgentActions.Fake;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using TofuPlugin.Agents.Targetable;
using TofuCore.Events;

namespace TofuPlugin.Agents.Tests
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
            _fakeActionFactory.BindServiceContext(_context, "AgentActionFactory");
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
        public void TestUnitSelfActionTargets()
        {
            //TODO: this
            AgentAction selfAction = _agent.Actions[1];
            //ITargetable target = selfAction.TargetingFunction();
            //Assert.AreEqual(target, _agent);
        }

        [Test]
        public void TestActionShouldPassThroughPhases()
        {
            AgentAction selfAction = _agent.Actions[1];
            selfAction.Cooldown = 5f;
            selfAction.FocusTime = 2f;

            Assert.AreEqual(ActionPhase.READY, selfAction.Phase);

            selfAction.TriggerAction(_agent);
            Assert.AreEqual(ActionPhase.FOCUS, selfAction.Phase);

            _agent.Update(1f);
            Assert.AreEqual(ActionPhase.FOCUS, selfAction.Phase);

            _agent.Update(1f);
            Assert.AreEqual(ActionPhase.COOLDOWN, selfAction.Phase);

            _agent.Update(4f);
            Assert.AreEqual(ActionPhase.COOLDOWN, selfAction.Phase);

            _agent.Update(1f);
            Assert.AreEqual(ActionPhase.READY, selfAction.Phase);

        }




    }
}
