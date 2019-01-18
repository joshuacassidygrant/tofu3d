using NUnit.Framework;
using System.Collections.Generic;
using TofuCore.Player;
using TofuCore.Service;
using TofuPlugin.Agents.Factions;

namespace TofuPlugin.Agents.Tests
{
    public class AgentFactionTests
    {

        private FactionManager _factionManager;
        private ServiceContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();
            _factionManager = new FactionManager().BindServiceContext(_context);
        }

        [Test]
        public void NewFactionShouldHaveFields()
        {
            Faction faction = new Faction("testIdName", "Test Faction", 0x314CA, _context);

            Assert.AreEqual("testIdName", faction.IdName);
            Assert.AreEqual("Test Faction", faction.Name);
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
            Faction fBob = _factionManager.Create("bobs", "Bob's Raiders");
            Faction fJim = _factionManager.Create("jims", "Jim's Patriots");
            Faction fSue = _factionManager.Create("sues", "Sue's Slaughterbunnies");

            Assert.AreEqual(3, _factionManager.CountActive());
            Assert.AreEqual(fBob, _factionManager.GetFactionByIdName("bobs"));
            Assert.AreEqual(fSue, _factionManager.GetFactionByIdName("sues"));
            Assert.AreEqual(fJim, _factionManager.GetFactionByIdName("jims"));


        }

        [Test]
        public void TestFactionLevelsLoad()
        {
            Assert.AreEqual("Unaffiliated", _factionManager.GetFactionRelationship(10).Name);

            //With a single level, always return this.
            List<FactionRelationshipLevel> levels = new List<FactionRelationshipLevel>();
            FactionRelationshipLevel def = new FactionRelationshipLevel(30, "Default");
            levels.Add(def);
            _factionManager.Configure(levels);

            Assert.AreEqual("Default", _factionManager.GetFactionRelationship(31).Name);


            levels = new List<FactionRelationshipLevel>();
            FactionRelationshipLevel friend = new FactionRelationshipLevel(30, "Friend");
            FactionRelationshipLevel neutral = new FactionRelationshipLevel(-10, "Neutral");
            FactionRelationshipLevel enemy = new FactionRelationshipLevel(-30, "Enemy");
            FactionRelationshipLevel archenemy = new FactionRelationshipLevel(-50, "Archenemy");
            levels.Add(friend);
            levels.Add(enemy);
            levels.Add(archenemy);
            levels.Add(neutral);

            _factionManager.Configure(levels);

            Assert.AreEqual("Friend", _factionManager.GetFactionRelationship(30).Name);
            Assert.AreEqual("Friend", _factionManager.GetFactionRelationship(500).Name);
            Assert.AreEqual("Neutral", _factionManager.GetFactionRelationship(29).Name);
            Assert.AreEqual("Neutral", _factionManager.GetFactionRelationship(0).Name);
            Assert.AreEqual("Neutral", _factionManager.GetFactionRelationship(-10).Name);
            Assert.AreEqual("Enemy", _factionManager.GetFactionRelationship(-11).Name);
            Assert.AreEqual("Enemy", _factionManager.GetFactionRelationship(-30).Name);
            Assert.AreEqual("Archenemy", _factionManager.GetFactionRelationship(-31).Name);
            Assert.AreEqual("Archenemy", _factionManager.GetFactionRelationship(-512).Name);
            

        }
    }

}
