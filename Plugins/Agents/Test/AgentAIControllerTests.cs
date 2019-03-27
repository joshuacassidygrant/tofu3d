using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AI.Behaviour;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;
using TofuPlugin.Agents.Behaviour;

namespace TofuPlugin.Agents.Tests
{
    public class AgentAIControllerTests
    {
        private AgentPrototype _prototype;
        private AgentActionFactory _factoryFake;
        private AgentSensorFactory _sensorFactory;
        private ServiceContext _context;
        private AgentContainer _agentContainer;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();

            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>();

            _sensorFactory = new AgentSensorFactory().BindServiceContext(_context);
            _agentContainer = new AgentContainer().BindServiceContext(_context);

        }

        [Test]
        public void TestAgentCreatesOwnAIControllerByDefault()
        {
            Agent a = new Agent();
            Assert.Null(a.Controller);
            a.Sensor = new AgentSensor(_context, a);
            a.Update(0f);
            Assert.NotNull(a.Controller);
            Assert.NotNull(a.Controller.GetSensor());
            Assert.AreEqual("AiBehaviourDefault", a.Controller.GetBehaviourName());
        }

        [Test]
        public void TestCanChangeAgentStrategy()
        {
            Agent a = new Agent();
            a.Update(0f);

            AIBehaviour subBehaviour = Substitute.For<AIBehaviour>();
            subBehaviour.GetName().Returns("AiBehaviourSub");

            a.Controller.SetBehaviour(subBehaviour);
            Assert.AreEqual("AiBehaviourSub", a.Controller.GetBehaviourName());

            a.Update(1f);
            Assert.AreEqual("AiBehaviourSub", a.Controller.GetBehaviourName());

            a.Controller.ClearBehaviour();
            Assert.AreEqual("AiBehaviourDefault", a.Controller.GetBehaviourName());

        }

    }
}

