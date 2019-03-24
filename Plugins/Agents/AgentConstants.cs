using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tuneable, but non-configurable, constants for Agent functionality
 */
namespace TofuPlugin.Agents
{

    public static class AgentConstants
    {
        /**
         * AI:
         */
        //To determine the priority for unspecified behaviour types
        public const float BehaviourCoefficientMinimum = 0.1f;
        public const float PriorityShiftThreshold = 100f;

        /**
         * Mobility:
         */
        public const float PositionTolerance = 0.1f;
        public const float MoveStepMax = 0.5f;
        public const float RepathDistance = 0.5f;
    }
}
