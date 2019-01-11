using System.Collections.Generic;
using System.Dynamic;
using Scripts;
using TofuPlugin.Agents.AI;
using TofuCore.Configuration;
using UnityEngine;

namespace TofuPlugin.Agents.AgentActions
{
    /*
     * AgentActions are functions that can be held by Agents and triggered by Commands
     * when specific criteria are met. Actions may take a set time to execute (i.e. an 
     * attack enemy action may factor in a short swing) or take a variable time (i.e.
     * walking along a path may take any number of frames to complete. 
     * 
     * A single AgentAction refers to a distinct entry within an Agent's action list.
     */
    public abstract class AgentAction : IConfigurable
    {
        public Agent Agent;
        public string Id;
        public string Name;

        /*
         * Cooldown refers to the time until this agent can use this action again
         */
        public float Cooldown;
        public float CurrentCooldown;

        /* 
         * DownTime refers to the time AFTER this is triggered before the agent can 
         * use another action.
         */
        public float DownTime;
        public float CurrentDownTime;

        /*
         * FocusTime refers to the amount of time required to focus on triggering an 
         * action. This could be 0 or very low for something quick (an attack, for 
         * instance) or longer for something more complicated (casting a ritual, carving
         * a small wooden duck). Some actions might allow focus for a variable amount of
         * time up to a maximum time.
         */
        public float FocusTime;
        public float CurrentFocusTime;
        public bool VariableFocusTime;
        public float MaximumFocusTime;

        //The range at which this action can be activated
        public float Range;

        //An action must be triggered by a command (to set a target) before it can be used.
        public bool Triggered;
        


        public Properties Properties {
            get { return _properties; }
            protected set {
                _properties = value;
            }
        }
        
        private Properties _properties;

        protected AgentAction(string id, string name)
        {
            Id = id;
            Name = name;
            Properties = new Properties();

        }

        /*
         * The targetting function determines the best target for an action.
         */
        public abstract ITargettable TargettingFunction();

        public virtual bool CanUse()
        {
            return 
                (Cooldown <= 0 &&
                TargettingFunction() != null);
        }

        public virtual void TriggerAction(ITargettable target)
        {
            CurrentCooldown = Cooldown;
        }

        public virtual void TryExecute(float time)
        {
        }

         
        //PROPERTIES
        public virtual void Configure(Configuration config)
        {
            Properties.SetProperty("Cooldown", 0f);
            Properties.SetProperty("Range", 0f);
            //Do something
        }


    }
}
