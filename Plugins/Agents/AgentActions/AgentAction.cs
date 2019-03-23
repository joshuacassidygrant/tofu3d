using System.Collections.Generic;
using System.Linq;
using TofuCore.Configuration;
using UnityEngine;
using TofuCore.Service;
using TofuCore.Tangible;

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
        public AgentSensor AgentSensor;
        
        public ActionPhase Phase {
            get {
                if (Triggered)
                {
                    return ActionPhase.FOCUS;
                } 

                if (CurrentCooldown > 0)
                {
                    return ActionPhase.COOLDOWN;
                }

                return ActionPhase.READY;
            }

        }

        /*
         *  Usage tag values define base values for the usefulness of the action in general 
         *  circumstances. They should be defined in terms of the dynamic values they might
         *  have, at a rate of ~1 damage = ~1 utility.
         *  
         *  AI Strategies consume these values and combine them with sensor information and
         *  their behaviour priorities to make decisions.
         */
        public abstract Dictionary<string, float> GetUsageTagValues();

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
        public ITangible StoredTarget;

        public ServiceContext ServiceContext;

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

        public virtual void BindDependencies()
        {
            //DO something
        }

        /*
         * The targetting function determines the best target for an action.
         */
        public virtual ActionTargetableValueTuple TargetingFunction()
        {
            if (AgentSensor == null) return ActionTargetableValueTuple.NULL;

            List<ITangible> units = GetTargets().OrderBy(ValueFunction).ToList();
            if (units.Count == 0) return ActionTargetableValueTuple.NULL;

            //Target value refers to how valuable it would be to target the given target
            //abstract this and use it in the above sorting function
            float targetValue = ValueFunction(units[0]);
            float value = Agent.Controller.GetBehaviour().GetUtilityValue(GetUsageTagValues(), targetValue);

            return new ActionTargetableValueTuple(this, units[0], value);
        }



        protected abstract IEnumerable<ITangible> GetTargets();
        protected abstract float ValueFunction(ITangible t);

        public virtual bool CanUse()
        {
            return 
                (Cooldown <= 0 &&
                TargetingFunction().Tangible != null);
        }


        public virtual void TriggerAction(ITangible target)
        {
            Triggered = true;
            StoredTarget = target;
        }

        public virtual void FireAction(ITangible target, float deltaTime) {
            CurrentCooldown = Cooldown;
        }

        public virtual void TryExecute(float time)
        {

        }

        public virtual void Update(float deltaTime)
        {
            switch (Phase)
            {
                case ActionPhase.COOLDOWN:
                    CurrentCooldown = Mathf.Max(0, CurrentCooldown - deltaTime);
                    break;
                case ActionPhase.FOCUS:
                    CurrentFocusTime += deltaTime;
                    
                    if (CurrentFocusTime >= FocusTime)
                    {
                        CurrentFocusTime = 0;
                        FireAction(StoredTarget, deltaTime);
                        StoredTarget = null;
                        Triggered = false; 
                    }
                    break;
                default:
                    break;
            }
        }

        public void InjectServiceContext(ServiceContext serviceContext)
        {
            ServiceContext = serviceContext;
        }
         
        //PROPERTIES
        public virtual void Configure(Configuration config)
        {
            Properties.SetProperty("Cooldown", 0f);
            Properties.SetProperty("Range", 0f);
            //Do something
        }

        public bool Ready()
        {
            return Phase == ActionPhase.READY && !Triggered;
        }

        public bool InRange(ITangible target)
        {
            return TangibleUtilities.GetDistanceBetween(Agent, target) <= Range;
        }



    }

    public enum ActionPhase
    {
        READY,
        FOCUS,
        COOLDOWN
    }
}
