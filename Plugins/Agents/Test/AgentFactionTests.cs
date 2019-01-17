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
        public void CanSetMutualFactionRelationship()
        {

        }

    }

}
