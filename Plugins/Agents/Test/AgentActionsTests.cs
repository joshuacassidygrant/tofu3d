using System;
using System.Collections.Generic;
using NUnit.Framework;
using TofuPlugin.Agents.AgentActions;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using TofuPlugin.Agents.AgentActions.Test;
using NSubstitute;

namespace TofuPlugin.Agents.Tests
{
    public class AgentActionsTests
    {
        //private AgentPrototype _prototype;

        private AgentContainer _subContainer;
        private Agent _agent;
        private Agent _agent2;
        private ServiceContext _context;
        

        [SetUp]
        public void SetUp()
        {

            _context = new ServiceContext();
            _subContainer = Substitute.For<AgentContainer>().BindServiceContext(_context);
            

            _agent = new Agent();
            _agent2 = new Agent();

            BindActionHelper(_agent, new AgentActionFake("act", "Act"));
            BindActionHelper(_agent, new AgentSelfActionFake("self", "SelfAct"));
            BindActionHelper(_agent2, new AgentActionFake("act", "Act"));
            BindActionHelper(_agent2, new AgentSelfActionFake("self", "SelfAct"));

            _agent.Sensor = new AgentSensor(_context, _agent);
            _agent2.Sensor = new AgentSensor(_context, _agent2);

        }



        [Test]
        public void TwoUnitsShouldNotShareTheSameAction() {

            //TODO: this
            Assert.AreNotSame(_agent.Actions[0], _agent2.Actions[0]);
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
            //ITangible target = selfAction.TargetingFunction();
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

        private void BindActionHelper(Agent agent, AgentAction action)
        {
            action.Agent = agent;
            action.InjectServiceContext(_context);
            action.BindDependencies();
            agent.Actions.Add(action);
        }

    }
}
