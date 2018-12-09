using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public int Id;
    public string Name;
    public float Speed;
    public Allegiance Allegiance;
    public bool Structure;
    public Vector3 Position;

    public Unit(int id)
    {
        Id = id;
    }

    public void Update()
    {
        MoveAlong();
    }

    public void MoveAlong()
    {
        Position = new Vector3(Position.x + Speed/100f, Position.y, Position.z);
    }



}
