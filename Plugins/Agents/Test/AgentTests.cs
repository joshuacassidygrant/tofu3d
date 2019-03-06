using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Sensors;
using UnityEngine;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;

namespace TofuPlugin.Agents.Tests
{
    
    public class AgentTests
    {
        private AgentPrototype _prototype;
        private ServiceContext _context;
        private AgentContainer _agentContainer;

        [SetUp]
        public void SetUp()
        {
            /*_context = new ServiceContext();
            new AgentFactory().BindServiceContext(_context);
            new AgentActionFactory().BindServiceContext(_context);
            new FactionContainer().BindServiceContext(_context);
            new AgentSensorFactory().BindServiceContext(_context);
            _agentContainer = new AgentContainer().BindServiceContext(_context);
            */

            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>
        {
            new PrototypeActionEntry("test1"),
            new PrototypeActionEntry("test2")
        };

            //_context.FullInitialization();
        }

        [Test]
        public void AgentShouldConstructWithNoPrototypeOrConfig()
        {
            Agent agent = new Agent();
            Assert.NotNull(agent);
            Assert.NotNull(agent.GetResourceModules());
            Assert.NotNull(agent.Properties);

        }

        [Test]
        public void AgentShouldConstructWithEmptyPropertiesAndActions()
        {
            Agent u = new Agent();
            u.ConsumePrototype(new AgentType("Test", new HashSet<string>(), new List<string>(), new List<AgentResourceModuleConfig>()), _prototype, new List<AgentAction>());

            //Assert
            Assert.AreEqual("T1P", u.GetName());
            Assert.AreEqual(null, u.Sprite);
            Assert.AreEqual(Vector3.zero, u.Position);
            Assert.AreEqual(0, u.Actions.Count);
        }
    }

}
