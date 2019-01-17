using NUnit.Framework;
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

    }

}
