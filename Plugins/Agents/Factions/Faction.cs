using System.Collections;
using System.Collections.Generic;
using TofuCore.Events;
using TofuCore.Glops;
using TofuCore.Player;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Agents.Factions
{
    public class Faction: Glop
    {


        //List of other allegiances and their relationships
        private Dictionary<Faction, int> _relationships;

        public string IdName;
        private string _name;

        public virtual Sprite Icon { get; protected set; }
        public Player Controller;

        protected FactionContainer FactionContainer;

        public Faction(FactionContainer container, string idName, string niceName) : base()
        {
            FactionContainer = container;
            IdName = idName;
            _name = niceName;
            _relationships = new Dictionary<Faction, int>();
        }

        public void SetMutualRelationship (Faction faction, int value)
        {
            SetRelationship(faction, value);
            faction.SetRelationship(this, value);

        }

        public void SetRelationship(Faction faction, int value)
        {
            if (faction == this) throw new ExceptionSelfRelationship();

            if (!_relationships.ContainsKey(faction))
            {
                _relationships.Add(faction, value);
            }
            else
            {
                _relationships[faction] = value;
            }
        }

        public void ChangeMutualRelationship (Faction faction, int value)
        {  
            ChangeRelationship(faction, value);
            faction.ChangeRelationship(this, value);
        }

        public void ChangeRelationship (Faction faction, int value)
        {
            if (faction == this) throw new ExceptionSelfRelationship();

            if (!_relationships.ContainsKey(faction))
            {
                _relationships.Add(faction, value);
            }
            else
            {
                _relationships[faction] += value;
            }
        }

        public void SetController(Player controller)
        {
            Controller = controller;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetRelationship(Faction faction)
        {
            if (!_relationships.ContainsKey(faction)) return 0;
            return _relationships[faction];
        }



        public override void Update(float frameDelta)
        {
            //Do something!
        }
    }

}

