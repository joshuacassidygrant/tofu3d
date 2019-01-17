using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Scripts.Agents.Strategy;
using TofuPlugin.Agents;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AgentActions.Fake;
using TofuPlugin.Agents.AI.Strategy;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;

namespace TofuPlugin.Agents.Tests
{
    public class AgentAIControllerTests
    {
        private AgentPrototype _prototype;
        private FakeAgentActionFactory _factoryFake;
        private AgentSensorFactory _sensorFactory;
        private ServiceContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();

            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>();

            _sensorFactory = new AgentSensorFactory();
            _sensorFactory.BindServiceContext(_context);

        }

        [Test]
        public void TestAgentCreatesOwnAIControllerByDefault()
        {
            Agent a = new Agent(12, _prototype, Vector3.zero, _context);
            Assert.Null(a.Controller);
            a.Update(0f);
            Assert.NotNull(a.Controller);
            Assert.NotNull(a.Controller.GetSensor());
            Assert.AreEqual("AIStrategyDefault", a.Controller.GetStrategyName());
        }

        [Test]
        public void TestCanChangeAgentStrategy()
        {
            Agent a = new Agent(12, _prototype, Vector3.zero, _context);
            a.Update(0f);
            a.Controller.SetStrategy(new AIStrategyFake());
            Assert.AreEqual("AIStrategyFake", a.Controller.GetStrategyName());

            a.Update(1f);
            Assert.AreEqual("AIStrategyFake", a.Controller.GetStrategyName());

            a.Controller.ClearStrategy();
            Assert.AreEqual("AIStrategyDefault", a.Controller.GetStrategyName());

        }

    }
}

