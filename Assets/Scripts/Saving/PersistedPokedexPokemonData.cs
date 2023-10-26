using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PersistedPokedexPokemonData
{
    public PokemonBaseId pokemonId;
    public string id;
    public int seenAmount = 0;
    public int caughtAmount = 0;

    public string GetId()
    {
        return string.IsNullOrEmpty(id) ? pokemonId.ToString() : id;
    }
}
