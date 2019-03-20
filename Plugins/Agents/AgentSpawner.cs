using System.Collections;
using System.Collections.Generic;
using TofuPlugin.Agents.Factions;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public class AgentSpawner : MonoBehaviour
    {

        private Dictionary<string, AgentPrototype> _loadedUnits;
        private float _sinceLastSpawn = 0;
        private AgentContainer _agentContainer;
        private bool _spawning = false;
        private Vector3 _position;

        //TESTING
        public float SpawnFrequency = 5f;
        private float _variance = 0.1f;
        private Faction _faction;

        public void Init(Vector3 position, AgentContainer agentContainer)
        {
            _agentContainer = agentContainer;
            _position = position;
            //_sinceLastSpawn = SpawnFrequency;
        }

        //TESTING
        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void LoadUnits(Dictionary<string, AgentPrototype> units)
        {
            _loadedUnits = units;

        }

        public void StartSpawning()
        {
            _spawning = true;
            StartCoroutine("Spawning");
        }

        private IEnumerator Spawning()
        {
            while (_spawning)
            {
                if (_faction == null)
                {
                    Debug.Log("No faction found for spawner.");
                    yield return null;
                }
                _sinceLastSpawn += Time.deltaTime;
                if (_sinceLastSpawn >= SpawnFrequency)
                {
                    _sinceLastSpawn = 0;
                    Agent u = _agentContainer.Spawn(new List<string>(_loadedUnits.Keys)[Random.Range(0, _loadedUnits.Count)], "Creature", _position);
                    u.Faction = _faction;
                }
                yield return null;
            }

        }

    

    

    }
}
