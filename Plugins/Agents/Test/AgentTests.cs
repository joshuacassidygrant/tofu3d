using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Configuration;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;
using TofuPlugin.Pathfinding;
using UnityEditor.VersionControl;

namespace TofuPlugin.Agents.Tests
{
    /**
     * Tests all Agent behaviours not requiring external services.
     */
    public class AgentTests
    {
        private AgentPrototype _prototype;
        private ServiceContext _context;
        private AgentContainer _agentContainer;
        private Configuration _config;

        [SetUp]
        public void SetUp()
        {

            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>
            {
                new PrototypeActionEntry("test1"),
                new PrototypeActionEntry("test2")
            };

            _config = new Configuration();
            _config.AddProperty("testInt", 3);
            _config.AddProperty("testStr", "abc");
            _config.AddProperty("testFloat", 5.2f);


        }

        /**
         * Config and construction:
         */
        [Test]
        public void AgentShouldConstructWithNoPrototypeOrConfig()
        {
            Agent agent = new Agent();
            Assert.NotNull(agent);
        }

        [Test]
        public void AgentShouldConsumePrototype()
        {
            Agent agent = new Agent();
            agent.ConsumePrototype(new AgentType("Test", new HashSet<string>(), new List<string>(), new List<AgentResourceModuleConfig>()), _prototype, new List<AgentAction>());

            //Assert
            Assert.AreEqual("T1P", agent.Name);
            Assert.AreEqual(null, agent.Sprite);
            Assert.AreEqual(Vector3.zero, agent.Position);
            Assert.AreEqual(0, agent.Actions.Count);
        }

        [Test]
        public void AgentShouldConsumeConfig()
        {
            Agent agent = new Agent();
            agent.ConsumeConfig(_config);

            //Assert
            Assert.NotNull(agent.Properties);
            Assert.False(agent.Properties.GetProperties().Count == 0);
            Assert.AreEqual(3, agent.Properties.GetProperty("testInt", 0));
            Assert.AreEqual(2, agent.Properties.GetProperty("unfound", 2));
            Assert.AreEqual(5.2f, agent.Properties.GetProperty("testFloat", 0f), 0.1f);
        }

        [Test]
        public void AgentShouldInjectDependencies()
        {
            ContentInjectablePayload payload = new ContentInjectablePayload();
            payload.Add("FactionContainer", new FactionContainer());
            payload.Add("EventContext", new EventContext());
            payload.Add("AIBehaviourManager", new AIBehaviourManager());
            payload.Add("PathRequestService", new PathRequestService());

            Agent agent = new Agent();
            agent.InjectDependencies(payload);
            //Pass if fine
           

        }

        [Test]
        public void AgentShouldConstructWithEmptyPropertiesAndActions()
        {
            Agent agent = new Agent();

            Assert.NotNull(agent.GetResourceModules());
            Assert.NotNull(agent.Properties);
            Assert.NotNull(agent.Actions);
            Assert.AreEqual(0, agent.GetResourceModules().Count);
            Assert.AreEqual(0, agent.Properties.GetProperties().Count);
            Assert.AreEqual(0, agent.Actions.Count);
            

        }

    }

}
