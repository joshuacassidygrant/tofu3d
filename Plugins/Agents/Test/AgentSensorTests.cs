using System.Collections.Generic;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using TofuPlugin.Agents;
using TofuCore.Events;
using TofuCore.Service;
using TofuCore.Tangible;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using UnityEngine.UI;

namespace TofuPlugin.Agents.Tests
{
    public class AgentSensorTests
    {
        private IServiceContext _context;
        private AgentSensor _sensor;
        private AgentSensorFactory _sensorFactory;
        private ITangibleContainer _tangibleContainer;
        private IAgentContainer _agentContainer;
        private IAgent _agent;
        private IEventContext _eventContext;


        [SetUp]
        public void SetUp()
        {
            _agentContainer = Substitute.For<IAgentContainer>();
            _tangibleContainer = Substitute.For<ITangibleContainer>();
            _eventContext = Substitute.For<IEventContext>();
            _agent = Substitute.For<IAgent>();

            _context = TestUtilities.BuildSubServiceContextWithServices(new Dictionary<string, object>
            {
                {"IAgentContainer", _agentContainer },
                {"IEventContext", _eventContext },
                {"ITangibleContainer", _tangibleContainer}
            });

            _sensor = new AgentSensor(_context, _agent);
        }

        [Test]
        public void TestSensorConstructs()
        {
            Assert.NotNull(_sensor);
            Assert.AreEqual(_agent, _sensor.Agent);
            Assert.AreEqual(0f, _sensor.SenseRange, 0.001f);
        }

        [Test]
        public void TestSetGetSensorRange()
        {
            _sensor.SenseRange = 10f;
            Assert.AreEqual(10f, _sensor.SenseRange);

            _sensor.SenseRange = 50f;
            Assert.AreEqual(50f, _sensor.SenseRange);

        }

        /*[Test]
        public void SensorShouldDetectNoOtherAgentsInRangeWhenNoAgentsSpawned()
        {
            List<Agent> agents = _sensor.GetAgentsInRange(10f);
    
            Assert.True(agents.Contains(_agent));
            Assert.AreEqual(1, agents.Count);
        }

        [Test]
        public void SensorShouldDetectAgentsInRange()
        {
            Agent _u1 = _agentManager.Spawn(AgentPrototype.GetNew(), Vector3.one);
            Agent _u2 = _agentManager.Spawn(AgentPrototype.GetNew(), new Vector3(10f,10f, 0));

            List<Agent> agents = _sensor.GetAgentsInRange(5f);

            Assert.True(agents.Contains(_agent));
            Assert.True(agents.Contains(_u1));
            Assert.False(agents.Contains(_u2));
            Assert.AreEqual(2, agents.Count);

            agents = _sensor.GetAgentsInRange(100f);
            Assert.True(agents.Contains(_agent));
            Assert.True(agents.Contains(_u1));
            Assert.True(agents.Contains(_u2));
            Assert.AreEqual(3, agents.Count);
        }

        [Test]
        public void SensorShouldIgnoreSelfAgentIfDirected()
        {
            //Arrange
            _agentManager.Spawn(AgentPrototype.GetNew(), Vector3.one);
            _agentManager.Spawn(AgentPrototype.GetNew(), new Vector3(10f, 10f, 0));

            //Act
            List<Agent> agents = _sensor.GetOtherAgentsInRange(100f);

            //Assert
            Assert.AreEqual(2, agents.Count);
            Assert.False(agents.Contains(_agent));
        }

        [Test]
        public void SensorShouldPickClosestUnitAndFurthestUnitWhenDirected()
        {
            //Arrange
            Agent _u1 = _agentManager.Spawn(AgentPrototype.GetNew(), Vector3.one);
            _agentManager.Spawn(AgentPrototype.GetNew(), new Vector3(10f, 10f, 0));
            Agent _u3 = _agentManager.Spawn(AgentPrototype.GetNew(), new Vector3(-2f, -2f, 0));
            

            //Act
            Agent closest = _sensor.GetClosestAgentInRange(14.5f);
            Agent furthest = _sensor.GetFurthestAgentInRange(5f);

            //Assert
            Assert.AreEqual(_u1, closest);
            Assert.AreEqual(_u3, furthest);

        }

    */

    }
}
