using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{

    public bool Passable;
    public Vector3Int Location;
    public Unit Structure = null;
    public bool BuildOnAble
    {
        get { return Structure == null && !Passable; }
    }

    public MapTile(Vector3Int location, bool passable)
    {
        Passable = passable;
        Location = location;
    }


    public void BuildOn(Unit unit)
    {


            Structure = unit;
        

    }

}
