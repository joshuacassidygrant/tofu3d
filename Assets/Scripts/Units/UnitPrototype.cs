using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Data/Unit", order = 1)]
public class UnitPrototype : ScriptableObject
{
    public string Name;
    public string Id;
    public Sprite Sprite;
}
