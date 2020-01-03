using NUnit.Framework;
using System.Collections.Generic;
using TofuCore.Player;
using TofuCore.Service;
using TofuPlugin.Agents.Factions;

namespace TofuPlugin.Agents.Tests
{
    public class AgentFactionTests
    {

        private FactionContainer _factionContainer;
        private AgentContainer _agentContainer;
        private ServiceContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ServiceContext();
            _factionContainer = new FactionContainer().BindServiceContext(_context);
            _agentContainer = new AgentContainer().BindServiceContext(_context);
            new AgentFactory().BindServiceContext(_context);
            _context.FullInitialization();

            /*_prototype = ScriptableObject.CreateInstance<AgentPrototype>();
            _prototype.Id = "t1p";
            _prototype.Name = "T1P";
            _prototype.Sprite = null;
            _prototype.Actions = new List<PrototypeActionEntry>();*/
        }

        [Test]
        public void NewFactionShouldHaveFields()
        {
            Faction faction = new Faction("testIdName", "Test Faction");

            Assert.AreEqual("testIdName", faction.IdName);
            Assert.AreEqual("Test Faction", faction.GetName());
            Assert.AreEqual(0, faction.GetRelationship(new Faction("f", "f")));
            
        }

        [Test]
        public void ShouldBeAbleToSetControllerOfFaction()
        {
            Faction faction = new Faction("testIdName", "Test Faction");

            Assert.Null(faction.Controller);

            Player player = new Player("testPlayerName");
            faction.SetController(player);

            Assert.AreEqual(player, faction.Controller);
        }

        [Test]
        public void SetMutualFactionRelationshipShouldWorkMutualUnmutual()
        {
            Faction faction = new Faction("testIdName", "Test Faction");
            Faction faction2 = new Faction("testIdName2", "Test Faction2");

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
            Faction faction = new Faction("testIdName", "Test Faction");
            Faction faction2 = new Faction("testIdName2", "Test Faction2");

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
            Faction faction = new Faction("testIdName", "Test Faction");
        
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
    }


    

}

