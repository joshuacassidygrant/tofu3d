﻿using TofuCore.Tangible;

//Represents an ITangible object in the world and allows querying and targeting that object through UI.

public interface ITangibleRenderer 
{
    ITangible Tangible { get; }
}
