using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using UnityEngine;

public interface IGlopContainer
{
    List<Glop> GetContents();
    Glop GetGlopById(int id);
    bool HasId(int id);
}
