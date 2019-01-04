using System.Collections.Generic;
using NUnit.Framework;
using TofuPlugin.Agents;
using TofuCore.Events;
using TofuCore.Service;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using UnityEngine.UI;

namespace Tests
{
    public class AgentSensorTests
    {
        private ServiceContext _context;
        private AgentSensor _sensor;
        private AgentSensorFactory _sensorFactory;
        private AgentManager _agentManager;
        private Agent _agent;
        private EventContext _eventContext;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();
            _eventContext = new EventContext();
            _eventContext.BindServiceContext(_context);
            _sensorFactory = new AgentSensorFactory();
            _sensorFactory.BindServiceContext(_context);
            _agentManager = new AgentManager();
            _agentManager.BindServiceContext(_context);

            _eventContext.GetPayloadTypeContainer().RegisterPayloadContentType("Agent", (x => x is Agent));

            Debug.Log(_context.Fetch("AgentSensorFactory"));

            _context.FullInitialization();
            Debug.Log(_context.Fetch("AgentSensorFactory"));

            _agent = _agentManager.Spawn(new AgentPrototype(), Vector3.zero);
            _sensor = new AgentSensor(_context, _agent);
        }

        [Test]
        public void SensorShouldDetectNoOtherAgentsInRangeWhenNoAgentsSpawned()
        {
            List<Agent> agents = _sensor.GetAgentsInRange(10f);
    
            Assert.True(agents.Contains(_agent));
            Assert.AreEqual(1, agents.Count);
        }

        [Test]
        public void SensorShouldDetectAgentsInRange()
        {
            Agent _u1 = _agentManager.Spawn(new AgentPrototype(), Vector3.one);
            Agent _u2 = _agentManager.Spawn(new AgentPrototype(), new Vector3(10f,10f, 0));

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
            Agent _u1 = _agentManager.Spawn(new AgentPrototype(), Vector3.one);
            Agent _u2 = _agentManager.Spawn(new AgentPrototype(), new Vector3(10f, 10f, 0));

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
            Agent _u1 = _agentManager.Spawn(new AgentPrototype(), Vector3.one);
            Agent _u2 = _agentManager.Spawn(new AgentPrototype(), new Vector3(10f, 10f, 0));
            Agent _u3 = _agentManager.Spawn(new AgentPrototype(), new Vector3(-2f, -2f, 0));
            

            //Act
            Agent closest = _sensor.GetClosestAgentInRange(14.5f);
            Agent furthest = _sensor.GetFurthestAgentInRange(5f);

            //Assert
            Assert.AreEqual(_u1, closest);
            Assert.AreEqual(_u3, furthest);

        }



    }
}
