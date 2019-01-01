using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TofuPlugin.Agents
{
    public interface ITargettable
    {
        Vector3 Position { get; }
    }
}
