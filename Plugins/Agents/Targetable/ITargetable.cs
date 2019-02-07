using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TofuPlugin.Agents.Targetable
{
    public interface ITargetable
    {
        Vector3 Position { get; }
        bool Active { get; }
        float SizeRadius { get; }
    }
}
