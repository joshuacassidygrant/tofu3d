using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Scripts.Agents.Behaviour;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AgentActions.Fake;
using TofuPlugin.Agents.AI.Behaviour;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;

namespace TofuPlugin.Agents.Tests
{
    public class AgentAIControllerTests
    {
        private AgentPrototype _prototype;
        private FakeAgentActionFactory _factoryFake;
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
            Agent a = new Agent(_prototype, Vector3.zero);
            Assert.Null(a.Controller);
            _agentContainer.Register(a);
            a.Update(0f);
            Assert.NotNull(a.Controller);
            Assert.NotNull(a.Controller.GetSensor());
            Assert.AreEqual("AiBehaviourDefault", a.Controller.GetBehaviourName());
        }

        [Test]
        public void TestCanChangeAgentStrategy()
        {
            Agent a = new Agent(_prototype, Vector3.zero);
            a.Update(0f);
            a.Controller.SetBehaviour(new AiBehaviourFake());
            Assert.AreEqual("AiBehaviourFake", a.Controller.GetBehaviourName());

            a.Update(1f);
            Assert.AreEqual("AiBehaviourFake", a.Controller.GetBehaviourName());

            a.Controller.ClearBehaviour();
            Assert.AreEqual("AiBehaviourDefault", a.Controller.GetBehaviourName());

        }

    }
}

