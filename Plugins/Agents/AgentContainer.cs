using System.Collections.Generic;
using System.Linq;
using TofuCore.Configuration;
using TofuCore.ContentInjectable;
using TofuCore.Events;
using TofuCore.Glops;
using TofuCore.Service;
using TofuCore.Tangible;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.Factions;
using TofuPlugin.Agents.Sensors;
using TofuPlugin.Pathfinding;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentContainer : GlopContainer<Agent>, ITangibleContainer {

        [Dependency] protected AgentFactory AgentFactory;
        [Dependency("CreaturesLibrary")] protected AgentPrototypeLibrary CreaturesLibrary;
        [Dependency("TowersLibrary")] protected AgentPrototypeLibrary TowersLibrary;

        [Dependency] [ContentInjectable] private EventContext _eventContext;
        [Dependency] [ContentInjectable] protected FactionContainer FactionContainer;
        [Dependency] [ContentInjectable] protected AIBehaviourManager AiBehaviourManager;
        [Dependency] [ContentInjectable] protected PathRequestService PathRequestService;
        [Dependency] [ContentInjectable] protected PositioningServices.PositioningService PositioningService;

        private List<AgentSpawner> _unitSpawners;

        public override void Build()
        {
            base.Build();
            _unitSpawners = new List<AgentSpawner>();
             
        }

        public override void Initialize()
        {
            base.Initialize();
            BindListeners();
        }

        private void BindListeners()
        {
            BindListener("UnitDies", UnitDies, EventContext);
        }

        public Agent Spawn(string prototypeId, string agentTypeLabel, Vector3 location, Configuration config = null)
        {
            AgentPrototypeLibrary activeLibrary;

            switch (agentTypeLabel)
            {
                case "Creature":
                    activeLibrary = CreaturesLibrary;
                    break;
                case "Structure":
                    activeLibrary = TowersLibrary;
                    break;
                default:
                    return null;

            }
            if (!activeLibrary.ContainsKey(prototypeId))
            {
                Debug.Log("No agent of type " + prototypeId + " in agent library");
            }

            AgentPrototype prototype = activeLibrary.Get(prototypeId);
            return Spawn(prototype, location, config);
        }

        public Agent Spawn(AgentPrototype prototype, Vector3 location, Configuration config = null)
        {
            Agent agent = AgentFactory.BuildAndRegisterNewAgent(this, location, prototype, config);
            EventContext.TriggerEvent("SpawnAgent", new EventPayload("Agent", agent));
            return agent;
        }

        public List<Agent> GetAllAgentsInRangeOfPoint(Vector3 point, float range)
        {
            return GetAgents().Where(x => (point - x.Position).sqrMagnitude <= range * range).ToList();

        }

        public List<Agent> GetAgents()
        {
            return GetContents().Cast<Agent>().ToList();
        }

        public List<ITangible> GetAllTangibles() {
            return GetContents().Cast<ITangible>().ToList();
        }

        public List<ITangible> GetAllTangiblesWithinRangeOfPoint(Vector3 point, float range)
        {
            return GetAllTangibles().Where(x => (point - x.Position).magnitude <= range).ToList();
        }

        public AgentSpawner CreateSpawner(Dictionary<string, AgentPrototype> units, Vector3 loc)
        {
            AgentSpawner spawner = new GameObject().AddComponent<AgentSpawner>();
            spawner.name = "Spawner";
            spawner.transform.position = loc;
            _unitSpawners.Add(spawner);
            spawner.Init(loc, this);
            spawner.LoadUnits(units);
            spawner.StartSpawning();
            return spawner;
        }


        private void UnitDies(EventPayload payload)
        {
            Agent u = payload.GetContent();
            u.Die();
        }


    }
}