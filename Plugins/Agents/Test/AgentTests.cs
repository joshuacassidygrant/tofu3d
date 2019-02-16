using System.Collections.Generic;
using NUnit.Framework;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions.Fake;
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

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();
            new FakeAgentActionFactory().BindServiceContext(_context, "AgentActionFactory");
            new FactionContainer().BindServiceContext(_context);
            new AgentSensorFactory().BindServiceContext(_context);



            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>
        {
            new PrototypeActionEntry("test1"),
            new PrototypeActionEntry("test2")
        };

            _context.FullInitialization();
        }

        [Test]
        public void AgentShouldConstructWithNullPrototype()
        {
            Agent u = new Agent(123, null, Vector3.back, _context);
            Assert.NotNull(u);
            Assert.AreEqual(123, u.GetId());
        }

        [Test]
        public void AgentShouldConstructWithPropertiesAndActions()
        {
            Agent u = new Agent(123, _prototype, Vector3.zero, _context);

            //Assert
            Assert.AreEqual(123, u.Id);
            Assert.AreEqual("T1P", u.GetName());
            Assert.AreEqual(null, u.Sprite);
            Assert.AreEqual(Vector3.zero, u.Position);
            Assert.AreEqual(2, u.Actions.Count);

            

        }
    }

}
