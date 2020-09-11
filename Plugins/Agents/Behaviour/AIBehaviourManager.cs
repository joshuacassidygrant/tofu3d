using System;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Service;
using TofuPlugin.Agents.Behaviour;
using UnityEngine;

namespace TofuPlugin.Agents {
    /*
     * Responsible for dispensing strategies to units.
     */
    public class AIBehaviourManager : AbstractService
    {
        private string _default = "AiBehaviourDefault";
        private Dictionary<string, Func<AIBehaviour>> _behaviours = new Dictionary<string, Func<AIBehaviour>>();
        

        public AIBehaviour ChooseStrategy(Agent _u)
        {
            //TODO: this
            return _behaviours.Values.ToList()[UnityEngine.Random.Range(1, _behaviours.Count)].Invoke();
                //return _behaviours["AiBehaviourMoveToObjective"].Invoke();
        }

        public void SetDefaultStrategy(string defaultName)
        {
            _default = defaultName;
            if (!_behaviours.ContainsKey(defaultName))
            {
                //Debug.Log("No default strategy found for " + defaultName);
            }
        }

        public AIBehaviour GetDefaultBehaviour()
        {
            return _behaviours[_default].Invoke();
        } 

        public AIBehaviour GetBehaviour(string behaviourId)
        {
            if (!_behaviours.ContainsKey(behaviourId))
            {
                Debug.Log("No strategy found for " + behaviourId);
                return _behaviours[_default].Invoke();
            }

            return _behaviours[behaviourId].Invoke();
        }

        public string GetNameOfDefaultStrategy()
        {
            return _default;
        }

        public void BindBehaviours(Dictionary<string, Func<AIBehaviour>> behaviours, string defaultName)
        {
            _behaviours = behaviours;
            SetDefaultStrategy(defaultName);
        }
        
    }
}
