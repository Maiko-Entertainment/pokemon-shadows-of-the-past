using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonBaseDataAreas
{
    public WorldMap encounteredIn;
    public List<PokemonBaseDataAreasEnvironment> environments = new List<PokemonBaseDataAreasEnvironment>();
}
