using UnityEngine;
using TofuPlugin.Renderable;
using System.Collections.Generic;
using System.Linq;
using TofuCore.Configuration;
using TofuCore.Glops;
using TofuPlugin.Agents.AgentActions;
using TofuPlugin.Agents.AI;
using TofuPlugin.Agents.Commands;
using TofuPlugin.Agents.Factions;
using TofuCore.Events;
using TofuCore.ResourceModule;
using TofuCore.Tangible;
using TofuPlugin.Agents.Components;
using TofuPlugin.Pathfinding;
using TofuPlugin.PositioningServices;

namespace TofuPlugin.Agents
{
    /*
     * Holds the state and manages functional subcomponents for a single instantiated Agent.
     * Agents are objects that have a position, a name/Id, and are commandable by an attached AI
     * or player input. Agents are managed by an agent manager class and rendered by an agent
     * renderer.
     */
    public class Agent: Glop, IRenderable, ITangible, IControllableAgent, IConfigurable, IResourceModuleOwner
    {
        public string Name { get; private set; }
        public bool Active { get; private set; }

        //Services
        protected AIBehaviourManager BehaviourManager;
        protected PathRequestService PathRequestService;
        protected PositioningService PositioningService;
        protected FactionContainer FactionContainer;
        protected EventContext EventContext;

        /*
         * ITangible and positioning
         */
        public Vector3 Position { get; set; }
        public float SizeRadius { get; protected set; }
        public ITangible TangibleSelf => this;

        /*
         * Rendering
         */
        //TODO: Refactor. Put Sprite and AnimationStates in a component that can interface with IRenderable and accept either a 3d model or a sprite
        public Sprite Sprite { get; set; }
        private Dictionary<string, bool> AnimationStates = new Dictionary<string, bool>();
        public string GetSortingLayer() { return "Unit"; }

        /*
         * Local modules
         */
        public AgentSensor Sensor { get; set; }
        public Dictionary<string, ResourceModule> ResourceModules { get; private set; }
        public HashSet<string> ExpectedProperties { get; private set; }
        public AIAgentController Controller { get; private set; }
        public List<AgentAction> Actions { get; private set; } //Add to this only with the AddAction() method to ensure actions are bound to agent
        public AgentType AgentType { get; private set; }
        public Faction Faction { get; set; }
        public Properties Properties { get; private set; }
        public AgentMobilityComponent Mobility;

        /*
         * Action/Command storage
         */
        //TODO: refactor. should these go in a module?
        public AgentCommand CurrentCommand {get; set;}
        public AgentAction CurrentAction {get; set;}
        public ITangible CurrentActionTarget {get; set;}


        /**
         * INITIALIZATION
         */
        public Agent()
        {
            ResourceModules = new Dictionary<string, ResourceModule>();
            Properties = new Properties();
            Actions = new List<AgentAction>();
        }

        public void ConsumeConfig(Configuration config)
        {
            Properties.Configure(config);
            Properties.Check(ExpectedProperties);
        }
        
        // Called by AgentFactory
        public void ConsumePrototype(AgentType type, AgentPrototype prototype, List<AgentAction> boundActions)
        {
            if (prototype == null) return;
            Sprite = prototype.Sprite;
            Name = prototype.Name;
            AgentType = type;
            ExpectedProperties = AgentType.ExpectedProperties;
            SizeRadius = prototype.SizeRadius;

            ConsumeConfig(prototype.Config);

            BindResourceModules();
            Actions = boundActions;
            Mobility = new AgentMobilityComponent(this, PathRequestService, PositioningService);

        }

        // Called by AgentFactory from AgentContainer to InjectDependencies
        public override void InjectDependencies(ContentInjectablePayload injectables)
        {
            FactionContainer = injectables.Get("FactionContainer");
            EventContext = injectables.Get("EventContext");
            BehaviourManager = injectables.Get("AIBehaviourManager");
            PathRequestService = injectables.Get("PathRequestService");
            PositioningService = injectables.Get("PositioningService");
        }

        private void BindResourceModules()
        {
            foreach (AgentResourceModuleConfig agentResourceModuleConfig in AgentType
                .ResourceModuleConfigs)
            {
                AssignResourceModule(agentResourceModuleConfig.Key, agentResourceModuleConfig.GenerateResourceModule(this, EventContext));
            }
        }

        public void SetController(AIAgentController controller)
        {
            Controller = controller;
            foreach (AgentAction action in Actions)
            {
                (action).AgentSensor = (AgentSensor)controller.GetSensor();
            }

        }

        public virtual void AutoSetController()
        {
            SetController(new AIAgentController(this, Sensor, BehaviourManager));
        }

        /**
         * UPDATE & COMMON METHODS
         */
        public override void Update(float frameDelta)
        {
            UpdateActions(frameDelta);

            if (Controller == null) AutoSetController();
            Controller.Update();
            //Controller should target and trigger actions.

            if (CurrentCommand != null && CurrentCommand.Action != null)
            {
                //Once action is set and targeted, agent is responsible for carrying it out
                CurrentCommand.TryExecute();
                SetAnimationStates();
            }
            Mobility.Update(frameDelta);
        }

        public override string ToString()
        {
            return base.ToString() + AgentType.ToString() + Id + " at " + Position.ToString();
        }

        public override void Die()
        {
            base.Die();
            Active = false;
        }

        /**
         * COMMANDS
         */
        public void ReceiveCommand(AgentCommand command)
        {
            throw new System.NotImplementedException();
        }

        /**
         * ACTIONS
         */

        protected void UpdateActions(float deltaTime)
        {
            foreach (AgentAction action in Actions)
            {
                action.Update(deltaTime);
            }
        }

        public void AddAction(AgentAction action)
        {
            if (action.Agent != this)
            {
                Debug.Log("Must bind action to this agent first!");
                return;
            }
            Actions.Add(action);
        }

        
        /**
         * FACTION METHODS
         */

        public FactionRelationshipLevel GetRelationshipWith(Agent agent)
        {
            return FactionContainer.GetFactionRelationship(this, agent);
        }

        public List<string> GetFactionPermissions(Agent agent)
        {
            return GetRelationshipWith(agent).Permissions;
        }

        public bool PermissionToDo(string factionAction, Agent agent)
        {
            return GetFactionPermissions(agent).Contains(factionAction);
        }



        /**
         * IRENDERABLE
         */
        public Dictionary<string, bool> GetAnimationStateBools()
        {
            return AnimationStates;
        }

        protected virtual void SetAnimationStates()
        {
            ClearAnimationStates();

            //TODO: make a better way to send these
            if (CurrentCommand.Action.Name == "Move")
            {
                SetAnimationState("Walking", true);
            }
            else if (CurrentCommand.Action.Phase == ActionPhase.FOCUS)
            {
                SetAnimationState("Focusing" ,true);
            }
            else if (CurrentCommand.Action.Phase == ActionPhase.READY && CurrentCommand.Action.CanUse())
            {
                SetAnimationState("Acting", true);
            }
        }

        protected void SetAnimationState(string key, bool value)
        {
            if (!AnimationStates.ContainsKey(key))
            {
                AnimationStates.Add(key, value);
            }
            else
            {
                AnimationStates[key] = value;
            }
        }

        protected void ClearAnimationStates()
        {
            string[] keys = AnimationStates.Keys.ToArray();
            foreach (string key in keys)
            {
                AnimationStates[key] = false;
            }
        }

        /**
         * RESOURCE MODULES
         */

        public Dictionary<string, ResourceModule> GetResourceModules()
        {
            return ResourceModules;
        }

        public void AssignResourceModule(string key, ResourceModule module)
        {
            if (ResourceModules.ContainsKey(key))
            {
                Debug.Log("Can't assign a second resource module to key " + key);
                return;
            }

            ResourceModules.Add(key, module);
        }

        public void RemoveResourceModule(string key)
        {
            ResourceModules.Remove(key);
        }

        public ResourceModule GetResourceModule(string key)
        {
            if (!ResourceModules.ContainsKey(key)) return null;
            return ResourceModules[key];
        }
    }
}
