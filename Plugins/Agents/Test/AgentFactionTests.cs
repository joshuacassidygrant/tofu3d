using NUnit.Framework;
using System.Collections.Generic;
using TofuCore.Player;
using TofuCore.Service;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Factions;
using UnityEngine;

namespace TofuPlugin.Agents.Tests
{
    public class AgentFactionTests
    {

        private FactionContainer _factionContainer;
        private ServiceContext _context;
        private AgentPrototype _prototype;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();
            _factionContainer = new FactionContainer().BindServiceContext(_context);


            _prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>();
        }

        [Test]
        public void NewFactionShouldHaveFields()
        {
            Faction faction = new Faction("testIdName", "Test Faction", 0x314CA, _context);

            Assert.AreEqual("testIdName", faction.IdName);
            Assert.AreEqual("Test Faction", faction.GetName());
            Assert.AreEqual(0x314CA, faction.Id);
            Assert.AreEqual(0, faction.GetRelationship(new Faction("f", "f", 3, _context)));
            
        }

        [Test]
        public void ShouldBeAbleToSetControllerOfFaction()
        {
            Faction faction = new Faction("testIdName", "Test Faction", 0x314CA, _context);

            Assert.Null(faction.Controller);

            Player player = new Player(0x43223, "testPlayerName", _context);
            faction.SetController(player);

            Assert.AreEqual(player, faction.Controller);
        }

        [Test]
        public void SetMutualFactionRelationshipShouldWorkMutualUnmutual()
        {
            Faction faction = new Faction("testIdName", "Test Faction", 0x314CA, _context);
            Faction faction2 = new Faction("testIdName2", "Test Faction2", 0x314CB, _context);

            faction.SetMutualRelationship(faction2, 33);

            Assert.AreEqual(33, faction.GetRelationship(faction2));
            Assert.AreEqual(33, faction2.GetRelationship(faction));

            faction.SetRelationship(faction2, 55);

            Assert.AreEqual(55, faction.GetRelationship(faction2));
            Assert.AreEqual(33, faction2.GetRelationship(faction));
        }

        [Test]
        public void SetFactionRelationshipDeltaShouldWorkMutualUnmutual()
        {
            Faction faction = new Faction("testIdName", "Test Faction", 0x314CA, _context);
            Faction faction2 = new Faction("testIdName2", "Test Faction2", 0x314CB, _context);

            faction.SetMutualRelationship(faction2, 5);
            faction.ChangeMutualRelationship(faction2, 20);

            Assert.AreEqual(25, faction.GetRelationship(faction2));
            Assert.AreEqual(25, faction2.GetRelationship(faction));

            faction.ChangeMutualRelationship(faction2, -30);

            Assert.AreEqual(-5, faction.GetRelationship(faction2));
            Assert.AreEqual(-5, faction2.GetRelationship(faction));

            faction.ChangeRelationship(faction2, 10);


            Assert.AreEqual(5, faction.GetRelationship(faction2));
            Assert.AreEqual(-5, faction2.GetRelationship(faction));
        }

        [Test]
        public void SetSelfRelationshipShouldThrowException()
        {
            Faction faction = new Faction("testIdName", "Test Faction", 0x314CA, _context);
        
            try
            {
                faction.ChangeRelationship(faction, 400);
                Assert.Fail();
            } catch (ExceptionSelfRelationship e)
            {
                Assert.AreEqual(0, faction.GetRelationship(faction));
            }
        }


        [Test]
        public void ShouldBeAbleToCreateAndGetFactionsThroughFactionManager()
        {
            Faction fBob = _factionContainer.Create("bobs", "Bob's Raiders");
            Faction fJim = _factionContainer.Create("jims", "Jim's Patriots");
            Faction fSue = _factionContainer.Create("sues", "Sue's Slaughterbunnies");

            Assert.AreEqual(3, _factionContainer.CountActive());
            Assert.AreEqual(fBob, _factionContainer.GetFactionByIdName("bobs"));
            Assert.AreEqual(fSue, _factionContainer.GetFactionByIdName("sues"));
            Assert.AreEqual(fJim, _factionContainer.GetFactionByIdName("jims"));


        }

        [Test]
        public void TestFactionLevelsLoad()
        {
            Assert.AreEqual("Unaffiliated", _factionContainer.GetFactionRelationship(10).Name);

            //With a single level, always return this.
            List<FactionRelationshipLevel> levels = new List<FactionRelationshipLevel>();
            FactionRelationshipLevel def = new FactionRelationshipLevel(30, "Default", new List<string>());
            levels.Add(def);
            _factionContainer.Configure(levels);

            Assert.AreEqual("Default", _factionContainer.GetFactionRelationship(31).Name);


            levels = new List<FactionRelationshipLevel>();
            FactionRelationshipLevel friend = new FactionRelationshipLevel(30, "Friend", new List<string>());
            FactionRelationshipLevel neutral = new FactionRelationshipLevel(-10, "Neutral", new List<string>());
            FactionRelationshipLevel enemy = new FactionRelationshipLevel(-30, "Enemy", new List<string>());
            FactionRelationshipLevel archenemy = new FactionRelationshipLevel(-50, "Archenemy", new List<string>());
            
            levels.Add(friend);
            levels.Add(enemy);
            levels.Add(archenemy);
            levels.Add(neutral);

            _factionContainer.Configure(levels);

            Assert.AreEqual("Friend", _factionContainer.GetFactionRelationship(30).Name);
            Assert.AreEqual("Friend", _factionContainer.GetFactionRelationship(500).Name);
            Assert.AreEqual("Neutral", _factionContainer.GetFactionRelationship(29).Name);
            Assert.AreEqual("Neutral", _factionContainer.GetFactionRelationship(0).Name);
            Assert.AreEqual("Neutral", _factionContainer.GetFactionRelationship(-10).Name);
            Assert.AreEqual("Enemy", _factionContainer.GetFactionRelationship(-11).Name);
            Assert.AreEqual("Enemy", _factionContainer.GetFactionRelationship(-30).Name);
            Assert.AreEqual("Archenemy", _factionContainer.GetFactionRelationship(-31).Name);
            Assert.AreEqual("Archenemy", _factionContainer.GetFactionRelationship(-512).Name);
            

        }

        [Test]
        public void TestFactionLevelsShouldWorkWithAgents()
        {
            List<FactionRelationshipLevel> levels = new List<FactionRelationshipLevel>();
            FactionRelationshipLevel friend = new FactionRelationshipLevel(30, "Friend", new List<string>());
            FactionRelationshipLevel neutral = new FactionRelationshipLevel(-10, "Neutral", new List<string>());
            FactionRelationshipLevel enemy = new FactionRelationshipLevel(-30, "Enemy", new List<string>());
            FactionRelationshipLevel archenemy = new FactionRelationshipLevel(-50, "Archenemy", new List<string>());

            levels.Add(friend);
            levels.Add(enemy);
            levels.Add(archenemy);
            levels.Add(neutral);

            _factionContainer.Configure(levels);

            Agent agent = new Agent(123, _prototype, Vector3.one, _context);
            Agent agent2 = new Agent(124, _prototype, Vector3.zero, _context);
            Agent agent3 = new Agent(125, _prototype, Vector3.left, _context);

            Faction bobs = _factionContainer.Create("bobs", "Bob's Raiders");
            Faction sues = _factionContainer.Create("sues", "Sue's Slaughterers");

            agent.Faction = bobs;
            agent2.Faction = sues;
            agent3.Faction = sues;

            bobs.SetMutualRelationship(sues, -29);

            Assert.AreEqual("Enemy", _factionContainer.GetFactionRelationship(agent, agent2).Name);
            Assert.AreEqual("Same", _factionContainer.GetFactionRelationship(agent2, agent3).Name);
            Assert.AreEqual("Same", _factionContainer.GetFactionRelationship(agent2, agent2).Name);

            bobs.SetMutualRelationship(sues, 30);
            Assert.AreEqual("Friend", _factionContainer.GetFactionRelationship(agent, agent2).Name);

            sues.SetRelationship(bobs, -50);
            Assert.AreEqual("Archenemy", _factionContainer.GetFactionRelationship(agent2, agent).Name);
            Assert.AreEqual("Friend", _factionContainer.GetFactionRelationship(agent, agent2).Name);

        }

        [Test]
        public void TestAgentFunctionsShouldReturnFactionLevelsAndPermissions()
        {
            List<FactionRelationshipLevel> levels = new List<FactionRelationshipLevel>();
            FactionRelationshipLevel friend = new FactionRelationshipLevel(30, "Friend", new List<string> {"help", "hug", "kiss" });
            FactionRelationshipLevel neutral = new FactionRelationshipLevel(-10, "Neutral", new List<string> { "stare" });
            FactionRelationshipLevel enemy = new FactionRelationshipLevel(-30, "Enemy", new List<string> { "attack" });
            FactionRelationshipLevel archenemy = new FactionRelationshipLevel(-50, "Archenemy", new List<string> { "hunt", "attack" });
            FactionRelationshipLevel same = new FactionRelationshipLevel(0, "Same", new List<string> { "help"});

            _factionContainer.SetSame(same);

            levels.Add(friend);
            levels.Add(enemy);
            levels.Add(archenemy);
            levels.Add(neutral);

            _factionContainer.Configure(levels);

            Agent agent = new Agent(123, _prototype, Vector3.one, _context);
            Agent agent2 = new Agent(124, _prototype, Vector3.zero, _context);
            Agent agent3 = new Agent(125, _prototype, Vector3.left, _context);

            Faction bobs = _factionContainer.Create("bobs", "Bob's Raiders");
            Faction sues = _factionContainer.Create("sues", "Sue's Slaughterers");

            agent.Faction = bobs;
            agent2.Faction = sues;
            agent3.Faction = sues;

            bobs.SetMutualRelationship(sues, -30);

            Assert.False(agent.PermissionToDo("help", agent2));
            Assert.True(agent.PermissionToDo("attack", agent2));
            Assert.True(agent2.PermissionToDo("help", agent3));

            bobs.SetMutualRelationship(sues, 40);
            Assert.True(agent.PermissionToDo("hug", agent2));
            Assert.True(agent.PermissionToDo("kiss", agent2));
            Assert.False(agent.PermissionToDo("hug", agent));
        }
    }


    

}

