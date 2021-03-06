﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TofuCore.Tangible
{
    /**
     * An object that implements ITangible exists at a point in space and has a size. This allows them to be sensed and targeted.
     */
    public interface ITangible
    {
        string Name { get; }
        Vector3 Position { get; }
        bool Active { get; }
        float SizeRadius { get; }
        int Id { get; }
        bool Hidden { get; }
        Vector3 PositionYZSwizzled { get; }
        HashSet<string> Tags { get; }
    }
}
