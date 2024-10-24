using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WorldMapPokemonEncounter
{
    public WorldMapPokemonEncounterCategory category;
    public List<PokemonEncounter> possibleEncounters = new List<PokemonEncounter>();

}
